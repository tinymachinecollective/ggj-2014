
namespace GGJ2014.Game.Engine.Controls
{
    using System;
    using Microsoft.Xna.Framework;

    public enum AntelopeGoal
    {
        Meander,
        Graze,
        Flee
    }

    public class FleeController : InputController
    {
        private Antelope monster;

        //  AI variables
        private Random random;
        private float timeUntilNextEvaluation = 0;

        //  persistence
        private float lastDirection = 0;
        private Player player;

        //  Core functions
        public FleeController(Antelope monster, Player player, Random random)
        {
            this.monster = monster;
            this.player = player;
            this.random = random;
        }

        protected override Vector2 GetMovementDirection(GameTime gameTime)
        {
            if (timeUntilNextEvaluation == 0)
            {
                EvaluateNewGoal();
                timeUntilNextEvaluation = (float)this.random.NextDouble() * 20 + 5;
            }

            return Vector2.Zero;
        }

        private void EvaluateNewGoal()
        {
            throw new NotImplementedException();
        }

        private float GetDistanceFromPlayer(Vector2 playerposition)
        {
            return (monster.Position - playerposition).Length();
        }
    }
}
