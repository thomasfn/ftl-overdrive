using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace FTLOverdrive.Client.Map
{
    public class Sector
    {
        public string Name { get; set; }
        public Color Color { get; set; }

        public string Background { get; set; }

        public List<Beacon> Beacons { get; set; }

        public Sector(string name, Color color)
        {
            Name = name;
            Color = color;
            Background = "img/map/zone_1.png";
            Beacons = new List<Beacon>();
        }
    }
}
