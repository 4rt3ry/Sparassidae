/*
 * Enemy class
 * Contains all code relevant to enemies
 * including Enemy State Machine
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
        ReturnState,
        EndGameChaseState
    }
    class Enemy : GameObject
    {
        // Fields
        private Vector2 startingPosition;
        private EnemyState currentState;
        private float chaseWindupTimer;
        private Random rng = new Random();

        //  Roam Variables
        private List<Vector2> roamLocations; // This should include spawn position
        private int roamTarget; //Int represent position in roam array that enemy is targeting
        private int roamCheckDistance; //Distance to mark a checkpoint as 'checked'
        private Boolean moving; //Is enemy currently moving?
        private float moveTime; //Time enemy will be moving towards roam point
        private float downTime; //Time enemy will wait before moving again
        private float speed;
        private float baseSpeed; //Base speed on the enemy
        private int isForward; // Let a bool, 0 = move forward, 1 = move backward

        //  Detection Variables
        private float detectionRadius; 
        private float chaseStartDistance;
        private LineCollider detectionLink; // This link is to check if there is wall between enemy to player/stones
        private CircleCollider roamDetectionTrigger;
        private Vector2 lastSeenPosition; // NOT SURE IF WE REALLY NEED IT !!!!!!!!!
        
        //  Referenc holder
        private Map map; // This is the map that current enemy is in; it is used to get access to stone
        private Player target;
        private Vector2 movingTowards;
        private Vector2 moveDir; // Normalized 

        //  Visual Variables
        private Texture2D enemyTexture;
        private Rectangle displayRectangle;

        // Animated Visual variables
        private Texture2D enemyAnimatedTexturesheet;
        private int numSpritesInSheet;
        private int widthOfSingleSprite;
        private int currentFrame;
        private double fps;
        private double secondsPerFrame;
        private double timeCounter;
        private bool isAnimated;

        // Rotation variables
        private Vector2 origin;
        private float rotation;
        private Vector2 topLeft;

        // Stone investigating varibles
        private float stoneInvestigateTimer;
        private bool isStoneInvestigation; // Detect stone

        // Return state timer
        private float returnTimer = 3; // After enemy lost player's position, the enemy will stay in the same position for 3 seconds
        private bool isAlerting;

        // Properties
        public EnemyState CurrentState { get => currentState; }
        public Rectangle DisplayRectangle { get => displayRectangle; }
        public float DetectionRadius { get => detectionRadius; set => detectionRadius = value; }
        public CircleCollider RoamDetectionTrigger { get => roamDetectionTrigger; }
        public float ChaseStartDistance { get => chaseStartDistance; set => chaseStartDistance = value; }
        // Constructor

        /// <summary>
        /// 1.Default constructor, DO NOT USE THIS ONE
        /// </summary>
        public Enemy()
        {
            //Starting state
            currentState = EnemyState.RoamingState;
            displayRectangle = default(Rectangle);
        }


        /// <summary>
        /// 2. This enemy is still and not chase player
        /// Constructor for unmoving enemy, stands at starting position
        /// The texture size will be the size for this enemy
        /// </summary>
        /// <param name="map">The map this enmey is in</param>
        /// <param name="position">The standing position</param>
        /// <param name="texture">The unchanged texture of the enmey</param>
        public Enemy(Map map, Vector2 position, Texture2D texture) : this()
        {
            this.map = map;
              
            this.enemyTexture = texture;
            this.origin = new Vector2(texture.Width / 2, texture.Height / 2);
            this.topLeft = this._position - origin;
            this.displayRectangle = new Rectangle((int)topLeft.X, (int)topLeft.Y, texture.Width, texture.Height);
            this.startingPosition = position;
            this._physicsCollider = new RectangleCollider(this, Vector2.Zero, new Vector2(texture.Width, texture.Height), true);

            // Set the still position
            this._position = position;
            this.roamLocations = new List<Vector2>() { position }; // The unmoving enemy's roamLocation will only have one
        }

        /// <summary>
        /// 3. This enemy will roam, but not chase the player
        /// Constructor that takes a starting position and an array of locations to roam to
        /// </summary>
        /// <param name="map">The map this enmey is in</param>
        /// <param name="position">The standing position</param>
        /// <param name="roamLocations">The list of roaming position (the spawn position should also be in this list)</param>
        /// <param name="texture">The unchanged texture of the enmey</param>
        public Enemy(Map map, Vector2 position, List<Vector2> roamLocations, Texture2D texture, float movingSpeed) : this()
        {
            this.map = map;
            this.startingPosition = position;
            if (texture!= null)
            {
                this.enemyTexture = texture;
                this.origin = new Vector2(texture.Width / 2, texture.Height / 2);
                this.topLeft = this._position - origin;
                this.displayRectangle = new Rectangle((int)topLeft.X, (int)topLeft.Y, texture.Width, texture.Height);
                this._physicsCollider = new RectangleCollider(this, Vector2.Zero, new Vector2(texture.Width, texture.Height), true);
            }

            // Set the roaming positions
            this._position = position;
            
            if (roamLocations == null)
            {
                this.roamLocations = new List<Vector2>();
                this.roamLocations.Add(position);
            }
            else if(roamLocations != null && roamLocations.Count == 0)
            {
                this.roamLocations = roamLocations;
                this.roamLocations.Add(position);
            }
            else if (roamLocations != null && roamLocations.Count == 1)
            {
                this.roamLocations = roamLocations;
                this.roamLocations.Add(position);
            }
            else
            {
                this.roamLocations = roamLocations;
            }
            
            
            this.speed = movingSpeed;
            this.baseSpeed = movingSpeed;
            this.isForward = 0; // Move Forward;

            // Defualt set
            moving = true;
            moveTime = 5; 
            roamCheckDistance = 10; // Check if the enemy reach a raoming point
        }

        /// <summary>
        /// 4. Constructor for still enemy but can chase palyer
        /// </summary>
        /// <param name="map">The map this enmey is in</param>
        /// <param name="position">The standing position</param>
        /// <param name="texture">The unchanged texture of the enmey</param>
        /// <param name="detectionRadius">Radius for detecting player/stone</param>
        /// <param name="movingSpeed">The base speed for investigating</param>
        public Enemy(Map map, Vector2 position, Texture2D texture, float detectionRadius, float movingSpeed): 
            this (map, position, texture)
        {
            // Set speed
            this.speed = movingSpeed;
            this.baseSpeed = movingSpeed;

            // Set Target
            this.target = map.Player;

            // Set Detection Range
            this.detectionRadius = detectionRadius;
            this.roamDetectionTrigger = new CircleCollider(this, Vector2.Zero, detectionRadius, true);
            this.detectionLink = new LineCollider(this, Vector2.Zero, target.Position); // Set the link between enemy and player first
            this.chaseStartDistance = detectionRadius - 250; // Default radius
        }

        /// <summary>
        /// 5. Constructor for roaming enemy with texture and can chase player
        /// </summary>
        /// <param name="map">The map this enmey is in</param>
        /// <param name="position">The standing position</param>
        /// <param name="roamLocations">The list of roaming position (the spawn position should also be in this list)</param>
        /// <param name="texture">The unchanged texture of the enmey</param>
        /// <param name="detectionRadius">Radius for detecting player/stone</param>
        /// <param name="movingSpeed">The base speed for roaming or investigating</param>
        public Enemy(Map map, Vector2 position, List<Vector2> roamLocations, Texture2D texture, float detectionRadius, float movingSpeed): 
            this(map, position, roamLocations, texture, movingSpeed)
        {
            this.detectionRadius = detectionRadius;

            // Set Target
            this.target = map.Player;

            // Set Detection Range
            this.detectionRadius = detectionRadius;
            this.roamDetectionTrigger = new CircleCollider(this, Vector2.Zero, detectionRadius, true);
            this.detectionLink = new LineCollider(this, Vector2.Zero, target.Position); // Set the link between enemy and player first
            this.chaseStartDistance = detectionRadius - 250; // Default radius
        }

        /// <summary>
        /// Animated Roaming and Chasing type of enemy
        /// </summary>
        /// <param name="enemyAnimatedTexturesheet"></param>
        /// <param name="map"></param>
        /// <param name="position"></param>
        /// <param name="roamLocations"></param>
        /// <param name="detectionRadius"></param>
        /// <param name="movingSpeed"></param>
        public Enemy(Texture2D enemyAnimatedTexturesheet, Map map, Vector2 position, List<Vector2> roamLocations, float detectionRadius, float movingSpeed) 
            : this(map, position, roamLocations, null, detectionRadius, movingSpeed)
        {
            this.enemyAnimatedTexturesheet = enemyAnimatedTexturesheet;
            EnemyAnimationSetUp();

            this.origin = new Vector2(widthOfSingleSprite / 2, enemyAnimatedTexturesheet.Height / 2);
            this.topLeft = this._position - origin;
            this.displayRectangle = new Rectangle((int)topLeft.X, (int)topLeft.Y, widthOfSingleSprite, enemyAnimatedTexturesheet.Height);
            this._physicsCollider = new RectangleCollider(this, Vector2.Zero, new Vector2(widthOfSingleSprite, enemyAnimatedTexturesheet.Height), true);
        }

        // Methods

        /// <summary>
        /// Handels all visual displays for enemy
        /// Note: I (Runi) moved enemy state machine in display, as currently we only have one animation
        /// </summary>
        /// <param name="batch"></param>
        public void Display(SpriteBatch batch)
        {
            if(isAnimated)
            {
                switch (currentState)
                {
                    case EnemyState.RoamingState:
                        if(roamLocations == null|| roamLocations.Count == 1 || roamLocations.Count == 0)
                        {
                            DrawEnemyStandingAnimation(batch);
                        }
                        else if(roamLocations.Count > 1 && !moving)
                        {
                            DrawEnemyStandingAnimation(batch);
                        }
                        else
                        {
                            DrawEnemyWalkingAnimation(batch);
                        }
                        break;
                    case EnemyState.InvestigateState:
                        if(map.LandedGlowsticks.Count > 5)
                        {
                            if (stoneInvestigateTimer > 0 && stoneInvestigateTimer < 5)
                            {
                                DrawEnemyStandingAnimation(batch);
                            }
                            else
                            {
                                DrawEnemyWalkingAnimation(batch);
                            }
                        }
                        else
                        {
                            if (stoneInvestigateTimer > 0 && stoneInvestigateTimer < map.LandedGlowsticks.Count)
                            {
                                DrawEnemyStandingAnimation(batch);
                            }
                            else
                            {
                                DrawEnemyWalkingAnimation(batch);
                            }
                        }
                        
                        break;
                    case EnemyState.ChaseWindupState:
                        DrawEnemyStandingAnimation(batch);
                        break;
                    case EnemyState.ChaseState:
                        DrawEnemyWalkingAnimation(batch);
                        break;
                    case EnemyState.PlayerDeadState:
                        DrawEnemyStandingAnimation(batch);
                        break;
                    case EnemyState.ReturnState:
                        DrawEnemyStandingAnimation(batch);
                        break;
                    case EnemyState.EndGameChaseState:
                        DrawEnemyWalkingAnimation(batch);
                        break;
                } 
            }
            else
            {
                //Constant display of enemy texture (For current version, not including animations)
                batch.Draw(enemyTexture, displayRectangle, Color.White);
            }
        }

        /// <summary>
        /// Handles all time-based functions for enemy
        /// </summary>
        /// <param name="dTime">Time passed (Seconds)</param>
        public void Update(float dTime)
        {
            //System.Diagnostics.Debug.WriteLine(currentState);
            switch (currentState)
            {
                case EnemyState.RoamingState:
                    // 1. Update the detection info
                    // 1.1 Player detection update
                    if (RoamDetectionTrigger.CheckCollision(target))
                    {
                        detectionLink.EndPosition = target.Position;
                        if (WallDetection()) // IF there is no wall between enemy and player
                        {
                            //System.Diagnostics.Debug.WriteLine("Dececting");
                            target.SetAfraidState();
                            movingTowards = target.Position;
                            System.Diagnostics.Debug.WriteLine($"Start to investigate {movingTowards}");
                            currentState = EnemyState.InvestigateState;
                        }
                    }

                    // 1.2 Stone Detection update
                    // Stone detection function
                    isStoneInvestigation = false;
                    foreach (Glowstick stone in map.LandedGlowsticks)
                    {
                        // Check if the stone has been investigated
                        if (!stone.IsInvestigated)
                        {
                            // Check uninvestigated stone is in the roam detection range
                            if (RoamDetectionTrigger.CheckCollision(stone))
                            {
                                // Set the Detection link
                                detectionLink.EndPosition = stone.Position;
                                // Check the wall between stone and enemy
                                if (WallDetection())
                                {
                                    // Set up moving position
                                    movingTowards = stone.Position;
                                    // set the stone as investigated
                                    stone.IsInvestigated = true;

                                    if (map.LandedGlowsticks.Count < 5)
                                    {
                                        stoneInvestigateTimer = map.LandedGlowsticks.Count;
                                    }
                                    else
                                    {
                                        stoneInvestigateTimer = 5;
                                    }

                                    // Change state
                                    currentState = EnemyState.InvestigateState;
                                    isStoneInvestigation = true;
                                }
                            }
                        }
                    }

                    // 2 Roaming Parts
                    if (roamLocations != null)
                    {
                        // 2.1 Now: It means enemy will not move
                        if (roamLocations.Count == 1)
                        {
                            if (Math.Abs((_position - roamLocations.ElementAt(0)).Length()) > roamCheckDistance)
                            {
                                moveDir = roamLocations[0] - this._position;
                                moveDir.Normalize();
                                this._position += moveDir * speed * dTime;
                            }
                            ////Movement code
                            //if (moving)
                            //{
                            //    //Move enemy


                                //    //Time Increment
                                //    moveTime -= dTime;
                                //    if (moveTime <= 0)
                                //    {
                                //        downTime = 1f;
                                //        moving = false;
                                //    }
                                //}
                                //else
                                //{
                                //    //Time Increment
                                //    downTime -= dTime;
                                //    if (downTime <= 0)
                                //    {
                                //        moveTime = 1.5f;
                                //        moving = true;
                                //    }
                                //}
                        }
                        // 2.2 Multiple locations (Roam between locations)
                        else if (roamLocations.Count > 1)
                        {
                            // Check distance to roam target pos
                            // Set new roam target if enemy arrive the current one
                            if (Math.Abs((_position - roamLocations.ElementAt(roamTarget)).Length()) <= roamCheckDistance)
                            {
                                isForward = rng.Next(0, 2);
                                //Update target location
                                if (roamTarget == 0)
                                {
                                    roamTarget += 1;
                                }
                                else if (roamTarget == roamLocations.Count - 1)
                                {
                                    roamTarget -= 1;
                                }
                                else if (isForward == 0)
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
                                System.Diagnostics.Debug.WriteLine(this.Position);
                                //Move enemy
                                moveDir = roamLocations[roamTarget] - this._position;
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
                                    moveTime = rng.Next(1, 6);
                                    moving = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        //Movement code
                        if (moving)
                        {
                            System.Diagnostics.Debug.WriteLine(this.Position);
                            //Move enemy
                            moveDir = startingPosition - this._position;
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
                                moveTime = rng.Next(1, 6);
                                moving = true;
                            }
                        }
                    }

                    break;

                case EnemyState.InvestigateState:
                     // 1. Movement code
                     moveDir = movingTowards - this._position;
                     moveDir.Normalize();
                     this._position += moveDir * speed * dTime;

                    // 2. Investigate Detection code
                    // 2.1 Check if the enemy arrives the investigate point (stone/player)
                    //       After enemy get to the point, and the player is still in th investigate range, 
                    //       the enemy will reset the last seen position and investigate new point
                    if (Math.Abs((this.Position - movingTowards).Length()) <= 10)
                    {
                        // 2.2 if the player is still in the investigate range, update the point
                        if (RoamDetectionTrigger.CheckCollision(target))
                        {
                            detectionLink.EndPosition = target.Position;
                            // Update the investigate position if player within the range and no wall in between
                            if(WallDetection())
                            {
                                movingTowards = target.Position;
                                speed = baseSpeed;
                                isStoneInvestigation = false;
                            }
                            // There is wall between the enemy and player
                            else
                            {
                                // 2.2.1 Investigating stone, it will stay around the stone
                                if (isStoneInvestigation)
                                {
                                    speed = 0;
                                    stoneInvestigateTimer -= dTime;
                                    if (stoneInvestigateTimer <= 0)
                                    {
                                        InvestiageTORoam();
                                    }
                                }
                                // 2.2.2 Return if no player is within the chase range
                                else
                                {
                                    InvestiageTORoam();
                                }
                            }
                        }
                        // Player is not within the investigate range
                        else 
                        {
                            // 2.2.1 Investigating stone, it will stay around the stone
                            if (isStoneInvestigation)
                            {
                                speed = 0;
                                stoneInvestigateTimer -= dTime;
                                if (stoneInvestigateTimer <= 0)
                                {
                                    InvestiageTORoam();
                                }
                            }
                            // 2.2.2 Return if no player is within the chase range
                            else
                            {
                                InvestiageTORoam();
                            }
                        }
                    }

                    // 3. Chase Detection Code
                    //    Check If player is within chase start distance
                    if (Math.Abs((this._position - target.Position).Length()) <= ChaseStartDistance)
                    {
                        detectionLink.EndPosition = target.Position;
                        // Check if there is wall between enemy and player
                        if (WallDetection())
                        {
                            currentState = EnemyState.ChaseWindupState;
                            chaseWindupTimer = 6f;
                            target.SetShockState();
                            System.Diagnostics.Debug.WriteLine("Chase Start to Wind Up");
                            speed = baseSpeed * 2;
                            movingTowards = target.Position;
                        }  
                    }
                    break;

                case EnemyState.ChaseWindupState:
                    if(isAlerting)
                    {
                        currentState = EnemyState.ChaseState;
                    }
                    else
                    {
                        // This means the enmey is first see the player and will enter the wind up state
                        // 1. The chase wind up timer count down
                        chaseWindupTimer -= dTime;
                        if (chaseWindupTimer <= 0)
                        {
                            currentState = EnemyState.ChaseState;
                            System.Diagnostics.Debug.WriteLine("Chase start");
                            isAlerting = true;
                        }
                    }
                    
                    break;

                case EnemyState.ChaseState:
                    // 1. Movement code
                    movingTowards = target.Position;
                    moveDir = movingTowards - this._position;
                    moveDir.Normalize();
                    this._position += moveDir * speed * dTime;

                    // 2. Detection code
                    // 2.1  Detect if the player is in the chase range + 100
                    if(Math.Abs((this.Position - movingTowards).Length()) > (detectionRadius + 100))
                    {
                        target.SetWalkingState();
                        speed = baseSpeed;
                        currentState = EnemyState.ReturnState;
                    }

                    // 2.2 Check the link between player and enemy
                    //     if there is wall in between, the link brake
                    detectionLink.EndPosition = target.Position;
                    if (!WallDetection())
                    {
                        target.SetWalkingState();
                        currentState = EnemyState.ReturnState;
                        speed = baseSpeed;
                    }

                    break;

                case EnemyState.PlayerDeadState:
                    break;

                case EnemyState.ReturnState:
                    // 1. Enemey stay in the same place for 3 second - Time Increment
                    returnTimer -= dTime;
                    if (returnTimer <= 0)
                    {
                        currentState = EnemyState.RoamingState;
                        isAlerting = false;
                        returnTimer = 3;
                    }

                    // 2. If player enter the investigate range during the return state
                    //    The enemy will get into a investigate mode and chase mode without chase wind up
                    if (RoamDetectionTrigger.CheckCollision(target))
                    {
                        detectionLink.EndPosition = target.Position;
                        if (WallDetection()) // IF there is no wall between enemy and player
                        {
                            //System.Diagnostics.Debug.WriteLine("Dececting");
                            target.SetAfraidState();
                            movingTowards = target.Position;
                            System.Diagnostics.Debug.WriteLine($"Start to investigate {movingTowards}");
                            currentState = EnemyState.InvestigateState;
                        }
                    }

                    break;

                // End game chase that ccontinues regardless of distance/breakage
                case EnemyState.EndGameChaseState:
                    // 1. Movement code
                    movingTowards = target.Position;
                    moveDir = movingTowards - this._position;
                    moveDir.Normalize();
                    this._position += moveDir * baseSpeed * dTime;

                    // 2. Change speed if wall between enemy and player
                    detectionLink.EndPosition = target.Position;
                    if (roamDetectionTrigger.Intersects(target.PhysicsCollider))
                    {
                        speed = baseSpeed/2;
                    }
                    else
                    {
                        speed = baseSpeed;
                    }

                    break;
            }

            //This code always runs regardless of state
            if (this.PhysicsCollider.CheckCollision(target))
            {
                System.Diagnostics.Debug.WriteLine("Player died");
                currentState = EnemyState.PlayerDeadState;
                target.SetDeadState();
            }
            //Update display rectangle based on position
            displayRectangle.X = (int)_position.X - (int)origin.X;
            displayRectangle.Y = (int)_position.Y - (int)origin.Y;

            //If the enemy is in a state where there is a target, the detection code will run
            //This updates the enemy/player link as well as the last seen position if the player is in vision
            //!!! Haven't checked if really need it
            if (target != null)
            {
                detectionLink.EndPosition = target.Position;
                //playerDetectionLink.Position = _position;
                //SOME SORT OF DETECTION OF LINES COLLISION WITH MAP OBJECTS
                //playerDetectionLink.CheckCollision();

                //if no collision with map objects
                if (true)
                {
                    lastSeenPosition = detectionLink.EndPosition;
                }
            }

            // Update the animation
            if (isAnimated) UpdateAnimation(dTime);

            // Rotation code
            rotation = (float)Math.Atan2(moveDir.Y, moveDir.X)  + 90;
        }

        /// <summary>
        /// Starts the end game chase, causing enemy to charge at player through walls
        /// </summary>
        public void StartEndGameChaseSequence()
        {
            currentState = EnemyState.EndGameChaseState;
        }

        private void InvestiageTORoam()
        {
            speed = baseSpeed;
            target.SetWalkingState();
            currentState = EnemyState.RoamingState;
        }

        /// <summary>
        /// Detect if wall is between the detection line
        /// </summary>
        /// <returns>Return true if there is no wall between the detection link</returns>
        private bool WallDetection()
        {
            foreach (Wall wall in map.Walls)
            {
                if (detectionLink.CheckCollision(wall))
                {

                    System.Diagnostics.Debug.WriteLine($"{wall.Position}");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Set up the enemy animation
        /// </summary>
        private void EnemyAnimationSetUp()
        {
            numSpritesInSheet = 7;
            widthOfSingleSprite = enemyAnimatedTexturesheet.Width / numSpritesInSheet;

            // Set up animation stuff
            currentFrame = 1;
            fps = 7.0;
            secondsPerFrame = 1.0f / fps;
            timeCounter = 0;
            isAnimated = true;
        }

        /// <summary>
        /// Updates the animation time
        /// </summary>
        /// <param name="gameTime">Game time information</param>
        private void UpdateAnimation(float dTime)
        {
            // Add to the time counter (need TOTALSECONDS here)
            timeCounter += dTime;

            // Has enough time gone by to actually flip frames?
            if (timeCounter >= secondsPerFrame)
            {
                // Update the frame and wrap
                currentFrame++;
                if (currentFrame >= 4) currentFrame = 1;

                // Remove one "frame" worth of time
                timeCounter -= secondsPerFrame;
            }
        }

        /// <summary>
        /// Draws Enemy with a walking animation
        /// </summary>
        /// <param name="flip"></param>
        private void DrawEnemyWalkingAnimation(SpriteBatch batch, SpriteEffects flip = SpriteEffects.None)
        {
            batch.Draw(
                enemyAnimatedTexturesheet,
                this.Position,
                new Rectangle(widthOfSingleSprite * currentFrame, 0, widthOfSingleSprite, enemyAnimatedTexturesheet.Height),
                Color.White,
                rotation, // Rotation
                origin,
                1.0f,
                flip,
                0.0f);
        }

        /// <summary>
        /// Draws mario standing still
        /// </summary>
        /// <param name="flip">Should he be flipped horizontally?</param>
        private void DrawEnemyStandingAnimation(SpriteBatch batch, SpriteEffects flip = SpriteEffects.None)
        {
            batch.Draw(
                enemyAnimatedTexturesheet,
                this.Position,
                new Rectangle(0, 0, widthOfSingleSprite, enemyAnimatedTexturesheet.Height),
                Color.White,
                rotation, // Rotation
                origin,
                1.0f,
                flip,
                0.0f);
        }

    }
}
