using GGJ2014.Game.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GGJ2014.Game.Engine.Controls;
using GGJ2014.Game.Editor;

namespace GGJ2014.Game.Logic
{
    public class EditorScreen : Screen
    {
        private Placer placer;
        private SpriteBatch spriteBatch;

        public EditorScreen(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            this.LoadContent();
        }

        public void LoadContent()
        {
            this.placer = new Placer();
            this.placer.Initialize();
        }

        public override void Draw(Bounds bounds)
        {
            this.spriteBatch.Begin();

            this.placer.Draw(spriteBatch);

            this.spriteBatch.End();
        }

        public override void Update(GameTime time)
        {
            placer.Update();
        }
    }
}
