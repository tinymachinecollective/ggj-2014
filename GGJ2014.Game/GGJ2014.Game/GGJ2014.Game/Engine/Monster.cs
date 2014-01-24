
namespace GGJ2014.Game.Engine
{
    using System;
    using System.Collections.Generic;
    using GGJ2014.Game.Engine.Animation;
    using GGJ2014.Game.Engine.Controls;
    using GGJ2014.Game.Engine.Graphics;
    using GGJ2014.Game.Engine.Physics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Monster : Sprite, IMoveable
    {
        public static bool InputFrozen = false;
        private const int ShieldMaxHealth = 5;
        public const int MaxHealth = 12;
        public const float MaxEnergy = 10;
        private const float EnergyRegenChargeTime = 3f;
        private const float MinEnergyPerSecond = 2f;
        private const float MaxEnergyPerSecond = 10f;
        public const float MaxSpeed = 350;
        public const int MaxBounce = 20;
        public const float BulletSpeed = 500f;
        private int deathNum;

        public const float EnergyPerShot = 1f;
        public float Speed { get; set; }

        private Vector2 direction;
        public Vector2 MovementDirection { get; set; }
        private List<float> xPenetrations;
        private List<float> yPenetrations;

        public const float DashCooldownTime = 1;
        public float TimeSinceLastDash { get; set; }

        public const float SpiralShotTime = 5;
        public float TimeSinceSpiralBegan { get; set; }

        public Sprite BulletSprite { get; set; }
        public string BulletSpriteName { get; private set; }
        private InputController inputController;

        public int Health { get; set; }
        public float Energy { get; set; }

        public int BulletBounce { get; set; }
        
        public int ShieldHealth { get; set; }

        public TimeSpan NoClipTime { get; set; }

        private GrowShrinkEffect effect;

        private Rectangle collisionRectangle;

        public Monster() : base(BigEvilStatic.Content.Load<Texture2D>("lion_placeholder"), 27, 27)
        {
            this.TimeSinceLastDash = 100;
            this.Speed = Monster.MaxSpeed;
            this.Health = Monster.MaxHealth;
            this.Energy = Monster.MaxEnergy;
            this.direction = new Vector2(0, 1);
            this.effect = new GrowShrinkEffect(750f, 0.02f);

            this.xPenetrations = new List<float>();
            this.yPenetrations = new List<float>();
        }

        public void Initialize(InputController controller)
        {
            this.inputController = controller;
            this.collisionRectangle = new Rectangle(0, 0, this.Width, this.Height);
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        public Vector2 LastPosition { get; set; }
        public Level Level{ get; set; }

        private Vector2 position;

        public void Spawn()
        {
            this.position = this.Level.FindSpawnPoint(true);
            this.LastPosition = this.position;
            this.Health = Monster.MaxHealth;
            this.Energy = Monster.MaxEnergy;
            this.Speed = Monster.MaxSpeed;
        }

        public void Update(GameTime gameTime)
        {
            this.UpdateMovement(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.Draw(spriteBatch, this.position);
        }

        private void UpdateMovement(GameTime gameTime)
        {
            //this.inputController.UpdateMovement(this, gameTime);

            if (!InputFrozen)
            {
                // Update Monster direction. Dont change if movement direction has no length
                if (MovementDirection.LengthSquared() != 0)
                {
                    this.direction = MovementDirection;
                }
            }

            this.position += MovementDirection * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void CheckForCollisions()
        {
            this.deathNum++;
            if (this.deathNum > 5)
            {
                return;
            }

            if (this.Position != this.LastPosition)
            {
                this.xPenetrations.Clear();
                this.yPenetrations.Clear();
                List<Rectangle> possibleRectangles = new List<Rectangle>(); // other collision rectangles

                foreach (Rectangle r in possibleRectangles)
                {
                    Vector2 penetration = CollisionSolver.SolveCollision(this, r);
                    if (penetration.X != 0)
                    {
                        this.xPenetrations.Add(penetration.X);
                    }
                    if (penetration.Y != 0)
                    {
                        this.yPenetrations.Add(penetration.Y);
                    }
                }

                //this.position = tempPosition;

                if (xPenetrations.Count != 0 || yPenetrations.Count != 0)
                {
                    if (xPenetrations.Count >= yPenetrations.Count)
                    {
                        this.xPenetrations.Sort();
                        this.position.X -= xPenetrations[0];
                        CheckForCollisions();
                    }
                    else
                    {
                        this.yPenetrations.Sort();
                        this.position.Y -= yPenetrations[0];
                        CheckForCollisions();
                    }
                }

                //collisionRectangle.Height = tempHeight;
            }
        }

        public Rectangle CollisionRectangle
        {
            get { return GeometryUtility.GetAdjustedRectangle(this.Position, this.collisionRectangle); }
        }
    }
}
