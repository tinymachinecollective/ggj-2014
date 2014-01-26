
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
        public float lastDirection;
        public float currDirection;
        private Player player;

        //  Core functions
        public AIController(Monster monster, Player player, Random random)
        {
            this.monster = monster;
            this.player = player;
            this.rng = random;

            //  start moving in a random direction
            lastDirection = rng.Next(0, 360);
        }

        protected override Vector2 GetMovementDirection(GameTime gameTime)
        {
            float distanceFromPlayer = GetDistanceFromPlayer(this.player.Position);
            Vector2 move;

            timeSinceMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (distanceFromPlayer > monster.LineOfSight) 
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
            //  debug
            if ( currDirection < 0 ) Console.WriteLine("currDirection = " + currDirection);

            if (timeSinceMove > 4.0)
            {
                currDirection = (float)Math.Abs(((360 + lastDirection + (rng.NextDouble()-0.5)*10)) % 360);
                timeSinceMove = 0;
            }
            else
            {
                currDirection = lastDirection;
            }

            float distance = rng.Next(lower, upper);

            float offsetX = distance * (float)Math.Cos(currDirection * MathHelper.Pi / 180);
            float offsetY = distance * (float)Math.Sin(currDirection * MathHelper.Pi / 180);

            Vector2 move;

            move = new Vector2(offsetX, offsetY);

            //  Check if we are leashed
            if ((monster.Position - monster.StartPosition).Length() > monster.TetherLength)
            {
                //  move back to start
                move = monster.StartPosition - monster.Position;

                //  reverse direction for future reference
                currDirection = currDirection - 180;
                if (currDirection < 0) currDirection = 360 + currDirection;

            }


            //  remember where you went...
            lastDirection = currDirection;

            //Console.WriteLine("curr loc: " + monster.Position);
            //Console.WriteLine("move to:  " + move);

            return move;
        }


        private Vector2 MoveTowardsPlayer(Vector2 playerPosition)
        {
            Vector2 move = playerPosition - monster.Position;

            //  preserve direction
            lastDirection = (float)Math.Atan(move.Y / move.X);

            return move;
        }



    }
}
