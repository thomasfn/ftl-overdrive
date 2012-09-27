using System;
using System.Collections.Generic;

using SFML.Graphics;
using SFML.Audio;

namespace FTLOverdrive.Client.UI
{

    public class ImageButton : Control
    {
        public delegate void Click(ImageButton sender);

        public event Click OnClick;

        public Texture Image { get; set; }
        public Texture HoveredImage { get; set; }
        public Texture DepressedImage { get; set; }
        public Texture DisabledImage { get; set; }

        public bool FlipH { get; set; }

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

        private Sprite sprButton;

        private bool enabled;
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                if (sprButton == null) return;
                if ((!Enabled) && (DisabledImage != null))
                    sprButton.Texture = DisabledImage;
                else
                    sprButton.Texture = Image;
                sprButton.Scale = Util.Scale(sprButton, new SFML.Window.Vector2f(Width, Height));
            }
        }

        public override void Init()
        {
            sprButton = new Sprite(Image);
            UpdateImage();
            base.Init();
            
        }

        protected override void UpdateLayout()
        {
            base.UpdateLayout();
            if (FlipH)
            {
                sprButton.Position = new SFML.Window.Vector2f(AbsX + Width, AbsY);
                sprButton.Scale = Util.Scale(sprButton, new SFML.Window.Vector2f(-Width, Height));
            }
            else
            {
                sprButton.Position = new SFML.Window.Vector2f(AbsX, AbsY);
                sprButton.Scale = Util.Scale(sprButton, new SFML.Window.Vector2f(Width, Height));
            }
        }

        public override void SetHovered(bool hovered)
        {
            base.SetHovered(hovered);
            UpdateImage();
            if (hovered && (sndHover != null) && Enabled)
            {
                sndHover.Stop();
                sndHover.Play();
            }
        }

        public override void SetPressed(bool pressed, bool mousemoveevent)
        {
            base.SetPressed(pressed, mousemoveevent);
            UpdateImage();
            if ((!pressed) && (!mousemoveevent) && (OnClick != null)) OnClick(this);
        }

        public virtual void UpdateImage()
        {
            if (Enabled)
            {
                if (Pressed && (DepressedImage != null))
                    sprButton.Texture = DepressedImage;
                else if (Hovered && (HoveredImage != null))
                    sprButton.Texture = HoveredImage;
                else
                    sprButton.Texture = Image;
            }
            else
            {
                if (DisabledImage != null)
                    sprButton.Texture = DisabledImage;
                else
                    sprButton.Texture = Image;
            }
            UpdateLayout();
        }

        protected override void Draw(RenderWindow window)
        {
            base.Draw(window);
            window.Draw(sprButton);
        }

        public override void Remove()
        {
            base.Remove();
            if (sndHover != null)
            {
                sndHover.Dispose();
                sndHover = null;
            }
            if (sprButton != null)
            {
                sprButton.Dispose();
                sprButton = null;
            }
        }

    }
}
