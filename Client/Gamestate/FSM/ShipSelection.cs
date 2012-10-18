using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SFML.Graphics;
using SFML.Window;
using SFML.Audio;

using FTLOverdrive.Client.UI;

namespace FTLOverdrive.Client.Gamestate
{
    class ShipSelection : IState, IRenderable
    {
        private RenderWindow window;
        private IntRect rctScreen;

        private Panel pnObscure;
        private ImagePanel pnWindow;
        private bool finishnow;
        private ImagePanel pnDescription;

        private ShipButton[] btnShips = new ShipButton[9];

        private int page;
        public int Page
        {
            get { return page; }
            set
            {
                foreach (var btn in btnShips)
                {
                    if (btn != null) btn.Remove();
                }
                page = value;

                var gens = Root.Singleton.mgrState.Get<Library>().GetPlayerShipGenerators();

                if (page < 0) page = gens.Count / 9;
                if (page > gens.Count / 9) page = 0;

                for (int i = 0; i < 9; i++)
                {
                    try
                    {
                        var gen = gens[page * 9 + i];
                        btnShips[i] = new ShipButton(gen);
                        Util.LayoutControl(btnShips[i], 24 + 205 * (i % 3), 52 + 135 * (i / 3), 191, 121, rctScreen);
                        btnShips[i].Parent = pnWindow;
                        btnShips[i].Init();
                    }
                    catch (ArgumentOutOfRangeException) { }
                }
            }
        }

        private class ShipButton : ImageButton
        {
            public string GeneratorName { get; set; }
            public bool Locked { get; set; }

            public Texture ShipImage { get; set; }
            public Texture LockImage { get; set; }
            public Texture HoveredLockImage { get; set; }
            public Texture DepressedLockImage { get; set; }
            public Texture DisabledLockImage { get; set; }

            private Sprite sprShip;
            private Sprite sprLock;

            public ShipButton()
            {
            }

            public ShipButton(Library.ShipGenerator gen)
            {
                Image = Root.Singleton.Material("img/customizeUI/ship_list_button_on.png");
                HoveredImage = Root.Singleton.Material("img/customizeUI/ship_list_button_select2.png");
                DisabledImage = Root.Singleton.Material("img/customizeUI/ship_list_button_off.png");
                LockImage = Root.Singleton.Material("img/customizeUI/box_lock_on.png");
                HoveredLockImage = Root.Singleton.Material("img/customizeUI/box_lock_selected.png");
                DisabledLockImage = Root.Singleton.Material("img/customizeUI/box_lock_off.png");

                this.HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");

                GeneratorName = gen.Name;
                ShipImage = Root.Singleton.Material(gen.MiniGraphic);
                Locked = !gen.Unlocked;
                Enabled = true;
            }

            public override void Init()
            {
                sprShip = new Sprite(ShipImage);
                sprLock = new Sprite(LockImage);
                
                OnClick += (sender) =>
                {
                    var gen = Root.Singleton.mgrState.Get<Library>().GetShipGenerator(GeneratorName);
                    if (gen.Unlocked)
                    {
                        //Console.WriteLine("Selecting unlocked ship: " + ShipName);
                        Root.Singleton.mgrState.Get<NewGame>().SetShipGenerator(gen);
                        Root.Singleton.mgrState.Get<ShipSelection>().finishnow = true;
                    }
                    else
                    {
                        //Console.WriteLine("Selecting locked ship: " + ShipName);
                    }
                };
                base.Init();
            }

            protected override void UpdateLayout()
            {
                int lockWidth = Width * 16 / 191;
                int lockHeight = Height * 22 / 121;
                if (FlipH)
                {
                    sprShip.Position = new SFML.Window.Vector2f(AbsX + Width, AbsY);
                    sprShip.Scale = Util.Scale(sprShip, new SFML.Window.Vector2f(-Width, Height));
                    sprLock.Position = new SFML.Window.Vector2f(AbsX + Width / 2 + lockWidth / 2, AbsY + Height / 2 - lockHeight / 2);
                    sprLock.Scale = Util.Scale(sprLock, new SFML.Window.Vector2f(-lockWidth, lockHeight));
                }
                else
                {
                    sprShip.Position = new SFML.Window.Vector2f(AbsX, AbsY);
                    sprShip.Scale = Util.Scale(sprShip, new SFML.Window.Vector2f(Width, Height));
                    sprLock.Position = new SFML.Window.Vector2f(AbsX + Width / 2 - lockWidth / 2, AbsY + Height / 2 - lockHeight / 2);
                    sprLock.Scale = Util.Scale(sprLock, new SFML.Window.Vector2f(lockWidth, lockHeight));
                }
                base.UpdateLayout();
            }

            public override void UpdateImage()
            {
                if (Enabled)
                {
                    if (Pressed && (DepressedLockImage != null))
                        sprLock.Texture = DepressedLockImage;
                    else if (Hovered && (HoveredLockImage != null))
                        sprLock.Texture = HoveredLockImage;
                    else
                        sprLock.Texture = LockImage;
                }
                else
                {
                    if (DisabledLockImage != null)
                        sprLock.Texture = DisabledLockImage;
                    else
                        sprLock.Texture = LockImage;
                }
                if (Locked)
                {
                    sprShip.Color = new Color(0, 0, 0, 255);
                }
                base.UpdateImage();
            }

            protected override void Draw(RenderWindow window)
            {
                base.Draw(window);
                window.Draw(sprShip);
                if (Locked)
                {
                    window.Draw(sprLock);
                }
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
            pnWindow.Image = Root.Singleton.Material("img/customizeUI/ship_list_main.png");
            Util.LayoutControl(pnWindow, (1280 / 2) - (647 / 2), (720 / 2) - (465 / 2), 647, 465, rctScreen);
            pnWindow.Parent = Root.Singleton.Canvas;
            pnWindow.Init();

            Page = 0;

            if (Root.Singleton.mgrState.Get<Library>().GetPlayerShipGenerators().Count > 9)
            {
                initPageButtons();
            }


            // Modal screen
            Root.Singleton.Canvas.ModalFocus = pnWindow;
        }

        private void initPageButtons()
        {
            var btnLeft = new ImageButton();
            btnLeft.Image = Root.Singleton.Material("img/customizeUI/button_arrow_on.png");
            btnLeft.HoveredImage = Root.Singleton.Material("img/customizeUI/button_arrow_select2.png");
            btnLeft.DisabledImage = Root.Singleton.Material("img/customizeUI/button_arrow_off.png");
            btnLeft.Enabled = true;
            btnLeft.HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
            btnLeft.OnClick += (sender) =>
            {
                Page--;
            };
            Util.LayoutControl(btnLeft, 125, 12, 32, 28, rctScreen);
            btnLeft.Parent = pnWindow;
            btnLeft.Init();

            var btnRight = new ImageButton();
            btnRight.Image = Root.Singleton.Material("img/customizeUI/button_arrow_on.png");
            btnRight.HoveredImage = Root.Singleton.Material("img/customizeUI/button_arrow_select2.png");
            btnRight.DisabledImage = Root.Singleton.Material("img/customizeUI/button_arrow_off.png");
            btnRight.Enabled = true;
            btnRight.FlipH = true;
            btnRight.HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
            btnRight.OnClick += (sender) =>
            {
                Page++;
            };
            Util.LayoutControl(btnRight, 463, 12, 32, 28, rctScreen);
            btnRight.Parent = pnWindow;
            btnRight.Init();
        }

        private void window_KeyPressed(object sender, KeyEventArgs e)
        {
            // Finish if escape
            if (e.Code == Keyboard.Key.Escape) finishnow = true;
        }

        public void Think(float delta)
        {
            // Check for escape
            if (finishnow)
            {
                // Close state
                Root.Singleton.mgrState.Deactivate<ShipSelection>();

                // Reopen hangar
                Root.Singleton.mgrState.FSMTransist<NewGame>();
            }
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
