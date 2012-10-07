using System;
using System.Collections.Generic;

using FTLOverdrive.Client.Gamestate;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace FTLOverdrive.Client.Ships
{
    public class Ship
    {
        public class ShipModifiedEventArgs : EventArgs
        {
            public enum ShipModifiedAction { Rooms, Doors, Reset, Crew }
            public ShipModifiedAction Action { get; set; }
            public NotifyCollectionChangedEventArgs CollectionEventArgs { get; set; }

            public ShipModifiedEventArgs(ShipModifiedAction action = ShipModifiedAction.Reset, NotifyCollectionChangedEventArgs e = null)
            {
                Action = action;
                CollectionEventArgs = e;
            }
        }

        public event EventHandler<ShipModifiedEventArgs> ShipModified;
        public void DoShipModified()
        {
            if (ShipModified != null) ShipModified(this, new ShipModifiedEventArgs());
        }
        public void DoShipModified(ShipModifiedEventArgs e)
        {
            if (ShipModified != null) ShipModified(this, e);
        }

        public string Name { get; set; }

        #region Textures

        private string baseGraphic;
        public string BaseGraphic
        {
            get { return baseGraphic; }
            set { baseGraphic = value; DoShipModified(); }
        }
        private string cloackedGraphic;
        public string CloakedGraphic
        {
            get { return cloackedGraphic; }
            set { cloackedGraphic = value; DoShipModified(); }
        }
        private string shildGraphic;
        public string ShieldGraphic
        {
            get { return shildGraphic; }
            set { shildGraphic = value; DoShipModified(); }
        }
        private string floorGraphic;
        public string FloorGraphic
        {
            get { return floorGraphic; }
            set { floorGraphic = value; DoShipModified(); }
        }
        private List<string> gibGraphics;
        public List<string> GibGraphics
        {
            get { return gibGraphics; }
            set { gibGraphics = value; DoShipModified(); }
        }

        #endregion

        public ObservableKeyedCollection<int, Room> Rooms { get; set; }
        public ObservableCollectionEx<Door> Doors { get; set; }
        public ObservableCollectionEx<CrewMember> Crew { get; set; }

        //public List<string> Weapons { get; set; }

        //public List<string> Crew { get; set; }

        private float tileHeight;
        public float TileHeight
        {
            get { return tileHeight; }
            set { tileHeight = value; DoShipModified(); }
        }

        private float tileWidth;
        public float TileWidth
        {
            get { return tileWidth; }
            set { tileWidth = value; DoShipModified(); }
        }

        public float FloorOffsetX { get; set; }
        public float FloorOffsetY { get; set; }

        public List<Library.System> Systems
        {
            get
            {
                var res = new List<Library.System>();
                foreach (var room in Rooms)
                {
                    if (room.System == "") continue;

                    var system = Root.Singleton.mgrState.Get<Library>().GetSystem(room.System);
                    if (system != null)
                    {
                        res.Add(system);
                    }
                    else
                    {
                        Root.Singleton.Log("Invalid system '" + room.System + "'");
                    }
                }
                res.Sort((a, b) =>
                {
                    if (a.Order < b.Order)
                        return -1;
                    else if (a.Order == b.Order)
                        return 0;
                    else
                        return 1;
                });
                return res;
            }
        }

        public Ship()
        {
            GibGraphics = new List<string>();
            Rooms = new ObservableKeyedCollection<int, Room>(r => r.ID);
            Rooms.CollectionChanged += (sender, e) =>
                DoShipModified(new ShipModifiedEventArgs(ShipModifiedEventArgs.ShipModifiedAction.Rooms, e));
            Doors = new ObservableCollectionEx<Door>();
            Doors.CollectionChanged += (sender, e) =>
                DoShipModified(new ShipModifiedEventArgs(ShipModifiedEventArgs.ShipModifiedAction.Doors, e));
            Crew = new ObservableCollectionEx<CrewMember>();
            Crew.CollectionChanged += (sender, e) =>
                DoShipModified(new ShipModifiedEventArgs(ShipModifiedEventArgs.ShipModifiedAction.Crew, e));
            //Weapons = new List<string>();
        }

        private void onDoorsChanged(Object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        private void onRoomsChanged(Object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        public T AddRoom<T>(T room) where T : Room
        {
            if (Rooms.Contains(room.ID))
            {
                throw new Exception("" + room + " already exists in ship " + ToString());
            }
            Rooms.Add(room);
            return room;
        }

        public RectRoom AddRectRoom(float x, float y, int w, int h)
        {
            return AddRectRoom(Rooms.Count, x, y, w, h);
        }

        public RectRoom AddRectRoom(int id, float x, float y, int w, int h)
        {
            return AddRoom(new RectRoom(id, x, y, w, h));
        }

        public CrewMember AddCrewMember(Library.Race race, string name)
        {
            var member = new CrewMember(race, this, name);
            Crew.Add(member);
            return member;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
