using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exfal.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exfal.Drawing
{
    public class Drawer
    {
        public delegate Rectangle ScaleFunction(in Point source, in Rectangle target);

        public RenderSource Source => Canvas.Source;
        public RenderOptions Options { get; set; } = new();

        public Canvas Canvas { get; set; }

        public static RectScaler OutputScaler { get; } = new();

        public ScaleFunction ScaleFunc { get; set; } = OutputScaler.Fit;
        public Color BackgroundColor { get; set; } = Color.Black;

        public Camera OutputCamera { get; set; }

        private GraphicsDevice Graphics => Source.Graphics;
        private SpriteBatch SpriteBatch => Source.SpriteBatch;

        public CameraCollection Cameras { get; } = new();

        private Rectangle destination;
        private Rectangle windowBounds;

        public Drawer(Canvas canvas)
        {
            Canvas = canvas;
            OutputCamera = CreateCamera();
        }
        public Drawer(RenderSource source, Point size) : this(new Canvas(source, size)) { }
        public Drawer(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsManager, Point size) : this(
            new Canvas(
                new RenderSource(spriteBatch, graphicsManager), 
                size)) { }

        public void Draw()
        {
            RenderAll();
            Clear();
            DrawCanvas();
        }

        public void RenderAll()
        {
            OutputCamera.Render();

            foreach (var item in Cameras.Values)
            {
                item.Render();
            }

            Canvas.Begin();

            void DrawItem(Camera item)
            {
                SpriteBatch.Draw(item.RenderTarget, new Rectangle(windowBounds.Location, Canvas.Size), Color.White);
            }

            DrawItem(OutputCamera);

            foreach (var item in Cameras.Values)
            {
                DrawItem(item);
            }

            Canvas.End();
        }
        public void DrawCanvas()
        {
            SpriteBatch batch = SpriteBatch;

            if (windowBounds != Graphics.Viewport.Bounds)
            {
                destination = ScaleFunc.Invoke(Canvas.Size, Graphics.Viewport.Bounds);
                windowBounds = Graphics.Viewport.Bounds;
            }

            batch.Begin(Options);
            
            batch.Draw(
                Canvas.RenderTarget,
                destination, 
                Color.White);

            batch.End();
        }
        public void Clear()
        {
            Graphics.Clear(BackgroundColor);
        }

        public Camera CreateCamera()
        {
            return new(Source, Canvas.Size)
            {
                BackgroundColor = Canvas.BackgroundColor,
                Options = Options
            };
        }

        public Vector2 ScreenToCanvasPoint(Vector2 point)
        {
            Vector2 normalizedPoint = NormalizePoint(point, destination);
            Vector2 canvasPoint = normalizedPoint * Canvas.Size.ToVector2();

            return canvasPoint.Floored();
        }
        public Vector2 ScreenToWorldPoint(Vector2 point, Camera camera)
        {
            Vector2 canvasPoint = ScreenToCanvasPoint(point);

            return (canvasPoint / (Canvas.Size.ToVector2() / camera.Size.ToVector2()) + camera.Position).Floored();
        }
        public Vector2 ScreenToWorldPoint(Vector2 point) => ScreenToWorldPoint(point, OutputCamera);

        public int SafeIndex()
        {
            return Cameras.Count > 0 ? Cameras.Last().Key + 1 : 0;
        }

        public static Vector2 NormalizePoint(Vector2 point, Rectangle destination)
        {
            return (point - destination.Location.ToVector2()) / destination.Size.ToVector2();
        }
    }
}