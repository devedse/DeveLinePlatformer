using Microsoft.Xna.Framework;
using System;

namespace DeveLinePlatformer.MonoGame.Core.LineIntersection
{
    public static class Vector2Extensions
    {
        public static float DistanceToPoint(this Vector2 point, Vector2 point2)
        {
            return
                (float)
                Math.Sqrt((point2.X - point.X) * (point2.X - point.X) + (point2.Y - point.Y) * (point2.Y - point.Y));
        }

        public static bool IsBetweenTwoPoints(this Vector2 targetPoint, Vector2 point1, Vector2 point2)
        {
            float minX = Math.Min(point1.X, point2.X);
            float minY = Math.Min(point1.Y, point2.Y);
            float maxX = Math.Max(point1.X, point2.X);
            float maxY = Math.Max(point1.Y, point2.Y);

            float targetX = targetPoint.X;
            float targetY = targetPoint.Y;

            return LessOrEqual(minX, targetX)
                   && LessOrEqual(targetX, maxX)
                   && LessOrEqual(minY, targetY)
                   && LessOrEqual(targetY, maxY);
        }

        private static bool LessOrEqual(float left, float right)
        {
            return left <= right || 1 <= left / right && left / right < 1.00000000001f;
        }
    }
}