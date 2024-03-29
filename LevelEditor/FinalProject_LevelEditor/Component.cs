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
        private Point adjustment;
        int index;

        private PictureBox directionIndicator;
        private BoxMovement direction;

        private Component parentEnemy;
        private List<Component> roamPoints;

        //Properties
        public TileType TileType { get => tileType; set => tileType = value; }
        internal List<Component> RoamPoints { get => roamPoints; set => roamPoints = value; }
        public BoxMovement Direction { get => direction; set => direction = value; }

        //Constructors
        public Component(Point location, TileType tileType, int width, int height, int bWidth, int bHeight, Color c, Point adjustment)
        {
            this.tileType = tileType;
            this.bHeight = bHeight;
            this.bWidth = bWidth;
            this.width = width;
            this.height = height;
            this.adjustment = adjustment;

            this.direction = BoxMovement.Up;
            this.directionIndicator = null;

            index = 1;

            roamPoints = null;
            parentEnemy = null;

            if(tileType == TileType.Enemy)
            {
                roamPoints = new List<Component>();
            }

            pBox = new PictureBox();
            pBox.Location = location;
            pBox.Width = width*bWidth;
            pBox.Height = height*bHeight;

            pBox.BackColor = Color.FromArgb(100, c.R, c.G, c.B);
            
            if(tileType == TileType.Arrow)
            {
                this.direction = BoxMovement.Up;
                this.directionIndicator = new PictureBox();
                directionIndicator.Location = new Point(pBox.Location.X, pBox.Location.Y - bHeight);
                directionIndicator.Width = width * bWidth;
                directionIndicator.Height = height * bHeight;
                directionIndicator.BackColor = Color.LightGray;
            }
        }

        public Component(Point location, TileType tileType, int width, int height, int bWidth, int bHeight, Color c, Point adjustment, Component parent) : this(location, tileType, width, height, bWidth, bHeight, c, adjustment)
        {
            parentEnemy = parent;
        }
        public Component(Point location, TileType tileType, int width, int height, int bWidth, int bHeight, Color c, Point adjustment, BoxMovement direction) : this(location, tileType, width, height, bWidth, bHeight, c, adjustment)
        {
            this.direction = direction;
            switch (direction)
            {
                case BoxMovement.Up:
                    directionIndicator.Location = new Point(pBox.Location.X, pBox.Location.Y - bHeight);
                    break;
                case BoxMovement.Down:
                    directionIndicator.Location = new Point(pBox.Location.X, pBox.Location.Y + bHeight);
                    break;
                case BoxMovement.Left:
                    directionIndicator.Location = new Point(pBox.Location.X, pBox.Location.Y - bWidth);
                    break;
                case BoxMovement.Right:
                    directionIndicator.Location = new Point(pBox.Location.X, pBox.Location.Y + bWidth);
                    break;
            }
            ResizeArrow();
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
                Color c = pBox.BackColor;
                pBox.BackColor = Color.FromArgb(255, c.R, c.G, c.B);
                if (roamPoints != null)
                {
                    foreach(Component comp in roamPoints)
                    {
                        comp.Select(true);
                    }
                }
                return this;
            }
            else
            {
                pBox.BorderStyle = BorderStyle.None;
                Color c = pBox.BackColor;
                pBox.BackColor = Color.FromArgb(100, c.R, c.G, c.B);
                if (roamPoints != null)
                {
                    foreach (Component comp in roamPoints)
                    {
                        comp.Select(false);
                    }
                }
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
                    if(directionIndicator != null)
                    {
                        directionIndicator.Location = new Point(directionIndicator.Location.X, directionIndicator.Location.Y - bHeight * mult);
                    }
                    break;
                case BoxMovement.Down:
                    pBox.Location = new Point(pBox.Location.X, pBox.Location.Y + bHeight*mult);
                    if (directionIndicator != null)
                    {
                        directionIndicator.Location = new Point(directionIndicator.Location.X, directionIndicator.Location.Y + bHeight * mult);
                    }
                    break;
                case BoxMovement.Left:
                    pBox.Location = new Point(pBox.Location.X - bWidth*mult, pBox.Location.Y);
                    if (directionIndicator != null)
                    {
                        directionIndicator.Location = new Point(directionIndicator.Location.X - bWidth * mult, directionIndicator.Location.Y);
                    }
                    break;
                case BoxMovement.Right:
                    pBox.Location = new Point(pBox.Location.X + bWidth*mult, pBox.Location.Y);
                    if (directionIndicator != null)
                    {
                        directionIndicator.Location = new Point(directionIndicator.Location.X + bWidth * mult, directionIndicator.Location.Y);
                    }
                    break;
            }
            ResizeArrow();
        }

        /// <summary>
        /// Extends the box in the given direction
        /// </summary>
        /// <param name="mv">Movement direction</param>
        /// <param name="mult">Multiplier for distance (number of points moved)</param>
        public void Extend(BoxMovement mv, int mult)
        {
            TileType t = tileType;
            if(t != TileType.Enemy
                && t != TileType.RoamPoint
                && t != TileType.GlowStick
                && t != TileType.Objective)
            switch (mv)
            {
                case BoxMovement.Down:
                    pBox.Height = pBox.Height + bHeight * mult;
                    break;
                case BoxMovement.Up:
                    if(pBox.Height > bHeight)
                        pBox.Height = pBox.Height - bHeight * mult;
                    break;
                case BoxMovement.Left:
                    if(pBox.Width > bWidth)
                        pBox.Width = pBox.Width - bWidth * mult;
                    break;
                case BoxMovement.Right:
                    pBox.Width = pBox.Width + bWidth * mult;
                    break;
            }
            ResizeArrow();
        }

        public PictureBox GetBox()
        {
            return pBox;
        }

        public override string ToString()
        {
            String name = "";
            if (tileType == TileType.RoamPoint)
            {
                return parentEnemy.ToString() + "  " + "Roam Point (" + pBox.Location.X / bWidth + ", " + pBox.Location.Y / bHeight + ")";
            }
            switch (tileType)
            {
                case TileType.Wall:
                    name += "Wall";
                    break;
                case TileType.Enemy:
                    name += "Enemy (" + pBox.Location.X / bWidth + ", " + pBox.Location.Y / bHeight + ")";
                    return name;
                    break;
                case TileType.Spawn:
                    name += "Spawn (" + pBox.Location.X / bWidth + ", " + pBox.Location.Y / bHeight + ")";
                    return name;
                    break;
                case TileType.Objective:
                    name += "Objective";
                    break;
                case TileType.Exit:
                    name += "Exit";
                    break;
                case TileType.GlowStick:
                    name += "Glow Stick";
                    break;
                case TileType.Arrow:
                    name += "Arrow";
                    break;
            }

            name += " (" + pBox.Location.X / bWidth + ", " + pBox.Location.Y / bHeight + ") {" + pBox.Width/bWidth + ", " + pBox.Height/bHeight + "}";
            return name;
        }

        public string FileIOToString()
        {
            string name = "";
            int x = (pBox.Location.X / bWidth)-adjustment.X;
            int y = (pBox.Location.Y / bHeight)-adjustment.Y;

            int w = pBox.Width / bWidth;
            int h = pBox.Height / bHeight;

            if (tileType == TileType.RoamPoint)
            {
                return "roampoint]" + x + "]" + y + "]";
            }

            switch (tileType)
            {
                case TileType.Wall:
                    name += "wall,"+x+","+y+","+w+","+h;
                    break;
                case TileType.Enemy:
                    name += "enemy," + x + "," + y + "," + 1 + "," + 1;
                    break;
                case TileType.Spawn:
                    name += "spawn," + x + "," + y + "," + 1 + "," + 1; 
                    break;
                case TileType.Objective:
                    name += "objective," + x + "," + y + "," + w + "," + h;
                    break;
                case TileType.Exit:
                    name += "exit," + x + "," + y + "," + w + "," + h;
                    break;
                case TileType.GlowStick:
                    name += "glow," + x + "," + y + "," + w + "," + h;
                    break;
                case TileType.Arrow:
                    name += "arrow," + x + "," + y + "," + w + "," + h;
                    switch (direction)
                    {
                        case BoxMovement.Up:
                            name += ",up";
                            break;
                        case BoxMovement.Down:
                            name += ",down";
                            break;
                        case BoxMovement.Left:
                            name += ",left";
                            break;
                        case BoxMovement.Right:
                            name += ",right";
                            break;
                    }
                    break;
            }
            return name;
        }

        public void AddRoamPoint(Component comp)
        {
            roamPoints.Add(comp);
        }

        public Component GetParentEnemy()
        {
            return parentEnemy;
        }

        public PictureBox GetDirectionIndicator()
        {
            return directionIndicator;
        }

        public void ReAdjust(int width, int height)
        {
            pBox.Location = new Point(pBox.Location.X/bWidth, pBox.Location.Y/bHeight);
            pBox.Width = pBox.Width / bWidth;
            pBox.Height = pBox.Height / bHeight;
            this.bWidth = width;
            this.bHeight = height;
            pBox.Width = pBox.Width * bWidth;
            pBox.Height = pBox.Height * bHeight;
            pBox.Location = new Point(pBox.Location.X * bWidth, pBox.Location.Y * bHeight);
        }

        public void ChangeScroll(int xOffset, int yOffset)
        {
            int dX = xOffset * bWidth;
            int dY = yOffset * bHeight;

            pBox.Location = new Point(pBox.Location.X + (dX - adjustment.X*bWidth), pBox.Location.Y + (dY - adjustment.Y*bHeight));

            adjustment = new Point(xOffset, yOffset);
        }

        public void RotateArrow()
        {
            if(tileType == TileType.Arrow)
            {
                switch (direction)
                {
                    case BoxMovement.Up:
                        direction = BoxMovement.Right;
                        directionIndicator.Location = new Point(pBox.Location.X + pBox.Width, pBox.Location.Y + pBox.Height / 2 - bHeight / 2);
                        break;
                    case BoxMovement.Down:
                        direction = BoxMovement.Left;
                        directionIndicator.Location = new Point(pBox.Location.X - bWidth, pBox.Location.Y + pBox.Height/2 - bHeight/2);
                        break;
                    case BoxMovement.Left:
                        direction = BoxMovement.Up;
                        directionIndicator.Location = new Point(pBox.Location.X + pBox.Width / 2 - bWidth / 2, pBox.Location.Y - bHeight);
                        break;
                    case BoxMovement.Right:
                        direction = BoxMovement.Down;
                        directionIndicator.Location = new Point(pBox.Location.X + pBox.Width / 2 - bWidth / 2, pBox.Location.Y + pBox.Height);
                        break;
                }
            }
        }

        public void ResizeArrow()
        {
            if(tileType == TileType.Arrow)
            {
                switch (direction)
                {
                    case BoxMovement.Up:
                        directionIndicator.Location = new Point(pBox.Location.X + pBox.Width / 2 - bWidth / 2, pBox.Location.Y - bHeight);
                        break;
                    case BoxMovement.Down:
                        directionIndicator.Location = new Point(pBox.Location.X + pBox.Width / 2 - bWidth / 2, pBox.Location.Y + pBox.Height);
                        break;
                    case BoxMovement.Left:
                        directionIndicator.Location = new Point(pBox.Location.X - bWidth, pBox.Location.Y + pBox.Height / 2 - bHeight / 2);
                        break;
                    case BoxMovement.Right:
                        directionIndicator.Location = new Point(pBox.Location.X + pBox.Width, pBox.Location.Y + pBox.Height / 2 - bHeight / 2);
                        break;
                }
                directionIndicator.Width = bWidth;
                directionIndicator.Height = bHeight;
            }
        }
    }
}
