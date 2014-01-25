using System;
using System.Collections.Generic;
using GGJ2014.Game.Engine;
using GGJ2014.Game.Engine.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GGJ2014.Game.Engine.Graphics3D;

namespace GGJ2014.Game.Logic
{
    public class GameScreen : Screen
    {
        private static readonly Random random = new Random();

        //  Characters
        private Player player;
        private List<Monster> monsters;
        private List<Antelope> antelope;

        //  game config
        private int numMonsters = 3;
        private int numAntelopes = 5;

        //  Graphics
        private SpriteBatch spriteBatch;
        private Level level;
        private Texture2D baseTile = BigEvilStatic.Content.Load<Texture2D>("utility-images\\water_tile");
        private int baseTileHeight = 32;
        private int baseTileWidth = 64;

        //  Core functionality
        public GameScreen(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            this.LoadContent();
        }

        public void LoadContent()
        {
            this.level = Level.Load();

            this.player = new Player();
            this.player.Initialize(new MouseInputController(player), BigEvilStatic.Viewport.Width/2, BigEvilStatic.Viewport.Height/2);
            this.level.RegisterCharacter(this.player);

            //  manage spawn points
            int numSpawns = this.level.SpawnPoints.Count;

            //  Create monsters
            this.monsters = new List<Monster>();
            //foreach (var spawnPoint in this.level.SpawnPoints)
            for (int i=0; i < numMonsters; i++)
            {
                SpawnPoint spawnPoint = this.level.SpawnPoints[i % numSpawns];

                var monster = new Monster("user", 16, 16, 50, 200, 100);
                monster.Initialize(new AIController(monster, player, random), spawnPoint.X, spawnPoint.Y);

                this.level.RegisterCharacter(monster);
                this.monsters.Add(monster);
            }

            //  Create antelope
            this.antelope = new List<Antelope>();
            for (int i =numMonsters; i < numMonsters + numAntelopes; i++)
            {

                SpawnPoint spawnPoint = this.level.SpawnPoints[i % numSpawns];

                Antelope antelope = new Antelope();
                antelope.Initialize(new AntelopeController(antelope, player, random), spawnPoint.X, spawnPoint.Y);
                this.level.RegisterCharacter(antelope);
                this.antelope.Add(antelope);
            }

            AudioManager.Instance.PlayMusic(Music.Intro);
            AudioManager.Instance.QueueMusic(Music.QuietLoop);
        }

        public override void Draw(Bounds bounds)
        {
            //  Draw background
            this.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.Opaque, SamplerState.LinearWrap, DepthStencilState.DepthRead, RasterizerState.CullNone);
            Rectangle baseRect = new Rectangle((int)player.Position.X, (int)player.Position.Y, BigEvilStatic.Viewport.Width, BigEvilStatic.Viewport.Height);
            spriteBatch.Draw(baseTile, Vector2.Zero, baseRect, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            this.spriteBatch.End();

            //  Draw the rest of it
            this.spriteBatch.Begin();
            //  player
            Vector2 cameraPos = player.Position - BigEvilStatic.GetScreenCentre();
            this.level.Draw(spriteBatch, cameraPos);

            //  draw monsters and antelope
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

            foreach (var antelope in this.antelope)
            {
                antelope.Draw(spriteBatch, cameraPos);
            }

            this.player.Draw(spriteBatch, cameraPos);

            //  debug
            spriteBatch.DrawString(BigEvilStatic.GetDefaultFont(), "PlayerPos: " + this.player.Position, new Vector2(10f, 10f), Color.White);
            spriteBatch.DrawString(BigEvilStatic.GetDefaultFont(), "Mouse: " + Mouse.GetState(), new Vector2(10f, 33f), Color.White);
            spriteBatch.DrawString(BigEvilStatic.GetDefaultFont(), "Current Track: " + AudioManager.Instance.CurrentTrack, new Vector2(10f, 56f), Color.White);

            //  Display player stats
            spriteBatch.DrawString(BigEvilStatic.GetDefaultFont(), 
                "PlayerLives: " + player.lives,
                new Vector2(BigEvilStatic.Viewport.Width - 180, 0), 
                Color.Yellow);
            spriteBatch.DrawString(BigEvilStatic.GetDefaultFont(),
                "PlayerScore: " + player.score,
                new Vector2(BigEvilStatic.Viewport.Width - 180, 23),
                Color.Yellow);

            if (player.score == numAntelopes)
            {
                spriteBatch.DrawString(BigEvilStatic.GetDefaultFont(),
                    "YOU WIN!",
                    new Vector2(BigEvilStatic.Viewport.Width / 2, BigEvilStatic.Viewport.Height / 2),
                    Color.Green);
            }

            this.spriteBatch.End();

            BigEvilStatic.Renderer.Draw();
        }

        public override void Update(GameTime gameTime)
        {
            this.level.SetDrawsWalkLayer(Keyboard.GetState().IsKeyDown(Keys.W));
            this.level.Update(gameTime);

            foreach (var monster in this.monsters)
            {
                monster.Update(gameTime);
            }

            foreach (var antelope in this.antelope)
            {
                antelope.Update(gameTime);
            }

            player.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.F2))
            {
                BigEvilStatic.ScreenManager.OpenScreen(new EditorScreen(this.Device));
            }
        }
    }
}
