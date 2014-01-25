﻿
namespace GGJ2014.Game.Engine.Controls
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    public class AIController : InputController
    {
        private Monster monster;

        //  AI variables
        private Random rng = new Random();
        private const int lower = 0;
        private const int upper = 100;

        //  persistence
        private float lastDirection = 0;
        private Player player;

        //  Core functions
        public AIController(Monster monster, Player player)
        {
            this.monster = monster;
            this.player = player;
        }

        protected override Vector2 GetMovementDirection()
        {
            float distanceFromPlayer = GetDistanceFromPlayer(this.player.Position);
            Vector2 move;

            if (distanceFromPlayer > monster.LineOfSight) 
            {
                //  player is too far away
                //  random movement
                move = MoveToRandomLocation();
            }
            else 
            {
                //  can see player!
                //  approach player

                AudioManager.Instance.QueueMusic(AudioManager.Instance.LoadCue("music-LoudLoop"));
                AudioManager.Instance.AndThenQueueMusic(AudioManager.Instance.LoadCue("music-QuietLoop"));
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
