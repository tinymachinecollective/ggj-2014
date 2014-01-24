using System.Collections.Generic;
using GGJ2014.Game.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ2014.Game.Editor
{
    public class Layer
    {
        public string TileSet;
        public int TileSetWidth;
        public int TileSetHeight;
        private List<Tile> Tiles = new List<Tile>();

        private Texture2D texture2d;

        public void Initialize()
        {
            this.texture2d = BigEvilStatic.Content.Load<Texture2D>("textures\\" + this.TileSet);
        }

        private Rectangle GetSourceRectangle(int index)
        {
            int tilesPerRow = texture2d.Width / TileSetWidth;
            int row = index / tilesPerRow;
            int column = index % tilesPerRow;

            return new Rectangle(row * TileSetWidth, column * TileSetHeight, TileSetWidth, TileSetHeight);
        }

        public int MaxIndex()
        {
            return (texture2d.Width / TileSetWidth) * (texture2d.Height / TileSetHeight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tile in this.Tiles)
            {
                this.DrawTile(spriteBatch, tile, Color.White);
            }
        }

        public void DrawTile(SpriteBatch spriteBatch, Tile tile, Color color)
        {
            spriteBatch.Draw(texture2d, new Rectangle((int)tile.Position.X, (int)tile.Position.Y, TileSetWidth, TileSetHeight), GetSourceRectangle(tile.Index),  color);
        }

        public class Tile
        {
            public int Index;
            public Vector2 Position;
        }

        public void AddTile(Vector2 position, int currentIndex)
        {
            this.Tiles.Add(new Tile()
            {
                Position = position,
                Index = currentIndex
            });
        }
    }
}
