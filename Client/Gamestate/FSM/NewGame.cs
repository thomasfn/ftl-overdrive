using System;
using System.Collections.Generic;

using SFML.Graphics;
using SFML.Window;
using SFML.Audio;

using FTLOverdrive.Client.UI;
using FTLOverdrive.Client.Ships;

namespace FTLOverdrive.Client.Gamestate.FSM
{
    public class NewGame : IState, IRenderable
    {
        private RenderWindow window;
        private IntRect rctScreen;

        private Sprite sprBackground;
        private ShipRenderer shipRenderer = new ShipRenderer();

        private ImagePanel pnRename;

        private Library.ShipGenerator currentShipGen;
        private Ship currentShip;

        private TextEntry tbShipName;

        private ImageToggleButton btnLayoutA;
        private ImageToggleButton btnLayoutB;

        private List<ImageButton> lstSystems;

        private bool easymode;
        private bool finishnow;

        public void OnActivate()
        {
            StoreWindow();
            LoadSprites();
            LoadUI();

			InitShipRename();

            InitListShipsButton();
			InitShipsArrowButtons();

            InitDifficultyButtons();
            InitStartButton();

            InitLayoutButtons();

            InitShowRoomsButton();

            if (currentShipGen == null)
            {
                // Locate the default ship
                SetShipGenerator(GetDefaultShipGenerator());
            }
            else
            {
                SetShipGenerator(currentShipGen);
            }
        }

        #region Init

        void StoreWindow()
        {
            window = Root.Singleton.Window;
            rctScreen = Util.ScreenRect(window.Size.X, window.Size.Y, 1.7778f);
            finishnow = false;
            window.KeyPressed += new EventHandler<KeyEventArgs>(window_KeyPressed);
        }

        void LoadSprites()
        {
            var texBackground = Root.Singleton.Material("img/customizeUI/custom_main.png");
            sprBackground = new Sprite(texBackground);
            sprBackground.Position = new Vector2f(rctScreen.Left, rctScreen.Top);
            sprBackground.Scale = Util.Scale(sprBackground, new Vector2f(rctScreen.Width, rctScreen.Height));
        }

        void LoadUI()
        {
            shipRenderer.ShowRooms = true;
            Util.LayoutControl(shipRenderer, 310, 0, 660, 450, rctScreen);
            shipRenderer.Parent = Root.Singleton.Canvas;
            shipRenderer.Init();
        }

        void InitShipRename()
        {
            pnRename = new ImagePanel();
            pnRename.Image = Root.Singleton.Material("img/customizeUI/box_shipname.png");
            Util.LayoutControl(pnRename, 10, 10, pnRename.Image.Size, rctScreen);
            pnRename.Parent = Root.Singleton.Canvas;
            pnRename.Init();

            var btnRenameShip = new ImageButton();
            btnRenameShip.Image = Root.Singleton.Material("img/customizeUI/button_name_on.png");
            btnRenameShip.HoveredImage = Root.Singleton.Material("img/customizeUI/button_name_select2.png");
            btnRenameShip.DisabledImage = Root.Singleton.Material("img/customizeUI/button_name_off.png");
            btnRenameShip.Enabled = true;
            btnRenameShip.OnClick += sender =>
            {
                tbShipName.EditMode = true;
                Root.Singleton.Canvas.Focus = tbShipName;
            };
            btnRenameShip.HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
            Util.LayoutControl(btnRenameShip, 8, 8, btnRenameShip.Image.Size, rctScreen);
            btnRenameShip.Parent = pnRename;
            btnRenameShip.Init();

            tbShipName = new TextEntry();
            tbShipName.Centered = true;
            tbShipName.AutoScale = false;
            tbShipName.Font = Root.Singleton.Font("fonts/num_font.ttf");
            tbShipName.Text = "test";
            Util.LayoutControl(tbShipName, 115, 4, 320, 33, rctScreen);
            tbShipName.Parent = pnRename;
            tbShipName.Init();
        }

        void InitListShipsButton()
        {
            var btnListShips = new ImageButton();
            btnListShips.Image = Root.Singleton.Material("img/customizeUI/button_list_on.png");
            btnListShips.HoveredImage = Root.Singleton.Material("img/customizeUI/button_list_select2.png");
            btnListShips.DisabledImage = Root.Singleton.Material("img/customizeUI/button_list_off.png");
            btnListShips.Enabled = true;
            btnListShips.HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
            Util.LayoutControl(btnListShips, 64, 194, btnListShips.Image.Size, rctScreen);
            btnListShips.Parent = Root.Singleton.Canvas;
            btnListShips.Init();
            btnListShips.OnClick += sender =>
            {
                Root.Singleton.mgrState.Activate<ShipSelection>();
            };
        }

        void InitShipsArrowButtons()
        {
            var btnShipLeft = new ImageButton();
            btnShipLeft.Image = Root.Singleton.Material("img/customizeUI/button_arrow_on.png");
            btnShipLeft.HoveredImage = Root.Singleton.Material("img/customizeUI/button_arrow_select2.png");
            btnShipLeft.DisabledImage = Root.Singleton.Material("img/customizeUI/button_arrow_off.png");
            btnShipLeft.Enabled = true;
            btnShipLeft.HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
            btnShipLeft.OnClick += (sender) =>
            {
                var shiplist = Root.Singleton.mgrState.Get<Library>().GetPlayerShipGenerators();
                int idx = shiplist.IndexOf(currentShipGen);
                do
                {
                    idx--;
                    if (idx < 0) idx = shiplist.Count - 1;
                } while (!shiplist[idx].Unlocked);
                SetShipGenerator(shiplist[idx]);
            };
            Util.LayoutControl(btnShipLeft, 30, 194, btnShipLeft.Image.Size, rctScreen);
            btnShipLeft.Parent = Root.Singleton.Canvas;
            btnShipLeft.Init();

            var btnShipRight = new ImageButton();
            btnShipRight.Image = Root.Singleton.Material("img/customizeUI/button_arrow_on.png");
            btnShipRight.HoveredImage = Root.Singleton.Material("img/customizeUI/button_arrow_select2.png");
            btnShipRight.DisabledImage = Root.Singleton.Material("img/customizeUI/button_arrow_off.png");
            btnShipRight.Enabled = true;
            btnShipRight.FlipH = true;
            btnShipRight.HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
            btnShipRight.OnClick += (sender) =>
            {
                var shiplist = Root.Singleton.mgrState.Get<Library>().GetPlayerShipGenerators();
                int idx = shiplist.IndexOf(currentShipGen);
                do
                {
                    idx++;
                    if (idx >= shiplist.Count) idx = 0;
                } while (!shiplist[idx].Unlocked);
                SetShipGenerator(shiplist[idx]);
            };
            Util.LayoutControl(btnShipRight, 128, 194, btnShipRight.Image.Size, rctScreen);
            btnShipRight.Parent = Root.Singleton.Canvas;
            btnShipRight.Init();
        }

        void InitDifficultyButtons()
        {
            ImageToggleButton btnNormal = null;
            ImageToggleButton btnEasy = null;
            easymode = false;
            btnEasy = new ImageToggleButton();
            btnEasy.Image = Root.Singleton.Material("img/customizeUI/button_easy_on.png");
            btnEasy.HoveredImage = Root.Singleton.Material("img/customizeUI/button_easy_select2.png");
            btnEasy.ToggledImage = Root.Singleton.Material("img/customizeUI/button_easy_select2.png");
            btnEasy.DisabledImage = Root.Singleton.Material("img/customizeUI/button_easy_off.png");
            btnEasy.Enabled = true;
            btnEasy.Toggled = true;
            btnEasy.HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
            btnEasy.OnClick += sender =>
            {
                btnEasy.Toggled = true;
                btnEasy.UpdateImage();
                btnNormal.Toggled = false;
                btnNormal.UpdateImage();
                easymode = true;
            };
            Util.LayoutControl(btnEasy, 977, 16, btnEasy.Image.Size, rctScreen);
            btnEasy.Parent = Root.Singleton.Canvas;
            btnEasy.Init();


            btnNormal = new ImageToggleButton();
            btnNormal.Image = Root.Singleton.Material("img/customizeUI/button_normal_on.png");
            btnNormal.HoveredImage = Root.Singleton.Material("img/customizeUI/button_normal_select2.png");
            btnNormal.ToggledImage = Root.Singleton.Material("img/customizeUI/button_normal_select2.png");
            btnNormal.DisabledImage = Root.Singleton.Material("img/customizeUI/button_normal_off.png");
            btnNormal.Enabled = true;
            btnNormal.HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
            btnNormal.OnClick += sender =>
            {
                btnEasy.Toggled = false;
                btnEasy.UpdateImage();
                btnNormal.Toggled = true;
                btnNormal.UpdateImage();
                easymode = false;
            };
            Util.LayoutControl(btnNormal, 977, 41, btnNormal.Image.Size, rctScreen);
            btnNormal.Parent = Root.Singleton.Canvas;
            btnNormal.Init();
        }

        void InitStartButton()
        {
            var btnStart = new ImageButton();
            btnStart.Image = Root.Singleton.Material("img/customizeUI/button_start_on.png");
            btnStart.HoveredImage = Root.Singleton.Material("img/customizeUI/button_start_select2.png");
            btnStart.DisabledImage = Root.Singleton.Material("img/customizeUI/button_start_off.png");
            btnStart.Enabled = true;
            btnStart.HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
            btnStart.OnClick += sender =>
            {
                var generators = Root.Singleton.mgrState.Get<Library>().GetSectorMapGenerators();
                // TODO allow player to chose which generator to use
                Root.Singleton.mgrState.Get<SectorScreen>().Sector = generators[0].Generate().CurrentNode.Sector;
                Root.Singleton.mgrState.Activate<SectorScreen>();
            };
            Util.LayoutControl(btnStart, 1082, 16, btnStart.Image.Size, rctScreen);
            btnStart.Parent = Root.Singleton.Canvas;
            btnStart.Init();
        }

        void InitLayoutButtons()
        {
            btnLayoutA = null;
            btnLayoutB = null;

            btnLayoutA = new ImageToggleButton();
            btnLayoutA.Image = Root.Singleton.Material("img/customizeUI/button_typea_on.png");
            btnLayoutA.HoveredImage = Root.Singleton.Material("img/customizeUI/button_typea_select2.png");
            btnLayoutA.ToggledImage = Root.Singleton.Material("img/customizeUI/button_typea_select2.png");
            btnLayoutA.DisabledImage = Root.Singleton.Material("img/customizeUI/button_typea_off.png");
            btnLayoutA.HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
            btnLayoutA.OnClick += sender =>
            {
                btnLayoutA.Toggled = true;
                btnLayoutA.UpdateImage();
                btnLayoutB.Toggled = false;
                btnLayoutB.UpdateImage();
                SetShipGenerator(currentShipGen, 0);
            };
            Util.LayoutControl(btnLayoutA, 18, 260, btnLayoutA.Image.Size, rctScreen);
            btnLayoutA.Parent = Root.Singleton.Canvas;
            btnLayoutA.Init();

            btnLayoutB = new ImageToggleButton();
            btnLayoutB.Image = Root.Singleton.Material("img/customizeUI/button_typeb_on.png");
            btnLayoutB.HoveredImage = Root.Singleton.Material("img/customizeUI/button_typeb_select2.png");
            btnLayoutB.ToggledImage = Root.Singleton.Material("img/customizeUI/button_typeb_select2.png");
            btnLayoutB.DisabledImage = Root.Singleton.Material("img/customizeUI/button_typeb_off.png");
            btnLayoutB.HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
            btnLayoutB.OnClick += sender =>
            {
                btnLayoutA.Toggled = false;
                btnLayoutA.UpdateImage();
                btnLayoutB.Toggled = true;
                btnLayoutB.UpdateImage();
                SetShipGenerator(currentShipGen, 1);
            };
            Util.LayoutControl(btnLayoutB, 100, 260, btnLayoutB.Image.Size, rctScreen);
            btnLayoutB.Parent = Root.Singleton.Canvas;
            btnLayoutB.Init();
        }

        void InitShowRoomsButton()
        {
            var btnHideRooms = new ImageButton();
            btnHideRooms.Image = Root.Singleton.Material("img/customizeUI/button_hide_on.png");
            btnHideRooms.HoveredImage = Root.Singleton.Material("img/customizeUI/button_hide_select2.png");
            btnHideRooms.DisabledImage = Root.Singleton.Material("img/customizeUI/button_hide_off.png");
            btnHideRooms.Enabled = true;
            btnHideRooms.HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
            btnHideRooms.OnClick += sender =>
            {
                if (!shipRenderer.ShowRooms)
                {
                    btnHideRooms.Image = Root.Singleton.Material("img/customizeUI/button_hide_on.png");
                    btnHideRooms.HoveredImage = Root.Singleton.Material("img/customizeUI/button_hide_select2.png");
                    btnHideRooms.DisabledImage = Root.Singleton.Material("img/customizeUI/button_hide_off.png");
                    shipRenderer.ShowRooms = true;
                }
                else
                {
                    btnHideRooms.Image = Root.Singleton.Material("img/customizeUI/button_show_on.png");
                    btnHideRooms.HoveredImage = Root.Singleton.Material("img/customizeUI/button_show_select2.png");
                    btnHideRooms.DisabledImage = Root.Singleton.Material("img/customizeUI/button_show_off.png");
                    shipRenderer.ShowRooms = false;
                }
                btnHideRooms.UpdateImage();
            };
            Util.LayoutControl(btnHideRooms, 23, 301, btnHideRooms.Image.Size, rctScreen);
            btnHideRooms.Parent = Root.Singleton.Canvas;
            btnHideRooms.Init();
        }
        #endregion

        private Library.ShipGenerator GetDefaultShipGenerator()
        {
            var lib = Root.Singleton.mgrState.Get<Library>();
            var gens = lib.GetPlayerShipGenerators();
            foreach (var gen in gens)
            {
                if (gen.Default)
                {
                    return gen;
                }
            }
            throw new Exception("No default ship generator!");
        }

        public void SetShipGenerator(Library.ShipGenerator gen, int layout = 0)
        {
            if (lstSystems != null)
            {
                foreach (var system in lstSystems)
                    system.Remove();
                lstSystems.Clear();
                lstSystems = null;
            }

            // Set current ship
            currentShipGen = gen;
            currentShip = gen.Generate(layout);

            // Update ship renderer
            shipRenderer.Ship = currentShip;

            // Update layout buttons
            btnLayoutA.Enabled = (currentShipGen.NumberOfLayouts >= 1);
            btnLayoutA.Toggled = (layout == 0);
            btnLayoutA.UpdateImage();
            btnLayoutB.Enabled = (currentShipGen.NumberOfLayouts >= 2);
            btnLayoutB.Toggled = (layout == 1);
            btnLayoutB.UpdateImage();

            // Create new UI
            tbShipName.Text = currentShip.Name;

            lstSystems = new List<ImageButton>();
            var systems = currentShip.Systems;
            for (int i = 0; i < systems.Count; i++)
            {
                var system = systems[i];
                var btnSystem = new ImageButton();
                btnSystem.Image = Root.Singleton.Material("img/customizeUI/box_system_on.png");
                btnSystem.HoveredImage = Root.Singleton.Material("img/customizeUI/box_system_select2.png");
                btnSystem.DisabledImage = Root.Singleton.Material("img/customizeUI/box_system_off.png");
                btnSystem.Enabled = true;
                //btnSystem.HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
                Util.LayoutControl(btnSystem, 370 + (i * 38), 380, 38, 96, rctScreen);
                btnSystem.Parent = Root.Singleton.Canvas;
                btnSystem.Init();

                var systembox = new SystemBox();
                systembox.SystemIcon = Root.Singleton.Material(system.IconGraphics["green"]);
                systembox.PowerLevel = system.MinPower;
                systembox.Width = btnSystem.Width - 2;
                systembox.Height = btnSystem.Height - 2;
                systembox.Parent = btnSystem;
                systembox.Init();

                lstSystems.Add(btnSystem);
            }
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
            }
        }
    }
}
