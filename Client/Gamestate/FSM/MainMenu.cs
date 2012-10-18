using System;
using System.Collections.Generic;

using SFML.Graphics;
using SFML.Window;
using SFML.Audio;

using FTLOverdrive.Client.UI;

namespace FTLOverdrive.Client.Gamestate.FSM
{
    public class MainMenu : IState, IRenderable
    {
        private RenderWindow window;
        private IntRect rctScreen;

        private Sprite sprBackground;

        private Music mscMenu;

        private SoundBuffer sndButtonHover;

        public void OnActivate()
        {
            // Store window
            window = Root.Singleton.Window;
            rctScreen = Util.ScreenRect(window.Size.X, window.Size.Y, 1.7778f);

            // Load sprites
            sprBackground = new Sprite(Root.Singleton.Material("img/main_menus/main_base2.png"));
            sprBackground.Position = new Vector2f(rctScreen.Left, rctScreen.Top);
            sprBackground.Scale = Util.Scale(sprBackground, new Vector2f(rctScreen.Width, rctScreen.Height));
            
            // Load audio
            mscMenu = Root.Singleton.Music("audio/music/bp_MUS_TitleScreen.ogg");
            sndButtonHover = Root.Singleton.Sound("audio/waves/ui/select_light1.wav");
            //mscMenu.Stop();
            //mscMenu.Play();

            // Load UI
			var btnContinue = new ImageButton();
            btnContinue.Image = Root.Singleton.Material("img/main_menus/continue_on.png");
            btnContinue.HoveredImage = Root.Singleton.Material("img/main_menus/continue_select2.png");
            btnContinue.DisabledImage = Root.Singleton.Material("img/main_menus/continue_off.png");
            btnContinue.Enabled = false;
            btnContinue.HoverSound = sndButtonHover;
            Util.LayoutControl(btnContinue, (int)(1200 - btnContinue.Image.Size.X), 260, btnContinue.Image.Size, rctScreen);
            btnContinue.Parent = Root.Singleton.Canvas;
            btnContinue.Init();

			var btnNewGame = new ImageButton();
            btnNewGame.Image = Root.Singleton.Material("img/main_menus/start_on.png");
            btnNewGame.HoveredImage = Root.Singleton.Material("img/main_menus/start_select2.png");
            btnNewGame.DisabledImage = Root.Singleton.Material("img/main_menus/start_off.png");
            btnNewGame.Enabled = true;
            btnNewGame.HoverSound = sndButtonHover;
            btnNewGame.OnClick += (sender) =>
            {
                Root.Singleton.mgrState.FSMTransist<NewGame>();
            };
            Util.LayoutControl(btnNewGame, (int)(1200 - btnNewGame.Image.Size.X), 320, btnNewGame.Image.Size, rctScreen);
            btnNewGame.Parent = Root.Singleton.Canvas;
            btnNewGame.Init();

			var btnTutorial = new ImageButton();
            btnTutorial.Image = Root.Singleton.Material("img/main_menus/tutorial_on.png");
            btnTutorial.HoveredImage = Root.Singleton.Material("img/main_menus/tutorial_select2.png");
            btnTutorial.DisabledImage = Root.Singleton.Material("img/main_menus/tutorial_off.png");
            btnTutorial.Enabled = false;
            btnTutorial.HoverSound = sndButtonHover;
            Util.LayoutControl(btnTutorial, (int)(1200 - btnTutorial.Image.Size.X), 380, btnTutorial.Image.Size, rctScreen);
            btnTutorial.Parent = Root.Singleton.Canvas;
            btnTutorial.Init();

			var btnStats = new ImageButton();
            btnStats.Image = Root.Singleton.Material("img/main_menus/stats_on.png");
            btnStats.HoveredImage = Root.Singleton.Material("img/main_menus/stats_select2.png");
            btnStats.DisabledImage = Root.Singleton.Material("img/main_menus/stats_off.png");
            btnStats.Enabled = false;
            btnStats.HoverSound = sndButtonHover;
            Util.LayoutControl(btnStats, (int)(1200 - btnStats.Image.Size.X), 440, btnStats.Image.Size, rctScreen);
            btnStats.Parent = Root.Singleton.Canvas;
            btnStats.Init();

			var btnOptions = new ImageButton();
            btnOptions.Image = Root.Singleton.Material("img/main_menus/options_on.png");
            btnOptions.HoveredImage = Root.Singleton.Material("img/main_menus/options_select2.png");
            btnOptions.DisabledImage = Root.Singleton.Material("img/main_menus/options_off.png");
            btnOptions.Enabled = true;
            btnOptions.HoverSound = sndButtonHover;
            btnOptions.OnClick += (sender) =>
            {
                Root.Singleton.mgrState.Activate<OptionsMenu>();
            };
            Util.LayoutControl(btnOptions, (int)(1200 - btnOptions.Image.Size.X), 500, btnOptions.Image.Size, rctScreen);
            btnOptions.Parent = Root.Singleton.Canvas;
            btnOptions.Init();

			var btnCredits = new ImageButton();
            btnCredits.Image = Root.Singleton.Material("img/main_menus/credits_on.png");
            btnCredits.HoveredImage = Root.Singleton.Material("img/main_menus/credits_select2.png");
            btnCredits.DisabledImage = Root.Singleton.Material("img/main_menus/credits_off.png");
            btnCredits.Enabled = false;
            btnCredits.HoverSound = sndButtonHover;
            Util.LayoutControl(btnCredits, (int)(1200 - btnCredits.Image.Size.X), 560, btnCredits.Image.Size, rctScreen);
            btnCredits.Parent = Root.Singleton.Canvas;
            btnCredits.Init();

			var btnQuit = new ImageButton();
            btnQuit.Image = Root.Singleton.Material("img/main_menus/quit_on.png");
            btnQuit.HoveredImage = Root.Singleton.Material("img/main_menus/quit_select2.png");
            btnQuit.DisabledImage = Root.Singleton.Material("img/main_menus/quit_off.png");
            btnQuit.Enabled = true;
            btnQuit.HoverSound = sndButtonHover;
            btnQuit.OnClick += (sender) => { Root.Singleton.Exiting = true; };
            Util.LayoutControl(btnQuit, (int)(1200 - btnQuit.Image.Size.X), 620, btnQuit.Image.Size, rctScreen);
            btnQuit.Parent = Root.Singleton.Canvas;
            btnQuit.Init();
        }

        public void OnDeactivate()
        {
            Root.Singleton.Canvas.Clear();
        }

        public void Think(float delta)
        {
            
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
