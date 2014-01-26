using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using GGJ2014.Engine.Graphics;
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
        private Level level = new Level();

        private bool spawned = false;

        private int currentIndex;
        private int currentTileSet;
        private int currentLayer;

        private bool snap = true;
        private bool placed = false;
        private bool keyPressed = false;
        private bool cameraMoving = false;
        private bool saved = false;
        Vector2 lastMousePos;
        Vector2 cameraPos;

        private bool editWalkLayer;

        public void Initialize()
        {
            string tileDefinitionFile = "Content\\textures\\image_index.csv";

            foreach (string tileDef in File.ReadAllLines(tileDefinitionFile))
            {
                string[] components = tileDef.Split(',');
                tilesets.Add(new TileSet()
                {
                    Name = components[0],
                    TileWidth = int.Parse(components[1]),
                    TileHeight = int.Parse(components[2])
                });
            }

            this.AddLayer();
        }

        private void AddLayer()
        {
            var layer = new Layer()
            {
                TileSet = tilesets[0].Name,
                TileSetHeight = tilesets[0].TileHeight,
                TileSetWidth = tilesets[0].TileWidth
            };

            layer.Initialize();
            this.level.AddLayer(layer);
        }

        public void Update()
        {
            int x = Mouse.GetState().X;
            int y = Mouse.GetState().Y;

            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                cameraPos += new Vector2(lastMousePos.X - x, lastMousePos.Y - y);
                Mouse.SetPosition((int)lastMousePos.X, (int)lastMousePos.Y);
                this.cameraMoving = true;
            }
            else
            {
                this.cameraMoving = false;
            }

            this.lastMousePos = new Vector2(x, y);

            if (this.snap)
            {
                int halfTexX = this.tilesets[this.currentTileSet].TileWidth / 4;
                int halfTexY = this.tilesets[this.currentTileSet].TileHeight / 4;
                x = x / halfTexX * halfTexX - ((int)cameraPos.X % halfTexX);
                y = y / halfTexY * halfTexY - ((int)cameraPos.Y % halfTexY);
            }

            this.position = new Vector2(x, y);
            var currentLayer = this.editWalkLayer ? this.level.WalkLayer : this.level.GetLayer(this.currentLayer);

            if (!placed && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                currentLayer.AddTile(position + cameraPos, currentIndex);
                placed = true;
            }
            else if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                placed = false;
            }

            if (!spawned && Keyboard.GetState().IsKeyDown(Keys.P))
            {
                //  place a spawn point here
                SpawnPoint spawn = new SpawnPoint();
                spawn.X = x;
                spawn.Y = y;

                this.level.SpawnPoints.Add(spawn);


                spawned = true;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.P))
            {
                spawned = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                this.snap = false;
            }
            else
            {
                this.snap = true;
            }

            if (!keyPressed)
            {
                ProcessInput();
            }

            if (Keyboard.GetState().GetPressedKeys().Length == 0)
            {
                keyPressed = false;
            }
        }

        private void ProcessInput()
        {
            var currentLayer = this.editWalkLayer ? this.level.WalkLayer : this.level.GetLayer(this.currentLayer);

            if (Keyboard.GetState().IsKeyDown(Keys.Tab))
            {
                this.currentTileSet++;
                if (this.currentTileSet >= this.tilesets.Count) this.currentTileSet = 0;

                currentLayer.TileSet = this.tilesets[this.currentTileSet].Name;
                currentLayer.TileSetWidth = this.tilesets[this.currentTileSet].TileWidth;
                currentLayer.TileSetHeight = this.tilesets[this.currentTileSet].TileHeight;
                currentLayer.Initialize();

                this.currentIndex = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.PageDown))
            {
                this.currentIndex--;
                if (this.currentIndex < 0) this.currentIndex = currentLayer.MaxIndex();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.PageUp))
            {
                this.currentIndex++;
                if (this.currentIndex > currentLayer.MaxIndex()) this.currentIndex = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Back))
            {
                List<int> indicesToKill = new List<int>();

                foreach (var tile in currentLayer.Tiles)
                {
                    foreach (var otherTile in currentLayer.Tiles)
                    {
                        if (tile != otherTile && tile.Position == otherTile.Position)
                        {
                            indicesToKill.Add(currentLayer.Tiles.IndexOf(tile));
                            indicesToKill.Add(currentLayer.Tiles.IndexOf(otherTile));
                        }
                    }
                }

                foreach (var index in indicesToKill.Distinct().OrderByDescending(x => x))
                {
                    currentLayer.Tiles.RemoveAt(index);
                }
            }

            if (!this.editWalkLayer)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    this.AddLayer();
                    this.currentLayer++;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Home))
                {
                    this.currentLayer++;
                    if (this.currentLayer >= this.level.LayerCount) this.currentLayer = 0;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.End))
                {
                    this.currentLayer--;
                    if (this.currentLayer < 0) this.currentLayer = this.level.LayerCount - 1;
                    if (this.level.LayerCount == 0) this.currentIndex = 0;
                }
            }

            //  save level
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                XmlSerializer xs = new XmlSerializer(typeof(Level));

                using (var fileStream = File.Open("level.xml", FileMode.Create))
                {
                    xs.Serialize(fileStream, this.level);
                }

                this.saved = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.L))
            {
                this.level = Level.Load();

                this.currentLayer = 0;
                this.currentIndex = 0;
                this.currentTileSet = 0;

                this.saved = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {

                if (this.editWalkLayer)
                {
                    this.editWalkLayer = false;
                }
                else
                {
                    this.editWalkLayer = true;
                }
            }

            if (Keyboard.GetState().GetPressedKeys().Length > 0)
            {
                keyPressed = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.level.Draw(spriteBatch, this.cameraPos);
            this.level.SetDrawsWalkLayer(this.editWalkLayer);

            if (!this.cameraMoving && !this.editWalkLayer)
            {
                this.level.GetLayer(this.currentLayer).DrawTile(spriteBatch, new Layer.Tile() { Index = currentIndex, Position = position }, Color.White, Vector2.Zero);
            }
            else if (this.editWalkLayer)
            {
                this.level.WalkLayer.DrawTile(spriteBatch, new Layer.Tile() { Index = currentIndex, Position = position }, Color.Red, Vector2.Zero);
            }

            spriteBatch.DrawString(BigEvilStatic.GetDefaultFont(), "Tileset: " + this.tilesets[this.currentTileSet].Name, new Vector2(10f, 10f), Color.White);
            spriteBatch.DrawString(BigEvilStatic.GetDefaultFont(), "Current Tile: " + this.currentIndex, new Vector2(10f, 33f), Color.White);

            if (this.editWalkLayer)
            {
                spriteBatch.DrawString(BigEvilStatic.GetDefaultFont(), "Current Layer: Walk Layer", new Vector2(10f, 56f), Color.White);
            }
            else
            {
                spriteBatch.DrawString(BigEvilStatic.GetDefaultFont(), "Current Layer: " + this.currentLayer, new Vector2(10f, 56f), Color.White);
            }

            if (saved)
            {
                spriteBatch.DrawString(BigEvilStatic.GetDefaultFont(), "Saved!", new Vector2(10f, 79f), Color.Yellow);
            }

            //  draw spawn points
            int h = 0;
            for (int i = 0; i < this.level.SpawnPoints.Count; i++ )
            {
                h = h - 23;
                spriteBatch.DrawString(BigEvilStatic.GetDefaultFont(),
                    "SpawnPoint:" + i + " " + this.level.SpawnPoints[i].GetPosition, 
                    new Vector2(10f, BigEvilStatic.Viewport.Height + h), 
                    Color.Green);
            }
        }
    }
}
