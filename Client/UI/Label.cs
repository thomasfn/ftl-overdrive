using System;
using System.Collections.Generic;

using SFML.Graphics;

namespace FTLOverdrive.Client.UI
{
    public class Label : Control
    {
        public Color Colour { get; set; }

        public Font Font { get; set; }

        public float Scale { get; set; }

        private Text label;

        

        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                if (label != null)
                {
                    label.DisplayedString = value;
                    var bounds = label.GetLocalBounds();
                    Height = (int)bounds.Height;
                    Width = (int)bounds.Width;
                }
            }
        }

        public override void Init()
        {
            label = new Text(Text, Font);
            label.Scale = new SFML.Window.Vector2f(Scale, Scale);
            var bounds = label.GetLocalBounds();
            Height = (int)bounds.Height;
            Width = (int)bounds.Width;
            base.Init();
        }

        protected override void UpdateLayout()
        {
            base.UpdateLayout();
            if (label != null)
            {
                label.Position = new SFML.Window.Vector2f(AbsX, AbsY);
            }
        }

        protected override void Draw(RenderWindow window)
        {
            window.Draw(label);
        }

    }
}
