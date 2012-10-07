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

        public bool Centered { get; set; }
        public bool AutoScale { get; set; }

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
                    UpdateScale();
                    UpdateLayout();
                }
            }
        }

        public Label()
        {
            AutoScale = true;
            Scale = 1.0f;
            Colour = Color.White;
            Text = "";
            Font = Root.Singleton.Font("fonts/JustinFont11.ttf");
        }

        public override void Init()
        {
            label = new Text(Text, Font);
            label.Scale = new SFML.Window.Vector2f(Scale, Scale);
            UpdateScale();
            base.Init();
        }

        private void UpdateScale()
        {
            if (!AutoScale) return;
            var bounds = label.GetLocalBounds();
            Height = (int)bounds.Height;
            Width = (int)bounds.Width;
        }

        protected override void UpdateLayout()
        {
            base.UpdateLayout();
            if (label != null)
            {
                if (Centered)
                {
                    var bounds = label.GetLocalBounds();
                    label.Position = new SFML.Window.Vector2f(AbsX + (Width * 0.5f) - (bounds.Width * 0.5f), AbsY + (Height * 0.5f) - (bounds.Height * 0.5f));
                }
                else
                    label.Position = new SFML.Window.Vector2f(AbsX, AbsY);
            }
        }

        protected override void Draw(RenderWindow window)
        {
            window.Draw(label);
        }

    }
}
