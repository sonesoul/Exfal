using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using Exfal.Extensions;

namespace Exfal.Drawing
{
    public delegate void DrawAction(DrawContext draw);

    [DebuggerDisplay("{ToString(),nq}")]
    public class Camera : Surface
    {
        public ref Vector2 Position => ref _position;
        public ref RenderOptions Options => ref _options;

        public const int DefaultLayer = 0;

        public SortedDictionary<int, List<DrawAction>> Layers { get; } = new();
        public DrawContext Context { get; private set; }

        protected Vector2 _position = Vector2.Zero;
        protected RenderOptions _options = new();

        public Camera(GraphicsProvider graphics, Point size) : base(graphics, size)
        {
            Layers[DefaultLayer] = new();
        }

        public void Draw()
        {
            Begin();

            foreach (var layer in Layers)
            {
                List<DrawAction> draws = layer.Value;
                Context.Layer = layer.Key;

                for (int i = 0; i < draws.Count; i++)
                {
                    draws[i](Context);
                }
            }

            Context.Layer = DefaultLayer;
            
            End();
        }

        public virtual Matrix GetViewMatrix() => Matrix.CreateTranslation(new(-Position, 0));

        public override void Begin()
        {
            base.Begin();
            Graphics.SpriteBatch.Begin(_options, GetViewMatrix());
        }
        public override void End()
        {
            Graphics.SpriteBatch.End();
            base.End();
        }

        protected override void SetGraphics(GraphicsProvider graphics)
        {
            base.SetGraphics(graphics);

            Context?.Dispose();
            Context = new(graphics)
            {
                Camera = this,
                Layer = DefaultLayer
            };
        }
        public override string ToString() => $"{Position} {Size.X}x{Size.Y}";

        public override Vector2 ToWorldPoint(ViewportPoint point)
        {
            return base.ToWorldPoint(point) + Position;
        }
    }
}