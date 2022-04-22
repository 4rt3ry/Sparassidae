
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
    class Glowstick : GameObject
    {

        //// Fields
        //private bool isInvestigated; // This is bool value used by enemy to record if this stone is investigated or not

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
        private float targetScale = 100;
        private bool landed;


        // Properties
        /// <summary>
        /// How many times the stone can bounce
        /// </summary>
        public int HitCount { get => _hitCount; set => _hitCount = value; }
        
        /// <summary>
        /// The target scale of the light of the stone
        /// </summary>
        public float TargetScale { get => targetScale; set => targetScale = value; }

        /// <summary>
        /// A small sphere of light that reveals the current stone.
        /// </summary>
        public PointLight Light { get => _pointLight; set => _pointLight = value; }

        /// <summary>
        /// A textured penumbra light that reveals the current stone
        /// </summary>
        public TexturedLight TLight { get => _texturedLight; set => _texturedLight = value; }

        /// <summary>
        /// Has this stone been investigated by an enemy
        /// </summary>
        //public bool IsInvestigated { get => isInvestigated; set => isInvestigated = value; }

        /// <summary>
        /// Has this stone landed, or is it still in motion
        /// </summary>
        public bool Landed { get => landed; set => landed = value; }


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

        public Glowstick(Vector2 position) : base(position)
        {
            // Position information
            _physicsCollider = new CircleCollider(this, new Vector2(0, 0), 10, false);

            // Lighting
            _pointLight = new PointLight
            {
                Position = _position,
                Scale = new Vector2(10),
                ShadowType = ShadowType.Solid,
                Color = new Color(0.35f, 0.42f, 0.35f),
                Intensity = 0.7f,
            };

            //IsInvestigated = false;
        }

        /// <summary>
        /// Creates a new stone with a given texture
        /// </summary>
        /// <param name="position">Location of the stone</param>
        /// <param name="texture">Texture the stones light will have</param>
        public Glowstick(Vector2 position, Texture2D texture) : this(position)
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

            // Drag
            _currentSpeed -= _drag * dTime;
            if (_currentSpeed <= 0)
            {
                _currentSpeed = 0;
                if(!landed)
                    targetScale = 600f;
                landed = true;
            }

            // Scales up light after thrown
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
        public void Throw(Vector2 target)
        {
            Vector2 trajectory = target - _position;
            Vector2 direction = trajectory;
            float throwDistance = MathF.Sqrt(trajectory.X * trajectory.X + trajectory.Y * trajectory.Y);
            float initialSpeed = MathF.Sqrt(2 * throwDistance * _drag);

            if (direction.LengthSquared() != 0) direction.Normalize();

            _direction = direction;
            _currentSpeed = initialSpeed;
            _velocity = direction * _currentSpeed;
        }
    }
}
