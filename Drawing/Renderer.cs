using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exfal.Extensions;

namespace Exfal.Drawing
{
    public class Renderer
    {
        public delegate Rectangle ScaleFunction(in Point source, in Rectangle target);

        public const int DefaultCameraIndex = 0;

        public static RectScaler OutputScaler { get; } = new();

        public RenderOptions Options { get; set; } = new();

        public Surface Surface { get; set; }

        public CameraCollection Cameras { get; } = new();

        public ScaleFunction ScaleFunc { get; set; } = OutputScaler.Fit;
        public Color BackgroundColor { get; set; } = Color.Black;

        public Rectangle WindowBounds
        {
            get => _windowBounds;
            set
            {
                if (_windowBounds != value)
                {
                    _windowBounds = value;
                    _destination = ScaleFunc.Invoke(Surface.Size, value);
                }
            }
        }

        public GraphicsProvider Graphics => Surface.Graphics;
        private GraphicsDevice Device => Graphics.Device;
        private SpriteBatch SpriteBatch => Graphics.SpriteBatch;

        private Rectangle _destination;
        private Rectangle _windowBounds;

        public Renderer(Surface surface)
        {
            Surface = surface;
            Cameras[DefaultCameraIndex] = CreateCamera();
            WindowBounds = Device.Viewport.Bounds;
        }
        public Renderer(GraphicsProvider provider, Point size) : this(new Surface(provider, size)) { }
        public Renderer(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsManager, Point size) : this(
            new Surface(
                new GraphicsProvider(spriteBatch, graphicsManager), 
                size)) { }

        public void Draw()
        {
            foreach (var item in Cameras.Values)
            {
                item.Draw();
            }

            Surface.Begin();
            Rectangle dst = new(_windowBounds.Location, Surface.Size);

            foreach (var item in Cameras.Values)
            {
                SpriteBatch.Draw(item.RenderTarget, dst, Color.White);
            }
            Surface.End();

            Device.Clear(BackgroundColor);
            
            SpriteBatch batch = SpriteBatch;

            batch.Begin(Options);
            batch.Draw(Surface.RenderTarget, _destination, Color.White);
            batch.End();
        }

        public Camera CreateCamera()
        {
            return new(Graphics, Surface.Size)
            {
                BackgroundColor = Surface.BackgroundColor,
                Options = Options
            };
        }

        public ViewportPoint ToViewportPoint(Vector2 point)
        {
            return new(
                new NormalizedPoint(point, _destination.ToRectangleF()), 
                Surface.Size.ToVector2());
        }
    }
}