using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GGJ2014.Game.Engine.Graphics
{
    public class Fade
    {
        private Texture2D texture;
        private const float fadeTime = 0.5f;
        private float fadingTime = 0;
        private bool started = false;
        private bool finished;
        private float t;

        public Action OnComplete { get; set; }
        public bool FadeOut;

        public bool Fading { get { return this.started; } }

        public void Initialize()
        {
            texture = BigEvilStatic.Content.Load<Texture2D>("utility-images/black");
        }

        public void Start()
        {
            this.started = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!this.started || this.finished)
            {
                return;
            }

            fadingTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            t = fadingTime / fadeTime;

            if (FadeOut)
            {
                t = 1 - t;
            }

            if (t >= 1 || t < 0)
            {
                if (this.OnComplete != null) this.OnComplete();
                this.finished = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.started)
            {
                spriteBatch.Draw(texture, new Rectangle(0, 0, BigEvilStatic.Viewport.Width, BigEvilStatic.Viewport.Height), new Color(0, 0, 0, t));
            }
        }
    }
}
