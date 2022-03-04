using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Author: Arthur Powers 3/4/2022
// Purpose: Contains only the necessary components for sub-colliders to draw from.

namespace FinalProject
{
    abstract class Collider
    {
        // Fields

        private GameObject parent;
        private Vector2 position;
        private Vector2 relativePosition;
        private bool isTrigger;


        // Properties

        /// <summary>
        /// The collider's absolute position (relative to its center point).
        /// 
        /// WARNING: INCOMPLETE
        /// TODO: get => parent.position + RelativePosition
        ///       set 
        ///       {
        ///           this.position = value;
        ///           this.relativePosition = value - parent.Position
        ///       }
        /// </summary>
        public Vector2 Position
        {
            get => Vector2.Zero;
            set
            {
                this.position = Vector2.Zero;
            }
        }

        public Vector2 RelativePosition => relativePosition;

        /// <summary>
        /// Trigger colliders cannot have physics collisions.
        /// </summary>
        public bool IsTrigger => isTrigger;

        /// <summary>
        /// Creates a collider relative to its parent GameObject.
        /// </summary>
        /// <param name="position">Center point relative to parent GameObject</param>
        /// <param name="isTrigger"></param>
        public Collider(GameObject parent, Vector2 position, bool isTrigger)
        {
            this.relativePosition = position;
            this.position = Vector2.Zero; // TODO: Position = parent.Position + position;
            this.isTrigger = isTrigger;
        }

        /// <summary>
        /// Creates a collider with a center point <paramref name="position"/> relative to point (0, 0).
        /// </summary>
        /// <param name="position"></param>
        /// <param name="isTrigger"></param>
        public Collider(Vector2 position, bool isTrigger) : this(new GameObject(), position, isTrigger) { }

        /// <summary>
        /// Creates a trigger collider with a center point <paramref name="position"/> relative to point (0, 0).
        /// </summary>
        /// <param name="position"></param>
        public Collider(Vector2 position) : this(new GameObject(), position, true) { }

        /// <summary>
        /// Determines whether or not the current collider contains the specified point. A point on the 
        /// outside boundary is considered inside.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Does this collider contain <paramref name="point"/></returns>
        public abstract bool ContainsPoint(Vector2 point);

        /// <summary>
        /// Determines if intersecting another collider.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Does this collider intersect <paramref name="other"/></returns>
        public abstract bool Intersects(Collider other);

        /// <summary>
        /// Determines if a physics collision has occured. 
        /// </summary>
        /// <returns></returns>
        public abstract bool CheckCollision(GameObject other);


    }
}
