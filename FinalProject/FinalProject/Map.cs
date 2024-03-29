﻿
// Author: Arthur Powers 3/12/2022
// Purpose:
//      Contains all objects that are within a level
//      Handles gameplay loop and level creation


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Penumbra;
using Microsoft.Xna.Framework.Content;
using System.IO;


namespace FinalProject
{
    enum Level
    {
        Tutorial,
        Test1,
        Level1,
        Level2,
        Level3
    }

    class Map
    {
        //Fields

        private readonly Player _player;
        private readonly List<Enemy> _enemies;
        private readonly List<Wall> _walls;
        private readonly List<Arrow> _arrows;
        private List<Glowstick> _glowsticks;
        private List<Glowstick> _landedGlowsticks;
        private List<Glowstick> _decayingStones;
        private readonly List<Vector2> _glowstickRevealAreas;
        private List<GlowstickPickup> _glowstickPickups;
        private int _glowstickCount;

        private readonly PenumbraComponent _penumbra;
        private readonly ContentManager _content;

        private const float _defaultWidth = 1920;
        private const float _defaultHeight = 1080;
        private float _width = _defaultWidth;
        private float _height = _defaultHeight;

        private Random rng;

        //End Game Chase variables
        private bool isEGCActive;
        private float stoneDecayTime;
        private float decayTimer;
        private float egcTimer;
        private List<Objective> endGoals = new List<Objective>();
        //State Manager reference
        GameStateManager stateManager;

        // Textures and Effects
        private Effect _maskEffect;
        private Effect _tileEffect;
        private Texture2D _tileTexture;
        private Texture2D _stoneRevealMask;
        private Texture2D _enemyTexture;
        private Texture2D _stoneMaskTexture;
        private Texture2D _glowstickTexture;
        private Texture2D _arrowTexture;
        private Texture2D _directionalPointerTexture;

        // Imported using LoadMap()
        private Texture2D _mapTexture;

        //Used for enemy test -- Delete later
        private Texture2D whiteTexture;
        private Texture2D circleTexture;

        //Properties
        public ContentManager Content => _content;
        public Player Player => _player;

        internal List<Glowstick> Glowsticks => _glowsticks;

        public int GlowstickCount { get => _glowstickCount; set => _glowstickCount = value; }

        internal List<Wall> Walls => _walls;

        internal List<Glowstick> LandedStones => _landedGlowsticks;
        internal List<Objective> EndGoals => endGoals;
        internal List<Glowstick> LandedGlowsticks => _landedGlowsticks;

        public bool IsEGCActive { get => isEGCActive; set => isEGCActive = value; }

        internal List<Arrow> Arrows => _arrows;

        internal List<Enemy> Enemies => _enemies;

        //Constructors
        public Map(PenumbraComponent penumbra, ContentManager content, Camera2D camera, GameStateManager stateManager)
        {
            _content = content;
            LoadContent();
            _player = new Player(new Vector2(500, 500), camera);
            _enemies = new List<Enemy>();
            _walls = new List<Wall>();
            _arrows = new List<Arrow>();
            _glowsticks = new List<Glowstick>();
            _landedGlowsticks = new List<Glowstick>();
            _decayingStones = new List<Glowstick>();
            _glowstickRevealAreas = new List<Vector2>();
            _glowstickPickups = new List<GlowstickPickup>();
            _penumbra = penumbra;

            // This will be external number.
            GlowstickCount = 10;

            //EGC Variables
            isEGCActive = false;
            stoneDecayTime = 5f;
            decayTimer = 3.8f;
            egcTimer = 30;

            this.stateManager = stateManager;
        }

        #region Draw Loop
        //Methods
        /// <summary>
        /// This method is used to show the enemy.
        /// </summary>
        /// <param name="batch"></param>
        public void DrawTest(SpriteBatch batch, Matrix transformMatrix)
        {
            //foreach (Wall wall in _walls)
            //{
            //    batch.Draw(whiteTexture, wall.WallRec, Color.Yellow); ;
            //}

            Vector2 offset = new Vector2(transformMatrix.Translation.X, transformMatrix.Translation.Y) * -1;
            foreach (Enemy enemy in Enemies)
            {
                batch.Draw(whiteTexture,
                    new Rectangle(enemy.DisplayRectangle.Location - new Point((int)offset.X,
                    (int)offset.Y),
                    enemy.DisplayRectangle.Size),
                    Color.White);
                enemy.RoamDetectionTrigger.DrawDebugTexture(batch, Color.Red);
                batch.Draw(circleTexture,
                    new Rectangle((int)enemy.Position.X - (int)enemy.DetectionRadius - (int)offset.X,
                    (int)enemy.Position.Y - (int)enemy.DetectionRadius - (int)offset.Y,
                    (int)enemy.DetectionRadius * 2, (int)enemy.DetectionRadius * 2), Color.White);
                batch.Draw(circleTexture,
                    new Rectangle((int)enemy.Position.X - (int)enemy.ChaseStartDistance - (int)offset.X,
                    (int)enemy.Position.Y - (int)enemy.ChaseStartDistance - (int)offset.Y,
                    (int)enemy.ChaseStartDistance * 2, (int)enemy.ChaseStartDistance * 2), Color.Yellow);

            }


        }

        /// <summary>
        /// Draws all elements of the map, including the player and any walls
        /// </summary>
        /// <param name="batch"></param>
        public void Draw(SpriteBatch batch)
        {
            // Probably where enemies, stones, and stone reveal areas would be drawn
            foreach (Wall wall in Walls)
            {
                wall.PhysicsCollider.DrawDebugTexture(batch, Color.White);
            }

            foreach (GlowstickPickup glowstickPickup in _glowstickPickups)
            {
                glowstickPickup.Draw(batch);
            }

            foreach (Enemy enemy in Enemies)
            {
                enemy.Display(batch);
            }

            foreach (Arrow arrow in _arrows)
            {
                arrow.Draw(batch, Color.White);
            }

        }

        public void DrawBackground(SpriteBatch batch, Matrix transformMatrix)
        {
            Matrix view = Matrix.Identity;

            int width = batch.GraphicsDevice.Viewport.Width;
            int height = batch.GraphicsDevice.Viewport.Height;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, width, height, 0, 0, 1);
            Vector2 translation2D = new Vector2(transformMatrix.Translation.X / _tileTexture.Width, transformMatrix.Translation.Y / _tileTexture.Height);


            _tileEffect.Parameters["ViewProjection"].SetValue(view * projection);
            _tileEffect.Parameters["UVScale"].SetValue(new Vector2(width / (float)_tileTexture.Width, height / (float)_tileTexture.Height));
            _tileEffect.Parameters["CameraOffset"].SetValue(translation2D);
            batch.Begin(effect: _tileEffect, samplerState: SamplerState.LinearWrap);
            batch.Draw(_tileTexture, batch.GraphicsDevice.Viewport.Bounds, Color.White);
            batch.End();
        }

        public void DrawDirectionalArrows(SpriteBatch batch)
        {
            float width = batch.GraphicsDevice.Viewport.Width;
            float height = batch.GraphicsDevice.Viewport.Height;
            for (int i = 0; i < Enemies.Count; i++)
            {
                if (_enemies[i].CurrentState == EnemyState.ChaseState || 
                    _enemies[i].CurrentState == EnemyState.InvestigateState ||
                    _enemies[i].CurrentState == EnemyState.ChaseWindupState)
                {
                    float angle = MathF.Atan2(Enemies[i].Y - _player.Y, Enemies[i].X - _player.X) + MathF.PI / 2;

                    // Get alpha from enemy distance
                    // Uses squares of distance to avoid sqrt()
                    float alpha = MathHelper.Clamp((MathF.Pow(Enemies[i].X - _player.X, 2) + MathF.Pow(Enemies[i].Y - _player.Y, 2)) /
                                                   (width * width / 4),
                                                   0, 1);

                    Color tint = _enemies[i].CurrentState == EnemyState.ChaseState ?
                                 new Color(1, 1, 1, 1 - alpha) :
                                 new Color(0.5f, 1, 0, 1 - alpha);

                    batch.Draw(_directionalPointerTexture,
                               new Vector2(width / 2 + MathF.Cos(angle) * 100, height / 2 + MathF.Sin(angle) * 100),
                               null,
                               tint,
                               angle,
                               new Vector2(_directionalPointerTexture.Width * 5 / 4, _directionalPointerTexture.Height / 2 + 100),
                               1,
                               SpriteEffects.None,
                               1);
                }
            }
        }
        #endregion

        /// <summary>
        /// Updates enemy positions and stone throws.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(float dTime)
        {

            // Player 
            _player.Move(dTime);
            _player.Update(dTime);
            _player.ThrowStone(Glowsticks, _penumbra, _stoneMaskTexture, this);

            foreach (Glowstick stone in Glowsticks) stone.Update(dTime);
            foreach (Glowstick stone in LandedGlowsticks) stone.Update(dTime);
            foreach (Glowstick stone in _decayingStones) stone.Update(dTime);

            Glowstick selected = null;
            if (Glowsticks.Count > 0)
            {
                selected = _glowsticks[0];
            }

            //Turns true if a stone has moved from the active list to the dead list
            List<Glowstick> removed = new List<Glowstick>();

            // Wall collisions
            foreach (Wall wall in Walls)
            {
                // Player collision
                ColliderHitInfo hit;
                if (wall.PhysicsCollider.CheckCollision(_player, out hit))
                {
                    _player.Position = hit.HitPoint + hit.Normal * ((CircleCollider)_player.PhysicsCollider).Radius;
                }

                // Stone collisions
                foreach (Glowstick stone in Glowsticks)
                {
                    if (wall.PhysicsCollider.CheckCollision(stone, out hit))
                    {
                        stone.Position = hit.HitPoint + hit.Normal * (((CircleCollider)stone.PhysicsCollider).Radius + 1);
                        stone.Bounce(hit.Normal);
                    }
                    if (stone.Landed)
                    {
                        bool availableLandingPosition = true;
                        foreach (Glowstick s in LandedGlowsticks)
                        {
                            if (Vector2.Distance(s.Position, stone.Position) < s.TargetScale / 1.8f)
                            {
                                availableLandingPosition = false;
                            }
                        }
                        if (availableLandingPosition)
                        {
                            LandedGlowsticks.Add(stone);
                        }
                        else
                        {
                            _glowstickCount += 1;
                            _decayingStones.Add(stone);
                            stone.TargetScale = 0;
                        }
                        removed.Add(stone);
                    }
                }
                if (removed.Count > 0)
                {
                    foreach (Glowstick s in removed)
                    {
                        _glowsticks.Remove(s);
                    }
                }
            }

            for (int i = _glowstickPickups.Count - 1; i >= 0; i--)
            {
                if (_player.PhysicsCollider.CheckCollision(_glowstickPickups[i]))
                {
                    _glowstickCount += _glowstickPickups[i].NumGlowsticks;
                    _penumbra.Lights.Remove(_glowstickPickups[i].PointLight);
                    _glowstickPickups.RemoveAt(i);
                }
            }

            // Enemy 
            foreach (Enemy enemy in Enemies)
            {
                enemy.Update(dTime);
            }

            //EGC Stuff

            if (isEGCActive)
            {
                if (decayTimer < 0)
                {
                    foreach (Glowstick stone in LandedGlowsticks)
                    {
                        if (stone.TargetScale > 0)
                        {
                            if (selected == null)
                            {
                                selected = stone;
                            }
                            else
                            {
                                if (Vector2.Distance(stone.Position, _player.Position) < Vector2.Distance(selected.Position, _player.Position))
                                {
                                    selected = stone;
                                }
                            }
                        }
                    }
                    if (selected != null)
                    {
                        selected.TargetScale = 0;

                    }
                    decayTimer = stoneDecayTime;
                }
                decayTimer -= dTime;
                egcTimer -= dTime;
                if (egcTimer < 0)
                {
                    stateManager.Set_WinState();
                    ResetMap();
                }
            }
        }


        public void LoadLevel(Level level)
        {
            switch (level)
            {
                case Level.Tutorial:
                    LoadTutorial();
                    break;

                case Level.Test1:
                    LoadFromFile("testLevel.lvl");
                    break;

                case Level.Level1:
                    LoadFromFile("MainLevel1.lvl");
                    break;

                case Level.Level2:
                    LoadFromFile("MainLevel2.lvl");
                    break;

                case Level.Level3:
                    LoadFromFile("MainLevel3.lvl");
                    break;

                default:
                    LoadTutorial();
                    break;
            }
        }

        public void LoadTutorial()
        {
            ResetMap();
            _width = 1920;
            _height = 1080;

            // Create walls

            // Boundaries
            Walls.Add(new Wall(new Vector2(_width / 2, 50), _width, 100));
            Walls.Add(new Wall(new Vector2(_width / 2, 1080 - 50), _width, 100));
            Walls.Add(new Wall(new Vector2(50, _height / 2), 100, _height));
            Walls.Add(new Wall(new Vector2(_width - 50, _height / 2), 100, _height));

            // Center wall
            Walls.Add(new Wall(new Vector2(_width / 2, _height / 2), 500, 500));

            //// Setting up wall graphics
            //foreach (Wall wall in _walls)
            //{
            //    wall.PhysicsCollider.SetDebugTexture(_penumbra.GraphicsDevice, Color.White);
            //}

            //Create Roam Points
            List<Vector2> roamPoints = new List<Vector2> { new Vector2(1500, 700), new Vector2(1500, 100), new Vector2(100, 100) };
            Enemies.Add(new Enemy(_enemyTexture, this, roamPoints[0], roamPoints, 800, 100));

            // Set up lighting after walls are created
            SetupPenumbraLighting();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="serviceProvider">The service provider that will be used to construct a ContentManager.</param>
        /// // Note: When use it in the Game1, pass "Services" as the serviceProvider
        public void LoadFromFile(string filepath)
        {
            /// |tiletype,x,y,width,height,roampoints|
            /// enemy tiles will contain a collection of child roam nodes, others will just say "empty"
            /// Roam Point Notation:
            /// [roampoint,x,y,index[

            ResetMap();

            //Create reader and grab the data
            string fullPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "..\\..\\..\\..\\" + filepath;
            fullPath = filepath;
            StreamReader reader = new StreamReader(fullPath);
            String data = reader.ReadToEnd();
            reader.Close();
            //Close the reader

            String[] fileLines = data.Split('|');
            SetupPenumbraLighting();
            List<Vector2> allGoals = new List<Vector2>();
            foreach (String currentLine in fileLines)
            {
                if (currentLine == "")
                {
                    continue;
                }
                String[] tileData = currentLine.Split(',');

                //X, Y, Width, Height variables
                int indexToPixels = 200;
                int x = Convert.ToInt32(tileData[1]) * indexToPixels;
                int y = Convert.ToInt32(tileData[2]) * indexToPixels;
                int w = Convert.ToInt32(tileData[3]) * indexToPixels;
                int h = Convert.ToInt32(tileData[4]) * indexToPixels;

                bool isArrow = false;

                //Switch for all different types of placeables
                switch (tileData[0])
                {
                    case "wall":
                        Walls.Add(new Wall(new Vector2(x + (w / 2), y + (h / 2)), w, h));
                        break;
                    case "enemy":
                        break;
                    case "spawn":
                        Player.Position = new Vector2(x + (indexToPixels / 2), y + (indexToPixels / 2));
                        break;
                    case "objective":
                        allGoals.Add(new Vector2(x + (indexToPixels / 2), y + (indexToPixels / 2)));


                        break;
                    case "exit":
                        break;
                    case "glow":
                        _glowstickPickups.Add(new GlowstickPickup(new Vector2(x + (indexToPixels / 2), y + (indexToPixels / 2)), _penumbra, _glowstickTexture));
                        break;
                    case "arrow":
                        //An arrow will store its direction as up/down/left/right within this variable, must be parsed
                        String arrowDirection = tileData[5];

                        Arrows.Add(new Arrow(new Vector2(x + w / 2, y + h / 2), _arrowTexture, tileData[5]));

                        isArrow = true;
                        break;
                }



                //Set up for the roam points, do nothing if empty
                if (!tileData[5].Equals("empty") && !isArrow)
                {
                    List<Vector2> roamPoints2 = new List<Vector2>();
                    String[] roamPoints = tileData[5].Split('[');
                    foreach (String roamPoint in roamPoints)
                    {
                        if (roamPoint != "")
                        {
                            //Set up code for roam points within a given enemy
                            String[] b2 = roamPoint.Split(']');
                            int rx = Convert.ToInt32(b2[1]);
                            int ry = Convert.ToInt32(b2[2]);
                            int index = Convert.ToInt32(b2[3]);
                            roamPoints2.Add(new Vector2(rx * indexToPixels, ry * indexToPixels));
                        }
                    }
                    //_enemies.Add(new Enemy(new Vector2(x + (indexToPixels / 2), y + (indexToPixels / 2)), roamPoints2, 800, _enemyTexture, 150, 150, 100, Player, _walls));
                    Enemies.Add(new Enemy(_enemyTexture, this, new Vector2(x + 130, y + 130), roamPoints2, 650, 100));
                }

            }

            // Create a new content manager to load content used just by this map
            // this content can be used to content.Load, not sure if we need it
            //_content = new ContentManager(serviceProvider, "Content");

            // Get ahold of the lighting system and reset it
            //_penumbra = (PenumbraComponent)serviceProvider.GetService(typeof(PenumbraComponent));
            SetupPenumbraLighting();

            foreach (Vector2 position in allGoals)
            {
                Objective newGoal = new Objective(position, _player);
                //Player.Position = position;
                _penumbra.Lights.Add(newGoal.PointLight);
                endGoals.Add(newGoal);

            }


            //Debug code that puts all enemies into end game chase sequence at the start of the game
            /*
            foreach(Enemy e in _enemies)
            {
                e.StartEndGameChaseSequence();
            }
            */
        }

        private void SetupPenumbraLighting()
        {
            _penumbra.Hulls.Clear();
            _penumbra.Lights.Clear();

            //Add the hulls into the penumbra system
            // Note: create walls before this and add walls into the walls list
            //       it should work, if not dm Runi :)
            foreach (Wall wall in Walls)
            {
                _penumbra.Hulls.Add(wall.Hull);
            }
            foreach (GlowstickPickup glowstickPickup in _glowstickPickups)
            {
                _penumbra.Lights.Add(glowstickPickup.PointLight);
            }
            _penumbra.Lights.Add(_player.Flashlight);
        }

        /// <summary>
        /// Load Method for map. Load basic texutures
        /// </summary>
        /// <param name="content">ContentManager</param>
        private void LoadContent()
        {
            _enemyTexture = _content.Load<Texture2D>("EnemySpriteSheet");
            _stoneMaskTexture = _content.Load<Texture2D>("Stone_Reveal_Mask");
            _glowstickTexture = _content.Load<Texture2D>("Glowstick_Lit");
            _arrowTexture = _content.Load<Texture2D>("BloodyArrowUp");
            _directionalPointerTexture = _content.Load<Texture2D>("directional_pointer");

            //Test purpose
            whiteTexture = _content.Load<Texture2D>("blackbox2");
            circleTexture = _content.Load<Texture2D>("TestCircleRange");
            _tileTexture = _content.Load<Texture2D>("MossBackground");
            _tileEffect = _content.Load<Effect>("TileBackgroundEffect");
            _maskEffect = _content.Load<Effect>("ImageMask");

        }

        /// <summary>
        /// Begins the end game chase
        /// </summary>
        public void TriggerEndGameChase()
        {
            if (isEGCActive == true)
            {
                return;
            }
            isEGCActive = true;

            foreach (Enemy e in Enemies)
            {
                e.StartEndGameChaseSequence(_penumbra);
            }
            _player.SetChaseState();
            _player.StartEndGameChase();
        }

        /// <summary>
        /// Clears all map data
        /// </summary>
        private void ResetMap()
        {
            _player.Reset();
            _walls.Clear();
            Enemies.Clear();
            _glowstickRevealAreas.Clear();
            _width = _defaultWidth;
            _height = _defaultHeight;
            _glowstickCount = 5;
            isEGCActive = false;
            egcTimer = 30f;
            decayTimer = 3.8f;
            _glowsticks = new List<Glowstick>();
            _landedGlowsticks = new List<Glowstick>();
            _decayingStones = new List<Glowstick>();
        }
    }
}
