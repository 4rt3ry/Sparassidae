
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
    class Map
    {
        //Fields

        private readonly Player _player;
        private readonly List<Enemy> _enemies;
        private readonly List<Wall> _walls;
        private readonly List<Stone> _stones;
        private readonly List<Vector2> _stoneRevealAreas;

        private readonly PenumbraComponent _penumbra;
        private readonly ContentManager _content;

        private const float _defaultWidth = 1920;
        private const float _defaultHeight = 1080;
        private float _width = _defaultWidth;
        private float _height = _defaultHeight;

        // Textures and Effects
        private Effect _maskEffect;
        private Texture2D _stoneRevealMask;
        private Texture2D _enemyTexture;

        // Imported using LoadMap()
        private Texture2D _mapTexture;

        //Used for enemy test -- Delete later
        private Texture2D whiteTexture;
        private Texture2D circleTexture;

        //Properties
        public ContentManager Content => _content;
        public Player Player => _player;

        //Constructors


        public Map(PenumbraComponent penumbra, ContentManager content, Camera2D camera)
        {
            _content = content;
            LoadContent();
            _player = new Player(new Vector2(500, 500),camera);
            _enemies = new List<Enemy>();
            _walls = new List<Wall>();
            _stones = new List<Stone>();
            _stoneRevealAreas = new List<Vector2>();
            _penumbra = penumbra;
        }

        //Methods
        /// <summary>
        /// This method is used to show the enemy.
        /// </summary>
        /// <param name="batch"></param>
        public void DrawTest(SpriteBatch batch)
        {
            //foreach (Wall wall in _walls)
            //{
            //    batch.Draw(whiteTexture, wall.WallRec, Color.Yellow); ;
            //}

            foreach (Enemy enemy in _enemies)
            {
                batch.Draw(whiteTexture, enemy.DisplayRectangle, Color.White);
                enemy.RoamDetectionTrigger.DrawDebugTexture(batch, Color.Red);
                batch.Draw(circleTexture,
                    new Rectangle((int)enemy.Position.X - (int)enemy.DetectionRadius + enemy.DisplayRectangle.Width / 2,
                    (int)enemy.Position.Y - (int)enemy.DetectionRadius + enemy.DisplayRectangle.Height / 2,
                    (int)enemy.DetectionRadius *2 , (int)enemy.DetectionRadius *2), Color.White);
                batch.Draw(circleTexture,
                    new Rectangle((int)enemy.Position.X - (int)enemy.ChaseStartDistance + enemy.DisplayRectangle.Width / 2,
                    (int)enemy.Position.Y - (int)enemy.ChaseStartDistance + enemy.DisplayRectangle.Height / 2,
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
            foreach (Wall wall in _walls)
            {
                wall.PhysicsCollider.DrawDebugTexture(batch, Color.White);
            }

            foreach (Enemy enemy in _enemies)
            {
                enemy.Display(batch);
            }
            
        }

        /// <summary>
        /// Updates enemy positions and stone throws.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(float dTime)
        {

            // Player 
            _player.Move(dTime);
            _player.Update(dTime);
            _player.ThrowStone(_stones, _penumbra);

            foreach (Stone stone in _stones) stone.Update(dTime);

            // Wall collisions
            foreach (Wall wall in _walls)
            {
                // Player collision
                ColliderHitInfo hit;
                if (wall.PhysicsCollider.CheckCollision(_player, out hit))
                {
                    _player.Position = hit.HitPoint + hit.Normal * ((CircleCollider)_player.PhysicsCollider).Radius;
                }

                // Stone collisions
                foreach (Stone stone in _stones)
                {
                    if (wall.PhysicsCollider.CheckCollision(stone, out hit))
                    {
                        stone.Position = hit.HitPoint + hit.Normal * ((CircleCollider)stone.PhysicsCollider).Radius;
                        stone.Bounce(hit.Normal);
                    }
                }
            }

            // Enemy 
            foreach (Enemy enemy in _enemies)
            {
                enemy.Update(dTime);
            }
        }

        public void LoadTutorial()
        {
            ResetMap();

            _width = 1920;
            _height = 1080;

            // Create walls

            // Boundaries
            _walls.Add(new Wall(new Vector2(_width / 2, 50), _width, 100));
            _walls.Add(new Wall(new Vector2(_width / 2, 1080 - 50), _width, 100));
            _walls.Add(new Wall(new Vector2(50, _height / 2), 100, _height));
            _walls.Add(new Wall(new Vector2(_width - 50, _height / 2), 100, _height));

            // Center wall
            _walls.Add(new Wall(new Vector2(_width / 2, _height / 2), 500, 500));

            //// Setting up wall graphics
            //foreach (Wall wall in _walls)
            //{
            //    wall.PhysicsCollider.SetDebugTexture(_penumbra.GraphicsDevice, Color.White);
            //}

            //Create Roam Points
            List<Vector2> roamPoints = new List<Vector2> { new Vector2(1500, 700), new Vector2(1500, 100), new Vector2(100, 100) };
            _enemies.Add(new Enemy(roamPoints[0], roamPoints, 800, _enemyTexture, 150, 150, 100, Player, _walls));
            //List<Vector2> roamPoints2 = null;
            //_enemies.Add(new Enemy(new Vector2(1550, 100), roamPoints2, 800, _enemyTexture, 150, 150, 100, Player, _walls));
            //_enemies.Add(new Enemy(new Vector2(1500, 100), _enemyTexture, 200, 200));

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
            StreamReader reader = new StreamReader(filepath);
            String data = reader.ReadToEnd();
            reader.Close();
            //Close the reader

            String[] fileLines = data.Split('|');
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

                //Switch for all different types of placeables
                switch (tileData[0])
                {
                    case "wall":
                        _walls.Add(new Wall(new Vector2(x + (w/2), y + (h/2)), w, h));
                        break;
                    case "enemy":
                        break;
                    case "spawn":
                        Player.Position = new Vector2(x + (indexToPixels / 2), y + (indexToPixels/2));
                        break;
                    case "objective":
                        break;
                    case "exit":
                        break;
                }

                

                //Set up for the roam points, do nothing if empty
                if (!tileData[5].Equals("empty"))
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
                    _enemies.Add(new Enemy(new Vector2(x + (indexToPixels / 2), y + (indexToPixels / 2)), roamPoints2, 800, _enemyTexture, 150, 150, 100, Player, _walls));
                }
                
            }



            // Create a new content manager to load content used just by this map
            // this content can be used to content.Load, not sure if we need it
            //_content = new ContentManager(serviceProvider, "Content");

            // Get ahold of the lighting system and reset it
            //_penumbra = (PenumbraComponent)serviceProvider.GetService(typeof(PenumbraComponent));

            SetupPenumbraLighting();
        }

        /// <summary>
        /// Clears all map data
        /// </summary>
        private void ResetMap()
        {
            _walls.Clear();
            _enemies.Clear();
            _stoneRevealAreas.Clear();
            _width = _defaultWidth;
            _height = _defaultHeight;
        }

        private void SetupPenumbraLighting()
        {
            _penumbra.Hulls.Clear();
            _penumbra.Lights.Clear();

            //Add the hulls into the penumbra system
            // Note: create walls before this and add walls into the walls list
            //       it should work, if not dm Runi :)
            foreach (Wall wall in _walls)
            {
                _penumbra.Hulls.Add(wall.Hull);
            }
            _penumbra.Lights.Add(_player.Flashlight);
        }

        /// <summary>
        /// Load Method for map. Load basic texutures
        /// </summary>
        /// <param name="content">ContentManager</param>
        private void LoadContent()
        {
            _enemyTexture = _content.Load<Texture2D>("Enemy");

            //Test purpose
            whiteTexture = _content.Load<Texture2D>("blackbox2");
            circleTexture = _content.Load<Texture2D>("TestCircleRange");
        }
    }
}
