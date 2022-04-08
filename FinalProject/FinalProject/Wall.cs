// Runi Jiang
// 3/22/2022
// Wall Class: Hold positions and hulls of wall
//            

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Penumbra;

namespace FinalProject
{
    class Wall : GameObject
    {
        //Fields
        private Hull _hull;
        private float _width;
        private float _height;
        private Rectangle _wallRec;

        public Hull Hull { get => _hull; set => _hull = value; }
        public float Width { get => _width; set => _width = value; }
        public float Height { get => _height; set => _height = value; }
        public Rectangle WallRec { get => _wallRec; set => _wallRec = value; }

        /// <summary>
        /// Create the wall rectangle shadow hull 
        /// </summary>
        /// <param name="width">Width of the wall</param>
        /// <param name="height">Height of the wall</param>
        /// <param name="position">Initial position of the wall</param>
        public Wall(Vector2 position, float width, float height): base()
        {
            // Set up Wall's private fields
            _position = position;
            _width = width;
            _height = height;
            _hull = Hull.CreateRectangle(position, new Vector2(width, height));

            // Set up Colliders
            _physicsCollider = new RectangleCollider(this, new Vector2(0, 0), new Vector2(width, height), false);

            _wallRec = new Rectangle((int)position.X, (int)position.Y, (int)width, (int)height);
        }

    }
}
