﻿/*
 * Authors: Runi Jiang, Arthur Powers
 * Game State Manager class
 * Handles game states and transitions
 */

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Penumbra;
using Microsoft.Xna.Framework.Content;

namespace FinalProject
{
    enum GameState
    {
        IntroState,
        MenuState,
        OptionState,
        InstructionState,
        PlayState,
        PauseState,
        GameOverState
    }

    class GameStateManager
    {
        //Fields
        private GameState currentState;
        private float introTimer;
        private float menuLightTimer;
        private bool isMenuLighted;

        private KeyboardState ks;
        private KeyboardState previousKs;

        Map map;
        GraphicsDeviceManager _graphics;
        private Camera2D _camera;

        //Texture2D for menu and buttons
        private Texture2D menuNoLight_Texture;
        private Texture2D menuLight_Texture;
        private Texture2D instruction_Texture;
        private Texture2D pauseMask;
        private Texture2D fade_Texture;
        private Texture2D black_Texture;
        private Texture2D gameOver_Texture;

        private Button playButton;
        private Button optionButton;
        private Button instructionButton;
        private Button backMainButton;
        private Button mainMenuButton;
        private Button backGameButton;
        private Slider volumeSlider;

        private Fade _fadeTransition;

        //Font
        private SpriteFont syneTactileFont;

        //Buttons for the main menu
        private List<Button> buttons = new List<Button>();

        private Random rng = new Random();
        //Properties
        public GameState CurrentState { get => currentState; }
        public Map Map => map;

        //Constructors
        public GameStateManager(ContentManager content, PenumbraComponent penumbra, GraphicsDeviceManager graphics, Camera2D camera)
        {
            _graphics = graphics;
            _camera = camera;

            //Set intro timer
            introTimer = 2f;
            menuLightTimer = 2f;

            isMenuLighted = false;

            //Load Texture2d For menu and buttons
            LoadMenuContent(content, camera);

            //Initialize buttons

            instructionButton.Click += Click_ToInstruction;
            optionButton.Click += Click_ToOption;
            playButton.Click += Click_ToPlay;
            backMainButton.Click += Click_ToMenu;
            mainMenuButton.Click += Click_ToMenu;
            backGameButton.Click += Click_ToPlay;
            volumeSlider.Click += Click_UpdateVolume;

            buttons.Add(playButton);
            buttons.Add(optionButton);
            buttons.Add(instructionButton);

            // Load map
            map = new Map(penumbra, content, camera);
            //map.LoadTutorial();
            map.LoadBackgroundTestLevel();
        }

        //Methods
        /// <summary>
        /// Handles all visual displays for game states
        /// (Mostly menu display)
        /// </summary>
        /// <param name="batch">Sprite Batch</param>
        public void Display(SpriteBatch batch)
        {
            //Current state actions
            switch (currentState)
            {
                case GameState.IntroState:
                    break;

                case GameState.MenuState:

                    break;

                //
                case GameState.OptionState:
                    break;

                case GameState.InstructionState:
                    break;

                case GameState.PlayState:
                    map.Draw(batch);
                    break;

                case GameState.PauseState:

                    break;
                case GameState.GameOverState:

                    break;
            }
        }

        public void DrawBackground(SpriteBatch batch, Matrix tranformMatrix)
        {
            switch (currentState)
            {
                case GameState.PlayState:
                    map.DrawBackground(batch, tranformMatrix);
                    break;
            }
        }

        /// <summary>
        /// Draw UI and things that are not effected by Light system
        /// </summary>
        /// <param name="batch"></param>
        public void DrawUI(SpriteBatch batch)
        {
            //Current state actions
            switch (currentState)
            {
                case GameState.IntroState:
                    batch.Draw(instruction_Texture,
                        new Rectangle(0, 0, instruction_Texture.Width, instruction_Texture.Height),
                        Color.White);
                    break;

                case GameState.MenuState:
                    //Switch between no light menu and lighted menu background
                    if (isMenuLighted)
                    {
                        batch.Draw(menuLight_Texture,
                        new Rectangle(0, 0, menuLight_Texture.Width, menuLight_Texture.Height),
                        Color.White);
                    }
                    else
                    {
                        batch.Draw(menuNoLight_Texture,
                        new Rectangle(0, 0, menuNoLight_Texture.Width, menuNoLight_Texture.Height),
                        Color.White);
                    }

                    //Draw the buttons
                    for (int i = 0; i < buttons.Count; i++)
                    {
                        buttons[i].Draw(batch);
                    }


                    break;

                case GameState.OptionState:
                    batch.Draw(black_Texture, new Rectangle(0, 0, black_Texture.Width, black_Texture.Height), Color.White);
                    backMainButton.Draw(batch);
                    volumeSlider.Draw(batch);
                    // Test volume number 
                    batch.DrawString(syneTactileFont, volumeSlider.CurValue.ToString(), new Vector2(800, 300), Color.White);

                    break;

                case GameState.InstructionState:
                    batch.Draw(black_Texture, new Rectangle(0, 0, black_Texture.Width, black_Texture.Height), Color.White);
                    backMainButton.Draw(batch);
                    break;

                case GameState.PlayState:
                    map.DrawTest(batch);
                    break;

                case GameState.PauseState:
                    batch.Draw(pauseMask, Vector2.Zero, Color.White);
                    mainMenuButton.Draw(batch);
                    volumeSlider.Draw(batch);
                    break;

                case GameState.GameOverState:
                    batch.Draw(gameOver_Texture, Vector2.Zero, Color.White);
                    mainMenuButton.Draw(batch);
                    break;
            }
        }

        /// <summary>
        /// Handles all time-based functionality for game states
        /// (State switching, motion/animation/state switch detection)
        /// </summary>
        /// <param name="dTime">Time passed (seconds)</param>
        public void Update(float dTime, PenumbraComponent penumbra)
        {
            ks = Keyboard.GetState();

            //Current state actions
            switch (currentState)
            {
                case GameState.IntroState:

                    introTimer -= dTime;
                    if (introTimer <= 0)
                    {
                        currentState = GameState.MenuState;
                    }

                    // Update lighting effects after buttons have been updated
                    UpdatePenumbraState(penumbra);
                    break;

                case GameState.MenuState:

                    //Switch between no light menu and lighted menu background
                    menuLightTimer -= dTime;
                    if (menuLightTimer <= 0)
                    {
                        menuLightTimer = rng.Next(1, 3);
                        if (isMenuLighted)
                        {
                            isMenuLighted = false;
                        }
                        else
                        {
                            isMenuLighted = true;
                        }
                    }

                    //Update the buttons
                    for (int i = 0; i < buttons.Count; i++)
                    {
                        buttons[i].Update();
                    }

                    // Update lighting effects after buttons have been updated
                    UpdatePenumbraState(penumbra);
                    break;

                case GameState.OptionState:

                    backMainButton.Update();
                    volumeSlider.Update();

                    // Update lighting effects after buttons have been updated
                    UpdatePenumbraState(penumbra);
                    break;

                case GameState.InstructionState:

                    backMainButton.Update();

                    // Update lighting effects after buttons have been updated
                    UpdatePenumbraState(penumbra);
                    break;

                case GameState.PlayState:

                    map.Update(dTime);

                    // Press start(gamepad) or P(keyboard) to pause
                    if (ks.IsKeyDown(Keys.P) && previousKs.IsKeyUp(Keys.P))
                    {
                        volumeSlider.UpdatePosition(map.Player.Position);
                        System.Diagnostics.Debug.WriteLine(map.Player.Position);
                        mainMenuButton.UpdatePosition(map.Player.Position);

                        currentState = GameState.PauseState;

                    }

                    if (map.Player.CurrentState == PlayerState.DeadState)
                    {
                        currentState = GameState.GameOverState;
                    }



                    // Update lighting effects after buttons have been updated
                    UpdatePenumbraState(penumbra);
                    break;

                case GameState.PauseState:

                    // Press start(gamepad) or P(keyboard) to unpause
                    if (ks.IsKeyDown(Keys.P) && previousKs.IsKeyUp(Keys.P))
                    {
                        currentState = GameState.PlayState;
                    }



                    volumeSlider.Update();
                    mainMenuButton.Update();

                    // Update lighting effects after buttons have been updated
                    UpdatePenumbraState(penumbra);
                    break;
                case GameState.GameOverState:

                    mainMenuButton.Update();

                    // Update lighting effects after buttons have been updated
                    UpdatePenumbraState(penumbra);
                    break;
            }

            previousKs = ks;
        }

        /// <summary>
        /// Load the menu contents and create main buttons
        /// </summary>
        /// <param name="content"></param>
        public void LoadMenuContent(ContentManager content, Camera2D camera)
        {
            menuLight_Texture = content.Load<Texture2D>("Menu_Light");
            menuNoLight_Texture = content.Load<Texture2D>("Menu_noLight");
            instruction_Texture = content.Load<Texture2D>("Instruction");
            pauseMask = content.Load<Texture2D>("PauseMask");
            fade_Texture = content.Load<Texture2D>("blackbox2");
            black_Texture = content.Load<Texture2D>("Blackbackground");
            gameOver_Texture = content.Load<Texture2D>("GameEnd");

            playButton = new Button(content.Load<Texture2D>("Controls/Play"), content.Load<Texture2D>("Controls/Play_hover"), 1500, 700, _graphics);
            optionButton = new Button(content.Load<Texture2D>("Controls/Options"), content.Load<Texture2D>("Controls/Options_hover"), 1500, 800, _graphics);
            instructionButton = new Button(content.Load<Texture2D>("Controls/Instruction"), content.Load<Texture2D>("Controls/Instruction_hover"), 1500, 900, _graphics);

            Texture2D back = content.Load<Texture2D>("Controls/Back");
            Texture2D back_Hover = content.Load<Texture2D>("Controls/Back_Hover");
            backMainButton = new Button(back, back_Hover, _graphics.PreferredBackBufferWidth / 2 - back.Width / 2, 700, _graphics);
            backGameButton = new Button(back, back_Hover, _graphics.PreferredBackBufferWidth / 2 - back.Width / 2, 700, _graphics);
            Texture2D mainMain = content.Load<Texture2D>("Controls/MainMenu");
            Texture2D mainMain_Hover = content.Load<Texture2D>("Controls/MainMenu_hover");
            mainMenuButton = new Button(mainMain, mainMain_Hover, _graphics.PreferredBackBufferWidth / 2 - mainMain.Width / 2, 800, _graphics, camera);

            Texture2D sliderWidget = content.Load<Texture2D>("SliderBackground");
            Texture2D sliderIndicator = content.Load<Texture2D>("SliderIndicator");
            volumeSlider = new Slider(sliderIndicator, sliderWidget, _graphics.PreferredBackBufferWidth / 2 - sliderWidget.Width / 2, 400, 60, 100, _graphics, camera);

            syneTactileFont = content.Load<SpriteFont>("SyneTactile24");

        }

        /// <summary>
        /// Change the state to Instruction state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Click_ToInstruction(object sender, System.EventArgs e)
        {
            currentState = GameState.InstructionState;
        }

        /// <summary>
        /// Change the state to Option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Click_ToOption(object sender, System.EventArgs e)
        {
            SFXManager.LoopInstancedSound(Sounds.HBNormal, false);
            SFXManager.LoopInstancedSound(Sounds.SAmbience, false);
            volumeSlider.UpdatePosition(new Vector2(
                _camera.Position.X + _graphics.PreferredBackBufferWidth / 2,
                _camera.Position.Y + _graphics.PreferredBackBufferHeight / 2));
            currentState = GameState.OptionState;
        }

        /// <summary>
        /// Change the state to Play
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Click_ToPlay(object sender, System.EventArgs e)
        {
            SFXManager.LoopInstancedSound(Sounds.HBNormal, true);
            currentState = GameState.PlayState;
        }

        /// <summary>
        /// Change the state to the Main Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Click_ToMenu(object sender, System.EventArgs e)
        {
            SFXManager.StopAllInstances();
            currentState = GameState.MenuState;
        }

        /// <summary>
        /// Update the volume
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Click_UpdateVolume(object sender, System.EventArgs e)
        {
            Slider cur = (Slider)sender;
            SFXManager.SetGlobalVolume((float)cur.Percentage);
            // Can use slider.Percentage / slider.CurValue to access the number
            // And then change the volume
        }

        /// <summary>
        /// Decide when to show penumbra lighting.
        /// Should be used whenever <see cref="currentState"/> is changed.
        /// </summary>
        /// <param name="penumbra"></param>
        private void UpdatePenumbraState(PenumbraComponent penumbra) =>
            penumbra.Visible = currentState switch
            {
                GameState.PlayState => true,
                GameState.PauseState => true,
                _ => false
            };
    }
}
