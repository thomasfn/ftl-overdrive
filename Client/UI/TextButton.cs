using System;
using System.Collections.Generic;

using SFML.Graphics;
using SFML.Audio;


namespace FTLOverdrive.Client.UI
{
    public class TextButton : Control
    {
        public delegate void Click(TextButton sender);

        public event Click OnClick;

        public Color Colour { get; set; }
        public Color HoveredColour { get; set; }
        public Color DepressedColour { get; set; }
        public Color DisabledColour { get; set; }

        public Font Font { get; set; }

        public float Scale { get; set; }

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

        private SoundBuffer snd;
        private Sound sndHover;
        public SoundBuffer HoverSound
        {
            get { return snd; }
            set
            {
                snd = value;
                if (sndHover != null) sndHover.Dispose();
                sndHover = new Sound(snd);
            }
        }

        private Text label;

        private bool enabled;
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                if (label == null) return;
                UpdateColour();
            }
        }

        public TextButton()
        {
            Colour = Color.White;
            HoveredColour = Color.Yellow;
            DisabledColour = new Color(128, 128, 128);
            DepressedColour = Color.Yellow;
            Text = "";
            Font = Root.Singleton.Font("fonts/JustinFont11.ttf");
            Enabled = true;
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
                UpdateColour();
                label.Position = new SFML.Window.Vector2f(AbsX, AbsY);
            }
        }

        private void UpdateColour()
        {
            if (!Enabled)
                label.Color = DisabledColour;
            else if (Pressed)
                label.Color = DepressedColour;
            else if (Hovered)
                label.Color = HoveredColour;
            else
                label.Color = Colour;
        }

        public override void SetHovered(bool hovered)
        {
            base.SetHovered(hovered);
            if (label != null)
                UpdateColour();
            if (hovered && (sndHover != null) && Enabled)
            {
                sndHover.Stop();
                sndHover.Play();
            }
        }

        public override void SetPressed(bool pressed, bool mousemoveevent)
        {
            base.SetPressed(pressed, mousemoveevent);
            if (label != null)
                UpdateColour();
            if (Enabled && (!pressed) && (!mousemoveevent)) DoClick();
        }

        protected virtual void DoClick()
        {
            if (OnClick != null) OnClick(this);
        }

        protected override void Draw(RenderWindow window)
        {
            window.Draw(label);
        }
    }
}
