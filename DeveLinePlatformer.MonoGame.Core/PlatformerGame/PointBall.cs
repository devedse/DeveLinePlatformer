using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeveLinePlatformer.MonoGame.Core.PlatformerGame
{
    public class PointBall
    {
        public static float Diameter = 20.0f;

        public Vector2 Position { get; set; }

        public PointBall(float x, float y)
        {
            Position = new Vector2(x, y);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, bool isSelected, bool isPossibleSelected)
        {
            Rectangle drawpos = new Rectangle((int)(Position.X - Diameter / 2), (int)(Position.Y - Diameter / 2), (int)Diameter, (int)Diameter);
            if (isPossibleSelected)
            {
                spriteBatch.Draw(ContentDing.ballTexture, drawpos, Color.Yellow);
            }
            else if (isSelected)
            {
                spriteBatch.Draw(ContentDing.ballTexture, drawpos, Color.Red);
            }
            else
            {
                spriteBatch.Draw(ContentDing.ballTexture, drawpos, Color.Green);
            }
        }

        public bool IsClickingOnIt(float x, float y)
        {
            return HelperMethods.CirclesColliding(x, y, 0, Position.X, Position.Y, Diameter);
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
