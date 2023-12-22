using Microsoft.Xna.Framework;

namespace DeveLinePlatformer.MonoGame.Core.HelperObjects
{
    public struct RectaleFloater
    {
        public float Height;
        public float Width;
        public float X;
        public float Y;

        public float Bottom
        {
            get { return Y + Height; }
            set { Y = value - Height; }
        }

        public float Left
        {
            get { return X; }
            set { X = value; }
        }

        public float PootjesX
        {
            get { return X + (Width / 2.0f); }
            set { X = value - (Width / 2.0f); }
        }

        public float Right
        {
            get { return X + Width; }
            set { X = value - Width; }
        }

        public float Top
        {
            get { return Y; }
            set { Y = value; }
        }

        public RectaleFloater(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public Rectangle ToRectangle()
        {
            Rectangle rect = new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
            return rect;
        }
    }
}
