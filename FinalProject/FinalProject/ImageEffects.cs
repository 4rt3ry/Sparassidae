
// Author: Arthur Powers: 3/12/2022
// Purpose: 
//      Handles all shader effects


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FinalProject
{
    static class ImageEffects
    {

        private static Effect _maskEffect;

        /// <summary>
        /// Must be invoked in order for image effects to work
        /// </summary>
        /// <param name="maskEffect"></param>
        public static void LoadContent(Effect maskEffect)
        {
            _maskEffect = maskEffect;
        }

        /// <summary>
        /// Draws a masked portion of a base texture
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="texture"></param>
        /// <param name="mask"></param>
        /// <param name="maskLocation"></param>
        /// <param name="destination"></param>
        private static void DrawImageMask(SpriteBatch batch, Texture2D texture, Texture2D mask, Vector2 maskLocation, Vector2 destination)
        {
            DrawImageMask(batch, texture, mask, maskLocation, new Vector2(mask.Width, mask.Height), destination);
        }

        /// <summary>
        /// Draws a masked portion of a base texture
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="texture"></param>
        /// <param name="mask"></param>
        /// <param name="maskSize"></param>
        /// <param name="maskLocation"></param>
        /// <param name="destination"></param>
        private static void DrawImageMask(SpriteBatch batch, Texture2D texture, Texture2D mask, Vector2 maskSize, Vector2 maskLocation, Vector2 destination)
        {
            if (_maskEffect == null) return;

            _maskEffect.Parameters["Mask"].SetValue(mask);
            _maskEffect.Parameters["MaskLocationX"].SetValue(maskLocation.X);
            _maskEffect.Parameters["MaskLocationY"].SetValue(maskLocation.Y);
            _maskEffect.Parameters["MaskWidth"].SetValue(maskSize.X);
            _maskEffect.Parameters["MaskHeight"].SetValue(maskSize.Y);
            _maskEffect.Parameters["BaseTextureWidth"].SetValue((float)texture.Width);
            _maskEffect.Parameters["BaseTextureHeight"].SetValue((float)texture.Height);

            batch.Begin(effect: _maskEffect, samplerState: SamplerState.PointClamp);

            batch.Draw(texture,
                new Rectangle((int)(destination.X - maskSize.X / 2), (int)(destination.Y - maskSize.X / 2), (int)maskSize.X, (int)maskSize.Y),
                new Rectangle((int)maskLocation.X, (int)maskLocation.Y, (int)maskSize.X, (int)maskSize.Y),
                Color.White);

            batch.End();
        }
    }
}
