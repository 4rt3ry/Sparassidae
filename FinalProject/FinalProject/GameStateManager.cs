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

        }

        //Methods
        public void Display(SpriteBatch batch)
        {

        }
        public void Update(float dTime)
        {

        }
    }
}
