
namespace GGJ2014.Game.Engine
{
    using Microsoft.Xna.Framework.Graphics;
    using GGJ2014.Game.Engine.Animation;
    using System;

    public class Monster : Character
    {


        private Delay spawnTime = null;
        private Random spawnRand = new Random();

        public float LineOfSight { get; set; }
        public float TetherLength { get; set; }

        public Monster(string texture, int width, int height, float speed, float lineOfSight, float tetherLength)
            : base(BigEvilStatic.Content.Load<Texture2D>(texture), width, height)
        {
            this.Speed = speed;
            this.LineOfSight = lineOfSight;
            this.TetherLength = tetherLength;
        }

        public void Destroy()
        {
            this.CanCollide = false;
            this.Effects.Add(new ExpandDeathEffect(500, 2.0f));

            //  start respawn timer
            spawnTime = new Delay(spawnRand.Next(4000, 8000));
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (spawnTime != null)
            {
                spawnTime.Update((float)gameTime.TotalGameTime.TotalSeconds);

                if (spawnTime.Over() && this.Effects.Count == 0 && this.CanCollide == false)
                {
                    //  respawn
                    this.Effects.Add(new FadeEffect(500, false));

                    this.Effects.Add(new ExpandDeathEffect(500, 1.0f));

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
