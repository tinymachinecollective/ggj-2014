
namespace GGJ2014.Game.Engine.Controls
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using GGJ2014.Game.Logic;

    public class AIController : InputController
    {
        private Monster monster;

        //  AI variables
        private Random rng;
        private const int lower = 0;
        private const int upper = 100;
        private float timeSinceMove = 0;

        //  persistence
        private float lastDirection = 0;
        private Player player;

        //  Core functions
        public AIController(Monster monster, Player player, Random random)
        {
            this.monster = monster;
            this.player = player;
            this.rng = random;
        }

        protected override Vector2 GetMovementDirection(GameTime gameTime)
        {
            float distanceFromPlayer = GetDistanceFromPlayer(this.player.Position);
            Vector2 move;

            timeSinceMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
            /*
            if ((monster.Position - monster.StartPosition).Length() > monster.TetherLength)
            {
                //  monster is at its leash
                //  turn around
                move = monster.StartPosition;
            }
            else */ if (distanceFromPlayer > monster.LineOfSight) 
            {
                //  player is too far away
                //  random movement
                move = MoveToRandomLocation(gameTime);
            }
            else 
            {
                //  can see player!
                //  approach player

                AudioManager.Instance.QueueMusic(Music.LoadLoop);
                AudioManager.Instance.AndThenQueueMusic(Music.QuietLoop);
                move = MoveTowardsPlayer(player.Position);
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

        private Vector2 MoveToRandomLocation(GameTime gameTime)
        {
            float direction;

            if (timeSinceMove > 4.0)
            {
                direction = (float)((lastDirection + (rng.NextDouble()-0.5)*10) % 360);
                timeSinceMove = 0;
            }
            else
            {
                direction = lastDirection;
            }

            lastDirection = direction;

            float distance = rng.Next(lower, upper);
            float offsetX = distance * (float)Math.Cos(direction);
            float offsetY = distance * (float)Math.Sin(direction);

            Vector2 move;
            move = new Vector2(offsetX, offsetY);

            if (move.Length() > 10)
            {
                return move;
            }

            return Vector2.Zero;
        }


        private Vector2 MoveTowardsPlayer(Vector2 playerPosition)
        {
            Vector2 move = playerPosition - monster.Position;

            lastDirection = -(float)Math.Atan(move.Y / move.X);

            return move;
        }

    }
}
