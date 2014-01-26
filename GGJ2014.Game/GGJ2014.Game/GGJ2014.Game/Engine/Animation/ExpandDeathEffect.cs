
namespace GGJ2014.Game.Engine.Animation
{
    using System;
    using GGJ2014.Game.Engine.Graphics;

    public class ExpandDeathEffect : SpriteEffect
    {
        private float expandFactor;

        public ExpandDeathEffect(float milliseconds, float amount)
        {
            this.Duration = TimeSpan.FromMilliseconds(milliseconds);
            this.expandFactor = amount;
        }

        protected override void Update(Sprite sprite, float time)
        {
            sprite.Zoom = time * this.expandFactor;

            if (time > 0.75f)
            {
                sprite.Alpha = 1f - ((time - 0.75f) / 0.25f);
            }
        }
    }
}
