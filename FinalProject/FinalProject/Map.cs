
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
        private List<Collider> _mapColliders;
        private List<Enemy> _enemies;
        private List<Wall> _walls;
        //private List<Stone> stones;

        private PenumbraComponent penumbra;
        private ContentManager content;
        private float width;
        private float height;

        // Textures and Effects
        private Effect _maskEffect;
        private Texture2D _stoneRevealMask;

        // Imported using LoadMap()
        private Texture2D _mapTexture;

        //Properties
        public ContentManager Content { get{return content;}}

        //Constructors


        public Map(Player player, Effect maskEffect, Texture2D stoneRevealMask)
        {
            _player = player;
            _maskEffect = maskEffect;
            _stoneRevealMask = stoneRevealMask;

            _mapColliders = new List<Collider>();
            _enemies = new List<Enemy>();
            _walls = new List<Wall>();
        }

        //Methods

        /// <summary>
        /// Draws all elements of the map, including the player and any walls
        /// </summary>
        /// <param name="batch"></param>
        public void Draw(SpriteBatch batch)
        {

        }

        /// <summary>
        /// Updates enemy positions and stone throws.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {

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
                    width = int.Parse(peices[0]);
                    height = int.Parse(peices[1]);
                }

                // The second line will be the player info
            }
            catch
            {

            }


            // Create a new content manager to load content used just by this map
            // this content can be used to content.Load, not sure if we need it
            content = new ContentManager(serviceProvider, "Content");

            // Get ahold of the lighting system and reset it
            penumbra = (PenumbraComponent)serviceProvider.GetService(typeof(PenumbraComponent));
            penumbra.Hulls.Clear();
            penumbra.Lights.Clear();

            //Add the hulls into the penumbra system
            // Note: create walls before this and add walls into the walls list
            //       it should work, if not dm Runi :)
            for(int i = 0; i < _walls.Count; i ++)
            {
                penumbra.Hulls.Add(_walls[i].Hull);
            }
            penumbra.Lights.Add(_player.Flashlight);
        }
    }
}
