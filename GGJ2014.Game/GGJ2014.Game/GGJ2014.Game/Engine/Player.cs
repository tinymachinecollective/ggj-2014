
namespace GGJ2014.Game.Engine
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using GGJ2014.Game.Engine.Animation;
    using GGJ2014.Game.Engine.Controls;
    using GGJ2014.Game.Engine.Graphics;
    using GGJ2014.Game.Engine.Physics;

    public class Player : IMoveable
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
        private DirectionalSprite sprite;
        public Vector2 BulletDirection { get; set; }
        private List<float> xPenetrations;
        private List<float> yPenetrations;
        public float ShootCooldown { get; set; }
        public float ShootTimer { get; set; }

        public Vector2 DashVelocity { get; set; }

        public const float DashCooldownTime = 1;
        public float TimeSinceLastDash { get; set; }

        public const float SpiralShotTime = 5;
        public float TimeSinceSpiralBegan { get; set; }

        private List<DashSprite> dashPath;
        private Vector2 lastDashSprite;

        public Sprite BulletSprite { get; set; }
        public string BulletSpriteName { get; private set; }
        private InputController inputController;

        public int Health { get; set; }
        public float Energy { get; set; }

        public int BulletBounce { get; set; }
        
        public int ShieldHealth { get; set; }
        private Embellishment shield;

        public TimeSpan NoClipTime { get; set; }

        private GrowShrinkEffect effect;

        private Rectangle collisionRectangle;

        public Player()
        {

            this.TimeSinceLastDash = 100;
            this.Speed = Player.MaxSpeed;
            this.Health = Player.MaxHealth;
            this.Energy = Player.MaxEnergy;
            this.direction = new Vector2(0, 1);
            this.effect = new GrowShrinkEffect(750f, 0.02f);

            this.xPenetrations = new List<float>();
            this.yPenetrations = new List<float>();

            this.ShootCooldown = 0.2f;
        }

        public void Initialize(InputController controller, DirectionalSprite sprite, string bulletSpriteName)
        {
            this.inputController = controller;
            this.sprite = sprite;

            Texture2D bulletTexture = BigEvilStatic.Content.Load<Texture2D>(bulletSpriteName);
            this.BulletSprite = new Sprite(bulletTexture, bulletTexture.Width, bulletTexture.Height);
            this.BulletSpriteName = bulletSpriteName;

            this.collisionRectangle = new Rectangle(0, 0, sprite.Width, sprite.Height);
            this.Spawn();
            
            //if (this.playerNumber == PlayerNumber.PlayerOne)
            //{
            //    this.position = new Vector2(Level.TileWidth) * 55;
            //}
            //else if (this.playerNumber == PlayerNumber.PlayerTwo)
            //{
            //    this.position = new Vector2(Level.TileWidth * 127, Level.TileWidth * 141);
            //}
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
            this.Health = Player.MaxHealth;
            this.Energy = Player.MaxEnergy;
            this.Speed = Player.MaxSpeed;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch batch, Bounds bounds)
        {
        }

        private void UpdateMovement(GameTime gameTime)
        {
            this.inputController.UpdateMovement(this, gameTime);

            if (this.DashVelocity != Vector2.Zero)
            {
                this.LastPosition = this.Position;
                this.position += DashVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.DashVelocity -= 30 * this.Speed * this.MovementDirection * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (DashVelocity.LengthSquared() <= Speed * Speed)
                {
                    this.DashVelocity = Vector2.Zero;
                }
            }
            else
            {
                this.lastDashSprite = new Vector2(float.PositiveInfinity);
                this.LastPosition = this.Position;
                this.position += MovementDirection * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            this.deathNum = 0;
            this.CheckForCollisions();

            // Update dash path transparency.
            for (int i = dashPath.Count - 1; i >= 0; --i )
            {
                dashPath[i].update(gameTime);
                if (dashPath[i].RemoveFromList)
                {
                    dashPath.RemoveAt(i);
                }
            }

            if(this.DashVelocity != Vector2.Zero)
            {   
                float distanceCheck = 20;

                if(Vector2.DistanceSquared(lastDashSprite, this.position) > distanceCheck * distanceCheck)
                {
                    if (float.IsInfinity(this.lastDashSprite.X))
                    {
                        this.lastDashSprite = this.position;
                    }
                    else
                    {
                        Vector2 posChange = (this.position - lastDashSprite);
                        posChange.Normalize();
                        lastDashSprite += (posChange * distanceCheck);
                    }

                    DashSprite dashSprite = new DashSprite(this.sprite.CreateSprite(this.MovementDirection), lastDashSprite);
                    this.dashPath.Add(dashSprite);
                }
            }
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
                //Vector2 tempPosition = this.position;
                //int tempHeight = collisionRectangle.Height;
                //collisionRectangle.Height = tempHeight / 2;
                //this.position.Y += (float)tempHeight / 4;

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
