using Exfal.Drawing;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Exfal.Extensions
{
    public static class Vector2Extensions
    {
        public readonly struct BothAxes
        {
            private readonly Vector2 vector;
            public readonly float X => vector.X;
            public readonly float Y => vector.Y;

            public BothAxes(float x, float y) => vector = new(x, y);
            public BothAxes(Vector2 vector) => this.vector = vector;

            public readonly Vector2 Clamp(float min, float max) => new(X.Clamp(min, max), Y.Clamp(min, max));

            public readonly override bool Equals([NotNullWhen(true)] object obj) => (obj is float axis) ? Equals(this, axis) : base.Equals(obj);
            private static bool Equals(BothAxes vector, float value) => vector.X == value && vector.Y == value;
            public readonly override int GetHashCode() => HashCode.Combine(vector.X, vector.Y);

            public static Vector2 operator +(BothAxes vector, float value) => new(vector.X + value, vector.Y + value);
            public static Vector2 operator -(BothAxes vector, float value) => new(vector.X - value, vector.Y - value);
            public static Vector2 operator *(BothAxes vector, float value) => new(vector.X * value, vector.Y * value);
            public static Vector2 operator /(BothAxes vector, float value) => new(vector.X / value, vector.Y / value);
            public static bool operator <(BothAxes vector, float value) => (vector.X < value && vector.Y < value);
            public static bool operator >(BothAxes vector, float value) => (vector.X > value && vector.Y > value);
            public static bool operator <=(BothAxes vector, float value) => vector < value || vector == value;
            public static bool operator >=(BothAxes vector, float value) => vector > value || vector == value;
            public static bool operator ==(BothAxes vector, float value) => vector.Equals(value);
            public static bool operator !=(BothAxes vector, float value) => !vector.Equals(value);
        }
        public readonly struct AnyAxis
        {
            private readonly Vector2 vector;
            public readonly float X => vector.X;
            public readonly float Y => vector.Y;

            public AnyAxis(float x, float y) => vector = new(x, y);
            public AnyAxis(Vector2 vector) => this.vector = vector;

            public readonly override bool Equals([NotNullWhen(true)] object obj) => (obj is float axis) ? Equals(this, axis) : base.Equals(obj);
            private static bool Equals(AnyAxis left, float value) => (left.X == value || left.Y == value);

            public static bool operator >(AnyAxis vector, float value) => (vector.X > value || vector.Y > value);
            public static bool operator <(AnyAxis vector, float value) => (vector.X < value || vector.Y < value);
            public static bool operator <=(AnyAxis vector, float value) => vector < value || vector == value;
            public static bool operator >=(AnyAxis vector, float value) => vector > value || vector == value;
            public static bool operator ==(AnyAxis vector, float value) => vector.Equals(value);
            public static bool operator !=(AnyAxis vector, float value) => !vector.Equals(value);

            public readonly override int GetHashCode() => HashCode.Combine(vector.X, vector.Y);
        }

        public static BothAxes Both(this Vector2 v) => new(v);
        public static AnyAxis Any(this Vector2 v) => new(v);
        
        public static float Dot(this Vector2 a, Vector2 b) => Vector2.Dot(a, b);
        public static float Cross(this Vector2 a, Vector2 b) => a.X * b.Y - a.Y * b.X;
        public static Vector2 RotatedAround(this Vector2 v, Vector2 origin, float rotationRad)
        {
            float cos = MathF.Cos(rotationRad);
            float sin = MathF.Sin(rotationRad);

            Vector2 translatedPoint = v - origin;

            float newX = translatedPoint.X * cos - translatedPoint.Y * sin;
            float newY = translatedPoint.X * sin + translatedPoint.Y * cos;

            return new Vector2(newX, newY) + origin;
        }
        public static Vector2 Perpendicular(this Vector2 v) => new(v.Y, -v.X);

        public static Vector2 Normalized(this Vector2 v, double epsilon = 0)
        {
            if (v.LengthSquared() > epsilon)
            {
                v = Vector2.Normalize(v);
            }

            return v;
        }

        public static float DistanceTo(this Vector2 a, Vector2 b) => Vector2.Distance(a, b);

        public static float Max(this Vector2 v) => Math.Max(v.X, v.Y);
        public static float Min(this Vector2 v) => Math.Min(v.X, v.Y);

        public static Vector2 Floored(this Vector2 v) => Vector2.Floor(v);
        public static Vector2 Ceiled(this Vector2 v) => Vector2.Ceiling(v);
        
        public static Vector2 Rounded(this Vector2 v, int digits = 0)
            => new(
                MathF.Round(v.X, digits),
                MathF.Round(v.Y, digits));
        public static Vector2 Truncated(this Vector2 v) => new((int)v.X, (int)v.Y);

        public static Vector2 Clamped(this Vector2 v, float min, float max) 
            => new(
                v.X.Clamp(min, max), 
                v.Y.Clamp(min, max));

        public static Vector2 Abs(this Vector2 v) => new(v.AbsX(), v.AbsY());
        public static float AbsX(this Vector2 v) => Math.Abs(v.X);
        public static float AbsY(this Vector2 v) => Math.Abs(v.Y);

        public static Vector2 WithX(this Vector2 v, Func<float, float> func) => v.WithX(func(v.X));
        public static Vector2 WithY(this Vector2 v, Func<float, float> func) => v.WithY(func(v.Y));

        public static Vector2 WithX(this Vector2 v, float x) => new(x, v.Y);
        public static Vector2 WithY(this Vector2 v, float y) => new(v.X, y);

        public static Vector2 TakeX(this Vector2 v) => v.WithY(0);
        public static Vector2 TakeY(this Vector2 v) => v.WithX(0);
    }
}