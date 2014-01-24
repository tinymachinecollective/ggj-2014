
namespace GGJ2014.Game.Engine.Physics
{
    using Microsoft.Xna.Framework;

    public interface IMoveable : IStatic
    {
        Vector2 Position { get; }
        Vector2 LastPosition { get; }
    }
}
