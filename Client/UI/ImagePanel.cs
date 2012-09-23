using System;
using System.Collections.Generic;

using SFML.Graphics;

namespace FTLOverdrive.Client.UI
{
    public class ImagePanel : Control
    {
        public Texture Image { get; set; }

        private Sprite sprBackground;

        public override void Init()
        {
            sprBackground = new Sprite(Image);
            base.Init();
        }

        protected override void UpdateLayout()
        {
            base.UpdateLayout();
            sprBackground.Position = new SFML.Window.Vector2f(AbsX, AbsY);
            sprBackground.Scale = Util.Scale(sprBackground, new SFML.Window.Vector2f(Width, Height));
        }

        protected override void Draw(RenderWindow window)
        {
            base.Draw(window);
            window.Draw(sprBackground);
        }

        public override void Remove()
        {
            base.Remove();
            if (sprBackground != null)
            {
                sprBackground.Dispose();
                sprBackground = null;
            }
        }

    }
}
