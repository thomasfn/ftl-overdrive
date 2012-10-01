using System;
using System.Collections.Generic;

using FTLOverdrive.Client.Gamestate;

using SFML.Graphics;
using SFML.Window;

namespace FTLOverdrive.Client.Ship
{
    public class Interior
    {
        public class Tile
        {
            public enum WallState { None, OuterWall, InnerWall, Door }

            public Library.Room Owner { get; set; }

            public WallState Up { get; set; }
            public WallState Left { get; set; }

            public bool Solid { get; set; }
        }

        private Tile[,] tiles;

        private Library.Ship ship;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Tile this[int x, int y]
        {
            get
            {
                return tiles[x, y];
            }
        }

        public Interior(Library.Ship ship)
        {
            this.ship = ship;

            // Determine the size
            int maxX = 0, maxY = 0;
            foreach (var room in ship.Rooms)
            {
                if (room.MaxX > maxX) maxX = room.MaxX;
                if (room.MaxY > maxY) maxY = room.MaxY;
            }
            Width = maxX;
            Height = maxY;

            // Create the tilesheet
            tiles = new Tile[Width + 1, Height + 1];

            // Populate the tiles
            foreach (var room in ship.Rooms)
                for (int x = room.MinX - 1; x <= room.MaxX - 1; x++)
                    for (int y = room.MinY - 1; y <= room.MaxY - 1; y++)
                    {
                        var tile = new Tile();
                        tile.Owner = room;
                        tile.Solid = true;
                        tile.Left = Tile.WallState.None;
                        tile.Up = Tile.WallState.None;
                        if (x == room.MinX - 1) tile.Left = Tile.WallState.InnerWall;
                        if (y == room.MinY - 1) tile.Up = Tile.WallState.InnerWall;
                        tiles[x, y] = tile;
                    }
            for (int x = 1; x <= Width; x++)
                for (int y = 1; y <= Height; y++)
                {
                    if (tiles[x, y] == null)
                    {
                        bool left = (tiles[x - 1, y] != null) && tiles[x - 1, y].Solid;
                        bool up = tiles[x, y - 1] != null && tiles[x, y - 1].Solid;
                        if (left || up)
                        {
                            var tile = new Tile();
                            tile.Owner = null;
                            tile.Solid = false;
                            tile.Left = left ? Tile.WallState.InnerWall : Tile.WallState.None;
                            tile.Up = up ? Tile.WallState.InnerWall : Tile.WallState.None;
                            tiles[x, y] = tile;
                        }
                    }
                }
            for (int x = 0; x <= Width; x++)
                for (int y = 0; y <= Height; y++)
                {
                    if (tiles[x, y] != null)
                    {
                        var tile = tiles[x, y];
                        if (tile.Left == Tile.WallState.InnerWall)
                        {
                            if ((x == 0) || (tiles[x - 1, y] == null) || (!tiles[x - 1, y].Solid) || (!tile.Solid))
                                tile.Left = Tile.WallState.OuterWall;
                        }
                        if (tile.Up == Tile.WallState.InnerWall)
                        {
                            if ((y == 0) || (tiles[x, y - 1] == null) || (!tiles[x, y - 1].Solid) || (!tile.Solid))
                                tile.Up = Tile.WallState.OuterWall;
                        }
                    }
                }
            // TODO: Doors
        }

        private static RectangleShape shpLine = new RectangleShape();
        private static Sprite sprTexture = new Sprite();

        private static void DrawLine(RenderTexture target, Vector2f a, Vector2f b, Color col, int thickness = 1)
        {
            shpLine.Size = new Vector2f((float)Math.Sqrt((b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y)), thickness);
            shpLine.Origin = shpLine.Size * 0.5f;
            shpLine.Position = (a + b) * 0.5f;
            shpLine.Rotation = (float)(Math.Atan2(b.Y - a.Y, b.X - a.X) * 180.0 / Math.PI);
            shpLine.FillColor = col;
            target.Draw(shpLine);
        }

        private static void DrawQuad(RenderTexture target, Vector2f a, Vector2f b, Color col)
        {
            shpLine.Size = b - a;
            shpLine.Origin = shpLine.Size * 0.5f;
            shpLine.Position = (a + b) * 0.5f;
            shpLine.Rotation = 0.0f;
            shpLine.FillColor = col;
            target.Draw(shpLine);
        }

        private static void DrawTexture(RenderTarget target, Vector2f a, Vector2f b, Texture texture)
        {
            sprTexture.Texture = texture;
            sprTexture.Scale = Util.Scale(sprTexture, b - a);
            sprTexture.Origin = new Vector2f(texture.Size.X * 0.5f, texture.Size.Y * 0.5f);
            sprTexture.Position = (a + b) * 0.5f;
            sprTexture.Rotation = 0.0f;
            target.Draw(sprTexture);
        }

        public RenderTexture CreateRender(int w, int h)
        {
            // Create the RT
            var rt = new RenderTexture((uint)w, (uint)h);
            rt.Clear(new Color(0, 0, 0, 0));

            // Draw the floor
            DrawTexture(rt, new Vector2f(0.0f, 0.0f), new Vector2f(w, h), Root.Singleton.Material(ship.FloorGraphic, true));

            // Colours
            var colTile = new Color(228, 226, 216);
            var colWall = new Color(0, 0, 0);
            var colTileBorder = new Color(198, 196, 192);

            // Vectors
            var origin = new Vector2f(ship.FloorOffsetX, ship.FloorOffsetY);
            var tileX = new Vector2f(ship.TileSize, 0.0f);
            var tileY = new Vector2f(0.0f, ship.TileSize);

            // Loop each tile and draw backgrounds + tile borders
            for (int x = 0; x <= Width; x++)
                for (int y = 0; y <= Height; y++)
                {
                    // Get it
                    var tile = tiles[x, y];
                    if (tile != null)
                    {
                        // Get vectors
                        var corner = origin + (tileX * x) + (tileY * y);

                        // Check for solidity
                        if (tile.Solid)
                        {
                            // Draw background
                            DrawQuad(rt, corner, corner + tileX + tileY, colTile);
                        }

                        // Check for tile borders
                        if ((x > 0) && (tiles[x - 1, y] != null) && (tiles[x - 1, y].Solid))
                            DrawLine(rt, corner, corner + tileY, colTileBorder, 2);
                        if ((y > 0) && (tiles[x, y - 1] != null) && (tiles[x, y - 1].Solid))
                            DrawLine(rt, corner, corner + tileX, colTileBorder, 2);
                    }
                }

            // Loop each room and draw backgrounds
            /*foreach (var room in ship.Rooms)
            {
                if ((room.BackgroundGraphic != null) && (room.BackgroundGraphic != ""))
                {
                    var corner = origin + (tileX * (room.MinX - 1)) + (tileY * (room.MinY - 1));
                    DrawTexture(rt, corner, corner + (tileX * (room.MaxX - room.MinX + 1)) + (tileY * (room.MaxY - room.MinY + 1)), Root.Singleton.Material(room.BackgroundGraphic, true));
                }
            }*/

            // Loop each tile and draw walls
            for (int x = 0; x <= Width; x++)
                for (int y = 0; y <= Height; y++)
                {
                    // Get it
                    var tile = tiles[x, y];
                    if (tile != null)
                    {
                        // Get vectors
                        var corner = origin + (tileX * x) + (tileY * y);

                        // Check for walls
                        if (tile.Left == Tile.WallState.InnerWall)
                            DrawLine(rt, corner, corner + tileY, colWall, 4);
                        else if (tile.Left == Tile.WallState.OuterWall)
                            DrawLine(rt, corner, corner + tileY, colWall, 2);
                        if (tile.Up == Tile.WallState.InnerWall)
                            DrawLine(rt, corner, corner + tileX, colWall, 4);
                        else if (tile.Up == Tile.WallState.OuterWall)
                            DrawLine(rt, corner, corner + tileX, colWall, 2);
                    }
                }

            

            // Return
            rt.Display();
            return rt;
        }

    }
}
