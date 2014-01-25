using System.Collections.Generic;
using GGJ2014.Game.Engine.Controls;
using GGJ2014.Game.Engine.Graphics;
using GGJ2014.Game.Engine.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ2014.Game.Engine
{
    public abstract class Character : Sprite, IMoveable
    {
        public static bool InputFrozen = false;
        public InputController InputController { get; set; }
        public Vector2 MovementDirection { get; set; }
        public float Speed { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 LastPosition { get; set; }
        private Rectangle collisionRectangle;
        private Vector2 direction;
        public Level Level { get; set; }
        public bool CanCollide { get; protected set; }

        public Vector2 StartPosition { get; set; }

        public Character(Texture2D texture, int width, int height)
            : base(texture, width, height)
        {
            this.direction = new Vector2(0, 1);
            this.CanCollide = true;
        }

        public virtual void Initialize(InputController controller, float startX = 0, float startY = 0)
        {
            this.InputController = controller;
            this.collisionRectangle = new Rectangle(0, 0, this.Width, this.Height);

            this.StartPosition = new Vector2(startX, startY);
        }

        protected void UpdateMovement(GameTime gameTime)
        {
            this.InputController.UpdateMovement(this, gameTime);

            if (!InputFrozen)
            {
                // Update player direction. Dont change if movement direction has no length
                if (MovementDirection.LengthSquared() != 0)
                {
                    this.direction = MovementDirection;
                }
            }

            Vector2 newPosition = Position + (MovementDirection * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (this.Level.PositionIsValid(LastPosition, newPosition))
            {
                LastPosition = this.Position;
                this.Position = newPosition;
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!InputFrozen)
            {
                this.UpdateMovement(gameTime);
            }

            this.UpdateEffects(gameTime);
            this.UpdateAnimation(gameTime);
            this.CheckForCollisions();
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 cameraPos)
        {
            this.Draw(spriteBatch, this.Position, cameraPos);
        }

        private void CheckForCollisions()
        {
            if (!this.CanCollide) return;

            foreach (var character in this.Level.RegisteredCharacters)
            {
                if (character.CanCollide && this.CollisionRectangle.Intersects(character.CollisionRectangle))
                {
                    this.OnCollision(character);
                }
            }
        }

        public Rectangle CollisionRectangle
        {
            get { return GeometryUtility.GetAdjustedRectangle(this.Position, this.collisionRectangle); }
        }

        public virtual void OnCollision(Character character)
        {

        }
    }
}
