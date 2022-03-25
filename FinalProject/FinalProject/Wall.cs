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
        private Hull hull;
        private float wdith;
        private float height;

        public Hull Hull { get => hull; set => hull = value; }
        public float Wdith { get => wdith; set => wdith = value; }
        public float Height { get => height; set => height = value; }

        /// <summary>
        /// Create the wall rectangle shadow hull 
        /// </summary>
        /// <param name="width">Width of the wall</param>
        /// <param name="height">Height of the wall</param>
        /// <param name="position">Initial position of the wall</param>
        public Wall(float width, float height, Vector2 position)
        {
            this.wdith = width;
            this.height = height;
            //Create the rectangle hull
            hull = Hull.CreateRectangle(position, new Vector2(width, height));
        }

    }
}
