using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SFML.Graphics;

namespace FTLOverdrive.Client.Ships
{
    public abstract class Room
    {
        public int ID { get; private set; }
        protected Ship Ship { get; private set; }

        public float X { get; private set; }
        public float Y { get; private set; }

        public abstract IEnumerable<Tile> GetTiles();
        public abstract IntRect GetBoundingBox();

        private string system = "";
        public string System
        {
            get
            {
                return system;
            }
            set
            {
                system = value;
                Ship.DoShipModified();
            }
        }

        public Room SetSystem(string sys)
        {
            System = sys;
            return this;
        }

        private string backgroundGraphic;
        public string BackgroundGraphic
        {
            get
            {
                return backgroundGraphic;
            }
            set
            {
                backgroundGraphic = value;
                Ship.DoShipModified();
            }
        }

        public Room SetBackgroundGraphic(string bgGraphic)
        {
            BackgroundGraphic = bgGraphic;
            return this;
        }

        public Room(Ship ship, int id, float x = 0, float y = 0)
        {
            Ship = ship;
            ID = id;
            X = x;
            Y = y;
            Ship.DoShipModified();
        }

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
                get
                {
                    return oxygen;
                }
                set
                {
                    oxygen = value;
                    Room.Ship.DoShipModified();
                }
            }

            // 0 = no breach, 100 = max breach
            private int breach;
            public int Breach
            {
                get
                {
                    return breach;
                }
                set
                {
                    breach = value;
                    Room.Ship.DoShipModified();
                }
            }

            private bool fire;
            public bool Fire
            {
                get
                {
                    return fire;
                }
                set
                {
                    fire = value;
                    Room.Ship.DoShipModified();
                }
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
