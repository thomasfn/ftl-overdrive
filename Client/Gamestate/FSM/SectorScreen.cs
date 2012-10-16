using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using FTLOverdrive.Client.UI;
using SFML.Window;
using FTLOverdrive.Client.Map;

namespace FTLOverdrive.Client.Gamestate
{
    public class SectorScreen : IState, IRenderable
    {
        private RenderWindow window;
        private IntRect rctScreen;

        private Panel pnObscure;
        private ImagePanel pnWindow;
        private bool finishnow;

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

        public void OnActivate()
        {
            // Store window
            window = Root.Singleton.Window;
            rctScreen = Util.ScreenRect(window.Size.X, window.Size.Y, 1.7778f);
            finishnow = false;
            window.KeyPressed += window_KeyPressed;

            // Create UI
            pnObscure = new Panel();
            pnObscure.Colour = new Color(0, 0, 0, 192);
            Util.LayoutControl(pnObscure, 0, 0, 1280, 720, rctScreen);
            pnObscure.Parent = Root.Singleton.Canvas;
            pnObscure.Init();

            pnWindow = new ImagePanel();
            pnWindow.Image = getBackgroundTexture();
            Util.LayoutControl(pnWindow, (1280 - 540) / 2,
                                         (720 - 420) / 2,
                                         (int)pnWindow.Image.Size.X,
                                         (int)pnWindow.Image.Size.Y,
                                         rctScreen);
            pnWindow.Parent = Root.Singleton.Canvas;
            pnWindow.Init();

            foreach (var b in Sector.Beacons)
            {
                var btn = new BeaconIcon();
                btn.Image = Root.Singleton.Material(b.Icon);
                btn.ShadowImage = Root.Singleton.Material(b.IconShadow);
                Util.LayoutControl(btn, b.X - 16, b.Y - 16, 32, 32, rctScreen);
                btn.Parent = pnWindow;
                btn.Init();
            }

            var btnClose = new ImageButton();
            btnClose.Image = Root.Singleton.Material("img/generalUI/general_close.png");
            btnClose.HoveredImage = Root.Singleton.Material("img/generalUI/general_close3.png");
            btnClose.DisabledImage = Root.Singleton.Material("img/generalUI/general_close4.png");
            btnClose.HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
            btnClose.OnClick += (sender) =>
            {
                finishnow = true;
            };
            Util.LayoutControl(btnClose, 504, 0, 64, 64, rctScreen);
            btnClose.Parent = pnWindow;
            btnClose.Init();

            // Modal screen
            Root.Singleton.Canvas.ModalFocus = pnWindow;
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

        public void Think(float delta)
        {
            // Check for escape
            if (finishnow)
            {
                // Close state
                Root.Singleton.mgrState.Deactivate<SectorScreen>();

                // Reopen hangar
                Root.Singleton.mgrState.FSMTransist<NewGame>();
            }
        }

        private void window_KeyPressed(object sender, KeyEventArgs e)
        {
            // Finish if escape
            if (e.Code == Keyboard.Key.Escape) finishnow = true;
        }

        public void OnDeactivate()
        {
            // Unmodal our window
            Root.Singleton.Canvas.ModalFocus = null;

            // Remove our controls
            pnObscure.Remove();
            pnWindow.Remove();
        }

        public void Render(RenderStage stage)
        {
        }
    }
}
