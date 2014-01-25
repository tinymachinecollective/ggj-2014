using System;
using Microsoft.Xna.Framework;

namespace GGJ2014.Game.Engine.Graphics3D
{
    public class TiltAnimation : Animation3D
    {
        private float animationTime = 5f;
        private float elapsedTime;
        public float TargetTilt = MathHelper.PiOver4;
        private float initialTilt;

        public TiltAnimation(float initialTilt)
        {
            this.initialTilt = initialTilt;
        }

        public override void Update(GameTime gameTime)
        {
            this.elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void ApplyTransformation(Model3D model)
        {
            model.Tilt = initialTilt + (TargetTilt - initialTilt) * (elapsedTime / animationTime);
        }
    }
}
