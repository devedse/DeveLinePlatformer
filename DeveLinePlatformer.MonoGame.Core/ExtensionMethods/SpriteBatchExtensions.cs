using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DeveLinePlatformer.MonoGame.Core.ExtensionMethods
{
    public static class SpriteBatchExtensions
    {
        public static void DrawLine(this SpriteBatch spriteBatch, float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            float halfwidth = width / 2.0f;

            Vector2 extrading = new Vector2((float)(Math.Sin(angle) * halfwidth), (float)(Math.Cos(angle) * halfwidth) * -1.0f);

            //Debug.WriteLine("X: " + extrading.X + ", Y: " + extrading.Y);

            spriteBatch.Draw(ContentDing.blankTexture, point1 + extrading, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0);
        }

        public static void DrawRectangle(this SpriteBatch spriteBatch, int width, Color color, Rectangle rect)
        {
            //spriteBatch.Draw(pixel, rect, Color.Green);
            spriteBatch.Draw(ContentDing.blankTexture, new Rectangle(rect.X - width, rect.Y, width, rect.Height), color); //Links
            spriteBatch.Draw(ContentDing.blankTexture, new Rectangle(rect.X - width, rect.Y - width, rect.Width + 2 * width, width), color); //Top
            spriteBatch.Draw(ContentDing.blankTexture, new Rectangle(rect.X + rect.Width, rect.Y, width, rect.Height), color); //Rechts
            spriteBatch.Draw(ContentDing.blankTexture, new Rectangle(rect.X - width, rect.Y + rect.Height, rect.Width + 2 * width, width), color); //Bottom
        }
    }
}
