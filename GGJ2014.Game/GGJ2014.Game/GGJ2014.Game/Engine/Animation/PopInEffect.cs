﻿
namespace GGJ2014.Game.Engine.Animation
{
    using System;
    using GGJ2014.Game.Engine.Graphics;

    public class PopInEffect : SpriteEffect
    {
        public float Exaggeration { get; private set; }

        private float timeToIncrease;
        private float timeToIncreaseMore;
        private bool reverse;

        public PopInEffect(float milliseconds, float exaggeration, bool reverse = false)
        {
            this.Exaggeration = exaggeration;
            this.Duration = TimeSpan.FromMilliseconds(milliseconds);

            this.timeToIncrease = (milliseconds - (exaggeration * milliseconds)) / milliseconds;
            this.timeToIncreaseMore = timeToIncrease + ((exaggeration * milliseconds) / (milliseconds * 2));
            this.reverse = reverse;
        }

        protected override void Update(Sprite sprite, float time)
        {
            if (reverse)
            {
                time = 1f - time;
            }

            if (time < this.timeToIncrease)
            {
                sprite.Zoom = time / timeToIncrease;
            }
            else if (time < this.timeToIncreaseMore)
            {
                sprite.Zoom = time / timeToIncrease;
            }
            else
            {
                sprite.Zoom = time;
            }
        }
    }
}
