using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FTLOverdrive.Client.Ships;

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

            public Dictionary<string, string> IconGraphics { get; set; }

            public int Order { get; set; }

            public int MinPower { get; set; }
            public int MaxPower { get; set; }

            public bool SubSystem { get; set; }

            public System() { IconGraphics = new Dictionary<string, string>(); }
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

        public interface ShipGenerator
        {
            string Name { get; set; }
            string DisplayName { get; set; }

            bool Unlocked { get; set; }
            bool Default { get; set; }

            // whether it's a player ship or NPC ship
            bool NPC { get; set; }

            string MiniGraphic { get; set; }

            Ship Generate(params object[] args);
        }

        private Dictionary<string, Weapon> dctWeapons;
        private Dictionary<string, System> dctSystems;
        private Dictionary<string, Race> dctRaces;
        private Dictionary<string, ShipGenerator> dctShipGenerators;

        public void OnActivate()
        {
            // Initialise database
            dctWeapons = new Dictionary<string, Weapon>();
            dctSystems = new Dictionary<string, System>();
            dctRaces = new Dictionary<string, Race>();
            dctShipGenerators = new Dictionary<string, ShipGenerator>();
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

        public void AddShipGenerator(string name, ShipGenerator gen)
        {
            gen.Name = name;
            dctShipGenerators.Add(name, gen);
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

        public ShipGenerator GetShipGenerator(string name)
        {
            if (!dctShipGenerators.ContainsKey(name)) return null;
            return dctShipGenerators[name];
        }

        public ICollection<ShipGenerator> GetShipGenerators()
        {
            return dctShipGenerators.Values;
        }

        public List<ShipGenerator> GetNPCShipGenerators()
        {
            var res = new List<ShipGenerator>();
            foreach (var gen in dctShipGenerators.Values)
            {
                if (gen.NPC)
                {
                    res.Add(gen);
                }
            }
            return res;
        }

        public List<ShipGenerator> GetPlayerShipGenerators()
        {
            var res = new List<ShipGenerator>();
            foreach (var gen in dctShipGenerators.Values)
            {
                if (!gen.NPC)
                {
                    res.Add(gen);
                }
            }
            return res;
        }

        public void OnDeactivate()
        {
            
        }

        public void Think(float delta)
        {
            
        }
    }
}
