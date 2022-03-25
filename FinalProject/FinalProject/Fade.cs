using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Penumbra;
using Microsoft.Xna.Framework.Content;


namespace FinalProject
{
    class Fade
    {
        private Texture2D fade_Texture;
        private float gameTime = 0;
        private float transparency = 1f;
        bool faded = false;
        bool finishedIn = false;
        float fadeInTime = 1;
        float fadeOutTime = 1;
        SpriteBatch batch;

        public Fade()
        {
        }

        public void LoadContent(ContentManager content)
        {
            fade_Texture = content.Load<Texture2D>("blackbox2");
        }

        public void Update(float dTime)
        {
            float scale = (1/fadeInTime);
            if (finishedIn == true)
            {
                scale = 1 / fadeOutTime;
            }
            dTime = dTime * scale;
            gameTime += dTime;
            if (transparency <= 1 && finishedIn == false )
            {
                transparency -= dTime;
                if (transparency <= 0)
                {

                    finishedIn = true;
                }
            }
            else if (finishedIn == true)
            {
                transparency += dTime;
                if (transparency >= 1)
                {
                    faded = true;
                }
            }
            System.Diagnostics.Debug.WriteLine(transparency);

        }

        public void StartFade(SpriteBatch batch, float fadeInTime, float fadeOutTime)
        {
            if (faded == true)
            {
                return;
            }

            this.fadeInTime = fadeInTime;
            this.fadeOutTime = fadeOutTime;

            batch.Draw(fade_Texture, new Rectangle(0, 0, 5000, 5000), Color.Black* transparency );

        }

    }
}
