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
                ReltiveEndPosition = value - parent?.Position ?? Vector2.Zero;
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
        /// <summary>
        /// Specify a <see cref="LineCollider"/>'s starting point <see cref="Vector2"/> <paramref name="position"/> and 
        /// ending point <see cref="Vector2"/>
        /// </summary>
        /// <param name="position">Starting point relative to origin (0, 0)</param>
        /// <param name="endPosition">Ending point relative to origin (0, 0)</param>
        /// <param name="isTrigger">Trigger colliders cannot cause physics collisions. For now, always default to true</param>
        public LineCollider(Vector2 position, Vector2 endPosition) : this(null, position, endPosition) { }

        public override bool CheckCollision(GameObject other)
        {
            if (IsTrigger)
            {
                return Intersects(other.PhysicsCollider);
            }
            // Currently don't need physics collisions

            return false;
        }

        public override bool ContainsPoint(Vector2 point)
        {
            // Considering AB = vector from startpoint to endpoint
            //             AP = vector from startpoint to specified point,
            //
            // Point and line segment are colinear if cross product between AB and AP == 0
            Vector2 AB = EndPosition - Position;
            Vector2 AP = point - Position;
            return AboutEquals(CrossProduct(AB, AP), 0);
        }

        public override bool Intersects(Collider other)
        {
            if (other is LineCollider)
            {
                LineCollider lc = (LineCollider)other;

                // See https://www.geeksforgeeks.org/orientation-3-ordered-points/

                // Considering A is startpoint, B is endpoint, C is other startpoint, D is other endpoint
                float orientationABC = Orientation(Position, EndPosition, lc.Position);
                float orientationABD = Orientation(Position, EndPosition, lc.EndPosition);
                float orientationCDA = Orientation(lc.Position, lc.EndPosition, Position);
                float orientationCDB = Orientation(lc.Position, lc.EndPosition, EndPosition);

                if (orientationABC !=orientationABD && orientationCDA != orientationCDB)
                {
                    return true;
                }

                // Check if any point lines on any either segment
                if (ContainsPoint(lc.Position) || ContainsPoint(lc.endPosition) || lc.ContainsPoint(Position) || lc.ContainsPoint(endPosition))
                {
                    return true;
                }
            }

            if (other is RectangleCollider)
            {
                // Converts rectangle into 4 line segments, checks if any intersect with this
                // I'm lazy so I turn them into LineColliders :)

                RectangleCollider rc = (RectangleCollider)other;
                LineCollider top = new LineCollider(rc.Position, new Vector2(rc.Position.X + rc.Size.X, rc.Position.Y));
                LineCollider right = new LineCollider(top.EndPosition, new Vector2(rc.Position.X + rc.Size.X, rc.Position.Y + rc.Size.Y));
                LineCollider bottom = new LineCollider(right.EndPosition, new Vector2(rc.Position.X, rc.Position.Y + rc.Size.Y));
                LineCollider left = new LineCollider(bottom.EndPosition, top.Position);

                if (Intersects(top) || Intersects(right) || Intersects(bottom) || Intersects(left)) return true;
            }


            // Priority since player is a circleCollider
            if (other is CircleCollider)
            {
                CircleCollider cc = (CircleCollider)other;
                return false;
            }

            return false;
        }

        /// <summary>
        /// Compares two floats for equivilence
        /// Epsilon value of 1E-6 is an arbitrary decision.
        /// </summary>
        private bool AboutEquals(float x, float y) => MathF.Abs(x - y) < MathF.Max(MathF.Abs(x), MathF.Abs(y)) * 1E-6;

        /// <summary>
        /// Gets the magnitude of the cross product between two <see cref="Vector2"/>
        /// </summary>
        private float CrossProduct(Vector2 a, Vector2 b) => a.X * b.Y - a.Y * b.X;

        /// <summary>
        /// Gets the orientation (either clockwise, counter-clockwise, or undefined) of three points.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns>1 = clockwise<br></br>2 = counter-clockwise<br></br>0 = colinear</returns>
        private int Orientation(Vector2 a, Vector2 b, Vector2 c)
        {
            // See https://www.geeksforgeeks.org/orientation-3-ordered-points/
            // Basically just compares slopes of ab and bc

            float orientation = (b.Y - a.Y) * (c.Y - b.Y) - (b.X - a.X) * (c.X - b.X);

            // clockwise if positive, counter-clockwise if negative, colinear if zero
            return orientation > 0 ? 1 : orientation < 0? 2 : 0;

        }
    }
}
