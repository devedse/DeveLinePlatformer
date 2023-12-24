using DeveLinePlatformer.MonoGame.Core.ExtensionMethods;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DeveLinePlatformer.MonoGame.Core.PlatformerGame
{
    public class PointLine
    {
        private PointBall firstBall;
        private PointBall secondBall;

        public PointBall LeftBall
        {
            get
            {
                if (firstBall.Position.X < secondBall.Position.X)
                {
                    return firstBall;
                }
                else
                {
                    return secondBall;
                }
            }
        }

        public PointBall RightBall
        {
            get
            {
                return LeftBall == firstBall ? secondBall : firstBall;
            }
        }

        public double Angle
        {
            get
            {
                return Math.Atan2(RightBall.Position.Y - LeftBall.Position.Y, RightBall.Position.X - LeftBall.Position.X);
            }
        }



        public PointLine(PointBall leftBall, PointBall rightBall)
        {
            firstBall = leftBall;
            secondBall = rightBall;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawLine(3, Color.Yellow, LeftBall.Position, RightBall.Position);
        }

        public float GetYHere(float x)
        {
            if (x < LeftBall.Position.X || x > RightBall.Position.X)
                return 0;


            var width = RightBall.Position.X - LeftBall.Position.X;
            var height = RightBall.Position.Y - LeftBall.Position.Y;

            var per1x = height / width;

            var curxinside = x - LeftBall.Position.X;

            var curyinside = curxinside * per1x;

            var cury = curyinside + LeftBall.Position.Y;

            return cury;
        }

        public override string ToString()
        {
            return $"({LeftBall.Position.X},{LeftBall.Position.Y}) ({RightBall.Position.X},{RightBall.Position.Y}) {Angle}";
        }
    }
}
