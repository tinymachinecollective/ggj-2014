using GGJ2014.Game.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GGJ2014.Game.Engine.Controls;
using Microsoft.Xna.Framework.Input;

namespace GGJ2014.Game.Logic
{
    public class GameScreen : Screen
    {
        private Player player;
        private SpriteBatch spriteBatch;

        public GameScreen(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            this.LoadContent();
        }

        public void LoadContent()
        {
            this.player = new Player();
            this.player.Initialize(new MouseInputController(player));
        }

        public override void Draw(Bounds bounds)
        {
            this.spriteBatch.Begin();

            this.player.Draw(spriteBatch);

            this.spriteBatch.End();
        }

        public override void Update(GameTime time)
        {
            player.Update(time);

            if (Keyboard.GetState().IsKeyDown(Keys.F2))
            {
                BigEvilStatic.ScreenManager.OpenScreen(new EditorScreen(this.Device));
            }
        }
    }
}
