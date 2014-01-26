using GGJ2014.Game.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ2014.Game.Engine.UI
{
    public class SpriteControl : Control
    {
        private Sprite sprite;
        public Vector2 Position;

        public SpriteControl(Sprite sprite, Vector2 position)
        {
            this.sprite = sprite;
            this.Position = position;
        }

        public override void Update(GameTime time)
        {
            // Chill
        }

        public override void Draw(SpriteBatch batch)
        {
            this.sprite.Draw(batch, Position, Vector2.Zero);
        }
    }
}
