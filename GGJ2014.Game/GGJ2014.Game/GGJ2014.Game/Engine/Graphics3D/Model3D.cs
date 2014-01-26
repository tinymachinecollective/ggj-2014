using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GGJ2014.Game.Engine.Graphics3D
{
    public class Model3D
    {
        public Model Model { get; private set; }
        public float Rotation { get; set; }
        public float Tilt { get; set; }
        public float Scale { get; set; }
        public Vector3 Offset;
        public Vector3 Translation { get; set; }
        private List<Effect3D> effects = new List<Effect3D>();

        public virtual void Initialize(string model, Vector3 offset, float scale)
        {
            this.Model = BigEvilStatic.Content.Load<Model>(model);
            this.Offset = offset;
            this.Scale = scale;
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var animation in this.effects)
            {
                animation.ApplyTransformation(this);
            }
        }

        public void AddEffect(Effect3D animation)
        {
            this.effects.Add(animation);
        }

        public Matrix Transformation
        {
            get
            {
                return Matrix.CreateScale(this.Scale) * Matrix.CreateTranslation(this.Translation) * Matrix.CreateRotationZ(this.Tilt) * Matrix.CreateRotationY(this.Rotation);
            }
        }
    }
}
