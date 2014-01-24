﻿
namespace GGJ2014.Game.Engine.Controls
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    public class MouseInputController : InputController
    {
        private Player player;

        public MouseInputController(Player player)
        {
            this.player = player;
        }

        protected override Vector2 GetMovementDirection()
        {
            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 diff = new Vector2(mouseState.X, mouseState.Y) - player.Position;

                if (diff.Length() > 10)
                {
                    return diff;
                }
            }

            return Vector2.Zero;
        }
    }
}
