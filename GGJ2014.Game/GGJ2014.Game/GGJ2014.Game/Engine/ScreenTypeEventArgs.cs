
namespace GGJ2014.Game.Engine
{
    using System;

    public class ScreenTypeEventArgs : EventArgs
    {
        public ScreenTypeEventArgs(Type screenType)
        {
            this.ScreenType = screenType;
        }
        
        public Type ScreenType { get; private set; }
    }
}
