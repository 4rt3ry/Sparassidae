using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;
namespace FinalProject
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch batch;

        private GameStateManager gameStateManager;

        Player playerObject;
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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            batch = new SpriteBatch(GraphicsDevice);

            //Load menu and button content
            //Initialized the gameStateManager
            gameStateManager = new GameStateManager(
                Content.Load<Texture2D>("Menu_Light"),
                Content.Load<Texture2D>("Menu_noLight"), 
                Content.Load<Texture2D>("Instruction"),
                Content.Load<Texture2D>("Controls/Instruction"), 
                Content.Load<Texture2D>("Controls/Instruction_hover"),
                Content.Load<Texture2D>("Controls/Options"),
                Content.Load<Texture2D>("Controls/Options_hover"),
                Content.Load<Texture2D>("Controls/Play"),
                Content.Load<Texture2D>("Controls/Play_hover"));

            //creates the player
            playerObject = new Player();

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

            switch (playerObject.CurrentState)
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
            playerObject.Velocity = Lerp(playerObject.Velocity, addVelocity,.1f);

            playerObject.Position += playerObject.Velocity;
            //Debug.WriteLine(playerObject.Position);
            //Debug.WriteLine("add velocity is " + addVelocity);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            gameStateManager.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            PlayerMovement((float) gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            batch.Begin();

            gameStateManager.Display(batch);

            batch.End();

            base.Draw(gameTime);
        }
    }
}
