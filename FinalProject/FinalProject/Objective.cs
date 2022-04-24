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
        private float fadeTime = 0f;
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
                Color = new Color(0.15f, 0.15f, .75f),
                Intensity = .9f,
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

        public void CloseLight(float dt)
        {
            if (fadeTime > 1)
            {
                fadeTime = 1;
                return;
            }
            fadeTime += dt;
            _pointLight.Scale = new Vector2(100 * ((1-fadeTime) / 1));
        }

    }
}
