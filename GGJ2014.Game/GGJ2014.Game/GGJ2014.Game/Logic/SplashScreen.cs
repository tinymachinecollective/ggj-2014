
namespace GGJ2014.Game.Logic
{
    using GGJ2014.Game.Engine;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class SplashScreen : Screen
    {
        private Texture2D splash;
        private GameScreen gameScreen;

        public SplashScreen(GraphicsDevice device) : base(device)
        {
            this.gameScreen = new GameScreen(this.Device);
            this.splash = BigEvilStatic.Content.Load<Texture2D>("splash");
        }

        public override void Draw(Bounds bounds)
        {
            SpriteBatch spriteBatch = new SpriteBatch(this.Device);

            spriteBatch.Begin();
            spriteBatch.Draw(splash, Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        public override void Update(GameTime time)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                BigEvilStatic.ScreenManager.OpenScreen(this.gameScreen);
            }
        }
    }
}
