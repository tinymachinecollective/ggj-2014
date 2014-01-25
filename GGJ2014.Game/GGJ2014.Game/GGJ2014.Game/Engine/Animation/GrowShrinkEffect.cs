
namespace GGJ2014.Game.Engine.Animation
{
    using System;
    using Microsoft.Xna.Framework;
    using GGJ2014.Game.Engine.Graphics;

    public class GrowShrinkEffect : SpriteEffect
    {
        private float rangeFromScale;
        private bool repeat;

        public GrowShrinkEffect(float milliseconds, float rangeFromScale, bool repeat)
        {
            this.Duration = TimeSpan.FromMilliseconds(milliseconds);
            this.Permanent = true;
            this.repeat = repeat;

            this.rangeFromScale = rangeFromScale;
        }

        protected override void Update(Sprite sprite, float time)
        {
            if (time > 2.0f)
            {
                if (!this.repeat)
                {
                    this.Permanent = false;
                    sprite.Zoom = 1f;
                    return;
                }
                else
                {
                    this.ResetTime();
                }
            }

            if (time > 1.0f) time = 1.0f - (time - 1.0f);

            time -= 0.5f;
            time *= 2.0f;

            sprite.Zoom = 1f + (rangeFromScale * time);
        }
    }
}
