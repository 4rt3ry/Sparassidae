
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

        private Player _player;
        private List<Enemy> _enemies;
        private List<Wall> _walls;
        private List<Vector2> _stoneRevealAreas;

        private PenumbraComponent _penumbra;
        private ContentManager _content;

        private const float _defaultWidth = 1920;
        private const float _defaultHeight = 1080;
        private float _width = _defaultWidth;
        private float _height = _defaultHeight;

        // Textures and Effects
        private Effect _maskEffect;
        private Texture2D _stoneRevealMask;

        // Imported using LoadMap()
        private Texture2D _mapTexture;

        //Properties
        public ContentManager Content => _content;

        //Constructors


        public Map(PenumbraComponent penumbra)
        {
            _player = new Player(new Vector2(500, 500));
            _enemies = new List<Enemy>();
            _walls = new List<Wall>();
            _stoneRevealAreas = new List<Vector2>();
            _penumbra = penumbra;
        }

        //Methods

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
        }

        /// <summary>
        /// Updates enemy positions and stone throws.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(float dTime)
        {

            _player.Move(dTime);
            _player.Update(dTime);

            foreach (Wall wall in _walls)
            {
                ColliderHitInfo hit;
                if (wall.PhysicsCollider.CheckCollision(_player, out hit))
                {
                    _player.Position = hit.HitPoint + hit.Normal * ((CircleCollider)_player.PhysicsCollider).Radius;
                }
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

            // Set up lighting after walls are created
            SetupPenumbraLighting();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="serviceProvider">The service provider that will be used to construct a ContentManager.</param>
        /// // Note: When use it in the Game1, pass "Services" as the serviceProvider
        public void LoadFromFile(string filepath, IServiceProvider serviceProvider)
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
                int x = Convert.ToInt32(tileData[1]);
                int y = Convert.ToInt32(tileData[2]);
                int w = Convert.ToInt32(tileData[3]);
                int h = Convert.ToInt32(tileData[4]);

                //Switch for all different types of placeables
                switch (tileData[0])
                {
                    case "wall":
                        _walls.Add(new Wall(new Vector2(x, y), w, h));
                        break;
                    case "enemy":
                        break;
                    case "spawn":
                        break;
                    case "objective":
                        break;
                    case "exit":
                        break;
                }

                //Set up for the roam points, do nothing if empty
                if (!tileData[5].Equals("empty"))
                {
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

                        }
                    }
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
    }
}
