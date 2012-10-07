using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SFML.Graphics;
using SFML.Window;

using FTLOverdrive.Client.Gamestate;

using System.ComponentModel;

namespace FTLOverdrive.Client.Ships
{
    public class CrewMember : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        public Library.Race Race { get; private set; }

        public string Name { get; private set; }

        private Texture tsRed, tsYellow, tsGreen, tsSelected;

        private int tileW, tileH;

        private Sprite sprGraphic;

        private Library.Animation currentanim;
        private float animstart;

        private Ships.Room.Tile tile;
        private Ships.Ship ship;

        public enum DrawMode { Red, Yellow, Green, Selected };
        public DrawMode Mode { get; set; }

        public CrewMember(Library.Race race, Ships.Ship ship, string name)
        {
            // Store details
            Race = race;
            Name = name;
            this.ship = ship;

            // Load spritesheets
            tsRed = Root.Singleton.Material(race.TilesheetRed, true);
            tsYellow = Root.Singleton.Material(race.TilesheetYellow, true);
            tsGreen = Root.Singleton.Material(race.TilesheetGreen, true);
            tsSelected = Root.Singleton.Material(race.TilesheetSelected, true);

            // Calculate tile size
            tileW = (int)(tsGreen.Size.X / race.TilesX);
            tileH = (int)(tsGreen.Size.Y / race.TilesY);

            // Create sprite
            sprGraphic = new Sprite();
            sprGraphic.Scale = new Vector2f(1.0f, 1.0f);

            // Default drawmode
            Mode = DrawMode.Yellow;
        }

        public void SetTile(Ships.Room.Tile tile)
        {
            // Check for first set
            if (this.tile == null)
            {
                this.tile = tile;
                return;
            }

            // TODO: Move from one room to another
        }

        public void SetRoom(Ships.Room room)
        {
            SetTile(room.GetTiles().First());
        }

        public void SetAnimation(string name)
        {
            currentanim = Race.Animations[name];
            animstart = Root.Singleton.Time;
        }

        private void UpdateTexture()
        {
            sprGraphic.Texture =
                (Mode == DrawMode.Green) ? tsGreen :
                (Mode == DrawMode.Yellow) ? tsYellow :
                (Mode == DrawMode.Red) ? tsRed :
                tsSelected;
        }

        private IntRect Tile(int i)
        {
            // Extract x and y
            int x = i % Race.TilesX;
            int y = (i - x) / Race.TilesY;

            // Return rect
            return new IntRect(x * tileW, y * tileH, tileW, tileH);
        }

        public void Draw(RenderWindow window)
        {
            // Calculate tile index
            float t = Root.Singleton.Time - animstart;
            int offset = (int)(t * currentanim.Speed) % (currentanim.TileEnd - currentanim.TileStart + 1);
            IntRect rtile = Tile(currentanim.TileStart + offset - 1);

            // Vectors
            var origin = new Vector2f(ship.FloorOffsetX, ship.FloorOffsetY);
            var tileX = new Vector2f(ship.TileWidth, 0.0f);
            var tileY = new Vector2f(0.0f, ship.TileHeight);
            var room = tile.Room;
            var roomCorner = origin + tileX * room.X + tileY * room.Y;

            // Calculate position
            Vector2f pos = roomCorner + (tile.X * tileX) + (tile.Y * tileY);

            // Draw
            sprGraphic.Position = pos;
            sprGraphic.TextureRect = rtile;
            UpdateTexture();
            window.Draw(sprGraphic);
        }


    }
}
