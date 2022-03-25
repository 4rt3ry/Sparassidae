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

namespace FinalProject
{
    class Map
    {
        //Fields

        private Player _player;
        private List<Collider> _mapColliders;
        private List<Enemy> _enemies;
        //private List<Stone> stones;


        // Textures and Effects
        private Effect _maskEffect;
        private Texture2D _stoneRevealMask;

        // Imported using LoadMap()
        private Texture2D _mapTexture;

        //Properties

        //Constructors


        public Map(Player player/*, Effect maskEffect, Texture2D stoneRevealMask*/)
        {
            _player = player;
            //_maskEffect = maskEffect;
            //_stoneRevealMask = stoneRevealMask;

            _mapColliders = new List<Collider>();
            _enemies = new List<Enemy>();

            //_mapColliders.Add(new RectangleCollider(null, new Vector2(1920 / 2, 0), new Vector2(1920, 10), false));
            //_mapColliders.Add(new RectangleCollider(null, new Vector2(1920 / 2, 1080), new Vector2(1920, 10), false));
            _mapColliders.Add(new RectangleCollider(null, new Vector2(300, 300), new Vector2(200, 200), false));
            _mapColliders.Add(new RectangleCollider(null, new Vector2(300, 500), new Vector2(200, 200), false));
        }

        //Methods

        /// <summary>
        /// Draws all elements of the map, including the player and any walls
        /// </summary>
        /// <param name="batch"></param>
        public void Draw(SpriteBatch batch)
        {
            foreach(Collider collider in _mapColliders)
            {
                collider.SetDebugTexture(batch.GraphicsDevice, Color.White);
                collider.DrawDebugTexture(batch, Color.White);
            }
            _player.Display(batch);
        }

        /// <summary>
        /// Updates enemy positions and stone throws.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(float dTime)
        {
            foreach(Collider collider in _mapColliders)
            {
                ColliderHitInfo hitInfo;
                if (collider.CheckCollision(_player, out hitInfo))
                {
                    _player.Position = hitInfo.HitPoint + hitInfo.Normal * ((CircleCollider)_player.PhysicsCollider).Radius;
                }
                //if (collider.Intersects(_player.PhysicsCollider))
                //{
                //    _player.Position = Vector2.Zero;
                //}
            }
        }

        public void Load(string filepath)
        {

        }
    }
}
