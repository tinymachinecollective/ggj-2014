
namespace GGJ2014.Game.Engine
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using GGJ2014.Game.Engine.UI;

    public abstract class Screen
    {
        public Form Hud { get; private set; }

        public Screen(GraphicsDevice device)
        {
            this.Hud = new Form(device);
            this.Device = device;
        }

        public GraphicsDevice Device { get; private set; }

        public abstract void Draw(Bounds bounds);
        public abstract void Update(GameTime time);

        public void LoadScreen(Screen screen)
        {
            if (LoadingScreen != null)
            {
                this.LoadingScreen(this, new ScreenEventArgs(screen));
            }
        }

        protected void CloseScreen()
        {
            this.ClosingScreen(this, EventArgs.Empty);
        }

        protected void CloseUntil(Type screenType)
        {
            this.OnControlLost();
            this.ClosingAllScreensUntil(this, new ScreenTypeEventArgs(screenType));
        }

        public event EventHandler<ScreenEventArgs> LoadingScreen;
        public event EventHandler ClosingScreen;
        public event EventHandler<ScreenTypeEventArgs> ClosingAllScreensUntil;

        public virtual void OnControlReturned()
        {
        }

        public virtual void OnControlLost()
        {
        }
    }
}
