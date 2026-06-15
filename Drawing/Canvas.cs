using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exfal.Extensions;
using System;

namespace Exfal.Drawing
{
    public class Canvas
    {
        public ref Vector2 Position => ref _position;

        public ref Color BackgroundColor => ref _backgroundColor;
        public ref RenderOptions Options => ref _options;

        public RenderTarget2D RenderTarget { get; private set; }
        public RenderSource Source { get => _source; set => SetSource(value); }
        public Point Size { get => _size; set => SetSize(value); } 

        protected RenderSource _source;

        protected Vector2 _position = Vector2.Zero;
        protected RenderOptions _options = new();
        protected Color _backgroundColor = Color.Black;

        protected Point _size;

        public Canvas(RenderSource source, Point size)
        {
            Source = source;
            SetSize(size);
        }
        
        public virtual void Begin()
        {
            var graphics = Source.Graphics;

            graphics.SetRenderTarget(RenderTarget);
            graphics.Clear(BackgroundColor);

            Source.SpriteBatch.Begin(Options, GetViewMatrix());
        }
        public virtual void End()
        {
            Source.SpriteBatch.End();
            Source.Graphics.SetRenderTarget(null);
        }

        public virtual Matrix GetViewMatrix() => Matrix.CreateTranslation(new(-Position, 0));
        
        protected virtual void SetSize(Point newSize)
        {
            if (_size == newSize)
                return;

            if (newSize.X <= 0 || newSize.Y <= 0)
                return;

            RenderTarget?.Dispose();
            RenderTarget = new(Source.Graphics, newSize.X, newSize.Y);
            _size = newSize;
        }
        protected virtual void SetSource(RenderSource source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source), "Render source can't be null."); ;
        }
    }
}