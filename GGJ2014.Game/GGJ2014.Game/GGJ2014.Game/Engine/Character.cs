using GGJ2014.Game.Engine.Graphics;
using GGJ2014.Game.Engine.Physics;
using Microsoft.Xna.Framework.Graphics;
using GGJ2014.Game.Engine.Controls;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GGJ2014.Game.Engine
{
    public abstract class Character : Sprite, IMoveable
    {
        public static bool InputFrozen = false;
        private InputController inputController;
        public Vector2 MovementDirection { get; set; }
        public float Speed { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 LastPosition { get; set; }
        private Rectangle collisionRectangle;
        private Vector2 direction;
        public Level Level { get; set; }

        private List<float> xPenetrations;
        private List<float> yPenetrations;

        public Character(Texture2D texture, int width, int height)
            : base(texture, width, height)
        {
            this.direction = new Vector2(0, 1);

            this.xPenetrations = new List<float>();
            this.yPenetrations = new List<float>();
        }

        public void Initialize(InputController controller)
        {
            this.inputController = controller;
            this.collisionRectangle = new Rectangle(0, 0, this.Width, this.Height);
        }

        protected void UpdateMovement(GameTime gameTime)
        {
            this.inputController.UpdateMovement(this, gameTime);

            if (!InputFrozen)
            {
                // Update player direction. Dont change if movement direction has no length
                if (MovementDirection.LengthSquared() != 0)
                {
                    this.direction = MovementDirection;
                }
            }

            Vector2 newPosition = Position + (MovementDirection * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (this.Level.PositionIsValid(newPosition))
            {
                this.Position = newPosition;
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            this.UpdateMovement(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 cameraPos)
        {
            this.Draw(spriteBatch, this.Position, cameraPos);
        }

        private void CheckForCollisions()
        {
            if (this.Position != this.LastPosition)
            {
                this.xPenetrations.Clear();
                this.yPenetrations.Clear();
                var possibleRectangles = this.Level.GetCollisionRectangles(); // other collision rectangles

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
                        this.Position -= new Vector2(-xPenetrations[0], 0);
                        CheckForCollisions();
                    }
                    else
                    {
                        this.yPenetrations.Sort();
                        this.Position -= new Vector2(0, -yPenetrations[0]);
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
