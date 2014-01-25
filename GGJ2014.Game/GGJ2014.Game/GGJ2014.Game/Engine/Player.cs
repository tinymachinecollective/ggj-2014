
namespace GGJ2014.Game.Engine
{
    using GGJ2014.Game.Engine.Animation;
    using GGJ2014.Game.Engine.Graphics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Player : Character
    {
        private Fade fade = new Fade();
        private int lives = 3;
        private Delay gameEnd = new Delay(1000);

        public Player() : base(BigEvilStatic.Content.Load<Texture2D>("cog"), 16, 16)
        {
            this.Speed = 350;
        }

        public override void Initialize(Controls.InputController controller)
        {
            base.Initialize(controller);
            this.fade.Initialize();
        }

        public override void OnCollision(Character character)
        {
            if (character is Monster)
            {
                lives--;
                this.Effects.Add(new PainEffect());
                AudioManager.Instance.LoadCue("human").Play();

                (character as Monster).Destroy();

                if (lives == 0)
                {
                    InputFrozen = true;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.fade.Update(gameTime);

            if (lives == 0 && !fade.Fading)
            {
                gameEnd.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);

                if (gameEnd.Over())
                {
                    fade.Start();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 cameraPos)
        {
            base.Draw(spriteBatch, cameraPos);
            this.fade.Draw(spriteBatch);
        }
    }
}
