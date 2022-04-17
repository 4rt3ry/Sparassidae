using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Collections;
using System;
using Penumbra;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace FinalProject
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _batch;

        private GameStateManager _gameStateManager;
        private Fade _fadeTransition;
        private PenumbraComponent _penumbra;
        private Camera2D _camera;

        //List of all sound effects,
        //
        //
        //
        //
        public List<SoundEffect> soundEffects;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            soundEffects = new List<SoundEffect>();
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            //_graphics.IsFullScreen = true;
            _graphics.HardwareModeSwitch = true;
            _graphics.ApplyChanges();

            //// Create lighting component and register it as a service so that subsystem can access it
            _penumbra = new PenumbraComponent(this)
            {
                AmbientColor = Color.Black
            };
            ////Services.AddService(_penumbra);

            _fadeTransition = new Fade();


            _penumbra.Initialize();
            _penumbra.SpriteBatchTransformEnabled = true;

            _camera = new Camera2D(GraphicsDevice.Viewport);

            base.Initialize();

        }

        protected override void LoadContent()
        {
            _batch = new SpriteBatch(GraphicsDevice);

            //Load menu and button content
            //Initialized the gameStateManager
            _fadeTransition.LoadContent(Content);
            _gameStateManager = new GameStateManager(Content, _penumbra, _graphics,_camera);

            //Add sound effects
            soundEffects.Add(Content.Load<SoundEffect>("SoundFX/CatchAmbience"));
            soundEffects.Add(Content.Load<SoundEffect>("SoundFX/SpiderClick1"));
            soundEffects.Add(Content.Load<SoundEffect>("SoundFX/SpiderClick2"));
            soundEffects.Add(Content.Load<SoundEffect>("SoundFX/SpiderClick3"));
            soundEffects.Add(Content.Load<SoundEffect>("SoundFX/SpiderClick4"));
            soundEffects.Add(Content.Load<SoundEffect>("SoundFX/SpiderAmbience"));
            soundEffects.Add(Content.Load<SoundEffect>("SoundFX/HeartbeatNormal"));
            soundEffects.Add(Content.Load<SoundEffect>("SoundFX/HeartbeatRushed"));
            soundEffects.Add(Content.Load<SoundEffect>("SoundFX/HeartbeatFrantic"));
            soundEffects.Add(Content.Load<SoundEffect>("SoundFX/NormalBreathing"));
            soundEffects.Add(Content.Load<SoundEffect>("SoundFX/MediumBreathing"));
            soundEffects.Add(Content.Load<SoundEffect>("SoundFX/HeavyBreathing"));
            soundEffects.Add(Content.Load<SoundEffect>("SoundFX/Sigh"));
            soundEffects.Add(Content.Load<SoundEffect>("SoundFX/Alert"));
            soundEffects.Add(Content.Load<SoundEffect>("SoundFX/SpiderAmbienceChase"));
            soundEffects.Add(Content.Load<SoundEffect>("SoundFX/FLOn"));
            soundEffects.Add(Content.Load<SoundEffect>("SoundFX/FLOff"));
            soundEffects.Add(Content.Load<SoundEffect>("SoundFX/FLAmbience"));
            soundEffects.Add(Content.Load<SoundEffect>("SoundFX/BHover"));
            soundEffects.Add(Content.Load<SoundEffect>("SoundFX/BHEnd"));
            soundEffects.Add(Content.Load<SoundEffect>("SoundFX/AmbientWhiteNoise"));



            //Give manager the sounds
            SFXManager.GiveSFX(soundEffects);
        }
        

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();
            float updateTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //PlayerMovement(updateTime);
            if (_gameStateManager.CurrentState == GameState.PlayState)
            {
                _camera.Position = _gameStateManager.Map.Player.Position + new Vector2(-_graphics.PreferredBackBufferWidth / 2, -_graphics.PreferredBackBufferHeight / 2);

            }
            else if (_gameStateManager.CurrentState == GameState.MenuState ||
                _gameStateManager.CurrentState == GameState.IntroState ||
                _gameStateManager.CurrentState == GameState.InstructionState ||
                _gameStateManager.CurrentState == GameState.GameOverState)
            {
                //_camera.Position = new Vector2(-_graphics.PreferredBackBufferWidth / 2, -_graphics.PreferredBackBufferHeight / 2);
                _camera.Position = Vector2.Zero;
            }

            _gameStateManager.Update(updateTime, _penumbra);
            _fadeTransition.Update(updateTime);

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            // Everything after this call will be affected by the lighting system.
            _penumbra.BeginDraw();
            Matrix transMatrix = _camera.GetViewMatrix();
            _penumbra.Transform = transMatrix;
            GraphicsDevice.Clear(Color.White);

            // Draws background depending on game state
            _gameStateManager.DrawBackground(_batch, transMatrix);

            // Draws level
            _batch.Begin(transformMatrix: transMatrix);
            _gameStateManager.Display(_batch);
            _batch.End();

            // Draw the actual lit scene.
            _penumbra.Draw(gameTime);

            // Draw stuff that is not affected by lighting (UI, etc).
            _batch.Begin(transformMatrix: transMatrix,sortMode: SpriteSortMode.FrontToBack);

            ShapeBatch.Begin(GraphicsDevice);
            _gameStateManager.DrawUI(_batch);
            _fadeTransition.StartFade(_batch, 1f, 1f,GameState.InstructionState);
            ShapeBatch.End();
            _batch.End();

            base.Draw(gameTime);
        }
    }
}
