using System;

using JdGameBase.Core.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Graphics {
    public class Sprite :ISprite{
        #region ISprite Implementation

        private Color _color = Color.White;
        private SpriteEffects _effects = SpriteEffects.None;
        private float _layerDepth;
        private Vector2 _origin = Vector2.Zero;
        private Vector2 _position;
        private float _rotation;
        private float _scale = 1f;
        private Rectangle? _sourceRect;
        private Texture2D _texture;

        public Color Color { get { return _color; } set { _color = value; } }
        public SpriteEffects Effects { get { return _effects; } set { _effects = value; } }
        public float LayerDepth { get { return _layerDepth; } set { _layerDepth = value; } }
        public Vector2 Origin { get { return _origin; } set { _origin = value; } }
        public Vector2 Position { get { return _position; } set { _position = value; } }
        public float Rotation { get { return _rotation; } set { _rotation = value; } }
        public float Scale { get { return _scale; } set { _scale = value; } }
        public Rectangle? SourceRect { get { return _sourceRect; } set { _sourceRect = value; } }
        public Texture2D Texture { get { return _texture; } set { _texture = value; } }

        public float Width{get { return Texture.Width * Scale; }}
        public float Height{get { return Texture.Height * Scale; }}

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
