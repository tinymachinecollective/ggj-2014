
namespace GGJ2014.Game.Engine
{
    using GGJ2014.Game.Engine.Animation;
    using GGJ2014.Game.Engine.Controls;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Antelope : Character
    {
        public Antelope() : base(BigEvilStatic.Content.Load<Texture2D>("antelope"), 32, 32)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 cameraPos)
        {
            base.Draw(spriteBatch, cameraPos);
        }

        public void NomNomNom()
        {
            this.CanCollide = false;
            this.Effects.Add(new FadeEffect(500, true));
        }
    }
}
