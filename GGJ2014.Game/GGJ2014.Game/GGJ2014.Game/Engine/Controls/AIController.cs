
namespace GGJ2014.Game.Engine.Controls
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    public class AIController 
    {
        private Monster monster;

        //  AI variables
        private Random rng = new Random();
        private const int lower = 0;
        private const int upper = 100;

        //  persistence
        private float lastDirection = 0;


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

            float distanceFromPlayer = GetDistanceFromPlayer(playerPosition);
            Vector2 move;

            if (distanceFromPlayer > monster.Sight) 
            {
                //  player is too far away
                //  random movement
                move = MoveToRandomLocation();
            }
            else 
            {
                //  can see player!
                //  approach player

                AudioManager.Instance.PlayMusic(AudioManager.Instance.LoadCue("music-LoudLoop")); ;
                move = MoveTowardsPlayer(playerPosition);
            }

            
            if (move.Length() > 10)
            {
                return move;
            }
            return Vector2.Zero;

            
        }

        //  AI code
        private float GetDistanceFromPlayer(Vector2 playerposition)
        {
            return (monster.Position - playerposition).Length();
        }

        private Vector2 MoveToRandomLocation()
        {

            float direction = (float)((lastDirection + rng.NextDouble() - 0.5) % 360); 
            float distance = rng.Next(lower, upper);

            lastDirection = direction;

            Vector2 move;

            float offsetX = distance * (float)Math.Cos(direction);
            float offsetY = distance * (float)Math.Sin(direction);

            move = new Vector2(offsetX, offsetY);
            if (move.Length() > 10)
            {
                return move;
            }

            return Vector2.Zero;
        }


        private Vector2 MoveTowardsPlayer(Vector2 playerPosition)
        {
            return playerPosition - monster.Position;
        }

    }
}
