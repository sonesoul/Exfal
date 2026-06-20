using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace Exfal.Drawing
{
    public delegate void DrawAction(DrawContext draw);

    [DebuggerDisplay("{ToString(),nq}")]
    public class Camera : Canvas
    {
        public const int DefaultLayer = 0;

        public SortedDictionary<int, List<DrawAction>> Layers { get; } = new();
        public DrawContext Context { get; private set; }

        public Camera(RenderSource source, Point size) : base(source, size)
        {
            Layers[DefaultLayer] = new();
        }

        public void Render()
        {
            Begin();

            foreach (var layer in Layers.Values)
            {
                foreach (var draw in layer)
                {
                    draw(Context);
                }
            }
            
            End();
        }

        protected override void SetSource(RenderSource source)
        {
            base.SetSource(source);
            Context = new(source);
        }
        public override string ToString() => $"{Position} {Size.X}x{Size.Y}";
    }
}