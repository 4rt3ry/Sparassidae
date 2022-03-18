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

        //GameState fields
        Map map;

        //Texture2D for menu and buttons
        private Texture2D menuNoLight_Texture;
        private Texture2D menuLight_Texture;
        private Texture2D instruction_Texture;

        //Buttons
        private List<Button> buttons = new List<Button>();

        private Random rng = new Random();
        //Properties
        public GameState CurrentState { get => currentState; } 

        //Constructors
        public GameStateManager(Texture2D menuNoLight_Texture, Texture2D menuLight_Texture,
            Texture2D instruction_Texture, Texture2D buttonInstruction_Texture,
            Texture2D buttonInstructionHover_Texture, Texture2D buttonOptions_Texture,
            Texture2D buttonOptionsHover_Texture, Texture2D buttonPlay_Texture, Texture2D buttonPlayHover_Texture)
        {
            //Set intro timer
            introTimer = 2f;
            menuLightTimer = 2f;

            isMenuLighted = false;

            //Load Texture2d For menu and buttons
            this.menuLight_Texture = menuLight_Texture;
            this.menuNoLight_Texture = menuNoLight_Texture;
            this.instruction_Texture = instruction_Texture;

            //Initialize buttons
            buttons.Add(new Button(buttonPlay_Texture, buttonPlayHover_Texture, 1500, 700));
            buttons.Add(new Button(buttonOptions_Texture, buttonOptionsHover_Texture, 1500, 800));
            buttons.Add(new Button(buttonInstruction_Texture, buttonInstructionHover_Texture, 1500, 900));
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
                    batch.Draw(instruction_Texture,
                        new Rectangle(0, 0, instruction_Texture.Width, instruction_Texture.Height),
                        Color.White);
                    break;

                case GameState.MenuState:
                    //Switch between no light menu and lighted menu background
                    if(isMenuLighted)
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
                    for(int i = 0; i < buttons.Count; i++)
                    {
                        buttons[i].Draw(batch);
                    }


                    break;

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
        /// Handles all time-based functionality for game states
        /// (State switching, motion/animation/state switch detection)
        /// </summary>
        /// <param name="dTime">Time passed (seconds)</param>
        public void Update(float dTime)
        {
            //Current state actions
            switch (currentState)
            {
                case GameState.IntroState:
                    introTimer -= dTime;
                    if(introTimer <= 0)
                    {
                        currentState = GameState.MenuState;
                    }
                    break;

                case GameState.MenuState:
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
                    break;

                case GameState.InstructionState:
                    break;

                case GameState.PlayState:
                    //map.Update(dTime);
                    break;
                case GameState.PauseState:

                    break;
                case GameState.GameOverState:

                    break;
            }
        }
    }
}
