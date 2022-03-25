
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
        //private List<Stone> stones;

        private PenumbraComponent _penumbra;
        private ContentManager _content;
        private float _width;
        private float _height;

        // Textures and Effects
        private Effect _maskEffect;
        private Texture2D _stoneRevealMask;

        // Imported using LoadMap()
        private Texture2D _mapTexture;

        //Properties
        public ContentManager Content => _content;

        //Constructors


        public Map(Player player, PenumbraComponent penumbra/*, Effect maskEffect, Texture2D stoneRevealMask*/)
        {
            _player = player;
            _player.Position = new Vector2(500, 500);

            _penumbra = penumbra;
            //_maskEffect = maskEffect;
            //_stoneRevealMask = stoneRevealMask;

            _width = 1920;
            _height = 1080;

            _enemies = new List<Enemy>();
            _walls = new List<Wall>();

            // Create walls

            // Boundaries
            _walls.Add(new Wall(new Vector2(_width / 2, 50), _width, 100));
            _walls.Add(new Wall(new Vector2(_width / 2, 1080 - 50), _width, 100));
            _walls.Add(new Wall(new Vector2(50, _height / 2), 100, _height));
            _walls.Add(new Wall(new Vector2(_width - 50, _height / 2), 100, _height));

            // Center wall
            _walls.Add(new Wall(new Vector2(_width / 2, _height / 2), 500, 500));

            // Set up lighting after walls are created
            SetupPenumbraLighting();
            
        }

        //Methods

        /// <summary>
        /// Draws all elements of the map, including the player and any walls
        /// </summary>
        /// <param name="batch"></param>
        public void Draw(SpriteBatch batch)
        {
            // Probably where enemies, stones, and stone reveal areas would be drawn
        }

        /// <summary>
        /// Updates enemy positions and stone throws.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(float dTime)
        {

            _player.Move(dTime);
            _player.Update(dTime);

            foreach(Wall wall in _walls)
            {
                ColliderHitInfo hit;
                if (wall.PhysicsCollider.CheckCollision(_player, out hit))
                {
                    _player.Position = hit.HitPoint + hit.Normal * ((CircleCollider)_player.PhysicsCollider).Radius;
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="serviceProvider">The service provider that will be used to construct a ContentManager.</param>
        /// // Note: When use it in the Game1, pass "Services" as the serviceProvider
        public void Load(string filepath, IServiceProvider serviceProvider)
        {
            // Open the file for reading
            StreamReader input = null;
            try
            {
                input = new StreamReader(filepath);
                string line = null;

                // The first line of level file is width and height of the map
                if((line = input.ReadLine())!= null)
                {
                    string[] peices = line.Split(',');
                    _width = int.Parse(peices[0]);
                    _height = int.Parse(peices[1]);
                }

                // The second line will be the player info
            }
            catch
            {

            }


            // Create a new content manager to load content used just by this map
            // this content can be used to content.Load, not sure if we need it
            //_content = new ContentManager(serviceProvider, "Content");

            // Get ahold of the lighting system and reset it
            //_penumbra = (PenumbraComponent)serviceProvider.GetService(typeof(PenumbraComponent));

            SetupPenumbraLighting();
        }

        private void SetupPenumbraLighting()
        {
            _penumbra.Hulls.Clear();
            _penumbra.Lights.Clear();

            //Add the hulls into the penumbra system
            // Note: create walls before this and add walls into the walls list
            //       it should work, if not dm Runi :)
            foreach(Wall wall in _walls)
            {
                _penumbra.Hulls.Add(wall.Hull);
            }
            _penumbra.Lights.Add(_player.Flashlight);
        }
    }
}
