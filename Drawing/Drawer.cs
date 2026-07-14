using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exfal.Extensions;

namespace Exfal.Drawing
{
    public class Drawer
    {
        public delegate Rectangle ScaleFunction(in Point source, in Rectangle target);

        public const int DefaultCameraIndex = 0;

        public static RectScaler OutputScaler { get; } = new();

        public RenderSource Source => Canvas.Source;
        public RenderOptions Options { get; set; } = new();

        public Canvas Canvas { get; set; }

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
                    _destination = ScaleFunc.Invoke(Canvas.Size, value);
                }
            }
        }

        private GraphicsDevice Graphics => Source.Graphics;
        private SpriteBatch SpriteBatch => Source.SpriteBatch;

        private Rectangle _destination;
        private Rectangle _windowBounds;

        public Drawer(Canvas canvas)
        {
            Canvas = canvas;
            Cameras[DefaultCameraIndex] = CreateCamera();
            WindowBounds = Graphics.Viewport.Bounds;
        }
        public Drawer(RenderSource source, Point size) : this(new Canvas(source, size)) { }
        public Drawer(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsManager, Point size) : this(
            new Canvas(
                new RenderSource(spriteBatch, graphicsManager), 
                size)) { }

        public void Draw()
        {
            DrawToCanvas();

            Graphics.Clear(BackgroundColor);
            
            SpriteBatch batch = SpriteBatch;

            batch.Begin(Options);
            batch.Draw(Canvas.RenderTarget, _destination, Color.White);
            batch.End();
        }
        private void DrawToCanvas()
        {
            foreach (var item in Cameras.Values)
            {
                item.Render();
            }

            Canvas.Begin();

            Rectangle dst = new(_windowBounds.Location, Canvas.Size);

            foreach (var item in Cameras.Values)
            {
                SpriteBatch.Draw(item.RenderTarget, dst, Color.White);
            }

            Canvas.End();
        }

        public Camera CreateCamera()
        {
            return new(Source, Canvas.Size)
            {
                BackgroundColor = Canvas.BackgroundColor,
                Options = Options
            };
        }

        public ViewportPoint ToViewportPoint(Vector2 point)
        {
            return new(
                new NormalizedPoint(point, _destination.ToRectangleF()), 
                Canvas.Size.ToVector2());
        }
    }
}