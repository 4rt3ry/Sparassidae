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
using System.Linq;

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
        private List<Vector2> roamLocations; // This should include spawn position
        private int roamTarget; //Int represent position in roam array that enemy is targeting
        private int roamCheckDistance; //Distance to mark a checkpoint as 'checked'
        private Boolean moving; //Is enemy currently moving?
        private float moveTime; //Time enemy will be moving towards roam point
        private float downTime; //Time enemy will wait before moving again
        private float speed;
        private Random rng = new Random();
        private int isForward; // Let a bool, 0 = move forward, 1 = move backward

        //Detection variables
        private float detectionRadius;
        private float chaseStartDistance;
        private LineCollider playerDetectionLink;
        private CircleCollider roamDetectionTrigger;
        private Vector2 lastSeenPosition;
        private List<Wall> walls;
        private bool isDetected;

        //Reference holder for target player
        private Player target;
        private Vector2 moveingTowards;

        //Visual Variables
        private Texture2D enemyTexture;
        private Rectangle displayRectangle;

        //Properties
        public EnemyState CurrentState { get => currentState;  }
        public Rectangle DisplayRectangle { get => displayRectangle; }
        public float DetectionRadius { get => detectionRadius; set => detectionRadius = value; }
        internal CircleCollider RoamDetectionTrigger { get => roamDetectionTrigger; set => roamDetectionTrigger = value; }

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
            this._position = position;
            this.roamLocations = null;
            this.enemyTexture = null;
        }

        /// <summary>
        /// Enemy constructor for simple stationary enemy
        /// </summary>
        /// <param name="position">Standing position of enemy</param>
        /// <param name="enemyTexture">Enemy visual texture</param>
        /// 
        public Enemy(Vector2 position, Texture2D enemyTexture, int width, int height) : this(position)
        {
            this.enemyTexture = enemyTexture;
            displayRectangle = new Rectangle(new Point((int)position.X, (int)position.Y), new Point(width, height));
        }

        /// <summary>
        /// Constructor that takes a starting position and an array of locations to roam to
        /// </summary>
        /// <param name="position">Starting position</param>
        /// <param name="roamLocations">Array of locations for the enemy to roam to</param>
        public Enemy(Vector2 position, List<Vector2> roamLocations) : this(position)
        {
            this.roamLocations = roamLocations;
        }

        /// <summary>
        /// Constructor for roaming enemy with unique detection radius
        /// </summary>
        /// <param name="position">Enemy starting position</param>
        /// <param name="roamLocations">Array of positions to roam to</param>
        /// <param name="detectionRadius">Radius for detecting player</param>
        public Enemy(Vector2 position, List<Vector2> roamLocations, float detectionRadius) : this(position, roamLocations)
        {
            this.DetectionRadius = detectionRadius;
        }

        /// <summary>
        /// Enemy constructor with unique detection radius and texture
        /// </summary>
        /// <param name="position">Enemy starting position</param>
        /// <param name="roamLocations">Array of positions to roam to</param>
        /// <param name="detectionRadius">Radius for detecting player</param>
        /// <param name="enemyTexture">Visual texture</param>
        public Enemy(Vector2 position, List<Vector2> roamLocations, float detectionRadius, Texture2D enemyTexture, int width, int height, float movingSpeed,
            Player target, List<Wall> walls) 
            : this(position, roamLocations, detectionRadius)
        {
            this.enemyTexture = enemyTexture;
            this._physicsCollider = new RectangleCollider(this, Vector2.Zero, new Vector2(width, height));
            displayRectangle = new Rectangle(new Point((int)position.X, (int)position.Y), new Point(width, height));
            moving = true;
            moveTime = 5;
            roamTarget = 1;
            roamCheckDistance = 10;
            this.speed = movingSpeed;
            isForward = 0; // Move forward
            this.target = target;
            this.walls = walls;
            RoamDetectionTrigger = new CircleCollider(this, new Vector2(width / 2, height / 2), detectionRadius, true);
            playerDetectionLink = new LineCollider(this, Vector2.Zero, target.Position);
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
                    //Check if the player is enter the detection range
                    //Still needs to add stone detection
                    if (RoamDetectionTrigger.CheckCollision(target))
                    {
                        isDetected = true;
                        foreach (Wall wall in walls)
                        {
                            if (playerDetectionLink.CheckCollision(wall))
                            {
                                isDetected = false;
                            }
                        }
                        if(isDetected)
                        {
                            System.Diagnostics.Debug.WriteLine("Dececting");
                            moveingTowards = target.Position;
                            currentState = EnemyState.InvestigateState;
                        }
                    }
                   

                    //No locations (Stand still)
                    if(roamLocations == null)
                    {
                        
                    }
                    else
                    {
                        //One location (Roam about a single point)
                        if (roamLocations.Count == 1)
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
                        if (roamLocations.Count > 1)
                        {
                            //Check distance to target pos
                            if (Math.Abs((_position - roamLocations.ElementAt(roamTarget)).Length()) <= roamCheckDistance)
                            {
                                isForward = rng.Next(0, 2);
                                //Update target location
                                if (roamTarget == 0)
                                {
                                    roamTarget += 1;
                                }
                                else if(roamTarget == roamLocations.Count -1)
                                {
                                    roamTarget -= 1;
                                }
                                else if(isForward == 0)
                                {
                                    roamTarget += 1;
                                }
                                else
                                {
                                    roamTarget -= 1;
                                }
                            }

                            //Movement code
                            if (moving)
                            {
                                //Move enemy
                                Vector2 moveDir = roamLocations[roamTarget] - this._position;
                                moveDir.Normalize();
                                this._position += moveDir * speed * dTime;
                               

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
                                    moveTime = rng.Next(1,6);
                                    moving = true;
                                }
                            }
                        }
                    }
                    
                    break;
                case EnemyState.InvestigateState:

                    //Movement code
                    Vector2 moveDir2 = moveingTowards - this._position;
                    moveDir2.Normalize();
                    this._position += moveDir2 * speed * dTime;

                    //Detection code
                    ///Run something here that updates the Enemy/Player link (needs additional help)

                    // If player is within chase start distance
                    // THIS IS TEMP CODE, WE CAN REPLACE WITH RAYTRACING/SENSOR COLLISIONS
                    if (Math.Abs((_position - target.Position).Length()) <= chaseStartDistance)
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
            displayRectangle.X = (int)_position.X;
            displayRectangle.Y = (int)_position.Y;

            //If the enemy is in a state where there is a target, the detection code will run
            //This updates the enemy/player link as well as the last seen position if the player is in vision
            if(target != null)
            {
                playerDetectionLink.EndPosition = target.Position;
                playerDetectionLink.Position = _position;
                //SOME SORT OF DETECTION OF LINES COLLISION WITH MAP OBJECTS
                //playerDetectionLink.CheckCollision();

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
            this.playerDetectionLink = new LineCollider(_position, p.Position);
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

        public void StoneDetection(Stone stone)
        {

        }

        public void WallDetection(Stone stone)
        {

        }
    }
}
