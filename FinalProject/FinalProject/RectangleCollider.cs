using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

// Author:  Arthur Powers 3/4/2022
// Purpose: Allows for both physics and non-physics collision detection
//          between objects with a RectangleCollider and any other collider.
//
// Restrictions: Physics collisions might not work with line colliders
// TODO: Implement method overrides

namespace FinalProject
{
    sealed class RectangleCollider : Collider
    {
        // Fields
        private Vector2 size;

        // Properties

        /// <summary>
        /// Represents width and height of a <see cref="RectangleCollider"/>
        /// </summary>
        public Vector2 Size => size;

        /// <summary>
        /// Specify a <see cref="RectangleCollider"/>'s center point <see cref="Vector2"/> <paramref name="position"/> relative to its  
        /// <see cref="GameObject"/> parent's position. <br></br>
        /// Trigger colliders will not check for physics collisions.
        /// </summary>
        /// <param name="parent">Position is relative to its parent <see cref="GameObject"/></param>
        /// <param name="position">Center point relative to parent <see cref="GameObject"/></param>
        /// <param name="isTrigger">Trigger colliders cannot cause physics collisions</param>
        /// <param name="size">Width and Height represented by a <see cref="Vector2"/></param>
        public RectangleCollider(GameObject parent, Vector2 position, Vector2 size, bool isTrigger) : base(parent, position, isTrigger)
        {
            this.size = size;
        }

        public override bool CheckCollision(GameObject other)
        {
            // If trigger, should just call Intersects() on other's physics collider
            // (possibly its first non-physics collider if doesn't exist). Otherwise,
            // use other GameObject's velocity to detect collision
            throw new NotImplementedException();
        }

        public override bool ContainsPoint(Vector2 point) => point.X > Position.X && point.X < Position.X + size.X &&
                                                             point.Y > Position.Y && point.Y < Position.Y + size.Y;

        public override bool Intersects(Collider other)
        {
            if (other is RectangleCollider)
            {
                RectangleCollider rc = (RectangleCollider)other;

                // Return true if other rectangle is within current rectangle's bounds
                return rc.Position.X + rc.Size.X > Position.X && rc.Position.X < Position.X + Size.X &&
                       rc.Position.Y + rc.size.Y > Position.Y && rc.Position.Y < Position.Y + Size.Y;
            }
            if (other is CircleCollider)
            {
                CircleCollider cc = (CircleCollider)other;
                return false; // TODO: https://stackoverflow.com/questions/1945632/2d-ball-collisions-with-corners#1945673
            }
            if (other is LineCollider)
            {
                LineCollider lc = (LineCollider)other;
                return lc.Intersects(this);
            }

            return false;
        }
    }
}
