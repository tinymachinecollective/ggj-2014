using GGJ2014.Game.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ2014.Game.Logic
{
    public class GameScreen : Screen
    {
        private Player player;
        private SpriteBatch spriteBatch;

        public GameScreen(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            this.spriteBatch = new SpriteBatch(graphicsDevice);
        }

        public void LoadContent()
        {
            this.player = new Player();
        }

        public override void Draw(Bounds bounds)
        {
            this.spriteBatch.Begin();

            this.player.Draw(this.spriteBatch, bounds);

            this.spriteBatch.End();
        }

        public override void Update(GameTime time)
        {
            player.Update(time);
        }
    }
}
