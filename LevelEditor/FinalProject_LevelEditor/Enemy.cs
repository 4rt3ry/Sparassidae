/*
 * Enemy Class
 * This class holds all variables needed for an enemy 
 * And can output all data that must be saved for that enemy
 * The main level editor will create enemies
 * And have the option to add positions to the enemy
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_LevelEditor
{
    class Enemy : Component
    {
        //Fields
        private List<MapPoint> locations;
        private MapPoint position;

        //Properties


        //Constructors
        public Enemy()
        {
            locations = new List<MapPoint>();
        }

        public Enemy(MapPoint pos)
        {
            locations = new List<MapPoint>();
            this.position = pos;
        }

        //Methods
        /// <summary>
        /// Adds a given point to the array of roaming locations for an enemy
        /// </summary>
        /// <param name="p">Point to add to roaming locations</param>
        public void Add(MapPoint p)
        {
            locations.Add(p);
        }

        /// <summary>
        /// Removes a given point P from the list of locations
        /// </summary>
        /// <param name="p">Point P to be removed</param>
        public void Remove(MapPoint p)
        {
            if (locations.Contains(p))
            {
                locations.Remove(p);
            }
        }
    }
}
