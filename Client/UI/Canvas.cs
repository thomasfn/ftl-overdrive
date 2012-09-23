using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTLOverdrive.Client.UI
{
    public class Canvas : Control
    {
        private Control hovered;

        private Control modelfocus;
        public Control ModalFocus
        {
            get { return modelfocus; }
            set
            {
                modelfocus = value;
                if (hovered != null)
                {
                    if (hovered.Pressed)
                        hovered.SetPressed(false, true);
                    hovered.SetHovered(false);
                    hovered = null;
                }
            }
        }

        public void MouseMove(int absx, int absy)
        {
            if (hovered != null && hovered.Removed) hovered = null;
            if (modelfocus != null && modelfocus.Removed) modelfocus = null;
            int mx = absx - X;
            int my = absy - Y;
            Control h = null;
            if (modelfocus != null)
            {
                mx -= modelfocus.AbsX;
                my -= modelfocus.AbsY;
                h = modelfocus.TraceRelative(mx, my);
            }
            else
                h = TraceRelative(mx, my);
            if (h != hovered)
            {
                bool pressed = false;
                if (hovered != null)
                {
                    if (hovered.Pressed)
                    {
                        hovered.SetPressed(false, true);
                        pressed = true;
                    }
                    hovered.SetHovered(false);
                }
                hovered = h;
                if (hovered != null)
                {
                    hovered.SetHovered(true);
                    if (pressed) hovered.SetPressed(true, true);
                }
            }
        }

        public void MouseClickLeft(bool pressed)
        {
            if (hovered != null && hovered.Removed) hovered = null;
            if (modelfocus != null && modelfocus.Removed) modelfocus = null;
            if (hovered != null) hovered.SetPressed(pressed, false);
        }


    }
}
