using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace DeveLinePlatformer.MonoGame.Core.PlatformerGame
{
    public static class HelperMethods
    {
        public static bool CirclesColliding(Vector2 vect1, Vector2 vect2, float radius)
        {
            return CirclesColliding(vect1.X, vect1.Y, radius, vect2.X, vect2.Y, radius);
        }

        public static bool CirclesColliding(float x1, float y1, float radius1, float x2, float y2, float radius2)
        {
            //compare the distance to combined radii
            float dx = x2 - x1;
            float dy = y2 - y1;
            float radii = radius1 + radius2;
            if (dx * dx + dy * dy < radii * radii)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<PointBall> GetCurrentlyCollidingBalls(List<PointBall> balls, Rectangle selectionRect)
        {
            List<PointBall> retval = new List<PointBall>();
            foreach (var pointBall in balls)
            {
                var rect = new Rectangle((int)(pointBall.Position.X - PointBall.Diameter / 2.0f), (int)(pointBall.Position.Y - PointBall.Diameter / 2.0f), (int)PointBall.Diameter, (int)PointBall.Diameter);
                if (selectionRect.Intersects(rect))
                {
                    retval.Add(pointBall);
                }
            }
            return retval;
        }

        public static Rectangle RectangleFromVector(Vector2 vector1, Vector2 vector2)
        {
            var left = (int)Math.Min(vector1.X, vector2.X);
            var top = (int)Math.Min(vector1.Y, vector2.Y);
            var right = (int)Math.Max(vector1.X, vector2.X);
            var bottom = (int)Math.Max(vector1.Y, vector2.Y);

            Rectangle rect = new Rectangle(left, top, right - left, bottom - top);
            return rect;
        }
    }
}
