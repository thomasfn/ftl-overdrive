using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace FTLOverdrive.Client.Ships
{
    public class Door
    {
        public DoorEntrance[] Entrances { get; private set; }

        protected Ship Ship { get; private set; }

        private bool open;
        public bool Open
        {
            get
            {
                return open;
            }
            set
            {
                open = value;
                Ship.DoShipModified();
            }
        }

        public Door(Ship ship, params DoorEntrance[] entrances)
        {
            Ship = ship;
            Entrances = entrances;
            Ship.DoShipModified();
        }

        public Door(Ship ship, int room1, int x1, int y1, Direction dir1, int room2, int x2, int y2, Direction dir2)
        {
            Ship = ship;
            Entrances = new DoorEntrance[2];
            Entrances[0] = new DoorEntrance(room1, x1, y1, dir1);
            Entrances[1] = new DoorEntrance(room2, x2, y2, dir2);
            Ship.DoShipModified();
        }

        public class DoorEntrance
        {
            public readonly int RoomID;
            public readonly int X;
            public readonly int Y;
            public readonly Direction Direction;

            public DoorEntrance(int roomID, int x, int y, Direction direction)
            {
                RoomID = roomID;
                X = x;
                Y = y;
                Direction = direction;
            }

            public DoorEntrance(int roomID, int x, int y, string direction)
            {
                RoomID = roomID;
                X = x;
                Y = y;
                Direction = DoorEntrance.DirectionFromString(direction);
            }

            public DoorEntrance(Room room, int x, int y, Direction direction) : this(room.ID, x, y, direction) { }

            public DoorEntrance(Room room, int x, int y, string direction) : this(room.ID, x, y, direction) { }

            public static Direction DirectionFromString(String s)
            {
                if (s.ToLower() == Direction.Up.ToString().ToLower()) return Direction.Up;
                if (s.ToLower() == Direction.Down.ToString().ToLower()) return Direction.Down;
                if (s.ToLower() == Direction.Left.ToString().ToLower()) return Direction.Left;
                if (s.ToLower() == Direction.Right.ToString().ToLower()) return Direction.Right;
                throw new ArgumentException(s + " is not a valid direction!");
            }
        }

        public enum Direction { Up, Down, Left, Right }
    }
}
