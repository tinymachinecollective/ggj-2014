﻿using GGJ2014.Game.Engine.Controls;
using GGJ2014.Game.Engine.Graphics;
using GGJ2014.Game.Engine.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ2014.Game.Engine
{
    public abstract class Character : Sprite, IMoveable
    {
        public static bool InputFrozen = false;
        public bool MovementFrozen { get; set; }
        public InputController InputController { get; set; }
        public Vector2 MovementDirection { get; set; }
        public Vector2 TargetDirection { get; set; }
        public float Speed { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 LastPosition { get; set; }
        public Level Level { get; set; }
        public bool CanCollide { get; protected set; }
        public float Maneuverability { get; set; }
        private Rectangle collisionRectangle;
        protected float ActualSpeed { get; private set; }

        public Vector2 StartPosition { get; set; }

        public Character(Texture2D texture, int width, int height)
            : base(texture, width, height)
        {
            this.CanCollide = true;
            this.Maneuverability = 5f;
        }

        public virtual void Initialize(InputController controller, float startX, float startY)
        {
            this.InputController = controller;
            this.collisionRectangle = new Rectangle(0, 0, this.Width, this.Height);

            this.StartPosition = new Vector2(startX, startY);

            this.Position = StartPosition;
        }

        protected void UpdateMovement(GameTime gameTime)
        {
            this.InputController.UpdateMovement(this, gameTime);
            this.MovementDirection += (this.TargetDirection - this.MovementDirection) * Maneuverability * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.TargetDirection.Length() > 0.1f && this.MovementDirection.Length() > 0)
            {
                this.MovementDirection = Vector2.Normalize(this.MovementDirection);
                ActualSpeed += Maneuverability * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (ActualSpeed >= Speed) ActualSpeed = Speed;
            }
            else
            {
                ActualSpeed -= Maneuverability * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (ActualSpeed < 0)
                {
                    ActualSpeed = 0;
                    //this.MovementDirection = Vector2.Zero;
                }
            }

            Vector2 delta = MovementDirection * ActualSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 newPosition = Position + delta;

            if (!MovementFrozen)
            {
                if (this.Level.PositionIsValid(LastPosition, newPosition))
                {
                    LastPosition = this.Position;
                    this.Position = newPosition;
                }
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
