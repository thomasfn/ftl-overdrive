using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using System.ComponentModel;

namespace FTLOverdrive.Client.Ships
{
    public class Door : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        public readonly DoorEntrance[] Entrances;

        private bool open;
        public bool Open
        {
            get { return open; }
            set { open = value; NotifyPropertyChanged("Open"); }
        }

        public Door(params DoorEntrance[] entrances)
        {
            Entrances = entrances;
        }

        public Door(int room1, int x1, int y1, Direction dir1, int room2, int x2, int y2, Direction dir2)
        {
            Entrances = new DoorEntrance[2];
            Entrances[0] = new DoorEntrance(room1, x1, y1, dir1);
            Entrances[1] = new DoorEntrance(room2, x2, y2, dir2);
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
                Direction = (Direction)Enum.Parse(typeof(Direction), direction);
            }

            public DoorEntrance(Room room, int x, int y, Direction direction) : this(room.ID, x, y, direction) { }

            public DoorEntrance(Room room, int x, int y, string direction) : this(room.ID, x, y, direction) { }
        }

        public enum Direction { Up, Down, Left, Right }
    }
}
