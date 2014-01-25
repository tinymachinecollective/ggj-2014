
namespace GGJ2014.Game.Engine
{
    using Microsoft.Xna.Framework.Graphics;

    public class Player : Character
    {
        public Player() : base(BigEvilStatic.Content.Load<Texture2D>("lion_placeholder"), 27, 27)
        {
            this.Speed = 350;
        }
    }
}
