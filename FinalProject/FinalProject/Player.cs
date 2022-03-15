/*
 * Player class
 * Contains all code relevant to player
 */

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FinalProject
{
    enum PlayerState
    {
        WalkingState,
        AfraidState,
        ShockState,
        ChaseState,
        DeadState
    }
    class Player
    {
        //Fields
        PlayerState currentState;

        //Properties
        Vector2 playerPosition;

        Vector2 velocity = new Vector2(0, 0);
        int x;
        int y;
        public int X
        {
            get => x;
            set
            {
                playerPosition.X = value;
                x = value;
            }
        }

        public int Y
        {
            get => y;
            set
            {
                playerPosition.Y = value;
                y = value;
            }
        }

        public Vector2 PlayerPosition { get => playerPosition; set => playerPosition = value; }
        public PlayerState CurrentState { get => currentState; set => currentState = value; }
        public Vector2 Velocity { get => velocity; set => velocity = value; }

        //Constructors

        public Player()
        {
            playerPosition = new Vector2(0,0);
        }


        //Methods
        /// <summary>
        /// Handles all visual displays for the player
        /// based on their current state (mostly flashlight)
        /// </summary>
        /// <param name="batch">Sprite batch</param>
        public void Display(SpriteBatch batch)
        {
            switch (CurrentState)
            {
                case PlayerState.WalkingState:

                    break;
                case PlayerState.AfraidState:

                    break;
                case PlayerState.ShockState:

                    break;
                case PlayerState.ChaseState:

                    break;
                case PlayerState.DeadState:

                    break;
            }
        }

        /// <summary>
        /// Handles all time-based functions for player
        /// (movement, state change detection)
        /// </summary>
        /// <param name="dTime">Time passed (in seconds)</param>
        public void Update(float dTime)
        {
            switch (CurrentState)
            {
                case PlayerState.WalkingState:

                    break;
                case PlayerState.AfraidState:

                    break;
                case PlayerState.ShockState:

                    break;
                case PlayerState.ChaseState:

                    break;
                case PlayerState.DeadState:

                    break;
            }
        }
    }
}
