using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace Exfal.Drawing
{
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    public struct RectangleF
    {
        public float X { readonly get => Location.X; set => Location.X = value; }
        public float Y { readonly get => Location.Y; set => Location.Y = value; }
        public float Width { readonly get => Size.X; set => Size.X = value; }
        public float Height { readonly get => Size.Y; set => Size.Y = value; }

        public readonly float Left => X;
        public readonly float Top => Y;
        public readonly float Right => X + Width;
        public readonly float Bottom => Y + Height;
        public readonly Vector2 Center => new(X + Width / 2, Y + Height / 2);

        public Vector2 Size;
        public Vector2 Location;

        public RectangleF(float x, float y, float width, float height) : this(new(x, y), new(width, height)) { }
        public RectangleF(Vector2 location, Vector2 size)
        {
            Location = location;
            Size = size;
        }

        public readonly bool Contains(float x, float y)
        {
            return x >= Left && x < Right && y >= Top && y < Bottom;
        }
        public readonly bool Contains(Vector2 point) => Contains(point.X, point.Y);
        
        public void Offset(Vector2 offset)
        {
            Location += offset;
        }
        public void Offset(float offsetX, float offsetY) => Offset(new(offsetX, offsetY));
        
        public void Inflate(float horizontalAmount, float verticalAmount)
        {
            Location -= new Vector2(horizontalAmount / 2, verticalAmount / 2);
            Size += new Vector2(horizontalAmount, verticalAmount);
        }
        public void Inflate(Vector2 amount) => Inflate(amount.X, amount.Y);

        public readonly bool Intersects(RectangleF other)
        {
            return Left < other.Right && Right > other.Left && Top < other.Bottom && Bottom > other.Top;
        }
        public readonly RectangleF Union(RectangleF other)
        {
            float x = Math.Min(Left, other.Left);
            float y = Math.Min(Top, other.Top);
            float width = Math.Max(Right, other.Right) - x;
            float height = Math.Max(Bottom, other.Bottom) - y;

            return new RectangleF(x, y, width, height);
        }

        public readonly Rectangle ToRectangle() => new(Location.ToPoint(), Size.ToPoint());

        public readonly override string ToString() => "{X:" + X + " Y:" + Y + " Width:" + Width + " Height:" + Height + "}";
        private readonly string DebugDisplayString => $"{X} {Y} {Width} {Height}";

        public static bool operator ==(RectangleF a, RectangleF b) => a.Location == b.Location && a.Size == b.Size;
        public static bool operator !=(RectangleF a, RectangleF b) => !(a == b);

        public readonly override bool Equals(object obj) => obj is RectangleF other && this == other;
        public readonly override int GetHashCode() => HashCode.Combine(Location, Size);
    }
}