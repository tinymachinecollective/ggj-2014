using Microsoft.Xna.Framework;

namespace GGJ2014.Game.Engine
{
    public class SpawnPoint
    {
        public float X;
        public float Y;

        public Vector2 GetPosition { get { return new Vector2(X, Y); } }
    }
}
