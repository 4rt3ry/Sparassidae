/*
 * Game Object class
 * Basis for all game objects, 
 * contains necessary variables to define 
 * an object within the game
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace FinalProject
{
    class GameObject
    {
        //Fields
        protected Vector2 _position;
        protected Vector2 _velocity;
        protected Collider _physicsCollider;
        protected List<Collider> _colliders;

        //Properties

        /// <summary>
        /// The <see cref="GameObject"/>'s position
        /// </summary>
        public Vector2 Position { get => _position; set => _position = value; }

        /// <summary>
        /// The <see cref="GameObject"/>'s velocity
        /// </summary>
        public Vector2 Velocity { get => _velocity; set => _velocity = value; }

        public float X { get => _position.X; set => _position.X = value; }
        public float Y { get => _position.Y; set => _position.Y = value; }


        internal Collider PhysicsCollider { get => _physicsCollider; set => _physicsCollider = value; }
        internal List<Collider> Colliders { get => _colliders; set => _colliders = value; }

        //Constructors

        public GameObject()
        {
            Position = new Vector2(0, 0);
            Velocity = new Vector2(0, 0);
        }


        //Methods

        public virtual void Draw(SpriteBatch sp, Rectangle rect, Texture2D texture, Color tint)
        {
            sp.Draw(texture, rect, tint);
        }


    }
}