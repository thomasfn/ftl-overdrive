using System;
using System.Collections.Generic;

using SFML.Graphics;
using SFML.Window;

namespace FTLOverdrive.Client.UI
{
    public abstract class Control
    {
        private List<Control> children;

        private Control _parent;
        public Control Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                if (_parent != null) _parent.children.Remove(this);
                _parent = value;
                if (_parent != null) _parent.children.Add(this);
            }
        }

        public bool Removed { get; private set; }

        private int x, y, w, h;

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                if (inited) UpdateLayout();
            }
        }
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
                if (inited) UpdateLayout();
            }
        }
        public int Width
        {
            get
            {
                return w;
            }
            set
            {
                w = value;
                if (inited) UpdateLayout();
            }
        }
        public int Height
        {
            get
            {
                return h;
            }
            set
            {
                h = value;
                if (inited) UpdateLayout();
            }
        }

        public int AbsX
        {
            get
            {
                if (Parent == null)
                    return X;
                else
                    return Parent.AbsX + X;
            }
        }
        public int AbsY
        {
            get
            {
                if (Parent == null)
                    return Y;
                else
                    return Parent.AbsY + Y;
            }
        }

        private bool inited;

        public bool Visible { get; set; }

        public bool Hovered { get; private set; }
        public bool Pressed { get; private set; }

        protected virtual bool RespondsToTrace { get { return true; } }

        public Control()
        {
            children = new List<Control>();
            Visible = true;
        }

        protected virtual void UpdateLayout()
        {

        }

        public virtual void Init()
        {
            inited = true;
            UpdateLayout();
        }

        protected virtual void Draw(RenderWindow window)
        {

        }

        protected virtual void Think()
        {

        }

        public void Render(RenderWindow window)
        {
            if (!Visible) return;
            Draw(window);
            for (int i = 0; i < children.Count; i++)
                children[i].Render(window);
        }

        public void Update()
        {
            Think();
            foreach (var c in children)
                c.Update();
        }

        public bool PointInsideRelative(int x, int y)
        {
            return (x >= 0) && (y >= 0) && (x <= Width) && (y <= Height);
        }

        public Control TraceRelative(int x, int y)
        {
            if (!PointInsideRelative(x, y)) return null;
            if (!RespondsToTrace) return null;
            foreach (var c in children)
            {
                var r = c.TraceRelative(x - c.X, y - c.Y);
                if (r != null) return r;
            }
            return this;
        }

        public virtual void SetHovered(bool hovered)
        {
            Hovered = hovered;
        }
        public virtual void SetPressed(bool pressed, bool mousemoveevent)
        {
            Pressed = pressed;
        }

        public virtual void Clear()
        {
            foreach (var c in children.ToArray())
                c.Remove();
            children.Clear();
        }
        public virtual void Remove()
        {
            Removed = true;
            Parent = null;
            Clear();
        }

        public virtual void FocusGained()
        {

        }
        public virtual void FocusLost()
        {

        }

        public virtual void KeyPress(Keyboard.Key key, bool pressed)
        {

        }
        public virtual void TextEntered(string txt)
        {

        }
        
    }
}
