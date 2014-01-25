﻿using GGJ2014.Game.Engine;
using GGJ2014.Game.Engine.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GGJ2014.Game.Logic
{
    public class GameScreen : Screen
    {
        private Player player;
        private Monster monster;
        private SpriteBatch spriteBatch;
        private Level level;

        public GameScreen(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            this.LoadContent();
        }

        public void LoadContent()
        {
            this.player = new Player();
            this.player.Initialize(new MouseInputController(player));

            //  Create monsters
            this.monster = new Monster("user", 16, 16, 150, 200); 
            this.monster.Initialize(new AIController(monster, player));

            this.level = Level.Load();
            this.level.RegisterCharacter(this.player);
            this.level.RegisterCharacter(this.monster);

            AudioManager.Instance.PlayMusic(AudioManager.Instance.LoadCue("music-Intro"));
            AudioManager.Instance.QueueMusic(AudioManager.Instance.LoadCue("music-QuietLoop"));
        }

        public override void Draw(Bounds bounds)
        {
            this.spriteBatch.Begin();

            Vector2 cameraPos = player.Position - BigEvilStatic.GetScreenCentre();

            this.level.Draw(spriteBatch, cameraPos);
            this.player.Draw(spriteBatch, cameraPos);
            this.monster.Draw(spriteBatch, cameraPos);

            spriteBatch.DrawString(BigEvilStatic.GetDefaultFont(), "PlayerPos: " + this.player.Position, new Vector2(10f, 10f), Color.White);
            spriteBatch.DrawString(BigEvilStatic.GetDefaultFont(), "MonsterPos: " + this.monster.Position, new Vector2(10f, 33f), Color.White);
            spriteBatch.DrawString(BigEvilStatic.GetDefaultFont(), "Distance: " + (this.monster.Position - this.player.Position).Length(), new Vector2(10f, 56f), Color.White);
            spriteBatch.DrawString(BigEvilStatic.GetDefaultFont(), "Mouse: " + Mouse.GetState(), new Vector2(10f, 79f), Color.White);
            spriteBatch.DrawString(BigEvilStatic.GetDefaultFont(), "Current Track: " + AudioManager.Instance.CurrentTrack, new Vector2(10f, 102f), Color.White);

            this.spriteBatch.End();
        }

        public override void Update(GameTime time)
        {
            this.level.Update(time);
            player.Update(time);
            monster.Update(time);

            if (Keyboard.GetState().IsKeyDown(Keys.F2))
            {
                BigEvilStatic.ScreenManager.OpenScreen(new EditorScreen(this.Device));
            }
        }
    }
}
