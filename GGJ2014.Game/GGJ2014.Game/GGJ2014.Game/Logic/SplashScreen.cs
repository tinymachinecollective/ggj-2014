
namespace GGJ2014.Game.Logic
{
    using GGJ2014.Game.Engine;
    using GGJ2014.Game.Engine.Graphics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class SplashScreen : Screen
    {
        private Texture2D splash;
        private GameScreen gameScreen;
        private Fade fade;
        private MouseState lastMouseState;

        public SplashScreen(GraphicsDevice device)
            : base(device)
        {
            this.gameScreen = new GameScreen(this.Device);
            this.splash = BigEvilStatic.Content.Load<Texture2D>("splash");
            this.fade = new Fade();
            this.fade.Initialize();
            lastMouseState = new MouseState(0, 0, 0, ButtonState.Pressed, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);

            this.fade.OnComplete = () =>
                {
                    this.fade = new Fade();
                    this.fade.Initialize();
                    this.fade.OnComplete = () => BigEvilStatic.ScreenManager.OpenScreen(gameScreen);
                    BigEvilStatic.ScreenManager.OpenScreen(gameScreen);
                    lastMouseState = new MouseState(0, 0, 0, ButtonState.Pressed, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
                };
        }

        public override void Draw(Bounds bounds)
        {
            SpriteBatch spriteBatch = new SpriteBatch(this.Device);

            spriteBatch.Begin();
            spriteBatch.Draw(splash, Vector2.Zero, Color.White);
            fade.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Update(GameTime time)
        {
            this.fade.Update(time);

            if (lastMouseState.LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Pressed && !this.fade.Fading)
            {
                this.fade.Start();
            }

            this.lastMouseState = Mouse.GetState();
        }
    }
}
