using System;
using System.Collections.Generic;

using SFML.Graphics;
using SFML.Window;
using SFML.Audio;

using FTLOverdrive.Client.UI;

namespace FTLOverdrive.Client.Gamestate
{
    public class NewGame : IState, IRenderable
    {
        private RenderWindow window;
        private IntRect rctScreen;

        private Sprite sprBackground;
        private Sprite sprShip;

        private ImagePanel pnRename;

        private Library.Ship currentship;

        private bool easymode;
        private bool finishnow;
        private bool firstActivation = true;

        public void OnActivate()
        {
            // Store window
            window = Root.Singleton.Window;
            rctScreen = Util.ScreenRect(window.Size.X, window.Size.Y, 1.7778f);
            finishnow = false;
            window.KeyPressed += new EventHandler<KeyEventArgs>(window_KeyPressed);

            // Load sprites
            var texBackground = Root.Singleton.Material("img/customizeUI/custom_main.png");
            sprBackground = new Sprite(texBackground);
            sprBackground.Position = new Vector2f(rctScreen.Left, rctScreen.Top);
            sprBackground.Scale = Util.Scale(sprBackground, new Vector2f(rctScreen.Width, rctScreen.Height));

            // Load UI
            pnRename = new ImagePanel();
            pnRename.Image = Root.Singleton.Material("img/customizeUI/box_shipname.png");
            Util.LayoutControl(pnRename, 10, 10, 442, 48, rctScreen);
            pnRename.Parent = Root.Singleton.Canvas;
            pnRename.Init();

            var btnRenameShip = new ImageButton();
            btnRenameShip.Image = Root.Singleton.Material("img/customizeUI/button_name_on.png");
            btnRenameShip.HoveredImage = Root.Singleton.Material("img/customizeUI/button_name_select2.png");
            btnRenameShip.DisabledImage = Root.Singleton.Material("img/customizeUI/button_name_off.png");
            btnRenameShip.Enabled = true;
            Util.LayoutControl(btnRenameShip, 8, 8, 95, 33, rctScreen);
            btnRenameShip.Parent = pnRename;
            btnRenameShip.Init();

            var btnListShips = new ImageButton();
            btnListShips.Image = Root.Singleton.Material("img/customizeUI/button_list_on.png");
            btnListShips.HoveredImage = Root.Singleton.Material("img/customizeUI/button_list_select2.png");
            btnListShips.DisabledImage = Root.Singleton.Material("img/customizeUI/button_list_off.png");
            btnListShips.Enabled = true;
            Util.LayoutControl(btnListShips, 64, 194, 62, 28, rctScreen);
            btnListShips.Parent = Root.Singleton.Canvas;
            btnListShips.Init();
            btnListShips.OnClick += (sender) =>
            {
                Root.Singleton.mgrState.Activate<ShipSelection>();
            };

            var btnShipLeft = new ImageButton();
            btnShipLeft.Image = Root.Singleton.Material("img/customizeUI/button_arrow_on.png");
            btnShipLeft.HoveredImage = Root.Singleton.Material("img/customizeUI/button_arrow_select2.png");
            btnShipLeft.DisabledImage = Root.Singleton.Material("img/customizeUI/button_arrow_off.png");
            btnShipLeft.Enabled = true;
            btnShipLeft.OnClick += (sender) =>
            {
                Library.Ship ship;
                var shiplist = Root.Singleton.mgrState.Get<Library>().GetShips();
                int idx = shiplist.IndexOf(currentship.Name);
                do
                {
                    idx--;
                    if (idx < 0) idx = shiplist.Count - 1;
                    ship = Root.Singleton.mgrState.Get<Library>().GetShip(shiplist[idx]);
                } while (!ship.Unlocked);
                SetShip(ship);
            };
            Util.LayoutControl(btnShipLeft, 30, 194, 32, 28, rctScreen);
            btnShipLeft.Parent = Root.Singleton.Canvas;
            btnShipLeft.Init();

            var btnShipRight = new ImageButton();
            btnShipRight.Image = Root.Singleton.Material("img/customizeUI/button_arrow_on.png");
            btnShipRight.HoveredImage = Root.Singleton.Material("img/customizeUI/button_arrow_select2.png");
            btnShipRight.DisabledImage = Root.Singleton.Material("img/customizeUI/button_arrow_off.png");
            btnShipRight.Enabled = true;
            btnShipRight.FlipH = true;
            btnShipRight.OnClick += (sender) =>
            {
                Library.Ship ship;
                var shiplist = Root.Singleton.mgrState.Get<Library>().GetShips();
                int idx = shiplist.IndexOf(currentship.Name);
                do
                {
                    idx++;
                    if (idx >= shiplist.Count) idx = 0;
                    ship = Root.Singleton.mgrState.Get<Library>().GetShip(shiplist[idx]);
                } while (!ship.Unlocked);
                SetShip(ship);
            };
            Util.LayoutControl(btnShipRight, 128, 194, 32, 28, rctScreen);
            btnShipRight.Parent = Root.Singleton.Canvas;
            btnShipRight.Init();

            var btnEasy = new ImageButton();
            easymode = true;
            ImageButton btnNormal = null;
            btnEasy.Image = Root.Singleton.Material("img/customizeUI/button_easy_on.png");
            btnEasy.HoveredImage = null;
            btnEasy.DisabledImage = Root.Singleton.Material("img/customizeUI/button_easy_off.png");
            btnEasy.Enabled = true;
            btnEasy.OnClick += (sender) =>
            {
                btnNormal.Image = Root.Singleton.Material("img/customizeUI/button_normal_on.png");
                btnNormal.UpdateImage();
                btnEasy.Image = Root.Singleton.Material("img/customizeUI/button_easy_select2.png");
                btnEasy.UpdateImage();
                easymode = true;
            };
            Util.LayoutControl(btnEasy, 977, 16, 95, 24, rctScreen);
            btnEasy.Parent = Root.Singleton.Canvas;
            btnEasy.Init();

            btnNormal = new ImageButton();
            btnNormal.Image = Root.Singleton.Material("img/customizeUI/button_normal_select2.png");
            btnNormal.HoveredImage = null;
            btnNormal.DisabledImage = Root.Singleton.Material("img/customizeUI/button_normal_off.png");
            btnNormal.Enabled = true;
            btnNormal.OnClick += (sender) =>
            {
                btnNormal.Image = Root.Singleton.Material("img/customizeUI/button_normal_select2.png");
                btnNormal.UpdateImage();
                btnEasy.Image = Root.Singleton.Material("img/customizeUI/button_easy_on.png");
                btnEasy.UpdateImage();
                easymode = false;
            };
            Util.LayoutControl(btnNormal, 977, 41, 95, 24, rctScreen);
            btnNormal.Parent = Root.Singleton.Canvas;
            btnNormal.Init();

            var btnStart = new ImageButton();
            btnStart.Image = Root.Singleton.Material("img/customizeUI/button_start_on.png");
            btnStart.HoveredImage = Root.Singleton.Material("img/customizeUI/button_start_select2.png");
            btnStart.DisabledImage = Root.Singleton.Material("img/customizeUI/button_start_off.png");
            btnStart.Enabled = true;
            btnStart.OnClick += (sender) =>
            {
                
            };
            Util.LayoutControl(btnStart, 1082, 16, 152, 48, rctScreen);
            btnStart.Parent = Root.Singleton.Canvas;
            btnStart.Init();

            // Locate the default ship
            if (firstActivation)
            {
                SetShip(GetDefaultShip());
            }
            firstActivation = false;
        }

        private Library.Ship GetDefaultShip()
        {
            var lib = Root.Singleton.mgrState.Get<Library>();
            var ships = lib.GetShips();
            foreach (var shipname in ships)
            {
                var ship = lib.GetShip(shipname);
                if (ship.Default)
                {
                    return ship;
                }
            }
            throw new Exception("No default ship!");
        }

        public void SetShip(Library.Ship ship)
        {
            // Set current ship
            currentship = ship;

            // Remove old UI
            if (sprShip != null) sprShip.Dispose();

            // Create new UI
            sprShip = new Sprite(Root.Singleton.Material(ship.BaseGraphic));
            sprShip.Texture.Smooth = true;
            Util.LayoutSprite(sprShip, 310, 0, 660, 450, rctScreen);
        }

        private void window_KeyPressed(object sender, KeyEventArgs e)
        {
            // Finish if escape
            if (e.Code == Keyboard.Key.Escape && Root.Singleton.Canvas.ModalFocus == null) finishnow = true;
        }

        public void OnDeactivate()
        {
            Root.Singleton.Canvas.Clear();
        }

        public void Think(float delta)
        {
            if (finishnow)
            {
                // Close state
                Root.Singleton.mgrState.Deactivate<NewGame>();

                // Reopen main menu
                Root.Singleton.mgrState.FSMTransist<MainMenu>();
            }
        }

        public void Render(RenderStage stage)
        {
            if (stage == RenderStage.PREGUI)
            {
                window.Draw(sprBackground);
                window.Draw(sprShip);
            }
        }
    }
}
