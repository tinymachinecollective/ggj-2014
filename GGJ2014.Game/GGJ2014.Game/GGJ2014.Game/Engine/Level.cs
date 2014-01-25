﻿
namespace GGJ2014.Game.Engine
{
    using System;
    using System.Collections.Generic;
    using GGJ2014.Engine.Graphics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System.Xml.Serialization;
    using System.IO;

    public class Level
    {
        private static readonly Random rng = new Random();
        private bool drawWalkLayer;

        public List<Layer> Layers = new List<Layer>();
        public Layer WalkLayer = new Layer();

        public Level()
        {
            this.WalkLayer.Tiles = new List<Layer.Tile>();
            this.WalkLayer.TileSet = "grass_and_water";
            this.WalkLayer.TileSetWidth = 64;
            this.WalkLayer.TileSetHeight = 64;
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public void SetDrawsWalkLayer(bool drawWalkLayer)
        {
            this.drawWalkLayer = drawWalkLayer;
        }

        public Vector2 FindSpawnPoint(bool playerSafe)
        {
            return Vector2.Zero;
        }

        public void AddLayer(Layer layer)
        {
            this.Layers.Add(layer);
        }

        public Layer GetLayer(int layerIndex)
        {
            return this.Layers[layerIndex];
        }

        public int LayerCount { get { return this.Layers.Count; } }

        public static Level Load()
        {
            XmlSerializer xs = new XmlSerializer(typeof(Level));
            Level level = null;

            if (File.Exists("level.xml"))
            {
                using (var fileStream = File.Open("level.xml", FileMode.Open))
                {
                    level = xs.Deserialize(fileStream) as Level;

                    foreach (var layer in level.Layers)
                    {
                        layer.Initialize();
                    }

                    level.WalkLayer.Initialize();
                }
            }

            return level ?? new Level();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPos)
        {
            for (int i = 0; i < this.Layers.Count; i++)
            {
                this.Layers[i].Draw(spriteBatch, cameraPos, Color.White);
            }

            if (drawWalkLayer)
            {
                this.WalkLayer.Draw(spriteBatch, cameraPos, Color.Red);
            }
        }
    }
}
