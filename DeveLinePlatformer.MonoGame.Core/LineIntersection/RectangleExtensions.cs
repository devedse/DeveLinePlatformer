using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeveLinePlatformer.MonoGame.Core.LineIntersection
{
    public static class RectangleExtensions
    {
        public static IEnumerable<LineEquation> GetLinesForRectangle(this Rectangle rectangle)
        {
            var lines = new List<LineEquation>{
                new LineEquation(new Vector2(rectangle.X, rectangle.Y),
                                 new Vector2(rectangle.X, rectangle.Y + rectangle.Height)),
                new LineEquation(new Vector2(rectangle.X, rectangle.Y + rectangle.Height),
                                 new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height)),
                new LineEquation(new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height),
                                 new Vector2(rectangle.X + rectangle.Width, rectangle.Y)),
                new LineEquation(new Vector2(rectangle.X + rectangle.Width, rectangle.Y),
                                 new Vector2(rectangle.X, rectangle.Y)),
            };
            return lines;
        }
    }
}