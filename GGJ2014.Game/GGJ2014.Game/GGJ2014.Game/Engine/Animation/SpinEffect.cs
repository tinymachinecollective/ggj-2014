
namespace GGJ2014.Game.Engine.Animation
{
    using System;
    using GGJ2014.Game.Engine.Graphics;
    using Microsoft.Xna.Framework;

    public class SpinEffect : SpriteEffect
    {
        public SpinEffect(float milliseconds)
        {
            this.Duration = TimeSpan.FromMilliseconds(milliseconds);
            this.Permanent = true;
        }

        protected override void Update(Sprite sprite, float time)
        {
            if (time > 1.0f) this.ResetTime();
            sprite.Rotation = MathHelper.WrapAngle(time * MathHelper.TwoPi);
        }
    }
}
