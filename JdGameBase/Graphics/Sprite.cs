// Project: JdGameBase
// Filename: Sprite.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Graphics {
    public class Sprite {
        public Color Color = Color.White;
        public SpriteEffects Effects = SpriteEffects.None;
        public float LayerDepth = 0;
        public Vector2 Origin = Vector2.Zero;
        public Vector2 Position;
        public float Rotation = 0f;
        public float Scale = 1f;
        public Rectangle? SourceRect = null;
        public Texture2D Texture;

        public void Draw(SpriteBatch spriteBatch) {
            if (Texture == null) return;

            spriteBatch.Draw(Texture,
                             Position,
                             SourceRect,
                             Color,
                             Rotation,
                             Origin,
                             Scale,
                             Effects,
                             LayerDepth);
        }
    }
}
