using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTLOverdrive.Client.Gamestate
{
    public class Library : IState
    {
        public class Weapon
        {
            public string Name { get; set; }
            public string DisplayName { get; set; }

            public string Graphic { get; set; }
            public string GlowGraphic { get; set; }

            public int PowerCost { get; set; }
        }

        public class ProjectileWeapon : Weapon
        {
            public string ProjectileGraphic { get; set; }
            public bool BypassShield { get; set; }
            public int Damage { get; set; }
            public float HitChance { get; set; }
        }

        public class System
        {
            public string Name { get; set; }
            public string DisplayName { get; set; }

            public string OverlayGraphic { get; set; }
            public List<string> IconGraphics { get; set; }

            public int MinPower { get; set; }
            public int MaxPower { get; set; }

            public bool SubSystem { get; set; }

            public System() { IconGraphics = new List<string>(); }
        }

        public class Race
        {
            public string Name { get; set; }
            public string DisplayName { get; set; }

            public string TilesheetGreen { get; set; }
            public string TilesheetRed { get; set; }
            public string TilesheetYellow { get; set; }
            public string TilesheetSelected { get; set; }

            public int TilesX { get; set; }
            public int TilesY { get; set; }

            public List<string> Names { get; set; }

            public Dictionary<string, Animation> Animations { get; set; }

            public Race() { Animations = new Dictionary<string, Animation>(); Names = new List<string>(); }
        }

        public class Animation
        {
            public int TileStart { get; set; }
            public int TileEnd { get; set; }
            public int Speed { get; set; }
        }

        public class Ship
        {
            public string Name { get; set; }
            public string DisplayName { get; set; }

            public bool Unlocked { get; set; }
            public bool Default { get; set; }

            public string BaseGraphic { get; set; }
            public string CloakedGraphic { get; set; }
            public string ShieldGraphic { get; set; }
            public List<string> GibGraphics { get; set; }

            public List<string> Weapons { get; set; }

            public List<string> Crew { get; set; }

            public List<Room> Rooms { get; set; }

            public List<string> Systems
            {
                get
                {
                    var result = new List<string>();
                    foreach (var room in Rooms)
                        if (room.System != null)
                            result.Add(room.System);
                    return result;
                }
            }

            public Ship()
            {
                GibGraphics = new List<string>();
                Rooms = new List<Room>();
                Crew = new List<string>();
                Weapons = new List<string>();
            }
        }

        public class Room
        {
            public int MinX { get; set; }
            public int MinY { get; set; }
            public int MaxX { get; set; }
            public int MaxY { get; set; }

            public List<Door> Doors { get; set; }

            public string BackgroundGraphic { get; set; }

            public string System { get; set; }

            public Room() { Doors = new List<Door>(); }
        }

        public class Door
        {
            public float X { get; set; }
            public float Y { get; set; }
        }

        private Dictionary<string, Weapon> dctWeapons;
        private Dictionary<string, System> dctSystems;
        private Dictionary<string, Race> dctRaces;
        private Dictionary<string, Ship> dctShips;

        public void OnActivate()
        {
            // Initialise database
            dctWeapons = new Dictionary<string, Weapon>();
            dctSystems = new Dictionary<string, System>();
            dctRaces = new Dictionary<string, Race>();
            dctShips = new Dictionary<string, Ship>();
        }

        public void AddWeapon(string name, Weapon wep)
        {
            wep.Name = name;
            dctWeapons.Add(name, wep);
        }

        public void AddSystem(string name, System sys)
        {
            sys.Name = name;
            dctSystems.Add(name, sys);
        }

        public void AddRace(string name, Race race)
        {
            race.Name = name;
            dctRaces.Add(name, race);
        }

        public void AddShip(string name, Ship ship)
        {
            ship.Name = name;
            dctShips.Add(name, ship);
        }

        public Weapon GetWeapon(string name)
        {
            if (!dctWeapons.ContainsKey(name)) return null;
            return dctWeapons[name];
        }

        public System GetSystem(string name)
        {
            if (!dctSystems.ContainsKey(name)) return null;
            return dctSystems[name];
        }

        public Race GetRace(string name)
        {
            if (!dctRaces.ContainsKey(name)) return null;
            return dctRaces[name];
        }

        public Ship GetShip(string name)
        {
            if (!dctShips.ContainsKey(name)) return null;
            return dctShips[name];
        }

        public List<string> GetShips()
        {
            var result = new List<string>();
            foreach (var pair in dctShips)
                result.Add(pair.Key);
            return result;
        }

        public void OnDeactivate()
        {
            
        }

        public void Think(float delta)
        {
            
        }
    }
}
