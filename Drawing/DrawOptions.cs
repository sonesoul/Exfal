using Microsoft.Xna.Framework;

namespace Exfal.Drawing
{
    public struct DrawOptions
    {
        public Vector2 position = Vector2.Zero;
        public Vector2 origin = Vector2.Zero;
        public Vector2 scale = Vector2.One;

        public Color color = Color.White;
        public float rotationRad = 0;

        public DrawOptions() { }
    }
}