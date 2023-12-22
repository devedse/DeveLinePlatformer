using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveLinePlatformer.MonoGame.Core.PlatformerGame
{
    public class CoolPlatformerGame
    {
        private MapData mapData;
        private Player player;

        public CoolPlatformerGame(MapData mapData)
        {
            this.mapData = mapData;
            player = new Player(mapData);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            player.Draw(gameTime, spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);
        }
    }
}
