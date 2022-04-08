using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FinalProject
{
    /// Runi Jiang
    /// 4/1/2022
    /// Slider class
    /// To do:  Color fill
    class Slider
    {
        //Fields
        //Texture
        private Texture2D indicator;
        private Texture2D widget;
        private Texture2D icon;

        private MouseState currentMouse;
        private MouseState previousMouse;
        private Vector2 mousePosition;

        private bool isHovering; //Hovering over the indicator
        private bool isHoveringSlider; // Hovering over the entire slider;

        // Slider Position and Rectangel
        private Rectangle sliderRec;
        private Rectangle indicatorRec;
        private int x_value; // X_value of the slider relative to the center of the screen
        private int y_value; // Y_value of the slider relative to the center of the screnn

        // Slider Statistics
        private double curValue;
        private double totalValue;
        private double percentage;

        public event EventHandler Click;

        private GraphicsDeviceManager graphics;
        private Camera2D camera;

        //Properties

        public Rectangle SliderRec { get => sliderRec; set => sliderRec = value; }
        public Rectangle IndicatorRec { get => indicatorRec; set => indicatorRec = value; }
        public double CurValue { get => curValue; set => curValue = value; }
        public double TotalValue { get => totalValue; set => totalValue = value; }
        public double Percentage { get => percentage; }

        /// <summary>
        /// Construct slider having indicator and widget texture, with position and value
        /// </summary>
        /// <param name="indicator">The indicator texture</param>
        /// <param name="widget">The widget texture</param>
        /// <param name="x_value">The relative x position to the scene</param>
        /// <param name="y_value">The relative y position to the scene</param>
        /// <param name="initialValue">The initial Valuee of the SLider</param>
        /// <param name="totalValue">The total value of the slider</param>
        public Slider(Texture2D indicator, Texture2D widget, int x_value, int y_value,
            double initialValue, double totalValue, GraphicsDeviceManager graphics)
        {
            isHovering = false;
            isHoveringSlider = false;
            this.indicator = indicator;
            this.widget = widget;
            curValue = initialValue;
            this.totalValue = totalValue;
            percentage = initialValue / totalValue;
            this.graphics = graphics;
            this.x_value = x_value - graphics.PreferredBackBufferWidth/2;
            this.y_value = y_value - graphics.PreferredBackBufferHeight/2;
            sliderRec = new Rectangle(x_value, y_value, widget.Width, widget.Height);
            indicatorRec = new Rectangle((int)(percentage * widget.Width + x_value - indicator.Width/2),
                y_value + widget.Height/2 - indicator.Height/2, indicator.Width, indicator.Height);
           
        }

        /// <summary>
        /// Constructor the slider that will based on the camera
        /// </summary>
        /// <param name="indicator"></param>
        /// <param name="widget"></param>
        /// <param name="x_value"></param>
        /// <param name="y_value"></param>
        /// <param name="initialValue"></param>
        /// <param name="totalValue"></param>
        /// <param name="graphics"></param>
        /// <param name="camera"></param>
        public Slider(Texture2D indicator, Texture2D widget, int x_value, int y_value,
            double initialValue, double totalValue, GraphicsDeviceManager graphics, Camera2D camera) 
            : this(indicator, widget, x_value, y_value, initialValue, totalValue, graphics)
        {
            this.camera = camera;
        }

        public Slider(Texture2D indicator, Texture2D widget, int x_value, int y_value,
            double initialValue, double totalValue, GraphicsDeviceManager graphics, Texture2D icon)
            : this(indicator, widget, x_value, y_value, initialValue, totalValue, graphics)
        {
            this.icon = icon;
        }

        public Slider(Texture2D indicator, Texture2D widget, int x_value, int y_value,
           double initialValue, double totalValue, GraphicsDeviceManager graphics, Camera2D camera, Texture2D icon)
           : this(indicator, widget, x_value, y_value, initialValue, totalValue, graphics)
        {
            this.camera = camera;
            this.icon = icon;
        }

        /// <summary>
        /// Draw the Slider
        /// </summary>
        /// <param name="sb">SpriteBatch</param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(widget, sliderRec, Color.White);
            sb.Draw(indicator, indicatorRec, Color.White);
        }

        /// <summary>
        /// Update the Slider
        /// </summary>
        public void Update()
        {
            // Update the mouse state and hover state
            currentMouse = Mouse.GetState();
            mousePosition = new Vector2(currentMouse.X, currentMouse.Y);

            //Check if the slider needs to be centered
            if(camera != null)
            {
                System.Diagnostics.Debug.WriteLine("Slider's camera");
                mousePosition = camera.ScreenToWorldSpace(mousePosition);
            }

            if(sliderRec.Contains(mousePosition.X, mousePosition.Y))
            {
                isHoveringSlider = true;

                //Click
                if (currentMouse.LeftButton == ButtonState.Pressed &&
                   previousMouse.LeftButton == ButtonState.Released)
                {
                    indicatorRec = new Rectangle((int)mousePosition.X - indicator.Width / 2, indicatorRec.Y,
                        indicator.Width, indicator.Height);

                    // If the click event is not null, call the method
                    if (Click != null)
                    {
                        Click(this, new EventArgs());
                    }
                    isHoveringSlider = false;
                }
            }


            if (isHoveringSlider)
            {
                //Drag
                if (indicatorRec.Contains(mousePosition.X, mousePosition.Y) && currentMouse.LeftButton == ButtonState.Pressed)
                {
                    isHovering = true;
                }

                if (isHovering)
                {
                    if (currentMouse.LeftButton == ButtonState.Pressed &&
                        previousMouse.LeftButton == ButtonState.Pressed)
                    {
                        if(mousePosition.X < sliderRec.X)
                        {
                            indicatorRec = new Rectangle(sliderRec.X - indicator.Width / 2, 
                                indicatorRec.Y, indicator.Width, indicator.Height);
                        }
                        else if(mousePosition.X > sliderRec.X + sliderRec.Width)
                        {
                            indicatorRec = new Rectangle(sliderRec.X + sliderRec.Width - indicator.Width / 2,
                                indicatorRec.Y, indicator.Width, indicator.Height);
                        }
                        else
                        {
                            indicatorRec = new Rectangle((int)mousePosition.X - indicator.Width / 2, 
                                indicatorRec.Y, indicator.Width, indicator.Height);
                        }

                        // If the click event is not null, call the method
                        if (Click != null)
                        {
                            Click(this, new EventArgs());
                        }
                    }

                    if (currentMouse.LeftButton == ButtonState.Released &&
                        previousMouse.LeftButton == ButtonState.Pressed)
                    {
                        isHovering = false;
                        isHoveringSlider = false;
                    }
                }
            }

           

            percentage = (double)((indicatorRec.X + indicator.Width/2) - sliderRec.X) / sliderRec.Width;
            curValue = Math.Round(percentage * totalValue);
            
            // Set the previous mouse state
            previousMouse = currentMouse;
        }

        /// <summary>
        /// Update the slider position based on the center of the screen
        /// </summary>
        /// <param name="center"></param>
        public void UpdatePosition(Vector2 center)
        {
            this.sliderRec = new Rectangle(
                x_value + (int)center.X,  y_value + (int)center.Y,
                sliderRec.Width, sliderRec.Height);

            this.indicatorRec = new Rectangle(
                (int)(percentage * this.sliderRec.Width) + this.SliderRec.X - this.indicatorRec.Width / 2,
                this.sliderRec.Y + this.sliderRec.Height / 2 - this.indicatorRec.Height / 2,
                this.indicatorRec.Width, this.indicatorRec.Height);

            System.Diagnostics.Debug.WriteLine(sliderRec);
            System.Diagnostics.Debug.WriteLine(currentMouse.Position);
        }

    }

  
}
