using GGJ2014.Game.Engine;
using GGJ2014.Game.Engine.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

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
            this.player.Initialize(new MouseInputController(player));
            this.level.RegisterCharacter(this.player);

            this.monsters = new List<Monster>();

            //  Create monsters
            for (int i = 0; i < 4; i++)
            {
                var monster = new Monster("user", 16, 16, 150, 200);
                monster.Initialize(new AIController(monster, player));

                var randomVector = new Vector2((float)random.NextDouble() * 300 + 150, (float)random.NextDouble() * 300 + 150);
                int randomNumber = random.Next(4);

                if (randomNumber == 0) monster.Position += randomVector;
                if (randomNumber == 1) monster.Position -= randomVector;
                if (randomNumber == 2) monster.Position += new Vector2(randomVector.X, -randomVector.Y);
                if (randomNumber == 3) monster.Position += new Vector2(-randomVector.X, randomVector.Y);

                this.level.RegisterCharacter(monster);
                this.monsters.Add(monster);
            }

            AudioManager.Instance.PlayMusic(AudioManager.Instance.LoadCue("music-Intro"));
            AudioManager.Instance.QueueMusic(AudioManager.Instance.LoadCue("music-QuietLoop"));
        }

        public override void Draw(Bounds bounds)
        {
            this.spriteBatch.Begin();

            Vector2 cameraPos = player.Position - BigEvilStatic.GetScreenCentre();

            this.level.Draw(spriteBatch, cameraPos);

            foreach (var monster in this.monsters)
            {
                monster.Draw(spriteBatch, cameraPos);
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
