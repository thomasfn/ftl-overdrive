using System;
using System.Collections.Generic;

using SFML.Graphics;
using SFML.Window;
using SFML.Audio;

using FTLOverdrive.Client.UI;
using FTLOverdrive.Client.Ships;

namespace FTLOverdrive.Client.Gamestate
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

        private List<ImageButton> lstSystems;

        private bool easymode;
        private bool finishnow;

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
            shipRenderer.ShowRooms = true;
            Util.LayoutControl(shipRenderer, 310, 0, 660, 450, rctScreen);
            shipRenderer.Parent = Root.Singleton.Canvas;
            shipRenderer.Init();

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
            btnRenameShip.OnClick += (sender) =>
            {
                tbShipName.EditMode = true;
                Root.Singleton.Canvas.Focus = tbShipName;
            };
            btnRenameShip.HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
            Util.LayoutControl(btnRenameShip, 8, 8, 95, 33, rctScreen);
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

            var btnListShips = new ImageButton();
            btnListShips.Image = Root.Singleton.Material("img/customizeUI/button_list_on.png");
            btnListShips.HoveredImage = Root.Singleton.Material("img/customizeUI/button_list_select2.png");
            btnListShips.DisabledImage = Root.Singleton.Material("img/customizeUI/button_list_off.png");
            btnListShips.Enabled = true;
            btnListShips.HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
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
            Util.LayoutControl(btnShipLeft, 30, 194, 32, 28, rctScreen);
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
            Util.LayoutControl(btnShipRight, 128, 194, 32, 28, rctScreen);
            btnShipRight.Parent = Root.Singleton.Canvas;
            btnShipRight.Init();

            ImageToggleButton btnNormal = null, btnEasy = null;
            easymode = false;

            btnEasy = new ImageToggleButton();
            btnEasy.Image = Root.Singleton.Material("img/customizeUI/button_easy_on.png");
            btnEasy.HoveredImage = Root.Singleton.Material("img/customizeUI/button_easy_select2.png");
            btnEasy.ToggledImage = Root.Singleton.Material("img/customizeUI/button_easy_select2.png");
            btnEasy.DisabledImage = Root.Singleton.Material("img/customizeUI/button_easy_off.png");
            btnEasy.Enabled = true;
            btnEasy.Toggled = true;
            btnEasy.HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
            btnEasy.OnClick += (sender) =>
            {
                btnEasy.Toggled = true;
                btnEasy.UpdateImage();
                btnNormal.Toggled = false;
                btnNormal.UpdateImage();
                easymode = true;
            };
            Util.LayoutControl(btnEasy, 977, 16, 95, 24, rctScreen);
            btnEasy.Parent = Root.Singleton.Canvas;
            btnEasy.Init();

            btnNormal = new ImageToggleButton();
            btnNormal.Image = Root.Singleton.Material("img/customizeUI/button_normal_on.png");
            btnNormal.HoveredImage = Root.Singleton.Material("img/customizeUI/button_normal_select2.png");
            btnNormal.ToggledImage = Root.Singleton.Material("img/customizeUI/button_normal_select2.png");
            btnNormal.DisabledImage = Root.Singleton.Material("img/customizeUI/button_normal_off.png");
            btnNormal.Enabled = true;
            btnNormal.HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
            btnNormal.OnClick += (sender) =>
            {
                btnEasy.Toggled = false;
                btnEasy.UpdateImage();
                btnNormal.Toggled = true;
                btnNormal.UpdateImage();
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
            btnStart.HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
            btnStart.OnClick += (sender) =>
            {
                
            };
            Util.LayoutControl(btnStart, 1082, 16, 152, 48, rctScreen);
            btnStart.Parent = Root.Singleton.Canvas;
            btnStart.Init();

            ImageToggleButton btnTypeA = null, btnTypeB = null;

            btnTypeA = new ImageToggleButton();
            btnTypeA.Image = Root.Singleton.Material("img/customizeUI/button_typea_on.png");
            btnTypeA.HoveredImage = Root.Singleton.Material("img/customizeUI/button_typea_select2.png");
            btnTypeA.ToggledImage = Root.Singleton.Material("img/customizeUI/button_typea_select2.png");
            btnTypeA.DisabledImage = Root.Singleton.Material("img/customizeUI/button_typea_off.png");            
            btnTypeA.Enabled = true;
            btnTypeA.Toggled = true;
            btnTypeA.HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
            btnTypeA.OnClick += (sender) =>
            {
                btnTypeA.Toggled = true;
                btnTypeA.UpdateImage();
                btnTypeB.Toggled = false;
                btnTypeB.UpdateImage();
            };
            Util.LayoutControl(btnTypeA, 18, 260, 80, 22, rctScreen);
            btnTypeA.Parent = Root.Singleton.Canvas;
            btnTypeA.Init();

            btnTypeB = new ImageToggleButton();
            btnTypeB.Image = Root.Singleton.Material("img/customizeUI/button_typeb_on.png");
            btnTypeB.HoveredImage = Root.Singleton.Material("img/customizeUI/button_typeb_select2.png");
            btnTypeB.ToggledImage = Root.Singleton.Material("img/customizeUI/button_typeb_select2.png");
            btnTypeB.DisabledImage = Root.Singleton.Material("img/customizeUI/button_typeb_off.png");
            btnTypeB.Enabled = false;
            btnTypeB.HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
            btnTypeB.OnClick += (sender) =>
            {
                btnTypeA.Toggled = false;
                btnTypeA.UpdateImage();
                btnTypeB.Toggled = true;
                btnTypeB.UpdateImage();
            };
            Util.LayoutControl(btnTypeB, 100, 260, 80, 22, rctScreen);
            btnTypeB.Parent = Root.Singleton.Canvas;
            btnTypeB.Init();

            var btnHideRooms = new ImageButton();
            btnHideRooms.Image = Root.Singleton.Material("img/customizeUI/button_hide_on.png");
            btnHideRooms.HoveredImage = Root.Singleton.Material("img/customizeUI/button_hide_select2.png");
            btnHideRooms.DisabledImage = Root.Singleton.Material("img/customizeUI/button_hide_off.png");
            btnHideRooms.Enabled = true;
            btnHideRooms.HoverSound = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
            btnHideRooms.OnClick += (sender) =>
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
            Util.LayoutControl(btnHideRooms, 23, 301, 150, 28, rctScreen);
            btnHideRooms.Parent = Root.Singleton.Canvas;
            btnHideRooms.Init();

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

        public void SetShipGenerator(Library.ShipGenerator gen)
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
            currentShip = gen.Generate();

            // Update ship renderer
            shipRenderer.Ship = currentShip;

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
