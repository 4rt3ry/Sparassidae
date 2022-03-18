/*
 * Map Point Class
 * This class handles all functionality of a point on the map
 * Including writing and output
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_LevelEditor
{
    class MapPoint
    {
        //Fields
        private Point position;

        //Properties
        public Point Position { get => position; set => position = value; }

        //Constructors
        public MapPoint(Point loc)
        {
            this.position = loc;
        }

        //Methods
        /// <summary>
        /// ToString method converts point into text output for file saving
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
