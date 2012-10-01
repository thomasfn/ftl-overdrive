using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SFML.Window;

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

        private Control focus;
        public Control Focus
        {
            get { return focus; }
            set
            {
                if (value == this) value = null;
                if (focus == value) return;
                if (focus != null) focus.FocusLost();
                focus = value;
                if (focus != null) focus.FocusGained();
            }
        }

        private void CheckState()
        {
            if (hovered != null && hovered.Removed) hovered = null;
            if (modelfocus != null && modelfocus.Removed) modelfocus = null;
            if (focus != null && focus.Removed) focus = null;
        }

        public void MouseMove(int absx, int absy)
        {
            CheckState();
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

        public override void KeyPress(Keyboard.Key key, bool pressed)
        {
            if (Focus == null) return;
            focus.KeyPress(key, pressed);
        }

        public override void TextEntered(string txt)
        {
            if (Focus == null) return;
            focus.TextEntered(txt);
        }

        public void MouseClickLeft(bool pressed)
        {
            CheckState();
            Focus = hovered;
            if (hovered != null)
            {
                hovered.SetPressed(pressed, false);
            }
            
        }


    }
}
