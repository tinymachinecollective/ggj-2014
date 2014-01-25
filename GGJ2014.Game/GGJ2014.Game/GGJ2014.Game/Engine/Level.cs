
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
        private List<Layer> layers = new List<Layer>();

        public Level()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public Vector2 FindSpawnPoint(bool playerSafe)
        {
            return Vector2.Zero;
        }

        public void AddLayer(Layer layer)
        {
            this.layers.Add(layer);
        }

        public Layer GetLayer(int layerIndex)
        {
            return this.layers[layerIndex];
        }

        public int LayerCount { get { return this.layers.Count; } }

        public void Load()
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<Layer>));

            using (var fileStream = File.Open("level.xml", FileMode.Open))
            {
                this.layers = xs.Deserialize(fileStream) as List<Layer>;

                foreach (var layer in layers)
                {
                    layer.Initialize();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPos)
        {
            for (int i = 0; i < this.layers.Count; i++)
            {
                this.layers[i].Draw(spriteBatch, cameraPos);
            }
        }
    }
}
