using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DeveMazeGeneratorMonoGame
{
    public static class ContentDing
    {
        public static Texture2D ballTexture;
        public static Texture2D blankTexture;
        public static Texture2D playerTexture;

        public static SpriteFont spriteFont;

        public static void GoLoadContent(GraphicsDevice graphicsDevice, ContentManager Content)
        {
            ballTexture = Content.Load<Texture2D>("ball");
            playerTexture = Content.Load<Texture2D>("player");

            blankTexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blankTexture.SetData(new[] { Color.White });


            spriteFont = Content.Load<SpriteFont>("SecularOne20");
            spriteFont.DefaultCharacter = '?';
        }
    }
}
