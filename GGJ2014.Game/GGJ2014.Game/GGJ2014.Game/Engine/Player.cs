
namespace GGJ2014.Game.Engine
{
    using GGJ2014.Game.Engine.Animation;
    using GGJ2014.Game.Engine.Graphics;
    using GGJ2014.Game.Engine.Graphics3D;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Graphics;
    using System;
    using GGJ2014.Game.Engine.Controls;
    using Microsoft.Xna.Framework.Input;
    using SkinnedModel;

    public enum PlayerState
    {
        Idle,
        Running,
        Pouncing
    }

    public class Player : Character
    {
        private Fade fade = new Fade();
        public int lives = 3;
        public int score = 0;
        private Delay gameEnd = new Delay(1000);
        private AnimatedModel3D model;

        private Cue humanVoice;
        private Cue endGame;
        private Cue purrMeow;
        private Cue attack;

        private PlayerState state;
        private PlayerState oldState;

        public Player()
            : base(BigEvilStatic.Content.Load<Texture2D>("shadow"), 64, 64)
        {
            this.Speed = 350;
        }

        public override void Initialize(Controls.InputController controller, float startX = 0, float startY = 0)
        {
            base.Initialize(controller, startX, startY);
            this.fade.Initialize();

            this.humanVoice = AudioManager.Instance.LoadCue("human");
            this.endGame = AudioManager.Instance.LoadCue("monster-laugh");
            this.purrMeow = AudioManager.Instance.LoadCue("purr-meow");
            this.attack = AudioManager.Instance.LoadCue("purr-shocked");

            this.model = new AnimatedModel3D();
            this.model.Initialize("leopard", new Vector3(5f, -15f, 15f), 0.06f);
            this.model.PlayAnimation("Idle");

            this.Effects.Add(new GrowShrinkEffect(500, -0.05f, true));

            // initial rotation
            this.Rotation = -(MathHelper.PiOver4 / 2f);

            this.model.RegisterEventListener("Pounce", "PounceComplete", x =>
                {
                    this.Speed = 350;
                    this.model.PlayAnimation("Idle");
                    this.state = PlayerState.Idle;
                });
        }

        public override void OnCollision(Character character)
        {
            if (character is Monster)
            {
                lives--;

                AudioManager.Instance.PlayCue(ref humanVoice, false);
                this.Effects.Add(new PainEffect());
                character.MovementFrozen = true;
                (character as Monster).Destroy();

                if (lives <= 0)
                {
                    this.MovementFrozen = true;
                    this.model.PlayAnimation("Idle");
                    InputFrozen = true;
                }
            }

            if (character is Antelope && this.state == PlayerState.Pouncing)
            {
                AudioManager.Instance.PlayCue(ref purrMeow, false);
                character.MovementFrozen = true;
                (character as Antelope).NomNomNom();
                score += 1;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.fade.Update(gameTime);

            this.PauseEffects = !Moving;
            this.model.Update(gameTime);

            if (lives <= 0 && !fade.Fading)
            {
                gameEnd.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);

                if (gameEnd.Over())
                {
                    AudioManager.Instance.PlayCue(ref endGame, false);
                    fade.Start();
                }
            }

            this.UpdateStates();
        }

        private void UpdateStates()
        {
            if (this.state != PlayerState.Pouncing)
            {
                if (this.Moving) this.state = PlayerState.Running;
                else this.state = PlayerState.Idle;

                if (this.state == PlayerState.Running && oldState != PlayerState.Running)
                {
                    this.model.PlayAnimation("Run");
                }

                if (this.state != PlayerState.Running && oldState == PlayerState.Running)
                {
                    this.model.PlayAnimation("Idle");
                }
            }

            if (this.Moving && Keyboard.GetState().IsKeyDown(Keys.Space) && this.oldState != PlayerState.Pouncing)
            {
                this.state = PlayerState.Pouncing;
                this.model.PlayAnimation("Pounce");
                AudioManager.Instance.PlayCue(ref attack, false);
                this.Speed = 500;
            }

            oldState = this.state;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 cameraPos)
        {
            if (this.Moving)
            {
                float targetAngle = this.ConvertVectorToAngle(this.TargetDirection);
                float angle = this.ConvertVectorToAngle(this.MovementDirection);
                base.Rotation = MathHelper.Pi - (angle + MathHelper.PiOver4);

                this.model.Rotation = angle;
            }

            base.Draw(spriteBatch, cameraPos);
            BigEvilStatic.Renderer.QueueModelForRendering(this.model);
            this.fade.Draw(spriteBatch);
        }

        private float ConvertVectorToAngle(Vector2 vector)
        {
            return MathHelper.WrapAngle(MathHelper.Pi - ((float)Math.Atan2(vector.Y, vector.X) + MathHelper.PiOver4));
        }

        public bool Moving
        {
            get { return this.ActualSpeed >= this.Speed / 4f; }
        }
    }
}
