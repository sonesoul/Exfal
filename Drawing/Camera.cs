using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Exfal.Drawing
{
    public delegate void DrawAction(DrawContext draw);

    [DebuggerDisplay("{ToString(),nq}")]
    public class Camera : Canvas
    {
        public const int DefaultLayer = 0;

        public DrawContext Context { get; private set; }

        private readonly SortedDictionary<int, List<DrawAction>> renderPipeline = new();

        public Camera(RenderSource source, Point size) : base(source, size)
        {
            AddLayer(DefaultLayer);
        }

        public void AddLayer(int layer) => renderPipeline[layer] = new();
        public void RemoveLayer(int layer) => renderPipeline.Remove(layer);

        public void Register(DrawAction drawAction, int layer = DefaultLayer)
        {
            ArgumentNullException.ThrowIfNull(drawAction);

            if (!renderPipeline.TryGetValue(layer, out var list))
                throw new ArgumentException($"Layer {layer} doesn't exist.", nameof(layer));

            list.Add(drawAction);
        }
        public void Unregister(DrawAction drawAction, int layer = DefaultLayer)
        {
            if (!renderPipeline.TryGetValue(layer, out var list))
                throw new ArgumentException($"Layer {layer} doesn't exist.", nameof(layer));

            list.Remove(drawAction);
        }

        public void Render()
        {
            Begin();

            foreach (var layer in renderPipeline.Values)
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