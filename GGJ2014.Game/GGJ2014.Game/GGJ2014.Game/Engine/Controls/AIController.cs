
namespace GGJ2014.Game.Engine.Controls
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    public class AIController 
    {
        private Monster monster;

        //  test code
        private Random rng = new Random();
        private int lower = -400;
        private int upper = 500;

        //  Limiting factors
        private int fogOfWarDistance = 100;
        private int maxSpeed = 350;

        //  Core functions
        public AIController(Monster monster)
        {
            this.monster = monster;
        }

        public void MonsterUpdateMovement(Monster monster, GameTime gameTime, Vector2 playerPosition)
        {
            monster.MovementDirection = this.GetMovementDirection(playerPosition);

            if (monster.MovementDirection.LengthSquared() != 0)
            {
                monster.MovementDirection = Vector2.Normalize(monster.MovementDirection);
            }
        }

        protected Vector2 GetMovementDirection(Vector2 playerPosition)
        {

            double distanceFromPlayer = GetDistanceFromPlayer(playerPosition);
            Vector2 move;

            if (distanceFromPlayer > fogOfWarDistance) 
            {
                //  player is too far away
                //  random movement
                move = MoveRandomly();
            }
            else 
            {
                //  can see player!
                //  approach player
                move = MoveTowardsPlayer(playerPosition);
            }

            //Vector2 diff = new Vector2(rng.Next(lower, upper) + playerPosition.X, rng.Next(lower, upper) + playerPosition.Y) - monster.Position;
            Vector2 diff = move - monster.Position;

            if (diff.Length() > 10)
            {
                return diff;
            }

            return Vector2.Zero;
        }

        //  AI code
        private double GetDistanceFromPlayer(Vector2 playerposition)
        {
            double xdist = monster.Position.X - playerposition.X;
            double ydist = monster.Position.Y - playerposition.Y;

            return Math.Sqrt(xdist * xdist + ydist * ydist);
        }

        private Vector2 MoveRandomly()
        {
            float offsetX = rng.Next(lower, upper) - rng.Next(lower, upper);
            float offsetY = rng.Next(lower, upper) - rng.Next(lower, upper);

            return new Vector2(offsetX, offsetY);
        }

        private Vector2 MoveTowardsPlayer(Vector2 playerPosition)
        {
            float moveX = playerPosition.X % maxSpeed;
            float moveY = playerPosition.Y % maxSpeed;

            return new Vector2(moveX, moveY);
        }
    }
}
