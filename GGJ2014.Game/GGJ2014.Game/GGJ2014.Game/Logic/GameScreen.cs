using System;
using System.Collections.Generic;
using GGJ2014.Game.Engine;
using GGJ2014.Game.Engine.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GGJ2014.Game.Logic
{
    public class GameScreen : Screen
    {
        private static readonly Random random = new Random();
        private Player player;
        private List<Monster> monsters;
        private SpriteBatch spriteBatch;
        private Level level;

        public GameScreen(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            this.LoadContent();
        }

        public void LoadContent()
        {
            this.level = Level.Load();

            this.player = new Player();
            this.player.Initialize(new MouseInputController(player), 0 , 0);
            this.level.RegisterCharacter(this.player);

            this.monsters = new List<Monster>();

            //  Create monsters
            foreach (var spawnPoint in this.level.SpawnPoints)
            {
                var monster = new Monster("user", 16, 16, 50, 200, 100);
                monster.Initialize(new AIController(monster, player, random), spawnPoint.X, spawnPoint.Y);

                this.level.RegisterCharacter(monster);
                this.monsters.Add(monster);
            }

            AudioManager.Instance.PlayMusic(Music.Intro);
            AudioManager.Instance.QueueMusic(Music.QuietLoop);
        }

        public override void Draw(Bounds bounds)
        {
            this.spriteBatch.Begin();

            Vector2 cameraPos = player.Position - BigEvilStatic.GetScreenCentre();

            this.level.Draw(spriteBatch, cameraPos);

            //  draw monsters
            int h = -10;
            foreach (var monster in this.monsters)
            {
                monster.Draw(spriteBatch, cameraPos);

                h = h - 23;
                spriteBatch.DrawString(BigEvilStatic.GetDefaultFont(),
                    "Monster: " + monster.Position + " started @ " + monster.StartPosition + " dist " + (monster.Position-monster.StartPosition).Length(),
                    new Vector2(10f, BigEvilStatic.Viewport.Height + h),
                    Color.Green);
            }

            this.player.Draw(spriteBatch, cameraPos);

            spriteBatch.DrawString(BigEvilStatic.GetDefaultFont(), "PlayerPos: " + this.player.Position, new Vector2(10f, 10f), Color.White);
            spriteBatch.DrawString(BigEvilStatic.GetDefaultFont(), "Mouse: " + Mouse.GetState(), new Vector2(10f, 33f), Color.White);
            spriteBatch.DrawString(BigEvilStatic.GetDefaultFont(), "Current Track: " + AudioManager.Instance.CurrentTrack, new Vector2(10f, 56f), Color.White);

            this.spriteBatch.End();
        }

        public override void Update(GameTime time)
        {
            this.level.SetDrawsWalkLayer(Keyboard.GetState().IsKeyDown(Keys.W));
            this.level.Update(time);

            foreach (var monster in this.monsters)
            {
                monster.Update(time);
            }

            player.Update(time);

            if (Keyboard.GetState().IsKeyDown(Keys.F2))
            {
                BigEvilStatic.ScreenManager.OpenScreen(new EditorScreen(this.Device));
            }
        }
    }
}
