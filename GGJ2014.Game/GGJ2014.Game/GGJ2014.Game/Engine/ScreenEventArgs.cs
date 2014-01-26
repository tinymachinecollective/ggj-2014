
namespace GGJ2014.Game.Engine
{
    using System;

    public class ScreenEventArgs : EventArgs
    {
        public Screen Screen { get; private set; }

        public ScreenEventArgs(Screen screen)
        {
            this.Screen = screen;
        }
    }
}
