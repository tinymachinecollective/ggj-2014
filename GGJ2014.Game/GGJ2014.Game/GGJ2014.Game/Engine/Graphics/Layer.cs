using System.Collections.Generic;
using GGJ2014.Game.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

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

        private Rectangle GetSourceRectangle(int index, SpriteBatch spriteBatch = null)
        {
            int tilesPerRow = texture2d.Width / TileSetWidth;
            int row = index / tilesPerRow;
            int column = index % tilesPerRow;

            return new Rectangle(column * TileSetWidth, row * TileSetHeight, TileSetWidth, TileSetHeight);
        }

        public int MaxIndex()
        {
            return (texture2d.Width / TileSetWidth) * (texture2d.Height / TileSetHeight) - 1;
        }


        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPos, Color color)
        {
            this.Tiles.Sort(new TileComp());
            foreach (var tile in this.Tiles)
            {
                this.DrawTile(spriteBatch, tile, color, cameraPos);
            }
        }

        public void DrawTile(SpriteBatch spriteBatch, Tile tile, Color color, Vector2 cameraPos)
        {

            spriteBatch.Draw(
                texture2d,
                new Rectangle((int)(tile.Position.X - cameraPos.X), (int)(tile.Position.Y - cameraPos.Y), TileSetWidth, TileSetHeight),
                GetSourceRectangle(tile.Index, spriteBatch),
                //GetSourceRectangle(tile.Index),
                color,
                0f,
                new Vector2(TileSetWidth / 2f, TileSetHeight / 2f),
                SpriteEffects.None,
                1f);

            //  display index on tile
            spriteBatch.DrawString(BigEvilStatic.GetDefaultFont(),
                "" + tile.Index,
                new Vector2(tile.Position.X - cameraPos.X - TileSetWidth / 4f, tile.Position.Y - cameraPos.Y - TileSetHeight / 4f),
                Color.White);

        }

        public class Tile
        {
            public int Index;
            public Vector2 Position;
        }

        public class TileComp : Comparer<Tile>
        {

            public override int Compare(Tile a, Tile b)
            {
                if (a.Position.Y.CompareTo(b.Position.Y) != 0)
                {
                    return a.Position.Y.CompareTo(b.Position.Y);
                }
                else if (a.Position.X.CompareTo(b.Position.X) != 0)
                {
                    return a.Position.X.CompareTo(b.Position.X);
                }
                else
                {
                    return 0;
                }
                    /*
                if (a.Position.X < b.Position.X) 
                {
                    if (a.Position.Y > b.Position.Y) return 1;
                    else return -1;
                }
                else if (a.Position.X > b.Position.X)
                {
                    if (a.Position.Y < b.Position.Y) return -1;
                    else return 1;
                }
                return 0;
                     */
            }
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
