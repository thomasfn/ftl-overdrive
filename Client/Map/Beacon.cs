using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTLOverdrive.Client.Map
{
    public class Beacon
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool Visited { get; set; }

        // Store, exit, distress, etc.
        public string Tag { get; set; }

        public virtual string Icon
        {
            get
            {
                return Visited ? "img/map/map_icon_diamond_blue.png" : "img/map/map_icon_diamond_yellow.png";
            }
        }

        public virtual string IconShadow { get; set; }

        public Beacon(int x, int y)
        {
            X = x;
            Y = y;
            Visited = false;
            IconShadow = "img/map/map_icon_diamond_shadow.png";
        }
    }
}
