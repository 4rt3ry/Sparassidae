/*
 * Game State Manager class
 * Handles game states and transitions
 */

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

        }

        //Methods
        public void Update(float dTime)
        {

        }
    }
}
