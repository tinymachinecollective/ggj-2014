
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
        private GameScreen gameScreen;
        private Fade fade;

        public EndScreen(GraphicsDevice device)
            : base(device)
        {
            this.gameScreen = new GameScreen(this.Device);
            this.splash = BigEvilStatic.Content.Load<Texture2D>("splash");
            this.fade = new Fade();
            this.fade.Initialize();
            this.fade.OnComplete = () => BigEvilStatic.ScreenManager.OpenScreen(gameScreen);
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

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && !this.fade.Fading)
            {
                this.fade.Start();
            }
        }
    }
}
