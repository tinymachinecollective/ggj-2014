
namespace GGJ2014.Game.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework.Graphics;

    public class Antelope : Character
    {
        public Antelope() : base(BigEvilStatic.Content.Load<Texture2D>("sport_basketball"), 16, 16)
        {
        }
    }
}
