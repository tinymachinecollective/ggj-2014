using System.Collections.Generic;
using GGJ2014.Game.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ2014.Engine.Graphics
{
    public class Layer
    {
        public string TileSet;
        public int TileSetWidth;
        public int TileSetHeight;
        public List<Tile> Tiles = new List<Tile>();

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

            return new Rectangle(column * TileSetHeight, row * TileSetWidth, TileSetWidth, TileSetHeight);
        }

        public int MaxIndex()
        {
            return (texture2d.Width / TileSetWidth) * (texture2d.Height / TileSetHeight) - 1;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPos, Color color)
        {
            foreach (var tile in this.Tiles)
            {
                this.DrawTile(spriteBatch, tile, color, cameraPos);
            }
        }

        public void DrawTile(SpriteBatch spriteBatch, Tile tile, Color color, Vector2 cameraPos)
        {
            int tileX = (int)(tile.Position.X);
            int tileY = (int)(tile.Position.Y);

            spriteBatch.Draw(
                texture2d,
                new Rectangle((int)(tile.Position.X - cameraPos.X), (int)(tile.Position.Y - cameraPos.Y), TileSetWidth, TileSetHeight),
                GetSourceRectangle(tile.Index),
                color,
                0f,
                new Vector2(TileSetWidth / 2f, TileSetHeight / 2f),
                SpriteEffects.None,
                1f);
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

        public void RemoveLastTile()
        {
            if (this.Tiles.Count > 0)
            {
                this.Tiles.RemoveAt(this.Tiles.Count - 1);
            }
        }
    }
}
