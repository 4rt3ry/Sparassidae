using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

// Author:  Arthur Powers 3/4/2022
// Purpose: Allows for both physics and non-physics collision detection
//          between objects with a CircleCollider and any other collider.
//
// Restrictions: Physics collisions might not work with line colliders.
// TODO: Implement method overrides

namespace FinalProject
{
    sealed class CircleCollider : Collider
    {

        // Fields
        private float radius;

        // Properties

        /// <summary>
        /// Specify a <see cref="CircleCollider"/>'s center point <see cref="Vector2"/> <paramref name="position"/> relative to its  
        /// <see cref="GameObject"/> parent's position. <br></br>
        /// Trigger colliders will not check for physics collisions.
        /// </summary>
        /// <param name="parent">Position is relative to its parent <see cref="GameObject"/></param>
        /// <param name="position">Center point relative to parent <see cref="GameObject"/></param>
        /// <param name="isTrigger">Trigger colliders cannot cause physics collisions</param>
        /// <param name="size"><see cref="CircleCollider"/>'s radius</param>
        public CircleCollider(GameObject parent, Vector2 position, float radius, bool isTrigger) : base(parent, position, isTrigger)
        {
            this.radius = radius;
        }


        // Methods

        public override bool CheckCollision(GameObject other)
        {
            // If trigger, should just call Intersects() on other's physics collider
            // (possibly its first non-physics collider if doesn't exist). Otherwise,
            // use other GameObject's velocity to detect collision
            throw new NotImplementedException();
        }

        public override bool ContainsPoint(Vector2 point) => MathF.Pow(point.X - Position.X, 2) + MathF.Pow(point.Y - Position.Y, 2) <= radius * radius;

        public override bool Intersects(Collider other)
        {
            if (other is CircleCollider)
            {
                CircleCollider cc = (CircleCollider)other;

                // Check if distance is less than radius1 + radius2 without sqrt()
                return MathF.Pow(cc.Position.X - Position.X, 2) + MathF.Pow(cc.Position.Y - Position.Y, 2) <= MathF.Pow(cc.radius + radius, 2);
                
            }
            if (other is RectangleCollider)
            {
                RectangleCollider rc = (RectangleCollider)other;
                return false; // TODO: https://stackoverflow.com/questions/1945632/2d-ball-collisions-with-corners#1945673
            }
            if (other is LineCollider)
            {
                LineCollider lc = (LineCollider)other;
                return false;
            }

            return false;
        }
    }
}
