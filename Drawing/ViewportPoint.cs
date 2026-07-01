using Microsoft.Xna.Framework;

namespace Exfal.Drawing
{
    public readonly struct ViewportPoint
    {
        public readonly Vector2 val;
        public readonly Vector2 view;

        public ViewportPoint(in NormalizedPoint point, Vector2 viewport)
        {
            val = point * viewport;
            view = viewport;
        }
    }
}