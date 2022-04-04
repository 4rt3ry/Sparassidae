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
    /// To do: Set up method and construction
    ///         Create event
    class Slider
    {
        //Fields
        //Texture
        private Texture2D indicator;
        private Texture2D widget;

        private MouseState currentMouse;
        private MouseState previousMouse;

        private bool isHovering;
        private Rectangle sliderRec;
        private Rectangle indicatorRec;

        private double curValue;
        private double totalValue;
        private double percentage;

        //Properties

        public Rectangle SliderRec { get => sliderRec; set => sliderRec = value; }
        public Rectangle IndicatorRec { get => indicatorRec; set => indicatorRec = value; }
        public double CurValue { get => curValue; set => curValue = value; }
        public double TotalValue { get => totalValue; set => totalValue = value; }
        public double Percentage { get => percentage; }

        public Slider(Texture2D indicator, Texture2D widget, int x_value, int y_value,
            double initialValue, double totalValue)
        {
            this.indicator = indicator;
            this.widget = widget;
            sliderRec = new Rectangle(x_value, y_value, widget.Width, widget.Height);
            indicatorRec = new Rectangle((int)(initialValue / totalValue * x_value), y_value, indicator.Width, indicator.Height);
            curValue = initialValue;
            this.totalValue = totalValue;
            percentage = initialValue / totalValue;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(widget, sliderRec, Color.White);
            sb.Draw(indicator, indicatorRec, Color.White);
        }

        public void Update()
        {
            // Update the mouse state and hover state
            currentMouse = Mouse.GetState();

            if(sliderRec.Contains(currentMouse.X, currentMouse.Y))
            {
                //Click
                if (currentMouse.LeftButton == ButtonState.Released &&
                   previousMouse.LeftButton == ButtonState.Pressed)
                {
                    indicatorRec = new Rectangle(currentMouse.X, indicatorRec.Y, indicator.Width, indicator.Height);
                }

                //Drag
            }


            // Set the previous mouse state
            previousMouse = currentMouse;
        }
    }
}
