using System;
using System.Collections.Generic;

using SFML.Window;

namespace FTLOverdrive.Client.UI
{
    public class TextEntry : Label
    {
        public bool EditMode { get; set; }

        private int caretpos;
        public int CaretPosition
        {
            get { return caretpos; }
            set
            {
                if (value < 0) value = 0;
                if (value > Text.Length) value = Text.Length;
                caretpos = value;
            }
        }

        private bool capsmode;

        public override void KeyPress(SFML.Window.Keyboard.Key key, bool pressed)
        {
            if (!EditMode) return;
            switch (key)
            {
                case Keyboard.Key.Left:
                    if (pressed) CaretPosition--;
                    break;
                case Keyboard.Key.Right:
                    if (pressed) CaretPosition++;
                    break;
                case Keyboard.Key.LShift:
                    capsmode = pressed;
                    break;
                case Keyboard.Key.End:
                    CaretPosition = Text.Length;
                    break;
                case Keyboard.Key.Home:
                    CaretPosition = 0;
                    break;
            }
        }

        public override void TextEntered(string txt)
        {
            if (!EditMode) return;
            if (txt[0] == '\r')
            {
                EditMode = false;
                return;
            }
            string left = Text.Substring(0, CaretPosition);
            string right = Text.Substring(CaretPosition);
            if (txt[0] == '\b')
            {
                if (left.Length > 0)
                {
                    Text = left.Substring(0, left.Length - 1) + right;
                    CaretPosition--;
                }
                return;
            }
            Text = left + txt + right;
            CaretPosition++;
        }

        public override void FocusLost()
        {
            capsmode = false;
            EditMode = false;
        }

        protected override void Draw(SFML.Graphics.RenderWindow window)
        {
            base.Draw(window);

        }


    }
}
