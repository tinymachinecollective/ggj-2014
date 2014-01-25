
namespace GGJ2014.Game.Engine
{
    using GGJ2014.Game.Engine.Animation;
    using GGJ2014.Game.Engine.Graphics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

    public class Player : Character
    {
        private Fade fade = new Fade();
        private int lives = 3;
        private Delay gameEnd = new Delay(1000);
        private Cue humanVoice;
        private Cue endGame;

        public Player() : base(BigEvilStatic.Content.Load<Texture2D>("cog"), 16, 16)
        {
            this.Speed = 350;
        }

        public override void Initialize(Controls.InputController controller, float startX = 0, float startY = 0)
        {
            base.Initialize(controller, startX, startY);
            this.fade.Initialize();
            this.humanVoice = AudioManager.Instance.LoadCue("human");
            this.endGame = AudioManager.Instance.LoadCue("monster-laugh");
        }

        public override void OnCollision(Character character)
        {
            if (character is Monster)
            {
                lives--;

                AudioManager.Instance.PlayCue(ref humanVoice, false);
                this.Effects.Add(new PainEffect());
                (character as Monster).Destroy();

                if (lives <= 0)
                {
                    InputFrozen = true;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.fade.Update(gameTime);

            if (lives <= 0 && !fade.Fading)
            {
                gameEnd.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);

                if (gameEnd.Over())
                {
                    AudioManager.Instance.PlayCue(ref endGame, false);
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
