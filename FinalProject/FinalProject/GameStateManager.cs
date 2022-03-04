/*
 * Game State Manager class
 * Handles game states and transitions
 */

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject
{
    enum GameState
    {
        IntroState,
        MenuState,
        PlayState,
        PauseState,
        GameOverState
    }
    class GameStateManager
    {
        //Fields
        private GameState currentState;
        private float introTimer;

        //Properties
        public GameState CurrentState { get => currentState; } 

        //Constructors
        public GameStateManager()
        {
            //Set intro timer
            introTimer = 2f;
        }

        //Methods
        public void Display(SpriteBatch batch)
        {
            //Current state actions
            switch (currentState)
            {
                case GameState.IntroState:

                    break;
                case GameState.MenuState:
                    //Runi's Menu Display Here

                    break;
                case GameState.PlayState:

                    break;
                case GameState.PauseState:

                    break;
                case GameState.GameOverState:

                    break;
            }
        }
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
                    //Runi's Menu Work here

                    break;
                case GameState.PlayState:

                    break;
                case GameState.PauseState:

                    break;
                case GameState.GameOverState:

                    break;
            }
        }
    }
}
