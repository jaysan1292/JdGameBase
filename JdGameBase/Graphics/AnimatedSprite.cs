using System;
using System.Collections.Generic;
using System.Diagnostics;

using JdGameBase.Core;
using JdGameBase.Core.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Graphics {
    public class AnimatedSprite : Entity, ISprite {
        public static readonly float Fps16 = FramesPerSecond(16);
        public static readonly float Fps24 = FramesPerSecond(24);
        public static readonly float Fps30 = FramesPerSecond(30);
        public static readonly float Fps60 = FramesPerSecond(60);

        private readonly bool _bounce;
        private readonly int _columns;
        private readonly float _frameRate;
        private readonly int _rows;
        private readonly Sprite _spriteSheet;
        private int _currentFrame;
        private bool _forward;
        private Rectangle[] _frames;
        private Rectangle? _sheetRect;
        private float _timeSinceLastFrame;
        private bool _active;

        public bool Active {
            get { return _active; }
            set {
                _active = value;
                if (!value) ResetAnimation();
            }
        }

        public Rectangle? SheetRect { get { return _sheetRect; } }
        public float FrameRate { get { return _frameRate; } }

        public event EventHandler OnAnimationCompleted;

        #region ISprite Implementation

        public Color Color { get { return _spriteSheet.Color; } set { _spriteSheet.Color = value; } }
        public SpriteEffects Effects { get { return _spriteSheet.Effects; } set { _spriteSheet.Effects = value; } }
        public float LayerDepth { get { return _spriteSheet.LayerDepth; } set { _spriteSheet.LayerDepth = value; } }
        public Vector2 Origin { get { return _spriteSheet.Origin; } set { _spriteSheet.Origin = value; } }
        public float Rotation { get { return _spriteSheet.Rotation; } set { _spriteSheet.Rotation = value; } }
        public float Scale { get { return _spriteSheet.Scale; } set { _spriteSheet.Scale = value; } }
        public Vector2 Position { get { return _spriteSheet.Position; } set { _spriteSheet.Position = value; } }
        public Texture2D Texture { get { return _spriteSheet.Texture; } set { InitTexture((_spriteSheet.Texture = value)); } }
        public Rectangle? SourceRect { get { return _frames[_currentFrame]; } set { throw new InvalidOperationException("Cannot modify source rectangle of animated sprite"); } }

        public float Width { get { return ((SourceRect.HasValue ? SourceRect.Value.Width : Texture.Width / _columns)) * Scale; } }
        public float Height { get { return ((SourceRect.HasValue ? SourceRect.Value.Height : Texture.Height / _rows)) * Scale; } }

        #endregion

        #region Constructors

        private AnimatedSprite(bool bounce) {
            _currentFrame = 0;
            _bounce = bounce;
            _forward = true;
            Active = true;
        }

        public AnimatedSprite(AnimatedSprite sprite) {
            _bounce = sprite._bounce;
            _frameRate = sprite._frameRate;
            _frames = sprite._frames;
            _spriteSheet = sprite._spriteSheet;
            _forward = sprite._forward;
            _columns = sprite._columns;
            _rows = sprite._rows;
            _sheetRect = sprite._sheetRect;

            Color = sprite.Color;
            Effects = sprite.Effects;
            LayerDepth = sprite.LayerDepth;
            Origin = sprite.Origin;
            Position = sprite.Position;
            Rotation = sprite.Rotation;
            Scale = sprite.Scale;
            Texture = sprite.Texture;
        }

        public AnimatedSprite(float frameRate, int cols, int rows, bool bounce = false)
            : this(null, null, frameRate, cols, rows, bounce) { }

        public AnimatedSprite(Rectangle? sourceRect, float frameRate, int cols, int rows, bool bounce = false)
            : this(null, sourceRect, frameRate, cols, rows, bounce) { }

        public AnimatedSprite(Texture2D spriteSheet, Rectangle? sourceRect, float frameRate, int cols, int rows, bool bounce = false)
            : this(bounce) {
            if (cols < 1 || rows < 1) throw new InvalidOperationException("spritesheet must have at least one row and column");
            _spriteSheet = new Sprite {
                Texture = spriteSheet
            };
            _frameRate = frameRate;
            _columns = cols;
            _rows = rows;
            _sheetRect = sourceRect;

            InitTexture(spriteSheet);
        }

        public AnimatedSprite(Texture2D spriteSheet, Vector2 position, float frameRate, params Rectangle[] frames)
            : this(spriteSheet, position, frameRate, false, frames) { }

        public AnimatedSprite(Texture2D spriteSheet, Vector2 position, float frameRate, bool bounce, params Rectangle[] frames)
            : this(bounce) {
            if (frames.Length == 0) throw new ArgumentException("You must specify at least one frame.");

            _spriteSheet = new Sprite {
                Texture = spriteSheet,
                Position = position,
                SourceRect = frames[0]
            };
            _frames = frames;
            _frameRate = frameRate;
        }

        #endregion

        public void ResetAnimation() {
            _currentFrame = 0;
            _shouldInvokeEvent = false;
            _timeSinceLastFrame = 0f;
            _forward = true;
        }

        private bool _shouldInvokeEvent;

        public override void Draw(SpriteBatch spriteBatch) {
            if (!Active) return;
            _spriteSheet.Draw(spriteBatch);

            // TODO: When in a SpriteSheet, Update() is called regardless of whether this sprite is being drawn, causing its AnimationCompleted event to fire when not needed.
            if (_shouldInvokeEvent && OnAnimationCompleted != null) OnAnimationCompleted.Invoke(this, EventArgs.Empty);
        }

        public override void Update(float delta) {
            if (!Active) return;
            _timeSinceLastFrame += delta;

            if (!(_timeSinceLastFrame > _frameRate)) return;

            if (_bounce) {
                if (_currentFrame == 0 && !_forward ||
                    _currentFrame == _frames.Length - 1 && _forward)
                    _forward = !_forward;
                _currentFrame += _forward ? 1 : -1;
            } else _currentFrame = (_currentFrame + 1) % _frames.Length;

            if (_currentFrame == 0) _shouldInvokeEvent = false;
            _shouldInvokeEvent = (_currentFrame == _frames.Length - 1);

            _spriteSheet.SourceRect = _frames[_currentFrame];
            _timeSinceLastFrame = 0f;
        }

        private void InitTexture(Texture2D spriteSheet) {
            var frames = new List<Rectangle>(_columns * _rows);
            int rectWidth, rectHeight;
            int xOffset = 0, yOffset = 0;
            if (_sheetRect.HasValue) {
                rectWidth = _sheetRect.Value.Width / _columns;
                rectHeight = _sheetRect.Value.Height / _rows;
                xOffset = _sheetRect.Value.X;
                yOffset = _sheetRect.Value.Y;
            } else {
                if (spriteSheet == null) return;
                rectWidth = spriteSheet.Width / _columns;
                rectHeight = spriteSheet.Height / _rows;
            }
            for (var y = 0; y < _rows; y++) {
                for (var x = 0; x < _columns; x++) {
                    frames.Add(new Rectangle {
                        X = x * rectWidth + xOffset,
                        Y = y * rectHeight + yOffset,
                        Width = rectWidth,
                        Height = rectHeight
                    });
                }
            }
            _spriteSheet.SourceRect = frames[0];
            _frames = frames.ToArray();
        }

        [DebuggerHidden]
        public static float FramesPerSecond(uint frames) {
            if (frames == 0) throw new ArgumentException("frames must not be zero", "frames");
            return 1f / frames;
        }
    }
}
