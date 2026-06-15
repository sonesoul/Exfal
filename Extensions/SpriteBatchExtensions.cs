using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exfal.Drawing;

namespace Exfal.Extensions
{
    public static class SpriteBatchExtensions
    {
        public static void Begin(this SpriteBatch batch, in RenderOptions options, in Matrix? transformMatrix = null)
        {
            batch.Begin(
                options.SpriteSortMode,
                options.BlendState,
                options.SamplerState,
                options.DepthStencilState,
                options.RasterizerState,
                options.Effect,
                transformMatrix);
        }
    }
}