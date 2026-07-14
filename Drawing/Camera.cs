using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace Exfal.Drawing
{
    public delegate void DrawAction(DrawContext draw);

    [DebuggerDisplay("{ToString(),nq}")]
    public class Camera : Surface
    {
        public const int DefaultLayer = 0;

        public SortedDictionary<int, List<DrawAction>> Layers { get; } = new();
        public DrawContext Context { get; private set; }

        public Camera(RenderSource source, Point size) : base(source, size)
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

        protected override void SetSource(RenderSource source)
        {
            base.SetSource(source);
            Context = new(source)
            {
                Camera = this,
                Layer = DefaultLayer
            };
        }
        public override string ToString() => $"{Position} {Size.X}x{Size.Y}";
    }
}