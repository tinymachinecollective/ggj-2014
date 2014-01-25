
namespace GGJ2014.Game.Engine
{
    using Microsoft.Xna.Framework.Graphics;
    using GGJ2014.Game.Engine.Controls;
    using Microsoft.Xna.Framework;

    public class Monster : Character
    {
        public float LineOfSight { get; set; }

        public Monster(string texture, int width, int height, float speed, float lineOfSight)
            : base(BigEvilStatic.Content.Load<Texture2D>(texture), width, height)
        {
            this.Speed = speed;
            this.LineOfSight = lineOfSight;
        }

        public override void Draw(SpriteBatch spriteBatch, Microsoft.Xna.Framework.Vector2 cameraPos)
        {
            base.Draw(spriteBatch, cameraPos);

            var aiController = this.InputController as AIController;
            spriteBatch.DrawString(BigEvilStatic.GetDefaultFont(), aiController.timeSinceMove.ToString(), new Microsoft.Xna.Framework.Vector2(500, 10f), Color.HotPink);
        }
    }
}
