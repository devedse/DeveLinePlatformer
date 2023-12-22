using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeveLinePlatformer.MonoGame.Core.LineIntersection
{
    /*
       * http://community.topcoder.com/tc?module=Static&d1=tutorials&d2=geometry2#line_line_intersection
       * http://www.wikihow.com/Algebraically-Find-the-Intersection-of-Two-Lines
       * http://www.webmath.com/_answer.php
       *
       * */

    public class LineEquation
    {
        public float A { get; private set; }

        public float B { get; private set; }

        public float C { get; private set; }

        public Vector2 End { get; private set; }

        public bool IsVertical { get; private set; }

        public float M { get; private set; }

        public Vector2 Start { get; private set; }

        public LineEquation(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;

            IsVertical = Math.Abs(End.X - start.X) < 0.00001f;
            M = (End.Y - Start.Y) / (End.X - Start.X);
            A = -M;
            B = 1;
            C = Start.Y - M * Start.X;
        }

        public bool GetIntersectionLineForRay(Rectangle rectangle, out LineEquation intersectionLine)
        {
            if (Start == End)
            {
                intersectionLine = null;
                return false;
            }
            IEnumerable<LineEquation> lines = rectangle.GetLinesForRectangle();
            intersectionLine = new LineEquation(new Vector2(0, 0), new Vector2(0, 0));
            var intersections = new Dictionary<LineEquation, Vector2>();
            foreach (LineEquation equation in lines)
            {
                Vector2 point;
                if (IntersectWithSegementOfLine(equation, out point))
                    intersections[equation] = point;
            }
            if (!intersections.Any())
                return false;

            var intersectionPoints = new SortedDictionary<float, Vector2>();
            foreach (var intersection in intersections)
            {
                if (End.IsBetweenTwoPoints(Start, intersection.Value) ||
                    intersection.Value.IsBetweenTwoPoints(Start, End))
                {
                    float distanceToPoint = Start.DistanceToPoint(intersection.Value);
                    intersectionPoints[distanceToPoint] = intersection.Value;
                }
            }
            if (intersectionPoints.Count == 1)
            {
                Vector2 endPoint = intersectionPoints.First().Value;
                intersectionLine = new LineEquation(Start, endPoint);
                return true;
            }

            if (intersectionPoints.Count == 2)
            {
                Vector2 start = intersectionPoints.First().Value;
                Vector2 end = intersectionPoints.Last().Value;
                intersectionLine = new LineEquation(start, end);
                return true;
            }

            return false;
        }

        public bool IntersectsWithLine(LineEquation otherLine, out Vector2 intersectionPoint)
        {
            intersectionPoint = new Vector2(0, 0);
            if (IsVertical && otherLine.IsVertical)
                return false;
            if (IsVertical || otherLine.IsVertical)
            {
                intersectionPoint = GetIntersectionPointIfOneIsVertical(otherLine, this);
                return true;
            }
            float delta = A * otherLine.B - otherLine.A * B;
            bool hasIntersection = Math.Abs(delta - 0) > 0.0001f;
            if (hasIntersection)
            {
                float x = (otherLine.B * C - B * otherLine.C) / delta;
                float y = (A * otherLine.C - otherLine.A * C) / delta;
                intersectionPoint = new Vector2(x, y);
            }
            return hasIntersection;
        }

        public bool IntersectWithSegementOfLine(LineEquation otherLine, out Vector2 intersectionPoint)
        {
            bool hasIntersection = IntersectsWithLine(otherLine, out intersectionPoint);
            if (hasIntersection)
            {
                var inbetween = intersectionPoint.IsBetweenTwoPoints(otherLine.Start, otherLine.End);
                if (!inbetween)
                {
                    intersectionPoint = new Vector2(0, 0);
                }
                return inbetween;
            }
            return false;
        }

        public bool ThisSegmentIntersectWithSegementOfLine(LineEquation otherLine, out Vector2 intersectionPoint)
        {
            bool hasIntersection = IntersectsWithLine(otherLine, out intersectionPoint);
            if (hasIntersection)
            {
                var inbetween = intersectionPoint.IsBetweenTwoPoints(otherLine.Start, otherLine.End) && intersectionPoint.IsBetweenTwoPoints(Start, End);
                if (!inbetween)
                {
                    intersectionPoint = new Vector2(0, 0);
                }
                return inbetween;
            }
            return false;
        }

        public override string ToString()
        {
            return "[" + Start + "], [" + End + "]";
        }

        private static Vector2 GetIntersectionPointIfOneIsVertical(LineEquation line1, LineEquation line2)
        {
            LineEquation verticalLine = line2.IsVertical ? line2 : line1;
            LineEquation nonVerticalLine = line2.IsVertical ? line1 : line2;

            float y = (verticalLine.Start.X - nonVerticalLine.Start.X) *
                       (nonVerticalLine.End.Y - nonVerticalLine.Start.Y) /
                       (nonVerticalLine.End.X - nonVerticalLine.Start.X) +
                       nonVerticalLine.Start.Y;
            float x = line1.IsVertical ? line1.Start.X : line2.Start.X;
            return new Vector2(x, y);
        }
    }
}