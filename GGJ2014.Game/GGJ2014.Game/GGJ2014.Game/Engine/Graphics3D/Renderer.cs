using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GGJ2014.Game.Engine.Graphics3D
{
    public class Renderer
    {
        private List<Model3D> renderQueue = new List<Model3D>();
        private GraphicsDevice device;

        public Renderer(GraphicsDevice device)
        {
            this.device = device;
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
                Matrix[] transforms = new Matrix[model.Model.Bones.Count];
                model.Model.CopyAbsoluteBoneTransformsTo(transforms);

                foreach (var mesh in model.Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();

                        effect.World = transforms[mesh.ParentBone.Index] * model.Transformation;
                        effect.Projection = Matrix.CreateOrthographic(BigEvilStatic.Viewport.Width, BigEvilStatic.Viewport.Height, 0.1f, 10000f);
                        effect.View = Matrix.CreateLookAt(new Vector3(100f), Vector3.Zero, Vector3.Up);
                    }

                    mesh.Draw();
                }
            }

            renderQueue.Clear();
        }
    }
}
