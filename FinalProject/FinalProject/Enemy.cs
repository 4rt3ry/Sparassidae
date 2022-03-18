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
        private int roamTarget; //Int represent position in roam array that enemy is targeting
        private int roamCheckDistance; //Distance to mark a checkpoint as 'checked'
        private Boolean moving; //Is enemy currently moving?
        private float moveTime; //Time enemy will be moving towards roam point
        private float downTime; //Time enemy will wait before moving again

        //Detection variables
        private float detectionRadius;
        private float chaseStartDistance;
        private LineCollider playerDetectionLink;
        private Vector2 lastSeenPosition;

        //Reference holder for target player
        private Player target;

        //Visual Variables
        private Texture2D enemyTexture;
        private Rectangle displayRectangle;

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
            displayRectangle = default(Rectangle);
        }

        /// <summary>
        /// Constructor for unmoving enemy, stands at starting position
        /// </summary>
        /// <param name="position">Starting/standing position</param>
        public Enemy(Vector2 position) : this()
        {
            this.position = position;
            this.roamLocations = null;
            this.enemyTexture = null;
        }

        /// <summary>
        /// Enemy constructor for simple stationary enemy
        /// </summary>
        /// <param name="position">Standing position of enemy</param>
        /// <param name="enemyTexture">Enemy visual texture</param>
        public Enemy(Vector2 position, Texture2D enemyTexture) : this(position)
        {
            this.enemyTexture = enemyTexture;
            displayRectangle = new Rectangle(new Point((int)position.X, (int)position.Y), new Point(enemyTexture.Width, enemyTexture.Height));
        }

        /// <summary>
        /// Constructor that takes a starting position and an array of locations to roam to
        /// </summary>
        /// <param name="position">Starting position</param>
        /// <param name="roamLocations">Array of locations for the enemy to roam to</param>
        public Enemy(Vector2 position, Vector2[] roamLocations) : this(position)
        {
            this.roamLocations = roamLocations;
        }

        /// <summary>
        /// Constructor for roaming enemy with unique detection radius
        /// </summary>
        /// <param name="position">Enemy starting position</param>
        /// <param name="roamLocations">Array of positions to roam to</param>
        /// <param name="detectionRadius">Radius for detecting player</param>
        public Enemy(Vector2 position, Vector2[] roamLocations, float detectionRadius) : this(position, roamLocations)
        {
            this.detectionRadius = detectionRadius;
        }

        /// <summary>
        /// Enemy constructor with unique detection radius and texture
        /// </summary>
        /// <param name="position">Enemy starting position</param>
        /// <param name="roamLocations">Array of positions to roam to</param>
        /// <param name="detectionRadius">Radius for detecting player</param>
        /// <param name="enemyTexture">Visual texture</param>
        public Enemy(Vector2 position, Vector2[] roamLocations, float detectionRadius, Texture2D enemyTexture) : this(position, roamLocations, detectionRadius)
        {
            this.enemyTexture = enemyTexture;
            displayRectangle = new Rectangle(new Point((int)position.X, (int)position.Y), new Point(enemyTexture.Width, enemyTexture.Height));
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
            //Constant display of enemy texture (For current version, not including animations)
            batch.Draw(enemyTexture, displayRectangle, Color.White);
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
                        //Movement code
                        if (moving)
                        {
                            //Move enemy

                            //Time Increment
                            moveTime -= dTime;
                            if (moveTime <= 0)
                            {
                                downTime = 1f;
                                moving = false;
                            }
                        }
                        else
                        {
                            //Time Increment
                            downTime -= dTime;
                            if (downTime <= 0)
                            {
                                moveTime = 1.5f;
                                moving = true;
                            }
                        }
                    }
                    //Multiple locations (Roam between locations)
                    if(roamLocations.Length > 1)
                    {
                        //Check distance to target pos
                        if (Math.Abs((position - roamLocations[roamTarget]).Length()) <= roamCheckDistance)
                        {
                            //Increment and ensure target location is within array
                            roamTarget += 1;
                            roamTarget = roamTarget % (roamLocations.Length - 1);
                        }

                        //Movement code
                        if (moving)
                        {
                            //Move enemy

                            //Time Increment
                            moveTime -= dTime;
                            if (moveTime <= 0)
                            {
                                downTime = 1f;
                                moving = false;
                            }
                        }
                        else
                        {
                            //Time Increment
                            downTime -= dTime;
                            if(downTime <= 0)
                            {
                                moveTime = 1.5f;
                                moving = true;
                            }
                        }
                    }
                    break;
                case EnemyState.InvestigateState:


                    //Movement code

                    //Detection code
                    ///Run something here that updates the Enemy/Player link (needs additional help)
                    
                    // If player is within chase start distance
                    // THIS IS TEMP CODE, WE CAN REPLACE WITH RAYTRACING/SENSOR COLLISIONS
                    if (Math.Abs((position - target.Position).Length()) <= chaseStartDistance)
                    {
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

            //This code always runs regardless of state

            //Update display rectangle based on position
            displayRectangle.X = (int)position.X;
            displayRectangle.Y = (int)position.Y;

            //If the enemy is in a state where there is a target, the detection code will run
            //This updates the enemy/player link as well as the last seen position if the player is in vision
            if(target != null)
            {
                playerDetectionLink.EndPosition = target.Position;
                playerDetectionLink.Position = position;
                //SOME SORT OF DETECTION OF LINES COLLISION WITH MAP OBJECTS

                //if no collision with map objects
                if (true)
                {
                    lastSeenPosition = playerDetectionLink.EndPosition;
                }
            }
        }

        /// <summary>
        /// Triggered on player collision with farthest detector
        /// Gives the enemy a reference to the player
        /// </summary>
        /// <param name="p">Player p that is encountered</param>
        public void StartPlayerEncounter(Player p)
        {
            this.target = p;
            currentState = EnemyState.InvestigateState;
            p.SetAfraidState();
            this.playerDetectionLink = new LineCollider(position, p.Position);
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

        /// <summary>
        /// Ran on player physics body collision with enemy
        /// Causes player to die
        /// (Runs players 'get caught' function)
        /// </summary>
        public void CatchPlayer()
        {
            currentState = EnemyState.PlayerDeadState;
            target.SetDeadState();
        }
    }
}
