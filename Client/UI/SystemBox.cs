using System;
using System.Collections.Generic;

using SFML.Graphics;

namespace FTLOverdrive.Client.UI
{
    public class SystemBox : Panel
    {
        public Texture SystemIcon { get; set; }
        public int PowerLevel { get; set; }

        private ImagePanel icon;

        private List<RectangleShape> bars;

        public override void Init()
        {
            base.Init();

            icon = new ImagePanel();
            icon.Image = SystemIcon;
            icon.Parent = this;
            icon.ImageScale = 1.9f;
            icon.Init();

            RebuildBars();

            UpdateLayout();
        }

        protected override void UpdateLayout()
        {
            base.UpdateLayout();
            if (icon != null)
            {
                icon.X = 1;
                icon.Width = Width;
                icon.Height = Width;
                icon.Y = Height - icon.Height;
            }
            if (bars != null) LayoutBars();
        }

        public void RebuildBars()
        {
            if (bars != null)
            {
                foreach (var rct in bars)
                    rct.Dispose();
                bars.Clear();
            }
            bars = new List<RectangleShape>();
            for (int i = 0; i < PowerLevel; i++)
            {
                var bar = new RectangleShape();
                bar.FillColor = new Color(100, 255, 100);
                bars.Add(bar);
            }
        }

        private void LayoutBars()
        {
            for (int i = 0; i < bars.Count; i++)
            {
                var bar = bars[i];
                bar.Position = new SFML.Window.Vector2f(AbsX + (Width * 0.3f) + 1, AbsY + icon.Y - ((i + 1) * (Width * 0.15f + 2)));
                bar.Size = new SFML.Window.Vector2f(Width * 0.4f, Width * 0.15f);
            }
        }

        protected override void Draw(RenderWindow window)
        {
            base.Draw(window);
            for (int i = 0; i < bars.Count; i++)
            {
                window.Draw(bars[i]);
            }
        }

    }
}
