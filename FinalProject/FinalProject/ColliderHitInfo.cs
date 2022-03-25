
// Author: Arthur Powers 3/18/2022
// Purpose: Contains information about a collision


using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace FinalProject
{
    struct ColliderHitInfo
    {
        /// <summary>
        /// The collision's normal represented as a unit vector
        /// </summary>
        public Vector2 Normal { get; }

        /// <summary>
        /// The collision's hit point
        /// </summary>
        public Vector2 HitPoint { get; }

        /// <summary>
        /// Contains information about a collision
        /// </summary>
        /// <param name="normal"></param>
        public ColliderHitInfo(Vector2 normal, Vector2 hitPoint)
        {
            Normal = normal;
            HitPoint = hitPoint;
        }
    }
}