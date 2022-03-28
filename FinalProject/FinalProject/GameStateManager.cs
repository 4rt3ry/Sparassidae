/*
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

        //Texture2D for menu and buttons
        private Texture2D menuNoLight_Texture;
        private Texture2D menuLight_Texture;
        private Texture2D instruction_Texture;
        private Texture2D pauseMask;
        private Texture2D fade_Texture;
        private Texture2D black_Texture;

        private Button playButton;
        private Button optionButton;
        private Button instructionButton;
        private Button backMainButton;
        private Button mainMenuButton;
        private Button backGameButton;

                private Fade _fadeTransition;


        //Buttons
        private List<Button> buttons = new List<Button>();

        private Random rng = new Random();
        //Properties
        public GameState CurrentState { get => currentState; } 

        //Constructors
        public GameStateManager(ContentManager content, PenumbraComponent penumbra, GraphicsDeviceManager graphics)
        {
            _graphics = graphics;

            //Set intro timer
            introTimer = 2f;
            menuLightTimer = 2f;

            isMenuLighted = false;

            //Load Texture2d For menu and buttons
            LoadMenuContent(content);

            //Initialize buttons

            instructionButton.Click += Click_ToInstruction;
            optionButton.Click += Click_ToOption;
            playButton.Click += Click_ToPlay;
            backMainButton.Click += Click_ToMenu;
            mainMenuButton.Click += Click_ToMenu;
            backGameButton.Click += Click_ToPlay;

            buttons.Add(playButton);
            buttons.Add(optionButton);
            buttons.Add(instructionButton);

            // Load map
            map = new Map( penumbra);

        
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
                    break;

                case GameState.InstructionState:
                    batch.Draw(black_Texture, new Rectangle(0, 0, black_Texture.Width, black_Texture.Height), Color.White);
                    backMainButton.Draw(batch);
                    break;

                case GameState.PlayState:
                    break;

                case GameState.PauseState:
                    batch.Draw(pauseMask, Vector2.Zero, Color.White);
                    mainMenuButton.Draw(batch);
                    break;

                case GameState.GameOverState:
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

                    penumbra.Visible = false;

                    introTimer -= dTime;
                    if(introTimer <= 0)
                    {
                        currentState = GameState.MenuState;
                    }
                    break;

                case GameState.MenuState:

                    penumbra.Visible = false;

                    //Switch between no light menu and lighted menu background
                    menuLightTimer -= dTime;
                    if(menuLightTimer <= 0)
                    {
                        menuLightTimer = rng.Next(1, 3);
                        if(isMenuLighted)
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

                    break;

                case GameState.OptionState:

                    penumbra.Visible = false;

                    backMainButton.Update();
                    break;

                case GameState.InstructionState:

                    penumbra.Visible = false;

                    backMainButton.Update();
                    break;

                case GameState.PlayState:

                    penumbra.Visible = true;


                    map.Update(dTime);

                    // Press start(gamepad) or P(keyboard) to pause
                    if (ks.IsKeyDown(Keys.P) && previousKs.IsKeyUp(Keys.P))
                    {
                        currentState = GameState.PauseState;
                    }
                    break;

                case GameState.PauseState:

                    penumbra.Visible = true;

                    // Press start(gamepad) or P(keyboard) to unpause
                    if (ks.IsKeyDown(Keys.P) && previousKs.IsKeyUp(Keys.P))
                    {
                        currentState = GameState.PlayState;
                    }
                    mainMenuButton.Update();

                    break;
                case GameState.GameOverState:

                    penumbra.Visible = false;

                    mainMenuButton.Update();
                    break;
            }

            previousKs = ks;
        }

        /// <summary>
        /// Load the menu contents and create main buttons
        /// </summary>
        /// <param name="content"></param>
        public void LoadMenuContent(ContentManager content)
        {
            menuLight_Texture = content.Load<Texture2D>("Menu_Light");
            menuNoLight_Texture = content.Load<Texture2D>("Menu_noLight");
            instruction_Texture = content.Load<Texture2D>("Instruction");
            pauseMask = content.Load<Texture2D>("PauseMask");
            fade_Texture = content.Load<Texture2D>("blackbox2");
            black_Texture = content.Load<Texture2D>("Blackbackground");

            playButton = new Button(content.Load<Texture2D>("Controls/Play"), content.Load<Texture2D>("Controls/Play_hover"), 1500, 700);
            optionButton = new Button(content.Load<Texture2D>("Controls/Options"), content.Load<Texture2D>("Controls/Options_hover"), 1500, 800);
            instructionButton = new Button(content.Load<Texture2D>("Controls/Instruction"), content.Load<Texture2D>("Controls/Instruction_hover"), 1500, 900);

            Texture2D back = content.Load<Texture2D>("Controls/Back");
            Texture2D back_Hover = content.Load<Texture2D>("Controls/Back_Hover");
            backMainButton = new Button(back, back_Hover, _graphics.PreferredBackBufferWidth/2 - back.Width/2, 700);
            backGameButton = new Button(back, back_Hover, _graphics.PreferredBackBufferWidth / 2 - back.Width / 2, 700);
            Texture2D mainMain = content.Load<Texture2D>("Controls/MainMenu");
            Texture2D mainMain_Hover = content.Load<Texture2D>("Controls/MainMenu_hover");
            mainMenuButton = new Button(mainMain, mainMain_Hover, _graphics.PreferredBackBufferWidth / 2 - mainMain.Width/2, 800); 
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
            currentState = GameState.OptionState;
        }

        /// <summary>
        /// Change the state to Play
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Click_ToPlay(object sender, System.EventArgs e)
        {

            currentState = GameState.PlayState;
        }

        public void Click_ToMenu(object sender, System.EventArgs e)
        {
            currentState = GameState.MenuState;
        }


    }
}
