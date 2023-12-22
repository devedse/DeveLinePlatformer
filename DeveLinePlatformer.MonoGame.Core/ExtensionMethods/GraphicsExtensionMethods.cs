﻿using Microsoft.Xna.Framework.Graphics;

namespace DeveLinePlatformer.MonoGame.Core.ExtensionMethods
{
    public static class GraphicsExtensionMethods
    {
        public static IndexElementSize GetPreferedIndexElementSize(this GraphicsDevice graphicsDevice)
        {
            return graphicsDevice.GraphicsProfile == GraphicsProfile.HiDef ? IndexElementSize.ThirtyTwoBits : IndexElementSize.SixteenBits;
        }
    }
}
