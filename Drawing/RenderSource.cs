using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exfal.Drawing
{
    public class RenderSource
    {
        public SpriteBatch SpriteBatch { get; }
        public GraphicsDevice Graphics { get; }
        public GraphicsDeviceManager GraphicsManager { get; }
        public Texture2D Pixel { get; }

        public RenderSource(SpriteBatch batch, GraphicsDeviceManager manager)
        {
            SpriteBatch = batch;
            GraphicsManager = manager;
            Graphics = manager.GraphicsDevice;

            Pixel = new Texture2D(Graphics, 1, 1);
            Pixel.SetData(new[] { Color.White });
        }
    }
}