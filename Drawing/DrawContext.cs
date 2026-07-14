using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Exfal.Extensions;

namespace Exfal.Drawing
{
    public class DrawContext
    {
        public SpriteBatch SpriteBatch { get; }
        public Texture2D PixelTexture { get; }
        public Camera Camera { get; internal set; }
        public int Layer { get; internal set; }

        public DrawContext(GraphicsProvider graphics)
        {
            SpriteBatch = graphics.SpriteBatch;
            PixelTexture = graphics.Pixel;
        }

        public void String(string str, SpriteFont font, Vector2 position, Color color, Vector2 scale, Vector2 origin, float rotationRad = 0)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;

            if (scale.X < 0)
            {
                spriteEffects |= SpriteEffects.FlipHorizontally;
                scale.X = -scale.X;
            }

            if (scale.Y < 0)
            {
                spriteEffects |= SpriteEffects.FlipVertically;
                scale.Y = -scale.Y;
            }

            SpriteBatch.DrawString(font, str, position, color, rotationRad, origin, scale, spriteEffects, 0);
        }
        public void String(string str, SpriteFont font, Vector2 position, Color color)
        {
            String(
                str, 
                font, 
                position, 
                color, 
                Vector2.One, 
                Vector2.Zero);
        }
        public void String(string str, SpriteFont font, in DrawOptions options)
        {
            String(
                str,
                font,
                options.position,
                options.color,
                options.scale,
                options.origin,
                options.rotationRad);
        }

        public void Texture(Texture2D texture, in DrawOptions options)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            Vector2 scale = options.scale;
            Vector2 origin = options.origin;

            if (scale.X < 0)
            {
                spriteEffects |= SpriteEffects.FlipHorizontally;
                scale.X = -scale.X;
                origin.X = texture.Width - origin.X - 1;
            }

            if (scale.Y < 0)
            {
                spriteEffects |= SpriteEffects.FlipVertically;
                scale.Y = -scale.Y;
                origin.Y = texture.Height - origin.Y - 1;
            }

            SpriteBatch.Draw(
                texture,
                options.position,
                null,
                options.color,
                options.rotationRad,
                origin,
                scale,
                spriteEffects,
                0);
        }

        public void Rectangle(Rectangle rect, Color color)
        {
            rect.Size = (rect.Size.ToVector2().Both() + 1).ToPoint();
            SpriteBatch.Draw(PixelTexture, rect, color);
        }

        public void Polygon(List<Vector2> vertices, Color color, float boundThickness)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                var current = vertices[i];
                var next = vertices[(i + 1) % vertices.Count];

                Line(current, next, color, boundThickness);
            }
        }
        public void Line(Vector2 start, Vector2 end, Color color, float thickness = 1f)
        {
            Vector2 edge = end - start;
            float angle = MathF.Atan2(edge.Y, edge.X);
            float length = edge.Length();
            
            SpriteBatch.Draw(
                PixelTexture, 
                start, 
                null, 
                color, 
                angle, 
                Vector2.Zero, 
                new Vector2(length, thickness), 
                SpriteEffects.None, 
                0);
        }
        public void Pixel(Vector2 position, Color color)
        {
            SpriteBatch.Draw(PixelTexture, position, color);
        }
    }
}