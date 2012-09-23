using System;
using System.Collections.Generic;

using SFML.Window;
using SFML.Graphics;
using SFML.Audio;

using FTLOverdrive.Data;

namespace FTLOverdrive.Client
{
    public class Root
    {
        #region Singleton

        private static Root _singleton;
        public static Root Singleton
        {
            get
            {
                if (_singleton == null)
                {
                    _singleton = new Root();
                }
                return _singleton;
            }
        }

        #endregion

        public bool Exiting { get; set; }

        private RenderWindow window;
        public RenderWindow Window { get { return window; } }

        public SettingsFile Settings { get; private set; }

        private Archive archive;

        private Dictionary<string, Texture> dctMaterials;
        private Dictionary<string, Music> dctMusic;
        private Dictionary<string, SoundBuffer> dctSound;
        private Dictionary<string, Font> dctFonts;

        public Gamestate.StateController mgrState { get; private set; }

        public UI.Canvas Canvas { get; private set; }

        private const string DIRECTORY = "C:/Program Files (x86)/Steam/steamapps/common/FTL Faster Than Light/";

        #region Logging

        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        #endregion

        #region Resources

        public System.IO.Stream Resource(string filename)
        {
            var file = archive[filename];
            if (file == null) return null;
            return archive[file];
        }

        public Texture Material(string filename)
        {
            if (dctMaterials.ContainsKey(filename)) return dctMaterials[filename];
            var res = Resource(filename);
            if (res == null) return null;
            var tex = new Texture(res);
            dctMaterials.Add(filename, tex);
            return tex;
        }

        public Music Music(string filename)
        {
            if (dctMusic.ContainsKey(filename)) return dctMusic[filename];
            var res = Resource(filename);
            if (res == null) return null;
            var music = new Music(res);
            dctMusic.Add(filename, music);
            return music;
        }

        public SoundBuffer Sound(string filename)
        {
            if (dctSound.ContainsKey(filename)) return dctSound[filename];
            var res = Resource(filename);
            if (res == null) return null;
            var snd = new SoundBuffer(res);
            dctSound.Add(filename, snd);
            return snd;
        }

        public Font Font(string filename)
        {
            if (dctFonts.ContainsKey(filename)) return dctFonts[filename];
            var res = Resource(filename);
            if (res == null) return null;
            var fnt = new Font(res);
            dctFonts.Add(filename, fnt);
            return fnt;
        }

        #endregion

        #region Core Methods

        public void Init()
        {
            // Not exiting
            Exiting = false;

            // Startup message
            Log("*****************");
            Log("* FTL Overdrive *");
            Log("*****************");

            // Load settings
            Log("Loading settings...");
            Settings = new SettingsFile("settings.ini");
            Settings.SetPopulate(true);

            // Init file system
            Log("Mounting resources.dat...");
            archive = new Archive(DIRECTORY + "resources/resource.dat");

            // Init window
            Log("Loading window...");
            LoadWindow();

            // Init resources
            dctMaterials = new Dictionary<string, Texture>();
            dctMusic = new Dictionary<string, Music>();
            dctSound = new Dictionary<string, SoundBuffer>();
            dctFonts = new Dictionary<string, SFML.Graphics.Font>();

            // Init state manager
            mgrState = new Gamestate.StateController();

            // Init mods
            Log("Loading modding API...");
            mgrState.Activate<Gamestate.Library>();
            mgrState.Activate<Gamestate.ModdingAPI>();
            Log("Loading mods...");
            mgrState.Get<Gamestate.ModdingAPI>().LoadMods();

            // Load library
            mgrState.Get<Gamestate.ModdingAPI>().CallHook("Game.LoadLibrary");

            // Main menu state
            Log("Starting game...");
            mgrState.FSMTransist<Gamestate.MainMenu>();

            // Save settings
            Settings.Save();
            Settings.SetPopulate(false);
        }

        public void ResetWindow()
        {
            // Close the old window
            window.Close();
            window.Dispose();
            window = null;

            // Reset canvas
            Canvas.Clear();
            Canvas = null;

            // Load new one
            LoadWindow();
        }

        private void LoadWindow()
        {
            // Determine settings
            var vmode = new VideoMode(
                (uint)Settings.ReadInt("Video", "ResX", 1024),
                (uint)Settings.ReadInt("Video", "ResY", 600),
                24);
            bool fscreen = Settings.ReadInt("Video", "Fullscreen", 0) == 1;
            bool vsync = Settings.ReadInt("Video", "VSync", 1) == 1;

            // Setup the new window
            window = new RenderWindow(vmode, "FTL: Overdrive", fscreen ? Styles.Fullscreen : Styles.Default, new ContextSettings(24, 8, 8));
            window.SetVisible(true);
            window.SetVerticalSyncEnabled(vsync);
            window.MouseMoved += new EventHandler<MouseMoveEventArgs>(window_MouseMoved);
            window.Closed += new EventHandler(window_Closed);
            window.MouseButtonPressed += new EventHandler<MouseButtonEventArgs>(window_MouseButtonPressed);
            window.MouseButtonReleased += new EventHandler<MouseButtonEventArgs>(window_MouseButtonReleased);

            // Init UI
            Canvas = new UI.Canvas();
            var screenrect = Util.ScreenRect(window.Size.X, window.Size.Y, 1.77778f);
            Canvas.X = screenrect.Left;
            Canvas.Y = screenrect.Top;
            Canvas.Width = screenrect.Width;
            Canvas.Height = screenrect.Height;

            // Load icon
            using (var bmp = new System.Drawing.Bitmap(Resource("img/exe_icon.bmp")))
            {
                byte[] data = new byte[bmp.Width * bmp.Height * 4];
                int i = 0;
                for (int y = 0; y < bmp.Height; y++)
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        var c = bmp.GetPixel(x, y);
                        data[i++] = c.R;
                        data[i++] = c.G;
                        data[i++] = c.B;
                        data[i++] = c.A;
                    }
                window.SetIcon((uint)bmp.Width, (uint)bmp.Height, data);
            }
        }

        public void Run()
        {
            // Loop until done
            while (!Exiting)
            {
                // Do frame
                PerformFrame();

                // Sleep
                System.Threading.Thread.Sleep(10);
            }
        }

        private void PerformFrame()
        {
            // Update window
            window.DispatchEvents();

            // Think on states
            mgrState.Think(0.0f);

            // Clear
            window.Clear(Color.Black);

            // Draw
            mgrState.Render(Gamestate.RenderStage.PREGUI);
            Canvas.Render(Window);
            mgrState.Render(Gamestate.RenderStage.POSTGUI);

            // Render window
            window.Display();
        }

        public void Shutdown()
        {
            // Close window
            window.Close();
            window.Dispose();
            window = null;
        }

        #endregion

        #region Window Events

        private void window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left) Canvas.MouseClickLeft(false);
        }

        private void window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left) Canvas.MouseClickLeft(true);
        }

        private void window_Closed(object sender, EventArgs e)
        {
            Exiting = true;
        }

        private void window_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            Canvas.MouseMove(e.X, e.Y);
        }

        #endregion
    }
}
