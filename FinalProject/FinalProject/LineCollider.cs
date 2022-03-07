using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

// Author:  Arthur Powers 3/4/2022
// Purpose: Allows for non-physics collision detection between objects with a
//          LineCollider and any other collider.
//
// Restrictions: Might have to change the structure of a collider for this to work properly
// TODO:         Implement method overrides

namespace FinalProject
{
    sealed class LineCollider : Collider
    {
        // Fields
        private Vector2 endPosition;

        // Properties

        /// <summary>
        /// The collider's center point relative to the origin (0, 0).
        /// 
        /// WARNING: Untested. Should always track parent GameObject's position.
        /// </summary>
        public Vector2 EndPosition
        {
            get => parent.Position + ReltiveEndPosition;
            set
            {
                ReltiveEndPosition = value - parent.Position;
            }
        }
        public Vector2 ReltiveEndPosition { get; private set; }

        /// <summary>
        /// Specify a <see cref="LineCollider"/>'s starting point <see cref="Vector2"/> <paramref name="position"/> and 
        /// ending point <see cref="Vector2"/> relative to its <see cref="GameObject"/> parent's position. <br></br>
        /// Trigger colliders will not check for physics collisions.
        /// </summary>
        /// <param name="parent">Starting and ending points are relative to their parent <see cref="GameObject"/></param>
        /// <param name="position">Starting point relative to parent <see cref="GameObject"/></param>
        /// <param name="endPosition">Ending point relative to parent <see cref="GameObject"/></param>
        /// <param name="isTrigger">Trigger colliders cannot cause physics collisions. For now, always default to true</param>
        public LineCollider(GameObject parent, Vector2 position, Vector2 endPosition) : base(parent, position, true)
        {
            this.endPosition = endPosition;
        }

        public override bool CheckCollision(GameObject other)
        {
            // For now, just call Intersects() with other's physics collider.
            throw new NotImplementedException();
        }

        public override bool ContainsPoint(Vector2 point)
        {
            throw new NotImplementedException();
        }

        public override bool Intersects(Collider other)
        {
            throw new NotImplementedException();
        }
    }
}
