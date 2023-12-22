using Microsoft.Xna.Framework.Content;
using System.IO;

namespace DeveLinePlatformer.MonoGame.Core.Data
{
    public interface IContentManagerExtension
    {
        Stream OpenStream(ContentManager content, string assetName);
    }
}
