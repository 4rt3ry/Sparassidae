// Runi Jiang
// 3/7/2022
// Puporse: Button class can hold two textures and link to method

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
        /// Construct a button
        /// </summary>
        /// <param name="buttonNorm">normal texture</param>
        /// <param name="buttonHover">texture when mouse is hovering over button</param>
        /// <param name="x_value">the x value of the position</param>
        /// <param name="y_value">the y value of the position</param>
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
            //Draw the hovering texture when the mouse is hovring on the button
            if(isHovering)
            {
                sb.Draw(buttonHover, buttonRectangle, Color.White);
            }
            else // Draw normal texture
            {
                sb.Draw(buttonNorm, buttonRectangle, Color.White);
            }

        }

        /// <summary>
        /// Check for mouse hover and click event
        /// </summary>
        public void Update()
        {
            // Update the mouse state and hover state
            currentMouse = Mouse.GetState();
            isHovering = false;

            // Check if the mouse is in/ovre the button
            if(buttonRectangle.Contains(currentMouse.X, currentMouse.Y))
            {
                isHovering = true;

                // If mouse is over the button, check if the player click the button
                if(currentMouse.LeftButton == ButtonState.Released &&
                    previousMouse.LeftButton == ButtonState.Pressed)
                {
                    // If the click event is not null, call the method
                    if(Click != null)
                    {
                        Click(this, new EventArgs());
                    }
                }
            }

            // Set the previous mouse state
            previousMouse = currentMouse;
        }

    }
}
