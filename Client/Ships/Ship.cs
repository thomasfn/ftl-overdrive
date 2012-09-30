using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SFML.Graphics;

namespace FTLOverdrive.Client.Ships
{
    public class Ship
    {
        public delegate void ShipMidifiedHandler(Ship sender);
        public event ShipMidifiedHandler ShipMidified;

        public string Name { get; set; }

        #region Textures

        private string baseGraphic;
        public string BaseGraphic
        {
            get
            {
                return baseGraphic;
            }
            set
            {
                baseGraphic = value;
                OnShipModified();
            }
        }
        private string cloackedGraphic;
        public string CloakedGraphic
        {
            get
            {
                return cloackedGraphic;
            }
            set
            {
                cloackedGraphic = value;
                OnShipModified();
            }
        }
        private string shildGraphic;
        public string ShieldGraphic
        {
            get
            {
                return shildGraphic;
            }
            set
            {
                shildGraphic = value;
                OnShipModified();
            }
        }
        private string floorGraphic;
        public string FloorGraphic
        {
            get
            {
                return floorGraphic;
            }
            set
            {
                floorGraphic = value;
                OnShipModified();
            }
        }
        private List<string> gibGraphics;
        public List<string> GibGraphics
        {
            get
            {
                return gibGraphics;
            }
            set
            {
                gibGraphics = value;
                OnShipModified();
            }
        }

        #endregion

        public void OnShipModified()
        {
            if (ShipMidified != null) { ShipMidified(this); }
        }

        public Dictionary<int, Room> Rooms { get; set; }

        //public List<string> Weapons { get; set; }

        //public List<string> Crew { get; set; }

        public float TileHeight { get; set; }

        public float TileWidth { get; set; }

        public float FloorOffsetX { get; set; }
        public float FloorOffsetY { get; set; }

        public Ship()
        {
            GibGraphics = new List<string>();
            Rooms = new Dictionary<int, Room>();
            //Crew = new List<string>();
            //Weapons = new List<string>();
        }

        public T AddRoom<T>(T room) where T : Room
        {
            if (Rooms.ContainsKey(room.ID))
            {
                throw new Exception("Room " + room.ID + " already exists in ship " + ToString());
            }
            Rooms[room.ID] = room;
            return room;
        }

        public RectRoom AddRectRoom(int x, int y, int w, int h)
        {
            return AddRectRoom(Rooms.Count, x, y, w, h);
        }

        public RectRoom AddRectRoom(int id, int x, int y, int w, int h)
        {
            return AddRoom(new RectRoom(this, id, x, y, w, h));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
