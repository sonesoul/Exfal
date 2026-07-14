using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exfal.Extensions;
using System;

namespace Exfal.Drawing
{
    public class Surface
    {
        public ref Color BackgroundColor => ref _backgroundColor;

        public RenderTarget2D RenderTarget { get; private set; }
        public GraphicsProvider Graphics { get => _graphics; set => SetGraphics(value); }
        public Point Size { get => _size; set => SetSize(value); } 

        protected GraphicsProvider _graphics;
        protected Color _backgroundColor = Color.Black;
        protected Point _size;

        public Surface(GraphicsProvider provider, Point size)
        {
            Graphics = provider;
            SetSize(size);
        }
        
        public virtual void Begin()
        {
            var graphics = Graphics.Device;
            graphics.SetRenderTarget(RenderTarget);
            graphics.Clear(BackgroundColor);
        }
        public virtual void End()
        {
            Graphics.Device.SetRenderTarget(null);
        }
        
        protected virtual void SetSize(Point newSize)
        {
            if (_size == newSize)
                return;

            if (newSize.X < 1)
                throw new ArgumentOutOfRangeException(nameof(newSize), "Surface width cannot be less than one.");

            if (newSize.Y < 1)
                throw new ArgumentOutOfRangeException(nameof(newSize), "Surface height cannot be less than one.");

            RenderTarget?.Dispose();
            RenderTarget = new(Graphics.Device, newSize.X, newSize.Y);
            _size = newSize;
        }
        protected virtual void SetGraphics(GraphicsProvider graphics)
        {
            _graphics = graphics ?? throw new ArgumentNullException(nameof(graphics), "Graphics provider can't be null."); ;
        }

        public virtual Vector2 ToWorldPoint(ViewportPoint point)
        {
            return point.val / (point.view / Size.ToVector2());
        }
    }
}