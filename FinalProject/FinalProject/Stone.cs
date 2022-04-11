﻿
// Author: Arthur Powers
// Purpose: Stones being tossed can collide with walls to reveal a section of the wall and ping closeby enemies


using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Penumbra;
using Microsoft.Xna.Framework.Graphics;

namespace FinalProject
{
    class Stone : GameObject
    {

        // Fields

        // Constants
        private const float _maxThrowSpeed = 800; // Pixels per second
        private const float _drag = 600f; // Pixels per second
        private const float _collisionSpeedReduction = 0.75f; // Percent of current speed

        // Lighting
        private PointLight _pointLight;
        private TexturedLight _texturedLight;

        // Stone throw info
        private Vector2 _direction;
        private float _currentSpeed = _maxThrowSpeed; // Pixels per second
        private int _hitCount = 1;
        private float targetScale = 50;


        // Properties
        /// <summary>
        /// How many times the stone can bounce
        /// </summary>
        public int HitCount { get => _hitCount; set => _hitCount = value; }

        /// <summary>
        /// A small sphere of light that reveals the current stone.
        /// </summary>
        public PointLight Light { get => _pointLight; set => _pointLight = value; }

        /// <summary>
        /// A textured penumbra light that reveals the current stone
        /// </summary>
        public TexturedLight TLight { get => _texturedLight; set => _texturedLight = value; }

        /// <summary>
        /// The stone's position
        /// </summary>
        public new Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                _pointLight.Position = value;
                _texturedLight.Position = value;
            }
        }

        /// <summary>
        /// Creates a new stone, adding a point light to <paramref name="penumbra"/>.
        /// </summary>
        /// <param name="penumbra"></param>

        public Stone(Vector2 position) : base()
        {
            // Position information
            _position = position;
            _physicsCollider = new CircleCollider(this, new Vector2(0, 0), 10, false);

            // Lighting
            _pointLight = new PointLight
            {
                Position = _position,
                Scale = new Vector2(50),
                ShadowType = ShadowType.Solid,
                Color = new Color(0.065f, 0.065f, 0.085f),
                Intensity = 1f,
                
            };

        }

        /// <summary>
        /// Creates a new stone with a given texture
        /// </summary>
        /// <param name="position">Location of the stone</param>
        /// <param name="texture">Texture the stones light will have</param>
        public Stone(Vector2 position, Texture2D texture) : this(position)
        {
            _texturedLight = new TexturedLight
            {
                Position = _position,
                Scale = new Vector2(50),
                ShadowType = ShadowType.Solid,
                Color = Color.AntiqueWhite,
                Texture = texture,
            };
        }

        /// <summary>
        /// Update the stone's position
        /// </summary>
        /// <param name="dTime"></param>
        public void Update(float dTime)
        {
            _currentSpeed -= _drag * dTime;
            if (_currentSpeed <= 0)
            {
                _currentSpeed = 0;
                targetScale = 350f;
            }

            if (_texturedLight.Scale.X < targetScale)
            {
                _texturedLight.Scale = new Vector2(_texturedLight.Scale.X + dTime * 250);
            }
            else if(_texturedLight.Scale.X > targetScale)
            {
                _texturedLight.Scale = new Vector2(_texturedLight.Scale.X - dTime * 250);
            }

            if (_pointLight.Scale.X < targetScale)
            {
                _pointLight.Scale = new Vector2(_pointLight.Scale.X + dTime * 250);
            }
            else if (_pointLight.Scale.X > targetScale)
            {
                _pointLight.Scale = new Vector2(_pointLight.Scale.X - dTime * 250);
            }


            _velocity = _direction * _currentSpeed;
            Position += _velocity * dTime;
        }

        /// <summary>
        /// Reflects the stone's movement across a normal vector
        /// </summary>
        /// <param name="normal"></param>
        public void Bounce(Vector2 normal)
        {
            if (_hitCount < 0)
            {
                _velocity = Vector2.Zero;
                _currentSpeed = 0;
                return;
            }

            // Ensure the normal is actually a normal
            if (normal.LengthSquared() != 0) normal.Normalize();

            Vector2 reflection = _direction - 2 * Vector2.Dot(_direction, normal) * normal;
            _direction = reflection;
            _currentSpeed *= _collisionSpeedReduction;
            _velocity = reflection * _currentSpeed;
            _hitCount--;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw()
        {
            // Possibly draw the stone's texture
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
