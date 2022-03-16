﻿/*
 * Enemy class
 * Contains all code relevant to enemies
 * including Enemy State Machine
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject
{
    enum EnemyState
    {
        RoamingState,
        InvestigateState,
        ChaseWindupState,
        ChaseState,
        PlayerDeadState,
        ReturnState
    }
    class Enemy
    {
        //Fields
        private EnemyState currentState;
        private Vector2 position;
        private float chaseWindupTimer;

        //Distance at which a player starts a chase
        private float chaseStartDistance;

        //Reference holder for target player
        private Player target;

        //Properties
        public EnemyState CurrentState { get => currentState;  }
        public Vector2 Position { get => position; set => position = value; }

        //Constructors


        //Methods
        /// <summary>
        /// Handles all visual displays for enemy
        /// </summary>
        /// <param name="batch">Sprite batch</param>
        public void Display(SpriteBatch batch)
        {
            switch (currentState)
            {
                case EnemyState.RoamingState:

                    break;
                case EnemyState.InvestigateState:

                    break;
                case EnemyState.ChaseWindupState:

                    break;
                case EnemyState.ChaseState:

                    break;
                case EnemyState.PlayerDeadState:

                    break;
                case EnemyState.ReturnState:

                    break;
            }
        }

        /// <summary>
        /// Handles all time-based functions for enemy
        /// </summary>
        /// <param name="dTime">Time passed (Seconds)</param>
        public void Update(float dTime)
        {
            switch (currentState)
            {
                case EnemyState.RoamingState:

                    break;
                case EnemyState.InvestigateState:
                    // If player is within chase start distance
                    // THIS IS TEMP CODE, WE CAN REPLACE WITH RAYTRACING/SENSOR COLLISIONS
                    if (Math.Abs((position - target.Position).Length()) <= chaseStartDistance) {
                        currentState = EnemyState.ChaseWindupState;
                        chaseWindupTimer = 4f;
                        target.SetShockState();
                    }
                    break;
                case EnemyState.ChaseWindupState:
                    chaseWindupTimer -= dTime;
                    if (chaseWindupTimer <= 0)
                    {
                        currentState = EnemyState.ChaseState;
                    }
                    break;
                case EnemyState.ChaseState:
                    // IDEA FOR CHASE STATE: Constant line between enemy and player
                    // Enemy is constantly pathing to a position
                    // This position updates to the player position every frame
                    // IFF the line does not collide with a wall
                    // If the enemy reaches its target position && player line is colliding with a wall
                    // Then the chase is broken
                    break;
                case EnemyState.PlayerDeadState:
                    // Check collision of bodies for detection
                    break;
                case EnemyState.ReturnState:
                    // Path back to start
                    break;
            }
        }

        /// <summary>
        /// Ran when player collides with farthest detector
        /// Gives the enemy a reference to the player
        /// </summary>
        /// <param name="p">Player p that is encountered</param>
        public void StartPlayerEncounter(Player p)
        {
            this.target = p;
            currentState = EnemyState.InvestigateState;
            p.SetAfraidState();
        }

        /// <summary>
        /// Ends the encounter with the player
        /// </summary>
        public void EndPlayerEncounter()
        {
            currentState = EnemyState.RoamingState;
            target.DeAgro();
            target = null;
        }
    }
}
