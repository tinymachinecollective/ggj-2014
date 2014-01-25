
namespace GGJ2014.Game.Engine.Controls
{
    using Microsoft.Xna.Framework;

    public abstract class InputController
    {
        public void UpdateMovement(Character character, GameTime gameTime)
        {
            character.TargetDirection = this.GetMovementDirection(gameTime);

            if (character.TargetDirection.LengthSquared() != 0)
            {
                character.TargetDirection = Vector2.Normalize(character.TargetDirection);
            }
        }

        protected abstract Vector2 GetMovementDirection(GameTime gameTime);
    }
}
