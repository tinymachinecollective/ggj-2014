
namespace GGJ2014.Game.Engine
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using GGJ2014.Game.Engine.Graphics;
    using Microsoft.Xna.Framework.Audio;
    using GGJ2014.Game.Engine.Animation;

    public class Level
    {
        private static readonly Random rng = new Random();

        protected Sprite purgatoryOverlay;

        protected int HalfTilesWideOnScreen;
        protected int HalfTilesLongOnScreen;

        public const int TileWidth = 32;

        protected List<Rectangle> rectangles;
        protected Sprite backgroundGround;
        protected Cue pickupSFX;
        private const int MaxPickups = 30;

        protected Sprite purgatoryText, findPortalText;

        protected Level()
        {
            this.purgatoryOverlay = new Sprite(BigEvilStatic.Content.Load<Texture2D>("WhiteOut"), 48, 48);
            this.purgatoryOverlay.Zoom = 100f;
            this.purgatoryOverlay.Alpha = 0f;

            Texture2D purgTextTex = BigEvilStatic.Content.Load<Texture2D>("purgatory");
            Texture2D portalTextTex = BigEvilStatic.Content.Load<Texture2D>("findportal");

            purgatoryText = new Sprite(purgTextTex, purgTextTex.Width, purgTextTex.Height);
            purgatoryText.Alpha = 0;
            findPortalText = new Sprite(portalTextTex, portalTextTex.Width, portalTextTex.Height);
            findPortalText.Alpha = 0;
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public Vector2 FindSpawnPoint(bool playerSafe)
        {
            return Vector2.Zero;
        }

        public virtual void Draw(SpriteBatch batch, Bounds bounds)
        {
        }
    }
}
