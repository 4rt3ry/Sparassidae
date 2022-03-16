/*
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
    class Enemy : GameObject
    {
        //Fields
        private EnemyState currentState;
        private float chaseWindupTimer;

        //Roam Variables
        private Vector2[] roamLocations;
        private int targetRoam; //Int represent position in roam array that enemy is targeting
        private int roamCheckDistance; //Distance to mark a checkpoint as 'checked'
        private Boolean moving; //Is enemy currently moving?
        private float moveTime; //Time enemy will be moving towards roam point
        private float downTime; //Time enemy will wait before moving again

        //Distance at which a player starts a chase
        private float chaseStartDistance;

        //Reference holder for target player
        private Player target;

        //Properties
        public EnemyState CurrentState { get => currentState;  }

        //Constructors
        /// <summary>
        /// Default constructor, DO NOT USE THIS ONE
        /// </summary>
        public Enemy()
        {
            //Starting state
            currentState = EnemyState.RoamingState;
        }

        /// <summary>
        /// Constructor for unmoving enemy, stands at starting position
        /// </summary>
        /// <param name="position">Starting/standing position</param>
        public Enemy(Vector2 position)
        {
            currentState = EnemyState.RoamingState;
            this.position = position;
            this.roamLocations = null;
        }

        /// <summary>
        /// Constructor that takes a starting position and an array of locations to roam to
        /// </summary>
        /// <param name="position">Starting position</param>
        /// <param name="roamLocations">Array of locations for the enemy to roam to</param>
        public Enemy(Vector2 position, Vector2[] roamLocations)
        {
            this.position = position;
            this.roamLocations = roamLocations;
        }

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
                    //No locations (Stand still)
                    if(roamLocations != null)
                    {
                        
                    }
                    //One location (Roam about a single point)
                    if (roamLocations.Length == 1)
                    {

                    }
                    //Multiple locations (Roam between locations)
                    if(roamLocations.Length > 1)
                    {

                    }
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
