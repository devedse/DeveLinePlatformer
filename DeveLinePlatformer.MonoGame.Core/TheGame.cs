using DeveLinePlatformer.MonoGame.Core.Data;
using DeveLinePlatformer.MonoGame.Core.HelperObjects;
using DeveLinePlatformer.MonoGame.Core.PlatformerGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace DeveLinePlatformer.MonoGame.Core
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TheGame : Game
    {
        public Platform Platform { get; }

        private IntSize? _desiredScreenSize = null;
        private IContentManagerExtension _contentManagerExtension = null;


        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private BasicEffect effect;


        public bool AllowMouseResets { get; }
        public int ScreenWidth { get; private set; }
        public int ScreenHeight { get; private set; }


        public static readonly string Version = typeof(TheGame).Assembly.GetName().Version.ToString();

        private MapData mapData;
        private MapEditor mapEditor;
        private CoolPlatformerGame platformerGame;


        public TheGame() : this(Platform.Desktop)
        {

        }

        public TheGame(Platform platform) : this(null, platform)
        {

        }

        public TheGame(IntSize? desiredScreenSize, Platform platform) : this(null, desiredScreenSize, platform)
        {
        }

        public TheGame(IContentManagerExtension contentManagerExtension, IntSize? desiredScreenSize, Platform platform) : base()
        {
            _contentManagerExtension = contentManagerExtension;
            _desiredScreenSize = desiredScreenSize;
            Platform = platform;

            AllowMouseResets = Platform != Platform.Blazor;
            graphics = new GraphicsDeviceManager(this);

            // Profile
            graphics.PreparingDeviceSettings += (sender, e) =>
            {
                if (e.GraphicsDeviceInformation.Adapter.IsProfileSupported(GraphicsProfile.HiDef))
                {
                    e.GraphicsDeviceInformation.GraphicsProfile = GraphicsProfile.HiDef;
                    graphics.GraphicsProfile = GraphicsProfile.HiDef;
                }

                Console.WriteLine($"Graphics profile is now: {e.GraphicsDeviceInformation.GraphicsProfile}");
            };

            //This is bugged in MonoGame 3.8.1 and creates a white wash over everything
            graphics.PreferMultiSampling = false;
            //GraphicsDevice.PresentationParameters.MultiSampleCount = 16;

            IsMouseVisible = true;


            //This is required for Blazor since it loads assets in a custom way
            Content = new ExtendibleContentManager(Services, _contentManagerExtension);
            Content.RootDirectory = "Content";
        }

        private void TheGame_Activated(object sender, EventArgs e)
        {
            //Console.WriteLine($"{DateTime.Now}: ACTIVATED");
            ResetMouseToCenter();
        }


        protected override void Initialize()
        {
            graphics.SynchronizeWithVerticalRetrace = true;
            //TargetElapsedTime = TimeSpan.FromTicks(1);
            TargetElapsedTime = TimeSpan.FromSeconds(1d / 240d);
            IsFixedTimeStep = false;


            Window.ClientSizeChanged += Window_ClientSizeChanged;
            Window.OrientationChanged += Window_OrientationChanged;

#if !BLAZOR
            if (_desiredScreenSize != null)
            {
                graphics.PreferredBackBufferWidth = _desiredScreenSize.Value.Width;
                graphics.PreferredBackBufferHeight = _desiredScreenSize.Value.Height;
            }
            else if (Platform == Platform.Android)
            {
                //For android I haven't been able to find the "FULL" screen size in the AndroidActivity
                //Whenever I tried it it would only give me the size of everything excluding system bars.
                //Unless I did GetRealMetrics but then it would always grab the full size, I want this to be dynamic.
                //So the value that is actually dynamic and correct is this:
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            else
            {
                //graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
                //graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            }
#endif

            Window.AllowUserResizing = true;

            if (Platform == Platform.UWP) // && Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile"
            {
                //To remove the Battery bar
                graphics.IsFullScreen = true;
            }

            graphics.ApplyChanges();

            Activated += TheGame_Activated;

            FixScreenSize();

            base.Initialize();




            mapData = new MapData();

            platformerGame = new CoolPlatformerGame(mapData);
            mapEditor = new MapEditor(mapData);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            effect = new BasicEffect(GraphicsDevice);

            ContentDing.GoLoadContent(GraphicsDevice, Content);
        }

        private void Window_OrientationChanged(object sender, EventArgs e)
        {
            FixScreenSize();
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            FixScreenSize();
        }

        private void FixScreenSize()
        {
            ScreenWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
            ScreenHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
        }

        public void ResetMouseToCenter()
        {
            if (AllowMouseResets)
            {
                Mouse.SetPosition(ScreenWidth / 2, ScreenHeight / 2);
            }
        }

        public void ToggleFullScreenBetter()
        {
            if (graphics.IsFullScreen)
            {
                if (_desiredScreenSize != null)
                {
                    graphics.PreferredBackBufferWidth = _desiredScreenSize.Value.Width;
                    graphics.PreferredBackBufferHeight = _desiredScreenSize.Value.Height;
                }
            }
            else
            {
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            graphics.IsFullScreen = !graphics.IsFullScreen;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            InputDing.PreUpdate();

            if (InputDing.CurKey.IsKeyDown(Keys.Escape) && Platform != Platform.Blazor)
            {
                Exit();
            }



            if (InputDing.KeyDownUp(Keys.Enter) && (InputDing.CurKey.IsKeyDown(Keys.LeftAlt) || InputDing.CurKey.IsKeyDown(Keys.RightAlt)))
            {
                //graphics.PreferredBackBufferWidth = 3840;
                //graphics.PreferredBackBufferHeight = 2160;
                //graphics.ApplyChanges();
                //graphics.ToggleFullScreen();
                ToggleFullScreenBetter();
            }

            if (InputDing.KeyDownUp(Keys.V))
            {
                graphics.SynchronizeWithVerticalRetrace = !graphics.SynchronizeWithVerticalRetrace;
                graphics.ApplyChanges();
            }

            if (InputDing.KeyDownUp(Keys.F))
            {
                IsFixedTimeStep = !IsFixedTimeStep;
            }

            mapEditor.Update();
            platformerGame.Update(gameTime);



            InputDing.AfterUpdate();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            mapEditor.Draw(gameTime, spriteBatch);
            platformerGame.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
