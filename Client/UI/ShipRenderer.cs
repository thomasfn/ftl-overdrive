using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SFML.Graphics;

using FTLOverdrive.Client.Ships;
using SFML.Window;
using FTLOverdrive.Client.Gamestate;
using System.Collections.Specialized;

namespace FTLOverdrive.Client.UI
{
    public class ShipRenderer : Control
    {
        public bool CanInteract { get; set; }

        private bool showRooms;
        public bool ShowRooms
        {
            get
            {
                return showRooms;
            }
            set
            {
                showRooms = value;
                UpdateLayout();
            }
        }

        private Ship ship;
        public Ship Ship
        {
            get
            {
                return ship;
            }
            set
            {
                if (ship != null)
                {
                    ship.ShipModified -= onShipModified;
                }
                ship = value;
                if (ship != null)
                {
                    ship.ShipModified += onShipModified;
                }
                UpdateLayout();
            }
        }
        private Sprite sprShip;

        private RenderTexture rtShip;

        private Dictionary<Door.DoorEntrance, DoorEntranceRenderer> doorRenderers;

        public override void Init()
        {
            sprShip = new Sprite();
            doorRenderers = new Dictionary<Door.DoorEntrance, DoorEntranceRenderer>();
            base.Init();
        }

        private void onShipModified(Object sender, Ship.ShipModifiedEventArgs e)
        {
            if (sender != Ship) return;
            switch (e.Action)
            {
                case Ships.Ship.ShipModifiedEventArgs.ShipModifiedAction.Doors:
                    updateDoors(e.CollectionEventArgs);
                    break;
                default:
                    UpdateLayout();
                    break;
            }
        }

        private void updateDoors(NotifyCollectionChangedEventArgs e)
        {
            foreach (var doorRenderer in doorRenderers.Values)
            {
                doorRenderer.Remove();
            }
            doorRenderers.Clear();
            foreach (var door in Ship.Doors)
            {
                foreach (var entrance in door.Entrances)
                {
                    if (entrance.RoomID != -1)
                    {
                        var doorRenderer = new DoorEntranceRenderer(this, door, entrance);
                        doorRenderers.Add(entrance, doorRenderer);
                        doorRenderer.Parent = this;
                        doorRenderer.Init();
                    }
                }
            }
        }

        protected override void UpdateLayout()
        {
            if (Ship != null && sprShip != null)
            {
                rtShip = GetRenderTexture(); // We cache this so it doesn't get GCed
                sprShip = new Sprite(rtShip.Texture);
                sprShip.Texture.Smooth = false;
                sprShip.Position = new SFML.Window.Vector2f(AbsX, AbsY);
                sprShip.Scale = getScale();

                updateDoors(null);
            }
            base.UpdateLayout();
        }

        public Vector2f getScale()
        {
            return new Vector2f(1, 1) * Height / sprShip.Texture.Size.Y;
        }

        private int updateCounter = 0;
        protected override void Draw(RenderWindow window)
        {
            // Every so ofter refresh the graphic in case it didn't update when something changed in the ship.
            // That should never happen, but better safe than sorry.
            updateCounter++;
            if (updateCounter > 100)
            {
                updateCounter = 0;
                UpdateLayout();
            }

            base.Draw(window);
            if (Ship == null) return;
            window.Draw(sprShip);
        }

        public override void Remove()
        {
            base.Remove();
            if (sprShip != null)
            {
                sprShip.Dispose();
                sprShip = null;
            }
        }




        private static void DrawLine(RenderTexture target, Vector2f a, Vector2f b, Color col, int thickness = 1)
        {
            var shpLine = new RectangleShape();
            shpLine.Size = new Vector2f((float)Math.Sqrt((b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y)), thickness);
            shpLine.Origin = shpLine.Size * 0.5f;
            shpLine.Position = (a + b) * 0.5f;
            shpLine.Rotation = (float)(Math.Atan2(b.Y - a.Y, b.X - a.X) * 180.0 / Math.PI);
            shpLine.FillColor = col;
            target.Draw(shpLine);
        }

        private static void DrawQuad(RenderTexture target, Vector2f a, Vector2f b, Color col)
        {
            var shpLine = new RectangleShape();
            shpLine.Size = b - a;
            shpLine.Origin = shpLine.Size * 0.5f;
            shpLine.Position = (a + b) * 0.5f;
            shpLine.Rotation = 0.0f;
            shpLine.FillColor = col;
            target.Draw(shpLine);
        }

        private static void DrawTexture(RenderTarget target, Vector2f a, Vector2f b, Texture texture, float rotation = 0)
        {
            var sprTexture = new Sprite(texture);
            sprTexture.Scale = Util.Scale(sprTexture, b - a);
            sprTexture.Origin = new Vector2f(texture.Size.X * 0.5f, texture.Size.Y * 0.5f);
            sprTexture.Position = (a + b) * 0.5f;
            sprTexture.Rotation = rotation;
            target.Draw(sprTexture);
        }

        private static void DrawColoredTexture(RenderTarget target, Vector2f a, Vector2f b, Texture texture, Color color, float rotation = 0)
        {
            var sprTexture = new Sprite(texture);
            sprTexture.Scale = Util.Scale(sprTexture, b - a);
            sprTexture.Origin = new Vector2f(texture.Size.X * 0.5f, texture.Size.Y * 0.5f);
            sprTexture.Position = (a + b) * 0.5f;
            sprTexture.Rotation = rotation;
            sprTexture.Color = color;
            target.Draw(sprTexture);
        }

        private RenderTexture GetRenderTexture()
        {
            int wallThickness = 2;
            int tileBorderThickness = 1;

            // Colours
            var colTile = new Color(228, 226, 216);
            var colWall = new Color(0, 0, 0);
            var colTileBorder = new Color(198, 196, 192);
            var colSystem = new Color(128, 128, 128);
            //colTile = new Color(0, 0, 50);

            // Vectors
            var origin = new Vector2f(Ship.FloorOffsetX, Ship.FloorOffsetY);
            var tileX = new Vector2f(Ship.TileWidth, 0.0f);
            var tileY = new Vector2f(0.0f, Ship.TileHeight);
            var tileBorder = new Vector2f(tileBorderThickness / 2, tileBorderThickness / 2);
            var wallX = new Vector2f(wallThickness / 2, 0.0f);
            var wallY = new Vector2f(0.0f, wallThickness / 2);

            // Textures
            Texture baseGraphic = Root.Singleton.Material(Ship.BaseGraphic, false);
            Texture floorGraphic = Root.Singleton.Material(Ship.FloorGraphic, false);

            var rtWidth = baseGraphic.Size.X;
            var rtHeight = baseGraphic.Size.Y;
            RenderTexture rt = new RenderTexture(rtWidth, rtHeight);
            rt.Clear(new Color(0, 0, 0, 0));

            // Draw the ship and floor
            DrawTexture(rt, new Vector2f(0.0f, 0.0f), new Vector2f(rtWidth, rtHeight), baseGraphic);
            if (!ShowRooms) return rt;
            DrawTexture(rt, new Vector2f(0.0f, 0.0f), new Vector2f(rtWidth, rtHeight), floorGraphic);

            // Draw rooms
            foreach (var room in Ship.Rooms)
            {
                var roomCorner = origin + tileX * room.X + tileY * room.Y;
                var roomCenter = roomCorner + tileX * (room.GetBoundingBox().Left + (float)room.GetBoundingBox().Width / 2) +
                                              tileY * (room.GetBoundingBox().Top + (float)room.GetBoundingBox().Height / 2);
                foreach (var tile in room.GetTiles())
                {
                    var tileCorner = roomCorner + tileX * tile.X + tileY * tile.Y;
                    DrawQuad(rt, tileCorner + tileBorder, tileCorner + tileX + tileY - tileBorder, colTile);
                    DrawLine(rt, tileCorner, tileCorner + tileX, colTileBorder, tileBorderThickness);
                    DrawLine(rt, tileCorner + tileY + wallX, tileCorner + wallX, colTileBorder, tileBorderThickness);
                }
                if (room.BackgroundGraphic != null)
                {
                    var roomGraphic = Root.Singleton.Material(room.BackgroundGraphic, false);
                    DrawTexture(rt, roomCorner, roomCorner + new Vector2f(roomGraphic.Size.X, roomGraphic.Size.Y), roomGraphic);
                }
                if (room.System != null)
                {
                    var system = Root.Singleton.mgrState.Get<Library>().GetSystem(room.System);
                    if (system != null)
                    {
                        var systemGraphic = Root.Singleton.Material(system.IconGraphics["overlay"], false);
                        if (systemGraphic != null)
                        {
                            DrawColoredTexture(rt, roomCenter - new Vector2f(systemGraphic.Size.X, systemGraphic.Size.Y) / 2,
                                                   roomCenter + new Vector2f(systemGraphic.Size.X, systemGraphic.Size.Y) / 2, systemGraphic, colSystem);
                        }
                    }
                }
            }
            
            // Draw walls
            foreach (var room in Ship.Rooms)
            {
                var roomCorner = origin + tileX * room.X + tileY * room.Y;
                foreach (var tile in room.GetTiles())
                {
                    var tileCorner = roomCorner + tileX * tile.X + tileY * tile.Y;

                    // Top
                    if (!room.HasTile(tile.X, tile.Y - 1))
                    {
                        DrawLine(rt, tileCorner + wallY, tileCorner + tileX + wallY, colWall, wallThickness);
                    }

                    // Right
                    if (!room.HasTile(tile.X + 1, tile.Y))
                    {
                        DrawLine(rt, tileCorner + tileX - wallX, tileCorner + tileX + tileY - wallX, colWall, wallThickness);
                    }

                    // Bottom
                    if (!room.HasTile(tile.X, tile.Y + 1))
                    {
                        DrawLine(rt, tileCorner + tileX + tileY - wallY, tileCorner + tileY - wallY, colWall, wallThickness);
                    }

                    // Left
                    if (!room.HasTile(tile.X - 1, tile.Y))
                    {
                        DrawLine(rt, tileCorner + tileY + wallX, tileCorner + wallX, colWall, wallThickness);
                    }
                }
            }

            // Doorways
            foreach (var door in Ship.Doors)
            {
                foreach (var entrance in door.Entrances)
                {
                    if (!Ship.Rooms.Contains(entrance.RoomID)) continue;
                    var room = Ship.Rooms[entrance.RoomID];
                    var tileCorner = origin + tileX * (room.X + entrance.X) + tileY * (room.Y + entrance.Y);
                    switch (entrance.Direction)
                    {
                        case Door.Direction.Up:
                            DrawLine(rt, tileCorner + tileX * 0.14F + wallY, tileCorner + tileX * (1 - 0.14F) + wallY, colTile, wallThickness);
                            break;
                        case Door.Direction.Down:
                            DrawLine(rt, tileCorner + tileX * 0.14F + tileY - wallY, tileCorner + tileX * (1 - 0.14F) + tileY - wallY, colTile, wallThickness);
                            break;
                        case Door.Direction.Left:
                            DrawLine(rt, tileCorner + tileY * 0.14F + wallX, tileCorner + tileY * (1 - 0.14F) + wallX, colTile, wallThickness);
                            break;
                        case Door.Direction.Right:
                            DrawLine(rt, tileCorner + tileY * 0.14F + tileX - wallX, tileCorner + tileY * (1 - 0.14F) + tileX - wallX, colTile, wallThickness);
                            break;
                    }
                }
            }

            // TODO: oxygen, breaches, fire, etc.
            rt.Display();
            return rt;
        }
    }
}
