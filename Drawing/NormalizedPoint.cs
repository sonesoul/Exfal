using Microsoft.Xna.Framework;

namespace Exfal.Drawing
{
    public readonly struct NormalizedPoint
    {
        public readonly Vector2 val;

        public NormalizedPoint(Vector2 point, in RectangleF dst)
        {
            val = (point - dst.Location) / dst.Size;
        }
        public NormalizedPoint(Vector2 value)
        {
            val = value;
        }

        public static implicit operator Vector2(NormalizedPoint p) => p.val;
    }
}
