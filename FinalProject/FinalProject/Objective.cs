using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Penumbra;
using System.Diagnostics;
namespace FinalProject
{
    class Objective
    {

        private Vector2 position;
        private PointLight _pointLight;
        private float timeHeld = 0f;
        private float maxHold = 1f;
        public PointLight PointLight { get => _pointLight; set => _pointLight = value; }

        Player player;

        public Objective(Vector2 position,Player player)
        {
            System.Diagnostics.Debug.WriteLine("new objective " + position);
            this.position = position;
            this.player = player;
            _pointLight = new PointLight
            {
                Position = position,
                Scale = new Vector2(100),
                ShadowType = ShadowType.Solid,
                Color = new Color(0f, 0f, .75f),
                Intensity = .5f,
            };

        }

        public bool CheckWin(float dt)
        {
            KeyboardState kb = Keyboard.GetState();
            if (Vector2.Distance(position,player.Position) <= 20f)
            {
                if (kb.IsKeyDown(Keys.E))
                {
                    System.Diagnostics.Debug.WriteLine("time hold " + timeHeld);

                    timeHeld += dt;
                } else
                {
                    timeHeld = 0f;
                }
                if (timeHeld >= maxHold)
                {
                    timeHeld = 0f;
                    return true;

                }
            }
            return false;
        }

    }
}
