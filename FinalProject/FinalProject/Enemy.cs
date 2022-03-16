/*
 * Enemy class
 * Contains all code relevant to enemies
 * including Enemy State Machine
 */

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

        //Reference holder for target player
        private Player target;

        //Properties
        public EnemyState CurrentState { get => currentState;  }

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
