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

        private MouseState currentMouse;
        private MouseState previousMouse;

        private bool isHovering; //Hovering over the indicator
        private Rectangle sliderRec;
        private Rectangle indicatorRec;

        private double curValue;
        private double totalValue;
        private double percentage;

        public event EventHandler Click;

        //Properties

        public Rectangle SliderRec { get => sliderRec; set => sliderRec = value; }
        public Rectangle IndicatorRec { get => indicatorRec; set => indicatorRec = value; }
        public double CurValue { get => curValue; set => curValue = value; }
        public double TotalValue { get => totalValue; set => totalValue = value; }
        public double Percentage { get => percentage; }

        /// <summary>
        /// Construct slider having indicator and widget texture, with position and value
        /// </summary>
        /// <param name="indicator"></param>
        /// <param name="widget"></param>
        /// <param name="x_value"></param>
        /// <param name="y_value"></param>
        /// <param name="initialValue"></param>
        /// <param name="totalValue"></param>
        public Slider(Texture2D indicator, Texture2D widget, int x_value, int y_value,
            double initialValue, double totalValue)
        {
            this.indicator = indicator;
            this.widget = widget;
            curValue = initialValue;
            this.totalValue = totalValue;
            percentage = initialValue / totalValue;
            sliderRec = new Rectangle(x_value, y_value, widget.Width, widget.Height);
            indicatorRec = new Rectangle((int)(percentage * widget.Width + x_value - indicator.Width/2),
                y_value + widget.Height/2 - indicator.Height/2, indicator.Width, indicator.Height);
           
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

            if(sliderRec.Contains(currentMouse.X, currentMouse.Y))
            {
                //Click
                if (currentMouse.LeftButton == ButtonState.Pressed &&
                   previousMouse.LeftButton == ButtonState.Released)
                {
                    indicatorRec = new Rectangle(currentMouse.X - indicator.Width/2, indicatorRec.Y, indicator.Width, indicator.Height);
                 
                    // If the click event is not null, call the method
                    if (Click != null)
                    {
                        Click(this, new EventArgs());
                    }
                }

                //Drag
                if(indicatorRec.Contains(currentMouse.X, currentMouse.Y))
                {
                    isHovering = true;
                }

                if(isHovering)
                {
                    if (currentMouse.LeftButton == ButtonState.Pressed &&
                        previousMouse.LeftButton == ButtonState.Pressed)
                    {
                        indicatorRec = new Rectangle(currentMouse.X - indicator.Width / 2, indicatorRec.Y, indicator.Width, indicator.Height);

                        // If the click event is not null, call the method
                        if (Click != null)
                        {
                            Click(this, new EventArgs());
                        }
                    }

                    if(currentMouse.LeftButton == ButtonState.Released &&
                        previousMouse.LeftButton == ButtonState.Pressed)
                    {
                        isHovering = false;
                    }
                }
            }

            percentage = (double)((indicatorRec.X + indicator.Width/2) - sliderRec.X) / sliderRec.Width;
            curValue = Math.Round(percentage * totalValue);
            
            // Set the previous mouse state
            previousMouse = currentMouse;
        }
    }
}
