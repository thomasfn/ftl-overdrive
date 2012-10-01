using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SFML.Graphics;
using FTLOverdrive.Client.Gamestate;

namespace FTLOverdrive.Client.Ships
{
    public class Ship
    {
        public delegate void ShipModifiedHandler(Ship sender);
        public event ShipModifiedHandler ShipModified;

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
                DoShipModified();
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
                DoShipModified();
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
                DoShipModified();
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
                DoShipModified();
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
                DoShipModified();
            }
        }

        #endregion

        public void DoShipModified()
        {
            if (ShipModified != null) { ShipModified(this); }
        }

        public Dictionary<int, Room> Rooms { get; set; }
        public List<Door> Doors { get; set; }

        //public List<string> Weapons { get; set; }

        //public List<string> Crew { get; set; }

        public float TileHeight { get; set; }

        public float TileWidth { get; set; }

        public float FloorOffsetX { get; set; }
        public float FloorOffsetY { get; set; }

        public List<Library.System> Systems
        {
            get
            {
                var res = new List<Library.System>();
                foreach (var room in Rooms.Values)
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
            Rooms = new Dictionary<int, Room>();
            Doors = new List<Door>();
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
            DoShipModified();
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
