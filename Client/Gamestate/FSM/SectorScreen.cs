using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using FTLOverdrive.Client.UI;
using SFML.Window;
using FTLOverdrive.Client.Map;

namespace FTLOverdrive.Client.Gamestate.FSM
{
    public class SectorScreen : ModalWindow<NewGame>
    {

        public Sector Sector { get; set; }

        private class BeaconIcon : ImageButton
        {
            public Texture ShadowImage { get; set; }
            private Sprite sprShadow;

            public override void Init()
            {
                sprShadow = new Sprite(ShadowImage);
                HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
 	            base.Init();
            }

            protected override void UpdateLayout()
            {
                sprShadow.Position = new SFML.Window.Vector2f(AbsX + (Width / 2), AbsY + (Height / 2));
                sprShadow.Scale = Util.Scale(sprShadow, new SFML.Window.Vector2f(Width, Height), ImageScale);
                base.UpdateLayout();
            }

            public override void UpdateImage()
            {
                sprShadow.Origin = new SFML.Window.Vector2f(sprShadow.Texture.Size.X * 0.5f, sprShadow.Texture.Size.Y * 0.5f);
                base.UpdateImage();
            }

            protected override void Draw(RenderWindow window)
            {
                window.Draw(sprShadow);
                base.Draw(window);
            }
        }

        public override void OnActivate()
        {
            BackgroundImage = getBackgroundTexture();
            base.OnActivate();

            foreach (var b in Sector.Beacons)
            {
                var btn = new BeaconIcon();
                btn.Image = Root.Singleton.Material(b.Icon);
                btn.ShadowImage = Root.Singleton.Material(b.IconShadow);
                Util.LayoutControl(btn, b.X - 16, b.Y - 16, 32, 32, ScreenRectangle);
                btn.Parent = Window;
                btn.Init();
            }

            var btnClose = new ImageButton();
            btnClose.Image = Root.Singleton.Material("img/generalUI/general_close.png");
            btnClose.HoveredImage = Root.Singleton.Material("img/generalUI/general_close3.png");
            btnClose.DisabledImage = Root.Singleton.Material("img/generalUI/general_close4.png");
            btnClose.HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
            btnClose.OnClick += (sender) =>
            {
                Finish = true;
            };
            Util.LayoutControl(btnClose, 504, 0, 64, 64, ScreenRectangle);
            btnClose.Parent = Window;
            btnClose.Init();
        }

        private Texture getBackgroundTexture()
        {
            var sprBack = new Sprite(Root.Singleton.Material("img/map/map_back.png"));
            var sprSectorBackground = new Sprite(Root.Singleton.Material(Sector.Background));
            var sprMask = new Sprite(Root.Singleton.Material("img/map/map_zonemask_white.png"));
            var sprOverlay = new Sprite(Root.Singleton.Material("img/map/map_overlay.png"));

            sprBack.Position += new Vector2f(0, 28);
            sprSectorBackground.Position += new Vector2f(0, 28);
            sprMask.Position += new Vector2f(0, 28);
            sprOverlay.Position += new Vector2f(0, 28);

            var rt = new RenderTexture(1000, 1000);

            rt.Draw(sprBack);
            rt.Draw(sprSectorBackground);
            rt.Draw(sprMask, new RenderStates(BlendMode.Multiply));
            rt.Draw(sprOverlay);

            rt.Display();
            return new Texture(rt.Texture);
        }
    }
}
