using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SFML.Graphics;

using FTLOverdrive.Client.Ships;
using SFML.Window;
using FTLOverdrive.Client.Gamestate;

namespace FTLOverdrive.Client.UI
{
    public class ShipRenderer : Control
    {
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

        public override void Init()
        {
            sprShip = new Sprite();
            base.Init();
        }

        private void onShipModified(Ship sender)
        {
            UpdateLayout();
        }

        protected override void UpdateLayout()
        {
            if (Ship != null && sprShip != null)
            {
                sprShip = new Sprite(GetRenderTexture().Texture);
                sprShip.Texture.Smooth = false;
                sprShip.Position = new SFML.Window.Vector2f(AbsX, AbsY);
                sprShip.Scale = new Vector2f(1, 1) * Height / sprShip.Texture.Size.Y;
            }
            base.UpdateLayout();
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
            Texture doorGraphic = Root.Singleton.Material("img/door_placeholder.png", false);
            // TODO use proper door textures (animations, different types, etc.)

            var rtWidth = baseGraphic.Size.X;
            var rtHeight = baseGraphic.Size.Y;
            RenderTexture rt = new RenderTexture(rtWidth, rtHeight);
            rt.Clear(new Color(0, 0, 0, 0));

            // Draw the ship and floor
            DrawTexture(rt, new Vector2f(0.0f, 0.0f), new Vector2f(rtWidth, rtHeight), baseGraphic);
            if (!ShowRooms) return rt;
            DrawTexture(rt, new Vector2f(0.0f, 0.0f), new Vector2f(rtWidth, rtHeight), floorGraphic);

            // Draw rooms
            foreach (var room in Ship.Rooms.Values)
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
            foreach (var room in Ship.Rooms.Values)
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

            // Draw doors
            foreach (var door in Ship.Doors)
            {
                foreach (var entrance in door.Entrances)
                {
                    if (!Ship.Rooms.ContainsKey(entrance.RoomID)) continue;

                    var room = Ship.Rooms[entrance.RoomID];
                    // Not sure why, but it looks better when I add new Vector2f(0.5F, 0.5F)
                    var tileCorner = origin + tileX * (room.X + entrance.X) + tileY * (room.Y + entrance.Y) + new Vector2f(0.5F, 0.5F);

                    switch (entrance.Direction)
                    {
                        case Door.Direction.Up:
                            DrawTexture(rt, tileCorner - tileY / 2, tileCorner + tileX + tileY / 2, doorGraphic, 90);
                            break;
                        case Door.Direction.Down:
                            DrawTexture(rt, tileCorner + tileY / 2, tileCorner + tileX + tileY * 3 / 2, doorGraphic, 90);
                            break;
                        case Door.Direction.Left:
                            DrawTexture(rt, tileCorner - tileX / 2, tileCorner + tileX / 2 + tileY, doorGraphic);
                            break;
                        case Door.Direction.Right:
                            DrawTexture(rt, tileCorner + tileX / 2, tileCorner + tileX * 3 / 2 + tileY, doorGraphic);
                            break;
                    }
                }
            }

            // TODO: system icons, oxygen, breaches, fire, etc.
            rt.Display();
            return rt;
        }
    }
}
