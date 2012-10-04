using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SFML.Graphics;
using System.ComponentModel;

namespace FTLOverdrive.Client.Ships
{
    public abstract class Room : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        public int ID { get; private set; }

        private float x;
        public float X
        {
            get { return x; }
            set { x = value; NotifyPropertyChanged("X"); }
        }
        
        private float y;
        public float Y
        {
            get { return y; }
            set { y = value; NotifyPropertyChanged("Y"); }
        }

        private string system = "";
        public string System
        {
            get { return system; }
            set { system = value; NotifyPropertyChanged("System"); }
        }
        public Room SetSystem(string sys)
        {
            System = sys;
            return this;
        }

        private string backgroundGraphic;
        public string BackgroundGraphic
        {
            get { return backgroundGraphic; }
            set { backgroundGraphic = value; NotifyPropertyChanged("BackgroundGraphic"); }
        }
        public Room SetBackgroundGraphic(string bgGraphic)
        {
            BackgroundGraphic = bgGraphic;
            return this;
        }

        public Room(int id, float x = 0, float y = 0)
        {
            ID = id;
            X = x;
            Y = y;
        }

        public abstract IEnumerable<Tile> GetTiles();

        public virtual Tile GetTile(int x, int y)
        {
            foreach (Tile t in GetTiles())
            {
                if (t.X == x && t.Y == y)
                {
                    return t;
                }
            }
            return null;
        }

        public virtual bool HasTile(int x, int y)
        {
            return (GetTile(x, y) != null);
        }

        public virtual IntRect GetBoundingBox()
        {
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;
            foreach (var tile in GetTiles())
            {
                minX = Math.Min(minX, tile.X);
                minY = Math.Min(minY, tile.Y);
                maxX = Math.Max(maxX, tile.X);
                maxY = Math.Max(maxY, tile.Y);
            }
            if (minX == int.MaxValue) throw new Exception("No tiles in " + ToString());
            return new IntRect(minX, minY, maxX - minX, maxY - minY);
        }

        public override string ToString()
        {
            return "Room #" + ID;
        }

        public class Tile
        {
            public Room Room { get; private set; }

            // Relative to room
            public int X { get; private set; }
            public int Y { get; private set; }

            // 0 = empty, 100 = full
            private int oxygen;
            public int Oxygen
            {
                get { return oxygen; }
                set { oxygen = value; Room.NotifyPropertyChanged("Tile.Oxygen"); }
            }

            // 0 = no breach, 100 = max breach
            private int breach;
            public int Breach
            {
                get { return breach; }
                set { breach = value; Room.NotifyPropertyChanged("Tile.Breach"); }
            }

            private bool fire;
            public bool Fire
            {
                get { return fire; }
                set { fire = value; Room.NotifyPropertyChanged("Tile.Fire"); }
            }

            public Tile(Room room, int x, int y)
            {
                Room = room;
                X = x;
                Y = y;
                Fire = false;
                Oxygen = 100;
                Breach = 0;
            }
        }
    }
}
