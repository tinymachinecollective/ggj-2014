using System.Collections.Generic;
using System.IO;
using GGJ2014.Game.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GGJ2014.Game.Editor
{
    public class Placer
    {
        private Vector2 position;
        private List<TileSet> tilesets = new List<TileSet>();
        private List<Layer> layers = new List<Layer>();

        private int currentIndex;
        private int currentTileSet;
        private int currentLayer;

        private bool placed = false;

        public void Initialize()
        {
            string tileDefinitionFile = "image_index.csv";

            foreach (string tileDef in File.ReadAllLines(tileDefinitionFile))
            {
                string[] components = tileDef.Split(',');
                tilesets.Add(new TileSet()
                {
                    Name = components[0],
                    SourceAsset = BigEvilStatic.Content.Load<Texture2D>(components[0]),
                    TileWidth = int.Parse(components[1]),
                    TileHeight = int.Parse(components[2])
                });
            }

            this.layers.Add(new Layer()
            {
                TileSet = tilesets[0].Name,
                TileSetHeight = tilesets[0].TileHeight,
                TileSetWidth = tilesets[0].TileWidth
            });

            this.layers[0].Initialize();
        }

        public void Update()
        {
            this.position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            if (!placed && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                this.layers[currentLayer].AddTile(position, currentIndex);

                placed = true;
            }
            else
            {
                placed = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.layers.Count; i++)
            {
                this.layers[i].Draw(spriteBatch);

                if (i == currentLayer)
                {
                    this.layers[i].DrawTile(spriteBatch, new Layer.Tile() { Index = currentIndex, Position = position }, Color.Red);
                }
            }
        }
    }
}
