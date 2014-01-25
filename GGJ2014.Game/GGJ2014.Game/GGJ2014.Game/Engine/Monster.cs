
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
        //  Core variables
        private AIController inputController;
        private Rectangle collisionRectangle;
        private GrowShrinkEffect effect;
        private Vector2 position = BigEvilStatic.GetScreenCentre();    // start in the middle of the screen

        public Vector2 MovementDirection { get; set; }

        //  Public constants
        public static bool InputFrozen = false;
        //public const int MaxHealth = 12;
        //public const float MaxEnergy = 10;
        public const float MaxSpeed = 300;
        public const int MaxBounce = 20;
        public const float BulletSpeed = 500f;
        public const float EnergyPerShot = 1f;
        public const float DashCooldownTime = 1;
        public const float SpiralShotTime = 5;

        //  Private constants
        private const int ShieldMaxHealth = 5;
        private const float EnergyRegenChargeTime = 3f;
        private const float MinEnergyPerSecond = 2f;
        private const float MaxEnergyPerSecond = 10f;

        //  Variables
        private int deathNum;
        private Vector2 direction;
        private List<float> xPenetrations;
        private List<float> yPenetrations;



        public float Speed { get; set; }

        public float TimeSinceLastDash { get; set; }

        public float TimeSinceSpiralBegan { get; set; }

        public Sprite BulletSprite { get; set; }
        public string BulletSpriteName { get; private set; }

        public int Health { get; set; }
        public float Energy { get; set; }

        public int BulletBounce { get; set; }

        public int ShieldHealth { get; set; }

        public TimeSpan NoClipTime { get; set; }


        //  Core functions
        public Monster() : base(BigEvilStatic.Content.Load<Texture2D>("user"), 54, 54)
        {
            this.TimeSinceLastDash = 100;
            this.Speed = Monster.MaxSpeed;
            //this.Health = Monster.MaxHealth;
            //this.Energy = Monster.MaxEnergy;
            this.direction = new Vector2(0, 1);
            this.effect = new GrowShrinkEffect(750f, 0.02f);

            this.xPenetrations = new List<float>();
            this.yPenetrations = new List<float>();
        }

        public void Initialize(AIController controller)
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
        public Level Level { get; set; }


        public void Spawn()
        {
            this.position = this.Level.FindSpawnPoint(true);
            this.LastPosition = this.position;
            //this.Health = Monster.MaxHealth;
            //this.Energy = Monster.MaxEnergy;
            this.Speed = Monster.MaxSpeed;
        }

        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            this.MonsterUpdateMovement(gameTime, playerPosition);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPos)
        {
            this.Draw(spriteBatch, this.position, cameraPos);
        }

        private void MonsterUpdateMovement(GameTime gameTime, Vector2 playerPosition)
        {
            this.inputController.MonsterUpdateMovement(this, gameTime, playerPosition);

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
