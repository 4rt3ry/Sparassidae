using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FinalProject
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch batch;

        private GameStateManager gameStateManager;

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
        }

        void PlayerMovement()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            gameStateManager.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

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
