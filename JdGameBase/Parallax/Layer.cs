// Project: JdGameBase
// Filename: Layer.cs
// 
// Author: Jason Recillo

using System;

using JdGameBase.Extensions;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Parallax {
    public class Layer {
        public readonly string Name;
        public readonly float ParallaxFactor;
        public readonly float PositionY;
        public readonly float Scale;
        public readonly Texture2D Texture;
        private float _scrollX;

        #region Constructors

        private Layer(Texture2D texture, string name, float factor) {
            Texture = texture;
            ParallaxFactor = factor;
            Name = name;
        }

        private Layer(ContentManager content, string name, float factor)
            : this(content.Load<Texture2D>(name), name, factor) { }

        private Layer(Texture2D texture, string name, float factor, float scale)
            : this(texture, name, factor) {
            Scale = scale;
        }

        private Layer(ContentManager content, string name, float factor, float scale)
            : this(content, name, factor) {
            Scale = scale;
        }

        public Layer(Texture2D texture, string name, float factor, float scale, float yPosition)
            : this(texture, name, factor, scale) {
            PositionY = yPosition;
        }

        public Layer(ContentManager content, string name, float factor, float scale, float yPosition)
            : this(content, name, factor, scale) {
            PositionY = yPosition;
        }

        public Layer(Texture2D texture, string name, float factor, bool fullscreen, Rectangle renderingArea)
            : this(texture, name, factor) {
            if (fullscreen)
                Scale = (float) renderingArea.Width / Texture.Width;
            else
                Scale = 1f;
        }

        public Layer(ContentManager content, string name, float factor, bool fullscreen, Rectangle renderingArea)
            : this(content, name, factor) {
            if (fullscreen)
                Scale = (float) renderingArea.Width / Texture.Width;
            else
                Scale = 1f;
        }

        public Layer(Texture2D texture, string name, float factor, float scale, Anchor anchor, Rectangle renderingArea)
            : this(texture, name, factor, scale) {
            switch (anchor) {
                case Anchor.Top:
                    PositionY = 0;
                    break;
                case Anchor.Bottom:
                    PositionY = renderingArea.Height - (Texture.Height * scale);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("anchor");
            }
        }

        public Layer(ContentManager manager, string name, float factor, float scale, Anchor anchor, Rectangle renderingArea)
            : this(manager, name, factor, scale) {
            switch (anchor) {
                case Anchor.Top:
                    PositionY = 0;
                    break;
                case Anchor.Bottom:
                    PositionY = renderingArea.Height - (Texture.Height * scale);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("anchor");
            }
        }

        #endregion

        public float ScrollX { get { return _scrollX; } set { _scrollX = Math.Abs(value) > (Texture.Width * Scale) ? 0 : value; } }

        public override string ToString() {
            return "Texture: {0}, factor: {1}, X: {2}, Y: {3}".Fmt(Name, ParallaxFactor, ScrollX, PositionY);
        }
    }
}
