using System;
using Microsoft.Xna.Framework;
using SkinnedModel;

namespace GGJ2014.Game.Engine.Graphics3D
{
    public class AnimatedModel3D : Model3D
    {
        private SkinningData skinningData;
        public AnimationPlayer Player { get; private set; }

        public override void Initialize(string model, Vector3 offset, float scale)
        {
            base.Initialize(model, offset, scale);

            skinningData = this.Model.Tag as SkinningData;
            if (skinningData == null) throw new InvalidOperationException("This model does not contain a SkinningData tag.");

            this.Player = new AnimationPlayer(skinningData);
        }

        public void PlayAnimation(string animation)
        {
            AnimationClip clip = skinningData.AnimationClips[animation];
            this.Player.StartClip(clip);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.Player.Update(gameTime.ElapsedGameTime, true, this.Transformation);
        }
    }
}
