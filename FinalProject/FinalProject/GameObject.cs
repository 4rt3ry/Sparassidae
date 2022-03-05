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
        Vector2 position;
        Collider physicsCollider;
        List<Collider> Colliders;

        //Properties
        public Vector2 Position { get => position; set => position = value; }
        internal Collider PhysicsCollider { get => physicsCollider; set => physicsCollider = value; }
        internal List<Collider> Colliders1 { get => Colliders; set => Colliders = value; }

        //Constructors

        public GameObject()
        {
            Position = new Vector2(0, 0);
        }


        //Methods

        public void Draw(SpriteBatch sp,Color tint)
        {
            
        }


    }
}
