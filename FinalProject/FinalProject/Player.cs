/*
 * Player class
 * Contains all code relevant to player
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Penumbra;

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
    class Player : GameObject
    {
        //Fields
        //State management variables
        private PlayerState currentState;
        private float shockTimer;
        private int numTargets;

        //Speed Variables
        private float currentSpeed;
        private float walkingSpeed;
        private float afraidSpeed;
        private float shockSpeed;
        private float chaseSpeed;

        //Light
        private Spotlight flashlight;

        //MouseState
        private MouseState ms;

        //Properties
        public PlayerState CurrentState { get => currentState; set => currentState = value; }
  
        public Spotlight Flashlight { get => flashlight; set => flashlight = value; }

        // Set position should also update the position of flashlight
        public new Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                flashlight.Position = value;
            }
        }

        //Constructors
        public Player(): base()
        {
            //Set standards for different speeds
            walkingSpeed = 0;
            afraidSpeed = 0;
            shockSpeed = 0;
            chaseSpeed = 0;

            //Initialize variables
            currentSpeed = walkingSpeed;
            currentState = PlayerState.WalkingState;

            //Creaet the spotlight
            //Now we only use default spotlight, we might add customed one in the future
            Flashlight = new Spotlight
            {
                Position = this.Position,
                Scale = new Vector2(800), //Range of the light source
                ShadowType = ShadowType.Solid,
                ConeDecay = 2.0f
            };

            _physicsCollider = new CircleCollider(this, new Vector2(0, 0), 20f, false);
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
            //This will make more sense if player's position can be set in the property
            //flashlight.Position = this.Position;
            ms = Mouse.GetState();
            //Rotate the flashlight direction based on the mouse position
            flashlight.Rotation = MathF.Atan2(ms.Y -  this.Position.Y, ms.X - this.Position.X);

            switch (CurrentState)
            {
                case PlayerState.WalkingState:

                    break;
                case PlayerState.AfraidState:
                    
                    break;
                case PlayerState.ShockState:
                    if (shockTimer <= 0)
                    {
                        currentState = PlayerState.ChaseState;
                        currentSpeed = chaseSpeed;
                    }
                    shockTimer -= dTime;
                    break;
                case PlayerState.ChaseState:

                    break;
                case PlayerState.DeadState:

                    break;
            }
        }

        /// <summary>
        /// Set player into afraid state
        /// </summary>
        public void SetAfraidState()
        {
            currentState = PlayerState.AfraidState;
            currentSpeed = afraidSpeed;
            numTargets += 1;
        }

        /// <summary>
        /// Put player into shock state
        /// </summary>
        public void SetShockState()
        {
            currentState = PlayerState.ShockState;
            currentSpeed = shockSpeed;
            shockTimer = 2f;
        }

        /// <summary>
        /// Called when an enemy loses a chase with the player, decreases number of targets attracted to player
        /// If no enemies are targeting the player, it reverts to walking state
        /// </summary>
        public void DeAgro()
        {
            numTargets -= 1;
            if(numTargets <= 0)
            {
                SetWalkingState();
            }
        }

        /// <summary>
        /// Sets the player state to the walking state
        /// </summary>
        public void SetWalkingState()
        {
            currentState = PlayerState.WalkingState;
            currentSpeed = walkingSpeed;
        }

        /// <summary>
        /// Ran when player is caught, sets them to dead
        /// </summary>
        public void SetDeadState()
        {
            currentState = PlayerState.DeadState;
        }
    }
}
