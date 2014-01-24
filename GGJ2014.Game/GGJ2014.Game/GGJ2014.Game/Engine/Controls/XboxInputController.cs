
namespace GGJ2014.Game.Engine.Controls
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    public class XboxInputController : InputController
    {
        private PlayerIndex playerIndex;

        public XboxInputController(PlayerIndex player)
        {
            this.playerIndex = player;
        }

        protected override Vector2 GetMovementDirection()
        {
            GamePadState state = GamePad.GetState(this.playerIndex);
            return state.ThumbSticks.Left * new Vector2(1f, -1f);
        }
    }
}
