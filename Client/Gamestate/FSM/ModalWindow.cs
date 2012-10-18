using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTLOverdrive.Client.UI;
using SFML.Graphics;
using SFML.Window;

namespace FTLOverdrive.Client.Gamestate.FSM
{
    public abstract class ModalWindow<Parent> : IState, IRenderable where Parent : IState
    {
        private RenderWindow window;
        protected IntRect ScreenRectangle { get; set; }

        private Panel pnObscure;
        protected ImagePanel Window { get; set; }

        protected Texture BackgroundImage { get; set; }

        protected bool Finish { get; set; }

        public virtual void OnActivate()
        {
            // Store window
            window = Root.Singleton.Window;
            ScreenRectangle = Util.ScreenRect(window.Size.X, window.Size.Y, 1.7778f);
            Finish = false;
            window.KeyPressed += OnKeyPressed;

            // Create UI
            pnObscure = new Panel();
            pnObscure.Colour = new Color(0, 0, 0, 192);
            Util.LayoutControl(pnObscure, 0, 0, 1280, 720, ScreenRectangle);
            pnObscure.Parent = Root.Singleton.Canvas;
            pnObscure.Init();

            Window = new ImagePanel();
            Window.Image = BackgroundImage;
            Util.LayoutControl(Window, (new Vector2u(1280U, 720U) - Window.Image.Size) / 2, Window.Image.Size, ScreenRectangle);
            Window.Parent = Root.Singleton.Canvas;
            Window.Init();

            // Modal screen
            Root.Singleton.Canvas.ModalFocus = Window;
        }

        public virtual void OnKeyPressed(object sender, KeyEventArgs e)
        {
            // Finish if escape
            if (e.Code == Keyboard.Key.Escape) Finish = true;
        }

        public virtual void Think(float delta)
        {
            // Check for escape
            if (Finish)
            {
                // Close state
                Root.Singleton.mgrState.Deactivate(this);

                // Reopen parent
                Root.Singleton.mgrState.FSMTransist<Parent>();
            }
        }

        public virtual void OnDeactivate()
        {
            // Unmodal our window
            Root.Singleton.Canvas.ModalFocus = null;

            // Remove our controls
            pnObscure.Remove();
            Window.Remove();
        }

        public virtual void Render(RenderStage stage)
        {
        }
    }
}
