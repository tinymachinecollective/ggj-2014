
namespace GGJ2014.Game.Engine.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using GGJ2014.Game.Engine.Graphics;
    using GGJ2014.Game.Engine.Animation;

    public abstract class InputController
    {
        public void UpdateMovement(Player player, GameTime gameTime)
        {
            if (player.DashVelocity == Vector2.Zero)
            {
                player.MovementDirection = this.GetMovementDirection();

                if (player.MovementDirection.LengthSquared() != 0)
                {
                    player.MovementDirection = Vector2.Normalize(player.MovementDirection);

                }
            }
        }

        protected abstract bool FirePressed();
        protected abstract Vector2 GetMovementDirection();
        protected abstract bool DashPressed();
    }
}
