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

        public Wall(float width, float height)
        {
            this.wdith = width;
            this.height = height;
            hull = new Hull(this.Position, new Vector2(this.X + width, this.Y),
                new Vector2(this.X, this.Y + height), new Vector2(this.X + width, this.Y + height))
            {
                Position = this.Position
            };
        }




    }
}
