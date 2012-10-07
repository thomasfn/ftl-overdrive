using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using FTLOverdrive.Client.UI;
using SFML.Window;
using FTLOverdrive.Client.Map;

namespace FTLOverdrive.Client.Gamestate.FSM
{
    class SectorMapScreen : IState, IRenderable
    {
        private RenderWindow window;
        private IntRect rctScreen;

        private Panel pnObscure;
        private ImagePanel pnWindow;
        private bool finishnow;

        private static readonly Color colCivilian = new Color(135, 200, 75);
        private static readonly Color colHostile = new Color(215, 50, 50);
        private static readonly Color colNebula = new Color(128, 50, 210);

        public SectorMap Map { get; set; }

        private class SectorMapPanel : Control
        {
            private SectorMap sectorMap;

            public SectorMapPanel(SectorMap sectorMap)
            {
                this.sectorMap = sectorMap;
            }

            protected override void Draw(RenderWindow window)
            {
                base.Draw(window);

                //var rect = new RectangleShape(new Vector2f(Width, Height));
                //rect.Position = new Vector2f(AbsX, AbsY);
                //window.Draw(rect);

                foreach (var node in sectorMap.Nodes)
                {
                    foreach (var nextNode in node.NextNodes)
                    {
                        var vertices = new Vertex[2];
                        vertices[0] = new Vertex(new Vector2f(node.X + AbsX, node.Y + AbsY));
                        vertices[1] = new Vertex(new Vector2f(nextNode.X + AbsX, nextNode.Y + AbsY));
                        window.Draw(vertices, PrimitiveType.Lines);
                    }

                    var shpCircle = new CircleShape();
                    shpCircle.Radius = 7;
                    shpCircle.Origin = new Vector2f(shpCircle.Radius, shpCircle.Radius);
                    shpCircle.Position = new Vector2f(node.X + AbsX, node.Y + AbsY);

                    //TODO make colors less neon
                    switch (node.Type)
                    {
                        case SectorMap.SectorType.Civilian:
                            shpCircle.FillColor = colCivilian;
                            break;
                        case SectorMap.SectorType.Hostile:
                            shpCircle.FillColor = colHostile;
                            break;
                        case SectorMap.SectorType.Nebula:
                            shpCircle.FillColor = colNebula;
                            break;
                    }
                    shpCircle.OutlineColor = Color.White;
                    shpCircle.OutlineThickness = 1;
                    window.Draw(shpCircle);
                }
            }
        }

        public void OnActivate()
        {
            // Store window
            window = Root.Singleton.Window;
            rctScreen = Util.ScreenRect(window.Size.X, window.Size.Y, 1.7778f);
            finishnow = false;
            window.KeyPressed += window_KeyPressed;

            // Create UI
            pnObscure = new Panel();
            pnObscure.Colour = new Color(0, 0, 0, 192);
            Util.LayoutControl(pnObscure, 0, 0, 1280, 720, rctScreen);
            pnObscure.Parent = Root.Singleton.Canvas;
            pnObscure.Init();

            pnWindow = new ImagePanel();
            pnWindow.Image = Root.Singleton.Material("img/box_text_sectors.png");
            Util.LayoutControl(pnWindow, (int)(1280 - pnWindow.Image.Size.X) / 2,
                                         (int)(720 - pnWindow.Image.Size.Y) / 2,
                                         (int)pnWindow.Image.Size.X,
                                         (int)pnWindow.Image.Size.Y,
                                         rctScreen);
            pnWindow.Parent = Root.Singleton.Canvas;
            pnWindow.Init();

            var lblTitle = new Label();
            lblTitle.X = 25;
            lblTitle.Y = 40;
            lblTitle.Text = "Choose the next sector:";
            lblTitle.Font = Root.Singleton.Font("fonts/JustinFont12Bold.ttf");
            lblTitle.AutoScale = false;
            lblTitle.Scale = 0.475f;
            lblTitle.Parent = pnWindow;
            lblTitle.Init();

            var nodes = Map.CurrentNode.NextNodes;
            for (int i = 0; i < nodes.Count; i++) {
                var node = nodes[i];
                AddButton(80 + i * 30, "" + (i + 1) + ". " + node.Name);
            }

            var map = new SectorMapPanel(Map);
            Util.LayoutControl(map, 28, 210, 558, 148, rctScreen);
            map.Parent = pnWindow;
            map.Init();

            // Modal screen
            Root.Singleton.Canvas.ModalFocus = pnWindow;
        }

        private TextButton AddButton(int y, string text)
        {
            var btn = new TextButton();
            btn.Font = Root.Singleton.Font("fonts/JustinFont12Bold.ttf");
            btn.Scale = 0.475f;
            btn.X = 25;
            btn.Y = y;
            btn.Text = text;
            btn.Parent = pnWindow;
            btn.Init();
            return btn;
        }

        public void Think(float delta)
        {
            // Check for escape
            if (finishnow)
            {
                // Close state
                Root.Singleton.mgrState.Deactivate<SectorMapScreen>();

                // Reopen hangar
                Root.Singleton.mgrState.FSMTransist<NewGame>();
            }
        }

        private void window_KeyPressed(object sender, KeyEventArgs e)
        {
            // Finish if escape
            if (e.Code == Keyboard.Key.Escape) finishnow = true;
        }

        public void OnDeactivate()
        {
            // Unmodal our window
            Root.Singleton.Canvas.ModalFocus = null;

            // Remove our controls
            pnObscure.Remove();
            pnWindow.Remove();
        }

        public void Render(RenderStage stage)
        {
        }
    }
}
