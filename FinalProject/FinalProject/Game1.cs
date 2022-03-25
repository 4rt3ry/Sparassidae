using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;
using Penumbra;
using Microsoft.Xna.Framework.Content;
namespace FinalProject
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _batch;

        private GameStateManager _gameStateManager;
        private PenumbraComponent _penumbra;

        Player _player;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.HardwareModeSwitch = true;
            _graphics.ApplyChanges();

            // Create lighting component and register it as a service so that subsystem can access it
            _penumbra = new PenumbraComponent(this)
            {
                AmbientColor = Color.Black
            };
            //Services.AddService(_penumbra);

            //creates the player
            _player = new Player();

            _penumbra.Initialize();

            base.Initialize();

        }

        protected override void LoadContent()
        {
            _batch = new SpriteBatch(GraphicsDevice);

            //Load menu and button content
            //Initialized the gameStateManager
            _gameStateManager = new GameStateManager(Content, _player, _penumbra);

        }
        

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            _gameStateManager.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Everything after this call will be affected by the lighting system.
            _penumbra.BeginDraw();

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _batch.Begin();
            _gameStateManager.Display(_batch);
            _gameStateManager.Fade(_batch, .5f, .5f);

            _batch.End();

            // Draw the actual lit scene.
            _penumbra.Draw(gameTime);

            // Draw stuff that is not affected by lighting (UI, etc).
            _batch.Begin();
            _gameStateManager.DrawUI(_batch);
            _batch.End();

            base.Draw(gameTime);
        }
    }
}
