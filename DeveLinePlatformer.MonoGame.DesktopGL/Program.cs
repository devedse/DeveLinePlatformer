using DeveLinePlatformer.MonoGame.Core;
using DeveLinePlatformer.MonoGame.Core.HelperObjects;
using System;

namespace DeveLinePlatformer.MonoGame.DesktopGL
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            using (var game = new TheGame(new(2460, 1340), Platform.Desktop))
            {
                game.Run();
            }
        }
    }
}
