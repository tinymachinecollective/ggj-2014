
namespace GGJ2014.Game.Engine
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using GGJ2014.Game.Engine.Controls;
    using GGJ2014.Game.Engine.Graphics;
using GGJ2014.Game.Engine.Graphics3D;

    public static class BigEvilStatic
    {
        private static SpriteFont defaultfont;
        private static Renderer renderer;

        public static ScreenManager ScreenManager { get; private set; }
        public static ContentManager Content { get; private set; }
        public static Viewport Viewport { get; private set; }
        public static GraphicsDevice GraphicsDevice { get; private set; }

        public static void Init(ScreenManager screenManager, GraphicsDevice device, ContentManager manager, Viewport viewport)
        {
            ScreenManager = screenManager;
            Content = manager;
            Viewport = viewport;
            GraphicsDevice = device;
        }

        public static SpriteFont GetDefaultFont()
        {
            if (defaultfont == null)
            {
                defaultfont = Content.Load<SpriteFont>("DefaultFont");
            }

            return defaultfont;
        }

        public static KeyboardInputController CreateControlSchemeWASD()
        {
            return new KeyboardInputController(Keys.W, Keys.S, Keys.A, Keys.D, Keys.Space, Keys.LeftShift);
        }

        public static KeyboardInputController CreateControlSchemeArrows()
        {
            return new KeyboardInputController(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Enter, Keys.RightShift);
        }

        public static KeyboardInputController CreateControlWinatronPlayerOne()
        {
            return new KeyboardInputController(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Z, Keys.X); // use X aswell
        }

        public static KeyboardInputController CreateControlWinatronPlayerTwo()
        {
            return new KeyboardInputController(Keys.I, Keys.K, Keys.J, Keys.L, Keys.M, Keys.M); // can use n aswell
        }

        public static XboxInputController CreateControlXboxPlayerOne()
        {
            return new XboxInputController(PlayerIndex.One);
        }

        public static XboxInputController CreateControlXboxPlayerTwo()
        {
            return new XboxInputController(PlayerIndex.Two);
        }

        public static Sprite CreateDeathWinBackground()
        {
            return new Sprite(Content.Load<Texture2D>("deathwins"), 1024, 768);
        }

        public static Sprite CreateLifeWinBackground()
        {
            return new Sprite(Content.Load<Texture2D>("lifewins"), 1024, 768);
        }

        internal static Vector2 GetScreenCentre()
        {
            return new Vector2(Viewport.Width / 2f, Viewport.Height / 2f);
        }

        public static Renderer Renderer
        {
            get
            {
                if (renderer == null)
                {
                    renderer = new Renderer(GraphicsDevice);
                }

                return renderer;
            }
        }
    }
}
