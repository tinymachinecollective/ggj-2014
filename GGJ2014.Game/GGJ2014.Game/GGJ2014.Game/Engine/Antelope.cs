﻿
namespace GGJ2014.Game.Engine
{
    using GGJ2014.Game.Engine.Animation;
    using GGJ2014.Game.Engine.Controls;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;

    public class Antelope : Character
    {
        private Delay spawnTime = null;
        private Random spawnRand = new Random();

        public Antelope() : base(BigEvilStatic.Content.Load<Texture2D>("antelope"), 32, 32)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 cameraPos)
        {
            base.Draw(spriteBatch, cameraPos);
        }

        public void NomNomNom()
        {
            this.CanCollide = false;
            this.Effects.Add(new FadeEffect(500, true));
            
            //  start respawn timer
            spawnTime = new Delay(spawnRand.Next(3000,6000));

        }

        public override void Update(GameTime gameTime)
        {
            if (spawnTime != null)
            {
                spawnTime.Update((float)gameTime.TotalGameTime.TotalSeconds);

                if (spawnTime.Over() && this.Effects.Count == 0 && this.CanCollide == false)
                {
                    //  respawn
                    this.Effects.Add(new FadeEffect(500, false));
                    
                    this.Position = this.StartPosition;
                    this.CanCollide = true;
                    this.MovementFrozen = false;

                    spawnTime = null;
                }

            }
            base.Update(gameTime);

        }
    }
}
