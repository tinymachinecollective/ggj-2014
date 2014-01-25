
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

    public class AntelopeController : InputController
    {
        private Antelope antelope;
        private AntelopeGoal goal;
        private Player player;
        private Random random;
        private Vector2 meanderDirection;
        private float lineOfSight = 50;
        private float timeUntilNextEvaluation = 0;

        public AntelopeGoal Goal { get { return this.goal; } }

        //  Core functions
        public AntelopeController(Antelope monster, Player player, Random random)
        {
            this.antelope = monster;
            this.player = player;
            this.random = random;
        }


        protected override Vector2 GetMovementDirection(GameTime gameTime)
        {
            timeUntilNextEvaluation -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeUntilNextEvaluation <= 0)
            {
                EvaluateNewGoal();

                if (goal == AntelopeGoal.Graze)
                {
                    timeUntilNextEvaluation = (float)this.random.NextDouble() * 5 + 7;
                }
                else
                {
                    timeUntilNextEvaluation = (float)this.random.NextDouble() * 3 + 2;
                }
            }

            if (goal == AntelopeGoal.Meander && AntelopeShouldRun())
            {
                EvaluateNewGoal();
            }

            if (goal == AntelopeGoal.Meander)
            {
                antelope.Speed = 50;
                return meanderDirection;
            }
            else if (goal == AntelopeGoal.Flee)
            {
                antelope.Speed = 350;
                return antelope.Position - player.Position;
            }
            else // graze
            {
                return Vector2.Zero;
            }
        }

        private void EvaluateNewGoal()
        {
            if (AntelopeShouldRun())
            {
                this.goal = AntelopeGoal.Flee;
            }
            else
            {
                if (this.goal == AntelopeGoal.Flee)
                {
                    this.goal = AntelopeGoal.Meander;
                }
                else
                {
                    this.goal = (AntelopeGoal)random.Next(2);
                }

                meanderDirection = Vector2.Normalize(new Vector2((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f));
            }
        }

        private bool AntelopeShouldRun()
        {
            return (antelope.Position - player.Position).Length() < lineOfSight;
        }
    }
}
