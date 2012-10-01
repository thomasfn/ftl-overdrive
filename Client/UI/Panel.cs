using System;
using System.Collections.Generic;

using SFML.Graphics;

namespace FTLOverdrive.Client.UI
{
    public class Panel : Control
    {
        public Color Colour { get; set; }

        private RectangleShape shpBackground;

        public override void Init()
        {
            shpBackground = new RectangleShape();
            shpBackground.FillColor = Colour;
            base.Init();
        }

        protected override void UpdateLayout()
        {
            base.UpdateLayout();
            shpBackground.Position = new SFML.Window.Vector2f(AbsX, AbsY);
            shpBackground.Size = new SFML.Window.Vector2f(Width, Height);
        }

        protected override void Draw(RenderWindow window)
        {
            base.Draw(window);
            window.Draw(shpBackground);
        }

        public override void Remove()
        {
            base.Remove();
            if (shpBackground != null)
            {
                shpBackground.Dispose();
                shpBackground = null;
            }
        }

    }
}
