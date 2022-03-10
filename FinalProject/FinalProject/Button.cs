// Runi Jiang
// 3/7/2022
// Puporse: 
// TODO: Add events

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace FinalProject
{
    class Button
    {
        //Field
        private MouseState currentMouse;
        private MouseState previousMouse;

        private bool isHovering;
        private Rectangle buttonRectangle;

        private Texture2D buttonNorm;
        private Texture2D buttonHover;

        //Properties
        public event EventHandler Click;
        public bool Clicked;
        public Rectangle ButtonRectangle
        {
            get { return buttonRectangle; }
            set { buttonRectangle = value; }
        }


        //Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buttonNorm"></param>
        /// <param name="buttonHover"></param>
        /// <param name="x_value"></param>
        /// <param name="y_value"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Button(Texture2D buttonNorm, Texture2D buttonHover, 
            int x_value, int y_value)
        {
            this.buttonNorm = buttonNorm;
            this.buttonHover = buttonHover;
            buttonRectangle = new Rectangle(x_value, y_value, buttonNorm.Width, buttonNorm.Height);
        }

        //Methods

        /// <summary>
        /// Draw the button based on the hovereing status
        /// </summary>
        /// <param name="sb"></param>
        public void Draw( SpriteBatch sb)
        {
            if(isHovering)
            {
                sb.Draw(buttonHover, buttonRectangle, Color.White);
            }
            else
            {
                sb.Draw(buttonNorm, buttonRectangle, Color.White);
            }

        }

        /// <summary>
        /// Check for mouse hover and click event
        /// </summary>
        public void Update()
        {
            currentMouse = Mouse.GetState();
            isHovering = false;

            if(buttonRectangle.Contains(currentMouse.X, currentMouse.Y))
            {
                isHovering = true;

                if(currentMouse.LeftButton == ButtonState.Released &&
                    previousMouse.LeftButton == ButtonState.Pressed)
                {
                    if(Click != null)
                    {
                        Click(this, new EventArgs());
                    }
                }
            }


            previousMouse = currentMouse;
        }

    }
}
