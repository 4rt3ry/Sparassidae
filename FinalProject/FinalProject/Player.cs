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
using System.Diagnostics;

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
        private Camera2D camera;

        //Light
        private Spotlight flashlight;
        private float targetScale;

        // Input
        private KeyboardState kb;
        private MouseState currentMouse;
        private MouseState previousMouse;

        //Sound Variables
        float sighTimer;
        float alertTimer;

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
        public Player() : base()
        {
            //Initialize variables
            currentState = PlayerState.WalkingState;

            //Creaet the spotlight
            //Now we only use default spotlight, we might add customed one in the future
            Flashlight = new Spotlight
            {
                Position = this.Position,
                Scale = new Vector2(800), //Range of the light source
                ShadowType = ShadowType.Solid,
                Color = new Color(0.45f, 0.45f, 0.44f),
                ConeDecay = 2.0f
            };
            targetScale = 650;

            _physicsCollider = new CircleCollider(this, new Vector2(0, 0), 20f, false);

            sighTimer = 3.1f;
            alertTimer = 2f;
        }

        public Player(Vector2 position, Camera2D camera): this()
        {
            Position = position;
            this.camera = camera;
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
            currentMouse = Mouse.GetState();
            Vector2 mousePos = new Vector2(currentMouse.Position.X, currentMouse.Position.Y);
            mousePos = camera.ScreenToWorldSpace(mousePos);
            //Rotate the flashlight direction based on the mouse position
            flashlight.Rotation = MathF.Atan2(mousePos.Y -  this.Position.Y, mousePos.X - this.Position.X);

            if(flashlight.Scale.X > targetScale)
            {
                flashlight.Scale = new Vector2(flashlight.Scale.X - (10 + Math.Abs((targetScale-flashlight.Scale.X)/2)) * dTime);
            }
            if(flashlight.Scale.X < targetScale)
            {
                flashlight.Scale = new Vector2(flashlight.Scale.X + (10 + Math.Abs((targetScale - flashlight.Scale.X) / 2)) * dTime);
            }

            switch (CurrentState)
            {
                case PlayerState.WalkingState:
                    sighTimer -= dTime;
                    if(sighTimer <= 0 && sighTimer > -10)
                    {
                        //SFXManager.LoopInstancedSound(Sounds.BrNormal, false);
                        sighTimer = -10;
                    }
                    break;
                case PlayerState.AfraidState:

                    break;
                case PlayerState.ShockState:
                    if (shockTimer <= 0)
                    {
                        SetChaseState();
                    }
                    shockTimer -= dTime;
                    break;
                case PlayerState.ChaseState:
                    if(alertTimer <= 0 && alertTimer > -5)
                    {
                        SFXManager.PlaySound(Sounds.Alert);
                        SFXManager.LoopInstancedSound(Sounds.SAmbChase, false);
                        alertTimer = 5;
                    }
                    alertTimer -= dTime;
                    break;
                case PlayerState.DeadState:

                    break;
            }
        }

        public void Move(float dt)
        {
            float speed = 3;
            kb = Keyboard.GetState();

            // Change player's speed based on their state
            speed = currentState switch
            {
                PlayerState.WalkingState => speed,
                PlayerState.AfraidState => speed / 2f,
                PlayerState.ShockState => 1,
                PlayerState.ChaseState => speed * 1.5f,
                PlayerState.DeadState => 0,
                _ => 0
            };

            // Get keyboard inputs
            bool aDown = kb.IsKeyDown(Keys.A);
            bool dDown = kb.IsKeyDown(Keys.D);
            bool wDown = kb.IsKeyDown(Keys.W);
            bool sDown = kb.IsKeyDown(Keys.S);


            Vector2 addVelocity = Vector2.Zero;
            if (aDown) { addVelocity += -Vector2.UnitX; }
            if (dDown) { addVelocity += Vector2.UnitX; }
            if (wDown) { addVelocity += -Vector2.UnitY; }
            if (sDown) { addVelocity += Vector2.UnitY; }

            // Ensures speed remains the same when moving diagonally
            if (addVelocity.LengthSquared() > 0)
            {
                addVelocity.Normalize();
                addVelocity *= speed;
            }

            // Adds acceleration/smoothing by lerping
            Velocity = Vector2.Lerp(Velocity, addVelocity, 0.1f);

            Position += Velocity;
            //Debug.WriteLine(playerObject.Position);
            //Debug.WriteLine("add velocity is " + addVelocity);
        }

        /// <summary>
        /// Throw a stone when the left mouse button is pressed
        /// </summary>
        /// <param name="stones"></param>
        /// <param name="penumbra"></param>
        public void ThrowStone(List<Stone> stones, PenumbraComponent penumbra)
        {
            currentMouse = Mouse.GetState();

            // Throw a stone when left mouse button is pressed
            if (currentMouse.LeftButton == ButtonState.Pressed &&
                previousMouse.LeftButton == ButtonState.Released)
            {
                Stone stone = new Stone(_position);
                Vector2 throwDirection = new Vector2(MathF.Cos(flashlight.Rotation), MathF.Sin(flashlight.Rotation));
                Debug.WriteLine("Throw: " + throwDirection);
                stone.Throw(throwDirection);
                stones.Add(stone);
                penumbra.Lights.Add(stone.Light);
                //Debug testing code, changes state when throwing a stone
                /*
                switch (currentState)
                {
                    case PlayerState.WalkingState:
                        SetAfraidState();
                        break;
                    case PlayerState.AfraidState:
                        SetShockState();
                        break;
                    case PlayerState.ShockState:
                        break;
                    case PlayerState.ChaseState:
                        SetWalkingState();
                        break;
                    case PlayerState.DeadState:
                        break;
                }
                */
            }
        }

        /// <summary>
        /// Throw stone method that takes a texture and throws a light of that texture
        /// </summary>
        /// <param name="stones"></param>
        /// <param name="penumbra"></param>
        /// <param name="texture"></param>
        public void ThrowStone(List<Stone> stones, PenumbraComponent penumbra, Texture2D texture, Map map)
        {
             currentMouse = Mouse.GetState();

             if(map.TotalStoneNumber >= 1)
             {
                // Throw a stone when left mouse button is pressed
                if (currentMouse.LeftButton == ButtonState.Pressed &&
                    previousMouse.LeftButton == ButtonState.Released)
                {
                    Stone stone = new Stone(_position, texture);
                    Vector2 throwDirection = new Vector2(MathF.Cos(flashlight.Rotation), MathF.Sin(flashlight.Rotation));
                    Debug.WriteLine("Throw: " + throwDirection);
                    stone.Throw(throwDirection);
                    stones.Add(stone);
                    penumbra.Lights.Add(stone.Light);
                    map.TotalStoneNumber--;
                }
             }
             previousMouse = currentMouse;
        }

        /// <summary>
        /// Set player into afraid state
        /// </summary>
        public void SetAfraidState()
        {
            currentState = PlayerState.AfraidState;
            SFXManager.StopAllHB();
            SFXManager.StopAllBr();
            SFXManager.LoopInstancedSound(Sounds.HBRushed, false);
            SFXManager.LoopInstancedSound(Sounds.SAmbience, false);
            SFXManager.LoopInstancedSound(Sounds.BrMedium, false);
            numTargets += 1;
            targetScale = 400;
        }

        /// <summary>
        /// Put player into shock state
        /// </summary>
        public void SetShockState()
        {
            currentState = PlayerState.ShockState;
            SFXManager.PlaySound(Sounds.Catch);
            SFXManager.StopAllHB();
            SFXManager.StopAllBr();
            shockTimer = 4.5f;
            targetScale = 300;
        }

        /// <summary>
        /// Called when an enemy loses a chase with the player, decreases number of targets attracted to player
        /// If no enemies are targeting the player, it reverts to walking state
        /// </summary>
        public void DeAgro()
        {
            numTargets -= 1;
            if (numTargets <= 0)
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
            SFXManager.StopAllHB();
            SFXManager.StopAllBr(); 
            SFXManager.LoopInstancedSound(Sounds.HBNormal, false);
            SFXManager.PlaySound(Sounds.BrSigh);
            sighTimer = 3.1f;
            targetScale = 650;
        }

        /// <summary>
        /// Sets the player into the chase state
        /// </summary>
        public void SetChaseState()
        {
            currentState = PlayerState.ChaseState;
            SFXManager.StopAllHB();
            SFXManager.StopAllBr();
            SFXManager.LoopInstancedSound(Sounds.HBFrantic, false);
            SFXManager.LoopInstancedSound(Sounds.BrHeavy, false);
            alertTimer = 2f;
            targetScale = 150;
            flashlight.Scale = new Vector2(150);
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
