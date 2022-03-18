
// Author:  Arthur Powers 3/4/2022
// Purpose: Allows for non-physics collision detection between objects with a
//          LineCollider and any other collider.
//
// Restrictions: Might have to change the structure of a collider for this to work properly
// TODO:         Implement method overrides

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FinalProject
{
    sealed class LineCollider : Collider
    { 

        // Properties

        /// <summary>
        /// The collider's center point relative to the origin (0, 0).
        /// 
        /// WARNING: Untested. Should always track parent GameObject's position.
        /// </summary>
        public Vector2 EndPosition
        {
            get => (parent?.Position ?? Vector2.Zero) + RelativeEndPosition;
            set
            {
                RelativeEndPosition = value - (parent?.Position ?? Vector2.Zero);
            }
        }
        public Vector2 RelativeEndPosition { get; private set; }

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
            RelativeEndPosition = endPosition;
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
            else
            {
                // Eventually, this should use other gameobject's velocity to determine if collisions occur
                return Intersects(other.PhysicsCollider);
            }

            return false;
        }

        public override bool CheckCollision(GameObject other, out ColliderHitInfo collisionInfo)
        {
            // Set collision information
            Vector2 collisionNormal = new Vector2();
            Vector2 hitPoint = new Vector2();

            collisionInfo = new ColliderHitInfo(collisionNormal, hitPoint);

            if (IsTrigger)
            {
                return Intersects(other.PhysicsCollider);
            }
            else
            {
                // Eventually, this should use other gameobject's velocity to determine if collisions occur
                return Intersects(other.PhysicsCollider);
            }

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
                if (ContainsPoint(lc.Position) || ContainsPoint(lc.EndPosition) || lc.ContainsPoint(Position) || lc.ContainsPoint(EndPosition))
                {
                    return true;
                }
            }

            if (other is RectangleCollider)
            {
                // Converts rectangle into 4 line segments, checks if any intersect with this
                // I'm lazy so I turn them into LineColliders :)
                // Also remember that the rectangle's position is its center

                RectangleCollider rc = (RectangleCollider)other;
                LineCollider top = new LineCollider(rc.Position - rc.Size / 2, new Vector2(rc.Position.X + rc.Size.X / 2, rc.Position.Y - rc.Size.Y / 2));
                LineCollider right = new LineCollider(top.EndPosition, new Vector2(rc.Position.X + rc.Size.X / 2, rc.Position.Y + rc.Size.Y / 2));
                LineCollider bottom = new LineCollider(right.EndPosition, new Vector2(rc.Position.X - rc.Size.X / 2, rc.Position.Y + rc.Size.Y / 2));
                LineCollider left = new LineCollider(bottom.EndPosition, top.Position);

                if (Intersects(top) || Intersects(right) || Intersects(bottom) || Intersects(left)) return true;
            }


            // Priority since player is a circleCollider
            if (other is CircleCollider)
            {
                CircleCollider cc = (CircleCollider)other;

                // Either of two endpoints lies in circle
                if (cc.ContainsPoint(Position) || cc.ContainsPoint(EndPosition)) return true;

                // Basically inflate line segment into a rectangle who's width = radius^2 and height = line length
                // and checks if the center of the circle is inside this rectangle
                // 
                Vector2 perpDirection = new Vector2(Position.Y - EndPosition.Y, EndPosition.X - Position.X);
                perpDirection.Normalize();

                // 4 rectangle corners
                Vector2 p1 = Position + perpDirection * cc.Radius;
                Vector2 p2 = Position - perpDirection * cc.Radius;
                Vector2 p3 = EndPosition - perpDirection * cc.Radius;
                Vector2 p4 = EndPosition + perpDirection * cc.Radius;

                if (SignedTriangleArea(cc.Position, p1, p2) > 0 && SignedTriangleArea(cc.Position, p2, p3) > 0 && 
                    SignedTriangleArea(cc.Position, p3, p4) > 0 && SignedTriangleArea(cc.Position, p4, p1) > 0)
                {
                    return true;
                }
                

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

        private float SignedTriangleArea(Vector2 a, Vector2 b, Vector2 c) => (a.X - c.X) * (b.Y - c.Y) - (b.X - c.X) * (a.Y - c.Y);

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

            float orientation = (b.Y - a.Y) * (c.X - b.X) - (b.X - a.X) * (c.Y - b.Y);

            // clockwise if positive, counter-clockwise if negative, colinear if zero
            return orientation > 0 ? 1 : orientation < 0? 2 : 0;

        }

        public override void SetDebugTexture(GraphicsDevice gd, Color baseColor)
        {
            int length = (int)(EndPosition - Position).Length();
            int strokeWeight = 2;

            if (length == 0) return;

            Texture2D texture = new Texture2D(gd, length, strokeWeight);
            Color[] colorData = new Color[length * strokeWeight];

            for(int i = 0; i < colorData.Length;i++)
            {
                colorData[i] = baseColor;
            }

            texture.SetData(colorData);
            debugTexture = texture;
        }

        public override void DrawDebugTexture(SpriteBatch sb, Color tint)
        {
            if (debugTexture == null) return;
            sb.Draw(debugTexture,
                    Position,
                    null,
                    tint,
                    MathF.Atan2(EndPosition.Y - Position.Y, EndPosition.X - Position.X),
                    Vector2.Zero, 1,
                    SpriteEffects.None,
                    0);
        }
    }
}
