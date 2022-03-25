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
        private Fade _fadeTransition;
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
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            // Create lighting component and register it as a service so that subsystem can access it
            _penumbra = new PenumbraComponent(this)
            {
                AmbientColor = Color.Black
            };
            //Services.AddService(_penumbra);

            //creates the player
            _player = new Player();
            _fadeTransition = new Fade();
            _penumbra.Initialize();

            base.Initialize();

        }

        protected override void LoadContent()
        {
            _batch = new SpriteBatch(GraphicsDevice);

            //Load menu and button content
            //Initialized the gameStateManager
            _fadeTransition.LoadContent(Content);
            _gameStateManager = new GameStateManager(Content, _player, _penumbra);

        }
        float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }
        Vector2 Lerp(Vector2 firstVector, Vector2 secondVector, float by)
        {
            float retX = Lerp(firstVector.X, secondVector.X, by);
            float retY = Lerp(firstVector.Y, secondVector.Y, by);
            return new Vector2(retX, retY);
        }

        void PlayerMovement(float dt)
        {
            float speed = 5;
            KeyboardState kb = Keyboard.GetState();

            switch (_player.CurrentState)
            {
                case PlayerState.WalkingState:
                    speed = 5;
                    break;
                case PlayerState.AfraidState:
                    speed = speed/2;
                    break;
                case PlayerState.ShockState:
                    speed = 1;
                    break;
                case PlayerState.ChaseState:
                    speed *= 2;
                    break;
                case PlayerState.DeadState:
                    speed = 0;
                    break;
            }

            //get keyboard inputs
            bool aDown = kb.IsKeyDown(Keys.A);
            bool dDown = kb.IsKeyDown(Keys.D);
            bool wDown = kb.IsKeyDown(Keys.W);
            bool sDown = kb.IsKeyDown(Keys.S);


            Vector2 addVelocity = Vector2.Zero;
            if (aDown) { addVelocity += -Vector2.UnitX; }
            if (dDown) { addVelocity += Vector2.UnitX; }
            if (wDown) { addVelocity += -Vector2.UnitY; }
            if (sDown) { addVelocity += Vector2.UnitY; }

            // Ensures speed remains the same when moving diagonally
            if (addVelocity.LengthSquared() > 0)
            {
                addVelocity.Normalize();
                addVelocity *= speed;
            }

            //adds acceleration/smoothing by lerping
            _player.Velocity = Lerp(_player.Velocity, addVelocity,.1f);

            _player.Position += _player.Velocity;
            //Debug.WriteLine(playerObject.Position);
            //Debug.WriteLine("add velocity is " + addVelocity);


        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();
            float updateTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            PlayerMovement(updateTime);
            _gameStateManager.Update(updateTime);
            _fadeTransition.Update(updateTime);
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            // Everything after this call will be affected by the lighting system.
           _penumbra.BeginDraw();

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _batch.Begin();
            _gameStateManager.Display(_batch);

            _batch.End();

            // Draw the actual lit scene.
            _penumbra.Draw(gameTime);

            // Draw stuff that is not affected by lighting (UI, etc).
            _batch.Begin();

            _gameStateManager.DrawUI(_batch);
            _fadeTransition.StartFade(_batch, 1f, 1f);

            _batch.End();

            base.Draw(gameTime);
        }
    }
}
