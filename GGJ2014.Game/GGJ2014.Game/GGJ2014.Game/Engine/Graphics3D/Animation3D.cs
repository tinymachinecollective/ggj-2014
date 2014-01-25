using Microsoft.Xna.Framework;

namespace GGJ2014.Game.Engine.Graphics3D
{
    public abstract class Animation3D
    {
        public abstract void Update(GameTime gameTime);
        public abstract void ApplyTransformation(Model3D model);
    }
}
