// Project: JdGameBase
// Filename: ColorTexture.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Graphics {
    // slightly modified from: http://www.xnawiki.com/index.php/Single-Color_Texture
    public static class ColorTexture {
        public static Texture2D Create(GraphicsDevice graphicsDevice, Color color) {
            return Create(graphicsDevice, 1, 1, color);
        }

        public static Texture2D Create(GraphicsDevice graphicsDevice, int width = 1, int height = 1, Color color = new Color()) {
            var texture = new Texture2D(graphicsDevice,
                                        width,
                                        height);

            var colors = new Color[width * height];
            for (var i = 0; i < colors.Length; i++) colors[i] = new Color(color.ToVector4());

            texture.SetData(colors);

            return texture;
        }
    }
}
