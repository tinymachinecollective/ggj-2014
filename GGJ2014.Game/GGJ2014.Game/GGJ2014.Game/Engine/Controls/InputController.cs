
namespace GGJ2014.Game.Engine.Controls
{
    using Microsoft.Xna.Framework;

    public abstract class InputController
    {
        public void UpdateMovement(Player player, GameTime gameTime)
        {
            player.MovementDirection = this.GetMovementDirection();

            if (player.MovementDirection.LengthSquared() != 0)
            {
                player.MovementDirection = Vector2.Normalize(player.MovementDirection);
            }
        }

        protected abstract Vector2 GetMovementDirection();
    }
}
