﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using FTLOverdrive.Client.UI;
using SFML.Window;
using FTLOverdrive.Client.Map;

namespace FTLOverdrive.Client.Gamestate.FSM
{
    class SectorMapScreen : ModalWindow<NewGame>
    {
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
                        Vector2f start=new Vector2f(node.X + AbsX, node.Y + AbsY),end=new Vector2f(nextNode.X + AbsX, nextNode.Y + AbsY);
                        Vector2f center = new Vector2f(.5f*(start.X+end.X),.5f*(start.Y+end.Y));
                        float deltaX = end.X-start.X;
                        float deltaY = end.Y-start.Y;
                        float length = (float)Math.Sqrt(deltaX*deltaX+deltaY*deltaY);
                        RectangleShape shpRectangle = new RectangleShape(new Vector2f(length,2.0f));
                        shpRectangle.Position=start;
                        shpRectangle.Rotation=(float)Math.Atan2(deltaY,deltaX)*(float)(180.0/Math.PI);
                        shpRectangle.OutlineColor = Color.White;
                        shpRectangle.FillColor = Color.White;
                        window.Draw(shpRectangle);
                    }

                    var shpCircle = new CircleShape();
                    shpCircle.Radius = 7;
                    shpCircle.Origin = new Vector2f(shpCircle.Radius, shpCircle.Radius);
                    shpCircle.Position = new Vector2f(node.X + AbsX, node.Y + AbsY);

                    //TODO make colors less neon
                    shpCircle.FillColor = node.Sector.Color;
                    shpCircle.OutlineColor = Color.White;
                    shpCircle.OutlineThickness = 1;
                    window.Draw(shpCircle);
                }
            }
        }

        public override void OnActivate()
        {
            BackgroundImage = Root.Singleton.Material("img/box_text_sectors.png");
            base.OnActivate();

            var lblTitle = new Label();
            lblTitle.X = 25;
            lblTitle.Y = 40;
            lblTitle.Text = "Choose the next sector:";
            lblTitle.Font = Root.Singleton.Font("fonts/JustinFont12Bold.ttf");
            lblTitle.AutoScale = false;
            lblTitle.Scale = 0.475f;
            lblTitle.Parent = Window;
            lblTitle.Init();

            var nodes = Map.CurrentNode.NextNodes;
            for (int i = 0; i < nodes.Count; i++) {
                var node = nodes[i];
                AddButton(80 + i * 25, "" + (i + 1) + ". " + node.Sector.Name);
            }

            var map = new SectorMapPanel(Map);
            Util.LayoutControl(map, 28, 210, 558, 148, ScreenRectangle);
            map.Parent = Window;
            map.Init();
        }

        private TextButton AddButton(int y, string text)
        {
            var btn = new TextButton();
            btn.Font = Root.Singleton.Font("fonts/JustinFont12Bold.ttf");
            btn.Scale = 0.475f;
            btn.X = 25;
            btn.Y = y;
            btn.Text = text;
            btn.Parent = Window;
            btn.Init();
            return btn;
        }
    }
}
