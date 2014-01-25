﻿
namespace GGJ2014.Game.Engine
{
    using Microsoft.Xna.Framework.Graphics;
    using GGJ2014.Game.Engine.Animation;

    public class Monster : Character
    {
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
        }
    }
}
