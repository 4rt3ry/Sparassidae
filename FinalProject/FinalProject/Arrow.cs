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
        private float rotation;

        //Properties


        //Constructors
        public Arrow()
        {
        }

        public Arrow(Vector2 position, Texture2D arrowTexture, String dir) : base(position)
        {
            rotation = 0;
            this.arrowTexture = arrowTexture;
            this.displayRect = new Rectangle((int)position.X, (int)position.Y, (int)(arrowTexture.Width/2.5f), (int)(arrowTexture.Height/2.5f));
            _physicsCollider = null;
            switch (dir)
            {
                case "up":
                    break;
                case "down":
                    rotation = (float)Math.PI;
                    break;
                case "left":
                    rotation = (float)((3f / 2f) * Math.PI);
                    break;
                case "right":
                    rotation = (float)(1 / 2f * Math.PI);
                    break;
            }
        }

        //Methods
        public void Draw(SpriteBatch sp, Color tint)
        {
            sp.Draw(arrowTexture, new Vector2(displayRect.X, displayRect.Y), null, tint, rotation, new Vector2(arrowTexture.Width / 2, arrowTexture.Height / 2), new Vector2(0.33f, 0.33f), SpriteEffects.None, 0f);
        }
    }
}
