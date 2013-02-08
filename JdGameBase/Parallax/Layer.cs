// Project: JdGameBase
// Filename: Layer.cs
// 
// Author: Jason Recillo

using System;

using JdGameBase.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Parallax {
    public class Layer {
        public readonly bool IsSeamless;
        public readonly int Order;
        public readonly Sprite Sprite;
        public Vector2 Parallax;

        public Layer(Sprite sprite, int zOrder, Vector2 parallax, bool seamless = false) {
            if (!sprite.SourceRect.HasValue) throw new ArgumentException("sprite must have a source rectangle");
            if (sprite.SourceRect != sprite.Texture.Bounds) {
                var srect = sprite.SourceRect.Value;
                var tex = new Texture2D(sprite.Texture.GraphicsDevice, srect.Width, srect.Height);
                var data = new Color[srect.Width * srect.Height];
                sprite.Texture.GetData(0, srect, data, 0, data.Length);
                tex.SetData(data);
                sprite.Texture = tex;
            }
            Sprite = sprite;
            Order = zOrder;
            Parallax = parallax;
            IsSeamless = seamless;
        }
    }
}
