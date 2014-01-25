using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SkinnedModel;

namespace GGJ2014.Game.Engine.Graphics3D
{
    public class Renderer
    {
        private List<Model3D> renderQueue = new List<Model3D>();
        private GraphicsDevice device;
        private Matrix view;
        private Matrix projection;

        public Renderer(GraphicsDevice device)
        {
            this.device = device;
            this.view = Matrix.CreateLookAt(new Vector3(100f), Vector3.Zero, Vector3.Up);
            this.projection = Matrix.CreateOrthographic(BigEvilStatic.Viewport.Width, BigEvilStatic.Viewport.Height, 0.1f, 10000f);
        }

        public void QueueModelForRendering(Model3D model)
        {
            this.renderQueue.Add(model);
        }

        public void Draw()
        {
            this.device.BlendState = BlendState.Opaque;
            this.device.DepthStencilState = DepthStencilState.Default;

            foreach (var model in this.renderQueue)
            {
                if (model is AnimatedModel3D)
                {
                    DrawAnimatedModel(model as AnimatedModel3D);
                }
                else
                {
                    DrawStaticModel(model);
                }
            }

            renderQueue.Clear();
        }

        private void DrawStaticModel(Model3D model)
        {
            Matrix[] transforms = new Matrix[model.Model.Bones.Count];
            model.Model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (var mesh in model.Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.World = transforms[mesh.ParentBone.Index] * model.Transformation;
                    effect.Projection = projection;
                    effect.View = view;
                }

                mesh.Draw();
            }
        }

        private void DrawAnimatedModel(AnimatedModel3D model)
        {
            Matrix[] bones = model.Player.GetSkinTransforms();

            foreach (ModelMesh mesh in model.Model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(bones);

                    effect.View = view;
                    effect.Projection = projection;

                    effect.EnableDefaultLighting();
                }

                mesh.Draw();
            }
        }
    }
}
