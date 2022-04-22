using System;
using System.Collections.Generic;
using System.Text;
using Penumbra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FinalProject
{
    class GlowstickPickup: GameObject
    {
        private PointLight _pointLight;
        private Texture2D _texure;
        private const int _numGlowsticks = 3;


        /// <summary>
        /// Number of glowsticks this pickup contains
        /// </summary>
        public int NumGlowsticks => _numGlowsticks;

        /// <summary>
        /// The glowstick pickup's point light
        /// </summary>
        public PointLight PointLight => _pointLight;

        /// <summary>
        /// The <see cref="PenumbraComponent"/> that contains the glowstick pickup's point light
        /// </summary>
        public PenumbraComponent Penumbra { get; set; }

        /// <summary>
        /// Creates a new glowstick pickup at <paramref name="position"/>
        /// </summary>
        /// <param name="position"></param>
        /// <param name="penumbra"></param>
        public GlowstickPickup(Vector2 position, PenumbraComponent penumbra, Texture2D texture) : base(position)
        {
            _texure = texture;
            Penumbra = penumbra;
            _pointLight = new PointLight
            {
                Position = _position,
                Scale = new Vector2(160),
                Intensity =  1.2f,
                Color = new Color(0.35f, 0.62f, 0.35f),
            };
            PhysicsCollider = new CircleCollider(this, Vector2.Zero, 25, true);
        }

        /// <summary>
        /// Draws the glowstick pickup sprite
        /// </summary>
        /// <param name="batch"></param>
        public void Draw(SpriteBatch batch)
        {
            if (_texure != null)
            {
                batch.Draw(_texure, new Rectangle((int)_position.X - 25, (int)_position.Y - 25, 50, 50), Color.White);
            }
        }
    }
}
