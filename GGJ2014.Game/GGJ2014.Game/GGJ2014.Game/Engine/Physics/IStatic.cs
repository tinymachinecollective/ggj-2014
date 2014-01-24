
namespace GGJ2014.Game.Engine.Physics
{
    using Microsoft.Xna.Framework;

    public interface IStatic
    {
        Vector2 Position { get; }
        Rectangle CollisionRectangle { get; }
    }
}
