using System;
using System.Collections.Generic;

using SFML.Graphics;

namespace FTLOverdrive.Client.UI
{
    public class ImageToggleButton : ImageButton
    {
        public Texture ToggledImage { get; set; }

        public bool Toggled { get; set; }

        public override void UpdateImage()
        {
            if (Toggled)
            {
                var old = Image;
                Image = ToggledImage;
                base.UpdateImage();
                Image = old;
            }
            else
                base.UpdateImage();
        }

        protected override void DoClick()
        {
            if (!Enabled) return;
            Toggled = !Toggled;
            base.DoClick();
        }

    }
}
