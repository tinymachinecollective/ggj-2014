
namespace GGJ2014.Game.Engine
{
    using Microsoft.Xna.Framework.Graphics;

    public class Monster : Character
    {
        public Monster() : base(BigEvilStatic.Content.Load<Texture2D>("user"), 54, 54)
        {
            this.Speed = 50;
        }
    }
}
