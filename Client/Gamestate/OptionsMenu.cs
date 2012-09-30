using System;
using System.Collections.Generic;

using SFML.Graphics;
using SFML.Window;
using SFML.Audio;

using FTLOverdrive.Client.UI;

namespace FTLOverdrive.Client.Gamestate
{
    public class OptionsMenu : IState, IRenderable
    {
        private RenderWindow window;
        private IntRect rctScreen;

        private Panel pnObscure;
        private ImagePanel pnWindow;

        private sealed class ResolutionSetting
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public ResolutionSetting(int w, int h) { Width = w; Height = h; }
            public override string ToString()
            {
                return Width.ToString() + " x " + Height.ToString();
            }
            public override bool Equals(object obj)
            {
                if (!(obj is ResolutionSetting)) return false;
                var other = obj as ResolutionSetting;
                return (other.Width == Width) && (other.Height == Height);
            }

            public override int GetHashCode()
            {
                return (int)(Width.GetHashCode() + Height.GetHashCode());
            }
        }

        private static readonly List<ResolutionSetting> resolutions = new List<ResolutionSetting>()
        {
            new ResolutionSetting(800, 600),
            new ResolutionSetting(1024, 600),
            new ResolutionSetting(1024, 720),
            new ResolutionSetting(1024, 768),
            new ResolutionSetting(1280, 720),
            new ResolutionSetting(1280, 768),
            new ResolutionSetting(1360, 720),
            new ResolutionSetting(1360, 768),
            new ResolutionSetting(1366, 720),
            new ResolutionSetting(1366, 768)
        };

        private ResolutionSetting currentres;
        private bool currentfs;
        private bool hotkeys;
        private bool backgrounds;
        private bool achievementPopups;
        private bool autoPause;
        private TextButton btnResolution;
        private TextButton btnFullscreen;
        private TextButton btnHotkeys;
        private TextButton btnDynamicBackgrounds;
        private TextButton btnAchievePopups;
        private TextButton btnWindowFocusPause;

        private bool windowresetneeded, finishnow;

        public void OnActivate()
        {
            // Store window
            window = Root.Singleton.Window;
            rctScreen = Util.ScreenRect(window.Size.X, window.Size.Y, 1.7778f);
            windowresetneeded = false;
            finishnow = false;
            window.KeyPressed += new EventHandler<KeyEventArgs>(window_KeyPressed);

            // Load settings
            currentres = new ResolutionSetting(
                Root.Singleton.Settings.ReadInt("Video", "ResX"),
                Root.Singleton.Settings.ReadInt("Video", "ResY")
            );
            currentfs = Root.Singleton.Settings.ReadInt("Video", "Fullscreen") == 1;
            hotkeys = Root.Singleton.Settings.ReadInt("Video", "Hotkeys") == 1;
            backgrounds = Root.Singleton.Settings.ReadInt("Video", "Backgrounds") == 1;
            achievementPopups = Root.Singleton.Settings.ReadInt("Video", "AchievementPopups") == 1;
            autoPause = Root.Singleton.Settings.ReadInt("Video", "AutoPause") == 1;

            // Create UI
            pnObscure = new Panel();
            pnObscure.Colour = new Color(0, 0, 0, 192);
            Util.LayoutControl(pnObscure, 0, 0, 1280, 720, rctScreen);
            pnObscure.Parent = Root.Singleton.Canvas;
            pnObscure.Init();

            pnWindow = new ImagePanel();
            pnWindow.Image = Root.Singleton.Material("img/box_text1.png");
            Util.LayoutControl(pnWindow, (1280 / 2) - (616 / 2), (720 / 2) - (384 / 2), 616, 384, rctScreen);
            pnWindow.Parent = Root.Singleton.Canvas;
            pnWindow.Init();

            var lblTitle = new Label();
            lblTitle.Colour = Color.White;
            lblTitle.Text = "Options:  (ESCAPE when done)";
            lblTitle.Font = Root.Singleton.Font("fonts/JustinFont12Bold.ttf");
            lblTitle.Scale = 0.475f;
            lblTitle.X = 22;
            lblTitle.Y = 35;
            lblTitle.Parent = pnWindow;
            lblTitle.Init();

            btnResolution = AddButton(70);
            btnResolution.OnClick += (sender) =>
            {
                int i = resolutions.IndexOf(currentres);
                i++;
                if (i >= resolutions.Count) i = 0;
                currentres = resolutions[i];
                updateButtons();
                windowresetneeded = true;
            };

            btnFullscreen = AddButton(96);
            btnFullscreen.OnClick += (sender) =>
            {
                currentfs = !currentfs;
                updateButtons();
                windowresetneeded = true;
            };

            btnHotkeys = AddButton(122);
            btnHotkeys.OnClick += (sender) =>
            {
                hotkeys = !hotkeys;
                updateButtons();
            };

            btnDynamicBackgrounds = AddButton(148);
            btnDynamicBackgrounds.OnClick += (sender) =>
            {
                backgrounds = !backgrounds;
                updateButtons();
            };

            btnAchievePopups = AddButton(174);
            btnAchievePopups.OnClick += (sender) =>
            {
                achievementPopups = !achievementPopups;
                updateButtons();
            };

            btnWindowFocusPause = AddButton(200);
            btnWindowFocusPause.OnClick += (sender) =>
            {
                autoPause = !autoPause;
                updateButtons();
            };
            updateButtons();

            // Modal screen
            Root.Singleton.Canvas.ModalFocus = pnWindow;
        }

        private void window_KeyPressed(object sender, KeyEventArgs e)
        {
            // Finish if escape
            if (e.Code == Keyboard.Key.Escape) finishnow = true;
        }

        private TextButton AddButton(int y)
        {
            var btn = new TextButton();
            btn.Colour = Color.White;
            btn.HoveredColour = Color.Yellow;
            btn.DisabledColour = new Color(128, 128, 128);
            btn.DepressedColour = Color.Yellow;
            btn.Text = "";
            btn.Font = Root.Singleton.Font("fonts/JustinFont12Bold.ttf");
            btn.Scale = 0.475f;
            btn.X = 25;
            btn.Y = y;
            btn.Enabled = true;
            btn.Parent = pnWindow;
            btn.Init();
            return btn;
        }

        private void updateButtons()
        {
            btnResolution.Text = "1. Resolution: " + currentres.ToString();
            btnFullscreen.Text = "2. Fullscreen: " + (currentfs ? "On" : "Off");
            btnHotkeys.Text = "3. Numerical hotkeys in dialog boxes: " + (hotkeys ? "Enabled" : "Disabled");
            btnDynamicBackgrounds.Text = "4. Dynamic Backgrounds: " + (backgrounds ? "Enabled" : "Disabled");
            btnAchievePopups.Text = "5. Achievement Popups: " + (achievementPopups ? "Enabled" : "Disabled");
            btnWindowFocusPause.Text = "6. Window Focus Auto-Pause: " + (autoPause ? "On" : "Off");
        }

        public void OnDeactivate()
        {
            // Unmodal our window
            Root.Singleton.Canvas.ModalFocus = null;

            // Remove our controls
            pnObscure.Remove();
            pnWindow.Remove();
        }

        public void Think(float delta)
        {
            // Check for escape
            if (finishnow)
            {
                // Store new settings
                Root.Singleton.Settings.WriteInt("Video", "ResX", currentres.Width);
                Root.Singleton.Settings.WriteInt("Video", "ResY", currentres.Height);
                Root.Singleton.Settings.WriteInt("Video", "Fullscreen", currentfs ? 1 : 0);
                Root.Singleton.Settings.WriteInt("Video", "Hotkeys", hotkeys ? 1 : 0);
                Root.Singleton.Settings.WriteInt("Video", "Backgrounds", backgrounds ? 1 : 0);
                Root.Singleton.Settings.WriteInt("Video", "AchievementPopups", achievementPopups ? 1 : 0);
                Root.Singleton.Settings.WriteInt("Video", "AutoPause", autoPause ? 1 : 0);
                Root.Singleton.Settings.Save();

                // Reset window if needed
                if (windowresetneeded)
                {
                    Root.Singleton.ResetWindow();
                }

                // Close state
                Root.Singleton.mgrState.Deactivate<OptionsMenu>();

                // Reopen main menu
                Root.Singleton.mgrState.FSMTransist<MainMenu>();
            }
            
        }

        public void Render(RenderStage stage)
        {
            
        }
    }
}
