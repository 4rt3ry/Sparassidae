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
        bool faded2 = false;
        float fadeInTime = 1;
        float fadeOutTime = 1;
        GameState prevState;
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
            if (faded == true)
            {
                transparency = 0;
                return;
            }
            float scale = (1/fadeInTime);
            if (finishedIn == true)
            {
                scale = 1 / fadeOutTime;
            }
            dTime = dTime * scale;
            gameTime += dTime;
            if (transparency <= 1f && finishedIn == false )
            {
                transparency -= dTime;
                if (transparency <= 0)
                {
                    transparency = 0;
                    finishedIn = true;
                    //faded = true;
                }
            }
            else if (finishedIn == true && faded2 == false)
            {

                transparency += dTime;
                if (transparency >= 1f)
                {
                    faded2 = true;
                }
            }
            if (transparency >= 0 && faded2 == true)
            {
                transparency -= dTime;
                if (transparency <= 0 )
                {
                    faded = true;
                }
            }

        }

        public void StartFade(SpriteBatch batch, float fadeInTime, float fadeOutTime,GameState currentState)
        {
            //System.Diagnostics.Debug.WriteLine(currentState.ToString() + faded);
            if (currentState != prevState && faded == true) {

                faded = false;
                finishedIn = false;
                prevState = currentState;
                transparency = 1f;
            }
            if (faded == true)
            {
                return;
            }
            prevState = currentState;
            this.fadeInTime = fadeInTime;
            this.fadeOutTime = fadeOutTime;

            batch.Draw(texture: fade_Texture, destinationRectangle: new Rectangle(0, 0, 5000, 5000), color: Color.Black* transparency,layerDepth:1f,origin: new Vector2(0,0),sourceRectangle: new Rectangle(0, 0, 5000, 5000),rotation:0f,effects:SpriteEffects.None);

        }

    }
}
