using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JdGameBase.Core.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Graphics {
    public class Sprite : ISprite {
        public static readonly Sprite Empty = new Sprite();

        #region ISprite Implementation

        private Color _color = Color.White;
        private SpriteEffects _effects = SpriteEffects.None;
        private Vector2 _origin = Vector2.Zero;
        private float _scale = 1f;
        [ContentSerializerIgnore] private Texture2D _texture;

        public Color Color { get { return _color; } set { _color = value; } }
        public SpriteEffects Effects { get { return _effects; } set { _effects = value; } }
        public float LayerDepth { get; set; }
        public Vector2 Origin { get { return _origin; } set { _origin = value; } }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float Scale { get { return _scale; } set { _scale = value; } }
        public Rectangle? SourceRect { get; set; }
        public Texture2D Texture { get { return _texture; } set { _texture = value; } }

        public float Width { get { return Texture.Width * Scale; } }
        public float Height { get { return Texture.Height * Scale; } }

        #endregion

        public Sprite() { }

        public Sprite(Sprite sprite) {
            Color = sprite.Color;
            Effects = sprite.Effects;
            LayerDepth = sprite.LayerDepth;
            Origin = sprite.Origin;
            Position = sprite.Position;
            Rotation = sprite.Rotation;
            Scale = sprite.Scale;
            SourceRect = sprite.SourceRect;
            Texture = sprite.Texture;
        }

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
