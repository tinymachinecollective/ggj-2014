
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
        private const int lower = -500;
        private const int upper = 500;

        //  Limiting factors
        private const float fogOfWarDistance = 200;

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

            if (distanceFromPlayer > fogOfWarDistance) 
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
            float offsetX = (rng.Next(lower, upper) - rng.Next(lower, upper));
            float offsetY = (rng.Next(lower, upper) - rng.Next(lower, upper));

            return new Vector2(offsetX, offsetY);
        }


        private Vector2 MoveTowardsPlayer(Vector2 playerPosition)
        {
            return playerPosition - monster.Position;
        }

    }
}
