﻿// Runi Jiang
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
        private Vector2 mousePosition;

        private bool isHovering;
        private bool wasHovering;
        private Rectangle buttonRectangle;
        private int x_value;
        private int y_value;

        private GraphicsDeviceManager graphics;
        //private Camera2D camera;

        private Texture2D buttonNorm;
        private Texture2D buttonHover;

        //Properties
        public event EventHandler Click;
        //public bool Clicked;
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
            int x_value, int y_value, GraphicsDeviceManager graphics)
        {
            this.buttonNorm = buttonNorm;
            this.buttonHover = buttonHover;
            buttonRectangle = new Rectangle(x_value, y_value, buttonNorm.Width, buttonNorm.Height);
            this.graphics = graphics;
            this.x_value = x_value - graphics.PreferredBackBufferWidth / 2;
            this.y_value = y_value - graphics.PreferredBackBufferHeight / 2;
        }

        /// <summary>
        /// Constructor for the button that needs to be centered
        /// </summary>
        /// <param name="buttonNorm">normal texture</param>
        /// <param name="buttonHover">texture when mouse is hovering over button</param>
        /// <param name="x_value">the x value of the position</param>
        /// <param name="y_value">the y value of the position</param>
        /// <param name="graphics">Graphics Device Manager</param>
        /// <param name="camera">Camera</param>
        public Button(Texture2D buttonNorm, Texture2D buttonHover,
          int x_value, int y_value, GraphicsDeviceManager graphics, Camera2D camera)
            : this(buttonNorm, buttonHover, x_value, y_value, graphics)
        {
            //this.camera = camera;
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
            mousePosition = new Vector2(currentMouse.X, currentMouse.Y);

            isHovering = false;

            //if (camera != null)
            //{
            //    mousePosition = camera.ScreenToWorldSpace(mousePosition);
            //}

            // Check if the mouse is in/ovre the button
            if (buttonRectangle.Contains(mousePosition.X, mousePosition.Y))
            {
                if (!wasHovering)
                {
                    SFXManager.PlaySound(Sounds.BHover);
                }
                isHovering = true;
                wasHovering = true;

                // If mouse is over the button, check if the player click the button
                if (currentMouse.LeftButton == ButtonState.Released &&
                    previousMouse.LeftButton == ButtonState.Pressed)
                {
                    // If the click event is not null, call the method
                    if (Click != null)
                    {
                        Click(this, new EventArgs());
                    }
                }
            }
            else
            {
                if (wasHovering)
                {
                    SFXManager.PlaySound(Sounds.BHEnd);
                    wasHovering = false;
                }
            }

            // Set the previous mouse state
            previousMouse = currentMouse;
        }

        /// <summary>
        /// Update the position based on the center of the camera
        /// </summary>
        /// <param name="center"></param>
        //public void UpdatePosition(Vector2 center)
        //{
        //    buttonRectangle = new Rectangle(
        //        (int)center.X + x_value, (int)center.Y + y_value,
        //        buttonRectangle.Width, buttonRectangle.Height);
        //}
    }
}
