using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Author:  Arthur Powers 3/4/2022
// Purpose: Contains only the necessary components for sub-colliders to draw from.

namespace FinalProject
{
    abstract class Collider
    {
        // Fields

        protected GameObject parent;
        protected bool isTrigger;


        // Properties

        /// <summary>
        /// The collider's center point relative to the origin (0, 0).
        /// 
        /// WARNING: Untested. Should always track parent GameObject's position.
        /// </summary>
        public Vector2 Position
        {
            get => parent.Position + RelativePosition;
            set
            {
                RelativePosition = value - parent?.Position?? Vector2.Zero;
            }
        }

        /// <summary>
        /// The collider's center point relative to its parent <see cref="GameObject"/>
        /// </summary>
        public Vector2 RelativePosition { get; private set; }

        /// <summary>
        /// Trigger colliders cannot have physics collisions.
        /// </summary>
        public bool IsTrigger => isTrigger;

        /// <summary>
        /// Specify the Collider's center point <see cref="Vector2"/> <paramref name="position"/> relative to its  
        /// <see cref="GameObject"/> parent's position. <br></br>
        /// Trigger colliders will not check for physics collisions.
        /// </summary>
        /// <param name="parent">Position is relative to its parent <see cref="GameObject"/></param>
        /// <param name="position">Center point relative to parent <see cref="GameObject"/></param>
        /// <param name="isTrigger">Trigger colliders cannot cause physics collisions</param>
        public Collider(GameObject parent, Vector2 position, bool isTrigger)
        {
            this.parent = parent;
            RelativePosition = position;
            this.isTrigger = isTrigger;
        }

        /// <summary>
        /// Specify the Collider's center point <see cref="Vector2"/> <paramref name="position"/> relative to the origin (0, 0). <br></br>
        /// Trigger colliders will not check for physics collisions.
        /// </summary>
        /// <param name="position">Center point relative to parent <see cref="GameObject"/></param>
        /// <param name="isTrigger">Trigger colliders cannot cause physics collisions</param>
        public Collider(Vector2 position, bool isTrigger) : this(null, position, isTrigger) { }

        /// <summary>
        /// Specify the Collider's center point <see cref="Vector2"/> <paramref name="position"/> relative to the origin (0, 0). <br></br>
        /// By default, this Collider is a trigger and will not check for physics collisions.
        /// </summary>
        /// <param name="position">Center point relative to parent <see cref="GameObject"/></param>
        public Collider(Vector2 position) : this(position, true) { }

        /// <summary>
        /// Determines whether or not the current collider contains the specified point. A point on the 
        /// <see cref="Collider"/>'s boundary is considered inside.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Does this collider contain <see cref="Vector2"/> <paramref name="point"/>?</returns>
        public abstract bool ContainsPoint(Vector2 point);

        /// <summary>
        /// Determines if intersecting another collider.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Does this collider intersect <see cref="Collider"/> <paramref name="other"/></returns>
        public abstract bool Intersects(Collider other);

        /// <summary>
        /// Determines if a physics collision has occured. 
        /// </summary>
        /// <returns></returns>
        public abstract bool CheckCollision(GameObject other);
    }
}
