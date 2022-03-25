
// Author:  Arthur Powers 3/4/2022
// Purpose: Allows for both physics and non-physics collision detection
//          between objects with a RectangleCollider and any other collider.
//
// Restrictions: Physics collisions might not work with line colliders
// TODO: Implement method overrides

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

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
            if (IsTrigger)
            {
                return Intersects(other.PhysicsCollider);
            }
            else
            {
                return Intersects(other.PhysicsCollider);
            }
        }

        public override bool CheckCollision(GameObject other, out ColliderHitInfo collisionInfo)
        {
            collisionInfo = new ColliderHitInfo();
            if (!Intersects(other.PhysicsCollider)) return false;

            if (other.PhysicsCollider is CircleCollider)
            {
                CircleCollider otherCircleCollider = (CircleCollider)other.PhysicsCollider;
                Vector2 previousPosition = other.Position - other.Velocity;
                Vector2 hitNormal;
                Vector2 hitPoint;
                //RectangleCollider interpolatedCollider = 

                // Check for collision on top side
                if (otherCircleCollider.Position.Y <= Position.Y - Size.Y / 2 && otherCircleCollider.Position.X >= Position.X - Size.X / 2 && otherCircleCollider.Position.X <= Position.X + Size.X / 2)
                {
                    hitPoint = new Vector2(otherCircleCollider.Position.X, Position.Y - Size.Y / 2);
                }
                // bottom side
                else if (otherCircleCollider.Position.Y >= Position.Y + Size.Y / 2 && otherCircleCollider.Position.X >= Position.X - Size.X / 2 && otherCircleCollider.Position.X <= Position.X + Size.X / 2)
                {
                    hitPoint = new Vector2(otherCircleCollider.Position.X, Position.Y + Size.Y / 2);
                }
                // left side
                else if (otherCircleCollider.Position.X <= Position.X - Size.X / 2 && otherCircleCollider.Position.Y >= Position.Y - Size.Y / 2 && otherCircleCollider.Position.Y <= Position.Y + Size.Y / 2)
                {
                    hitPoint = new Vector2(Position.X - Size.X / 2, otherCircleCollider.Position.Y);
                }
                // right side
                else if (otherCircleCollider.Position.X >= Position.X + Size.X / 2 && otherCircleCollider.Position.Y >= Position.Y - Size.Y / 2 && otherCircleCollider.Position.Y <= Position.Y + Size.Y / 2)
                {
                    hitPoint = new Vector2(Position.X + Size.X / 2, otherCircleCollider.Position.Y);
                }

                // Check if circle contains any corner points
                // Top left corner
                else if ((otherCircleCollider.Position - (Position - Size / 2)).LengthSquared() <= otherCircleCollider.Radius * otherCircleCollider.Radius)
                {
                    hitPoint = Position - Size / 2;
                }
                // top right corner
                else if ((otherCircleCollider.Position - (Position - Size / 2 + Vector2.UnitX * Size)).LengthSquared() <= otherCircleCollider.Radius * otherCircleCollider.Radius)
                {
                    hitPoint = Position - Size / 2 + Vector2.UnitX * Size;
                }
                // bottom right corner
                else if ((otherCircleCollider.Position - (Position + Size / 2)).LengthSquared() <= otherCircleCollider.Radius * otherCircleCollider.Radius)
                {
                    hitPoint = Position + Size / 2;
                }
                // bottom left corner
                else if ((otherCircleCollider.Position - (Position - Size / 2 + Vector2.UnitY * Size)).LengthSquared() <= otherCircleCollider.Radius * otherCircleCollider.Radius)
                {
                    hitPoint = Position - Size / 2 + Vector2.UnitY * Size;
                }
                else
                {
                    hitPoint = previousPosition;
                }

                hitNormal = otherCircleCollider.Position - hitPoint;
                hitNormal.Normalize();

                collisionInfo = new ColliderHitInfo(hitNormal, hitPoint);
            }
            return true;
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

                // Check if circle contains any corner points
                if ((cc.Position - Position + Size / 2).LengthSquared() <= cc.Radius * cc.Radius ||
                    (cc.Position - Position + Size / 2 - Vector2.UnitX * Size).LengthSquared() <= cc.Radius * cc.Radius ||
                    (cc.Position - Position - Size / 2).LengthSquared() <= cc.Radius * cc.Radius ||
                    (cc.Position - Position + Size / 2 - Vector2.UnitY * Size).LengthSquared() <= cc.Radius * cc.Radius)
                {
                    return true;
                }

                // Inflates left and right sides by circle's radius, checks if its centerpoint is contained
                if (cc.Position.X >= Position.X - Size.X / 2 - cc.Radius && cc.Position.X <= Position.X + Size.X / 2 + cc.Radius &&
                    cc.Position.Y >= Position.Y - Size.Y / 2 && cc.Position.Y <= Position.Y + size.Y / 2)
                {
                    return true;
                }
                // Inflates top and bottomm sides by circle's radius, checks if centerpoint is contained
                if (cc.Position.X >= Position.X - Size.X / 2 && cc.Position.X <= Position.X + Size.X / 2 &&
                    cc.Position.Y >= Position.Y - Size.Y / 2 - cc.Radius && cc.Position.Y <= Position.Y + size.Y / 2 + cc.Radius)
                {
                    return true;
                }

                return false;
            }
            if (other is LineCollider)
            {
                LineCollider lc = (LineCollider)other;
                return lc.Intersects(this);
            }

            return false;
        }

        public override void SetDebugTexture(GraphicsDevice gd, Color baseColor)
        {
            int width = (int)size.X;
            int height = (int)size.Y;

            if (width * height == 0) return;

            Texture2D texture = new Texture2D(gd, width, height);
            Color[] colorData = new Color[width * height];

            int strokeWeight = 2;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int index = j * width + i;
                    if (i <= strokeWeight || i >= width - strokeWeight || j <= strokeWeight || j >= height - strokeWeight)
                    {
                        colorData[index] = baseColor;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            debugTexture = texture;
        }

        public override void DrawDebugTexture(SpriteBatch sb, Color tint)
        {
            if (debugTexture == null) return;
            sb.Draw(debugTexture, Position - Size / 2, tint);
        }
    }
}