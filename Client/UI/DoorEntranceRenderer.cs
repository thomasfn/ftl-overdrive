using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTLOverdrive.Client.Ships;
using SFML.Graphics;
using SFML.Window;

namespace FTLOverdrive.Client.UI
{
    // TODO animatons; a proper way to choose door type; hiding the wall under open door
    public class DoorEntranceRenderer : Control
    {
        private ShipRenderer shipRenderer;
        private Door door;
        private Door.DoorEntrance entrance;

        private Sprite sprHighlight;
        private Sprite sprDoor;

        private int type = 0;

        public DoorEntranceRenderer(ShipRenderer shipRenderer, Door door, Door.DoorEntrance doorEntrance)
        {
            this.shipRenderer = shipRenderer;
            this.door = door;
            this.entrance = doorEntrance;
        }

        public override void Init()
        {
            sprHighlight = new Sprite(Root.Singleton.Material("img/effects/door_highlight.png"));
            sprDoor = new Sprite(Root.Singleton.Material("img/effects/door_sheet.png", false));

            Ship s = shipRenderer.Ship;
            float x = s.FloorOffsetX + s.TileWidth * (s.Rooms[entrance.RoomID].X + entrance.X);
            float y = s.FloorOffsetY + s.TileHeight * (s.Rooms[entrance.RoomID].Y + entrance.Y);

            switch (entrance.Direction)
            {
                case Door.Direction.Left:
                    y += (int)(s.TileHeight / 2);
                    sprHighlight.Rotation = 0;
                    sprDoor.Rotation = 0;
                    break;
                case Door.Direction.Up:
                    x += (int)(s.TileWidth / 2);
                    sprHighlight.Rotation = 90;
                    sprDoor.Rotation = 90;
                    break;
                case Door.Direction.Right:
                    x += (int)(s.TileWidth);
                    y += (int)(s.TileHeight / 2);
                    sprHighlight.Rotation = 180;
                    sprDoor.Rotation = 180;
                    break;
                case Door.Direction.Down:
                    x += (int)(s.TileWidth / 2);
                    y += (int)(s.TileHeight);
                    sprHighlight.Rotation = 270;
                    sprDoor.Rotation = 270;
                    break;
            }

            x *= shipRenderer.getScale().X;
            y *= shipRenderer.getScale().Y;

            Width = (int)(30 * shipRenderer.getScale().X);
            Height = (int)(30 * shipRenderer.getScale().Y);

            X = (int)x - Width / 2;
            Y = (int)y - Height / 2;

            base.Init();
        }

        protected override void UpdateLayout()
        {
            base.UpdateLayout();
            sprHighlight.Color = new Color(255, 255, 255, 128);
            sprHighlight.Origin = new Vector2f(sprHighlight.TextureRect.Width / 2, sprHighlight.TextureRect.Height / 2);
            sprHighlight.Position = new SFML.Window.Vector2f(AbsX + (Width / 2), AbsY + (Height / 2));
            sprHighlight.Scale = shipRenderer.getScale();

            sprDoor.TextureRect = new IntRect(35 * (door.Open ? 4 : 0),  type * 35, 35, 35);
            sprDoor.Origin = new Vector2f(sprDoor.TextureRect.Width / 2, sprDoor.TextureRect.Height / 2);
            sprDoor.Position = new SFML.Window.Vector2f(AbsX + (Width / 2), AbsY + (Height / 2));
            sprDoor.Scale = shipRenderer.getScale();
        }

        public override void SetPressed(bool pressed, bool mousemoveevent)
        {
            base.SetPressed(pressed, mousemoveevent);
            UpdateLayout();
            if ((!pressed) && (!mousemoveevent))
                door.Open = !door.Open;
        }

        protected override void Draw(RenderWindow window)
        {
            if (Hovered)
            {
                window.Draw(sprHighlight);
            }
            window.Draw(sprDoor);
        }

        public override void Remove()
        {
            base.Remove();
            if (sprDoor != null)
            {
                sprDoor.Dispose();
                sprDoor = null;
            }
            if (sprHighlight != null)
            {
                sprHighlight.Dispose();
                sprHighlight = null;
            }
        }
    }
}
