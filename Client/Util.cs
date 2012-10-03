using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using SFML.Window;
using SFML.Graphics;

namespace FTLOverdrive.Client
{
    public static class Util
    {
        public static Vector2f Scale(Sprite spr, Vector2f pixelsize, float mult = 1.0f)
        {
            return new Vector2f(pixelsize.X / (float)spr.TextureRect.Width, pixelsize.Y / (float)spr.TextureRect.Height) * mult;
        }

        public static IntRect ScreenRect(uint w, uint h, float aspect)
        {
            float a = w / (float)h;
            if (a > aspect)
            {
                // Screen is too wide
                int neww = (int)(aspect * h);
                return new IntRect((int)(w / 2) - (neww / 2), 0, neww, (int)h);
            }
            else if (a < aspect)
            {
                // Screen is too tall
                int newh = (int)(w / aspect);
                return new IntRect(0, (int)(h / 2) - (newh / 2), (int)w, newh);
            }
            else
                return new IntRect(0, 0, (int)w, (int)h);
        }

        public static void LayoutControl(UI.Control ctrl, int x, int y, int w, int h, IntRect screenrect)
        {
            // x,y,w,h are relative to 1280x720
            ctrl.X = (int)((x / 1280.0f) * screenrect.Width);
            ctrl.Y = (int)((y / 720.0f) * screenrect.Height);
            ctrl.Width = (int)((w / 1280.0f) * screenrect.Width);
            ctrl.Height = (int)((h / 720.0f) * screenrect.Height);
        }

        public static void LayoutSprite(Sprite sprite, int x, int y, int w, int h, IntRect screenrect)
        {
            // x,y,w,h are relative to 1280x720
            sprite.Position = new Vector2f((x / 1280.0f) * screenrect.Width, (y / 720.0f) * screenrect.Height);
            sprite.Scale = new Vector2f(((w / 1280.0f) * screenrect.Width) / sprite.Texture.Size.X, ((h / 720.0f) * screenrect.Height) / sprite.Texture.Size.Y);
        }

        public static string LocateFTLPath()
        {
            // First, locate program files
            string progfiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86).Replace('\\', '/');
            if (!Directory.Exists(progfiles + "/")) return null;

            if (Directory.Exists(progfiles + "/FTL/"))
            {
                return progfiles + "/FTL/";
            }

            // Next, locate steam
            if (!Directory.Exists(progfiles + "/Steam/")) return null;

            // Next, locate FTL
            if (!Directory.Exists(progfiles + "/Steam/steamapps/common/FTL Faster Than Light/")) return null;

            // Done
            return progfiles + "/Steam/steamapps/common/FTL Faster Than Light/";
        }

    }
}
