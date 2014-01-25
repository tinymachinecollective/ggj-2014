
namespace GGJ2014.Game.Engine.Controls
{
    using Microsoft.Xna.Framework;

    public abstract class InputController
    {
        public void UpdateMovement(Character character, GameTime gameTime)
        {
            character.MovementDirection = this.GetMovementDirection();

            if (character.MovementDirection.LengthSquared() != 0)
            {
                character.MovementDirection = Vector2.Normalize(character.MovementDirection);
            }
        }

        protected abstract Vector2 GetMovementDirection();
    }
}
