
namespace GGJ2014.Game.Logic
{
    using GGJ2014.Game.Engine;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using GGJ2014.Game.Engine.Graphics;

    public class EndScreen : Screen
    {
        private Texture2D splash;
        private Fade fade;
        private int score;
        private MouseState lastMouseState;

        public EndScreen(int score, GraphicsDevice device)
            : base(device)
        {
            this.splash = BigEvilStatic.Content.Load<Texture2D>("endscreen");
            this.score = score;
            this.fade = new Fade();
            fade.FadeOut = true;
            this.fade.Initialize();
            this.fade.Start();
        }

        public override void Draw(Bounds bounds)
        {
            SpriteBatch spriteBatch = new SpriteBatch(this.Device);

            spriteBatch.Begin();
            spriteBatch.Draw(splash, Vector2.Zero, Color.White);

            spriteBatch.DrawString(BigEvilStatic.Content.Load<SpriteFont>("ribeye"), score + " cat toys murdered", new Vector2(350, 590), Color.Black);

            fade.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Update(GameTime time)
        {
            this.fade.Update(time);
            var mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released && !this.fade.Fading)
            {
                this.CloseUntil(typeof(SplashScreen));
            }

            this.lastMouseState = Mouse.GetState();
        }
    }
}
