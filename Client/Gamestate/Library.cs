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

            public Dictionary<string, Animation> Animations { get; set; }

            public Race() { Animations = new Dictionary<string, Animation>(); }
        }

        public class Animation
        {
            public int TileStart { get; set; }
            public int TileEnd { get; set; }
            public int Speed { get; set; }
        }

        private Dictionary<string, Weapon> dctWeapons;
        private Dictionary<string, System> dctSystems;
        private Dictionary<string, Race> dctRaces;

        public void OnActivate()
        {
            // Initialise database
            dctWeapons = new Dictionary<string, Weapon>();
            dctSystems = new Dictionary<string, System>();
            dctRaces = new Dictionary<string, Race>();
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

        public void OnDeactivate()
        {
            
        }

        public void Think(float delta)
        {
            
        }
    }
}
