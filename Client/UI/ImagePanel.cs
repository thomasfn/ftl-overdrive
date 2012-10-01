using System;
using System.Collections.Generic;

using SFML.Graphics;

namespace FTLOverdrive.Client.UI
{
    public class ImagePanel : Control
    {
        public Texture Image { get; set; }

        private Sprite sprBackground;

        public float ImageScale { get; set; }

        /*protected override bool RespondsToTrace
        {
            get
            {
                return false;
            }
        }*/

        public ImagePanel()
        {
            ImageScale = 1.0f;
        }

        public override void Init()
        {
            sprBackground = new Sprite(Image);
            sprBackground.Origin = new SFML.Window.Vector2f(sprBackground.Texture.Size.X * 0.5f, sprBackground.Texture.Size.Y * 0.5f);
            base.Init();
        }

        protected override void UpdateLayout()
        {
            base.UpdateLayout();
            sprBackground.Position = new SFML.Window.Vector2f(AbsX + (Width * 0.5f), AbsY + (Height * 0.5f));
            sprBackground.Scale = Util.Scale(sprBackground, new SFML.Window.Vector2f(Width, Height), ImageScale);
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
