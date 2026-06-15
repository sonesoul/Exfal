using Microsoft.Xna.Framework.Graphics;

namespace Exfal.Drawing
{
    public struct RenderOptions
    {
        public SpriteSortMode SpriteSortMode { get; set; } = SpriteSortMode.Deferred;
        public BlendState BlendState { get; set; } = null;
        public SamplerState SamplerState { get; set; } = null;
        public DepthStencilState DepthStencilState { get; set; } = null;
        public RasterizerState RasterizerState { get; set; } = null;
        public Effect Effect { get; set; } = null;

        public RenderOptions()
        {
            SpriteSortMode = SpriteSortMode.Deferred;
            BlendState = null;
            SamplerState = null;
            DepthStencilState = null;
            RasterizerState = null;
            Effect = null;
        }
    }
}