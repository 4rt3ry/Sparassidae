﻿/*
 * Component class
 * Represents a placeable component within the level
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject_LevelEditor
{
    public enum BoxMovement
    {
        Up,
        Down,
        Left,
        Right
    }
    class Component
    {
        //Fields
        private TileType tileType;
        private int bWidth;
        private int bHeight;
        private int width;
        private int height;
        private PictureBox pBox;

        //Constructors
        public Component(Point location, TileType tileType, int width, int height, int bWidth, int bHeight, Color c)
        {
            this.tileType = tileType;
            this.bHeight = bHeight;
            this.bWidth = bWidth;
            this.width = width;
            this.height = height;

            pBox = new PictureBox();

            pBox.Location = location;
            pBox.Width = width*bWidth;
            pBox.Height = height*bHeight;

            pBox.BackColor = Color.FromArgb(100, c.R, c.G, c.B);

            
        }

        public Component(Point location, TileType tileType, List<PictureBox> boxes)
        {

        }

        //Methods
        /// <summary>
        /// Selects/Deselects this componenet, setting its active visual display
        /// </summary>
        /// <param name="setActive">True if active, false if not</param>
        /// <returns>This object or null depending on active state</returns>
        public Component Select(bool setActive)
        {
            if(setActive)
            {
                pBox.BorderStyle = BorderStyle.FixedSingle;
                return this;
            }
            else
            {
                pBox.BorderStyle = BorderStyle.None;
                return null;
            }
        }

        /// <summary>
        /// Moves the given componenet by an amount in a direction
        /// </summary>
        /// <param name="mv">Movement direction</param>
        /// <param name="mult">Multiplier for distance (number of points moved)</param>
        public void Move(BoxMovement mv, int mult)
        {
            switch (mv)
            {
                case BoxMovement.Up:
                    pBox.Location = new Point(pBox.Location.X, pBox.Location.Y - bHeight*mult);
                    break;
                case BoxMovement.Down:
                    pBox.Location = new Point(pBox.Location.X, pBox.Location.Y + bHeight*mult);
                    break;
                case BoxMovement.Left:
                    pBox.Location = new Point(pBox.Location.X - bWidth*mult, pBox.Location.Y);
                    break;
                case BoxMovement.Right:
                    pBox.Location = new Point(pBox.Location.X + bWidth*mult, pBox.Location.Y);
                    break;
            }
        }

        public PictureBox GetBox()
        {
            return pBox;
        }
    }
}
