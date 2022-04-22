using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject
{
    class Arrow : GameObject
    {

        //Fields
        private Texture2D arrowTexture;
        private Rectangle displayRect;

        //Properties


        //Constructors
        public Arrow()
        {
        }

        public Arrow(Vector2 position, Texture2D arrowTexture) : base(position)
        {
            this.arrowTexture = arrowTexture;
            this.displayRect = new Rectangle((int)position.X, (int)position.Y, arrowTexture.Width, arrowTexture.Height);
            _physicsCollider = null;
        }

        //Methods
        public void Draw(SpriteBatch sp, Rectangle rect, Color tint)
        {
            base.Draw(sp, displayRect, arrowTexture, tint);
        }
    }
}
