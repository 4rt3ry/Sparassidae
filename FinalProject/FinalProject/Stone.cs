
// Author: Arthur Powers
// Purpose: Stones being tossed can collide with walls to reveal a section of the wall and ping closeby enemies


using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject
{
    class Stone: GameObject
    {

        // Fields
        private const float _maxThrowSpeed = 400; // Pixels per second
        private const float _drag = 20f; // Pixels per second
        private const float _collisionSpeedReduction = 0.5f; // Percent of current speed

        private int _hitCount = 1;
        private Vector2 _direction;
        private float _currentSpeed = _maxThrowSpeed; // Pixels per second

        // Properties
        public int HitCount { get => _hitCount; set => _hitCount = value; }

        public Stone(): base()
        {
            _physicsCollider = new CircleCollider(this, new Vector2(0, 0), 10, false);
        }

        /// <summary>
        /// Update the stone's position
        /// </summary>
        /// <param name="dTime"></param>
        public void Update(float dTime)
        {
            _currentSpeed -= _drag * dTime;
            _velocity = _direction * _currentSpeed;
            _position += _velocity * dTime;
        }

        /// <summary>
        /// Reflects the stone's movement across a normal vector
        /// </summary>
        /// <param name="normal"></param>
        public void Bounce(Vector2 normal)
        {
            if (_hitCount <= 0)
            {
                _velocity = Vector2.Zero;
                _currentSpeed = 0;
                return;
            }

            // Ensure the normal is actually a normal
            if (normal.LengthSquared() != 0) normal.Normalize();

            Vector2 reflection = _direction - 2 * Vector2.Dot(_direction, normal) * normal;
            _velocity = reflection * _currentSpeed;
            _hitCount--;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>
        public void Throw(Vector2 direction)
        {
            if (direction.LengthSquared() != 0) direction.Normalize();

            _direction = direction;
            _velocity = direction * _maxThrowSpeed;
        }
    }
}
