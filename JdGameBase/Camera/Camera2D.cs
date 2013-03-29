using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JdGameBase.Core;
using JdGameBase.Core.GameComponents;
using JdGameBase.Core.Interfaces;
using JdGameBase.Extensions;
using JdGameBase.Interpolators;
using JdGameBase.Utils;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Camera {
    // Slightly modified from http://stackoverflow.com/questions/712296/xna-2d-camera-engine-that-follows-sprite
    public class Camera2D : JdComponent, ICamera2D {
        private static readonly Random Random = new Random();
        private readonly Game _game;
        private IFocusable _focus;
        private Rectangle? _limits;
        private Vector2 _oldPosition;
        private Vector2 _position;
        private bool _transformDirty;
        private Viewport _viewport;
        private float _zoom;

        public Camera2D(JdGame game)
            : base(game) {
            _game = game;
        }

        #region Properties

        public Rectangle? Limits {
            get {
                if (!_limits.HasValue) return null;
                return new Rectangle {
                    X = _limits.Value.X,
                    Y = _limits.Value.Y,
                    Width = Math.Max(_viewport.Width, _limits.Value.Width),
                    Height = Math.Max(_viewport.Height, _limits.Value.Height)
                };
            }
            set {
                _limits = value;
                if (value != null) {
                    _limits = new Rectangle {
                        X = value.Value.X,
                        Y = value.Value.Y,
                        Width = Math.Max(_viewport.Width, value.Value.Width),
                        Height = Math.Max(_viewport.Height, value.Value.Height)
                    };

                    Position = Position;
                } else _limits = null;
            }
        }

        public Rectangle ScreenSize { get { return _viewport.Bounds; } }

        public Vector2 Position {
            get { return _position; }
            set {
                _position = value;
                if (!Limits.HasValue) return;
                var limit = Limits.Value;
                var minX = limit.X + _viewport.Width / 2f;
                var maxX = limit.X + limit.Width - _viewport.Width / 2f;
                var minY = limit.Y + _viewport.Height / 2f;
                var maxY = limit.Y + limit.Height - _viewport.Height / 2f;
                _position.X = MathHelper.Clamp(_position.X, minX, maxX);
                _position.Y = MathHelper.Clamp(_position.Y, minY, maxY);
            }
        }

        public float Rotation { get; set; }

        public Vector2 Origin { get; private set; }

        public float Zoom {
            get { return _zoom; }
            set {
                _zoom = MathHelper.Clamp(value, 0.1f, float.MaxValue);
                if (!Limits.HasValue) return;
                var minZoomX = _viewport.Width / Limits.Value.Width;
                var minZoomY = _viewport.Height / Limits.Value.Height;
                _zoom = MathHelper.Max(Zoom, MathHelper.Max(minZoomX, minZoomY));
            }
        }

        public Vector2 ScreenCenter { get; protected set; }

        public Matrix Transform {
            get {
                // Only recalculate the transform matrix if it needs to be changed
                if (_transformDirty) _transform = CalculateTransform();
                return _transform;
            }
        }

        public IFocusable Focus {
            get { return _focus; }
            set {
                _focus = value;
                _position = _focus.FocusPosition;
            }
        }

        public float MoveSpeed { get; set; }

        private Matrix CalculateTransform() {
            _transformDirty = false;
            return Matrix.Identity *
                   Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateTranslation(Origin.X, Origin.Y, 0) *
                   Matrix.CreateScale(Zoom);
        }

        #endregion

        public bool IsInView(Vector2 position, Texture2D texture) {
            return IsInView(position, texture.Bounds);
        }

        public override void Initialize() {
            _viewport = _game.GraphicsDevice.Viewport;
            ScreenCenter = new Vector2(_viewport.Width / 2f, _viewport.Height / 2f);
            Zoom = 1f;
            MoveSpeed = 1.25f;
            _magnitudeInterpolator = new FloatInterpolator();
            _postShakeRotationInterpolator = new FloatInterpolator();
            OnFinishedShaking += OnCameraFinishedShaking;

            base.Initialize();
        }

        public override void Update(float delta, GameTime gameTime) {
            //TODO: Dynamically zoom to keep all entities on screen
            var focus = Focus as FocusPoint;
            if (focus != null) focus.Update(delta, gameTime);

            DoShake(delta);

            Origin = ScreenCenter / Zoom;

            // Move the camera to the position that it needs to go
            // use the property setter, to limit the camera if needed
            Position = new Vector2(_position.X + (Focus.FocusPosition.X - Position.X) * MoveSpeed * delta,
                                   _position.Y + (Focus.FocusPosition.Y - Position.Y) * MoveSpeed * delta);

            if (Position != _oldPosition) _transformDirty = true;

            _oldPosition = Position;

            base.Update(delta, gameTime);
        }

        public Matrix GetViewMatrix(Vector2 parallax) {
            return Matrix.CreateTranslation(new Vector3(-Position * parallax, 0f)) *
                   Matrix.CreateTranslation(new Vector3(-Origin, 0f)) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(Zoom, Zoom, 1) *
                   Matrix.CreateTranslation(new Vector3(Origin, 0f));
        }

        public Vector2 WorldToScreen(Vector2 worldPosition) {
            return WorldToScreen(worldPosition, Vector2.One);
        }

        public Vector2 WorldToScreen(Vector2 worldPosition, Vector2 parallax) {
            return Vector2.Transform(worldPosition, GetViewMatrix(parallax));
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition) {
            return ScreenToWorld(screenPosition, Vector2.One);
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition, Vector2 parallax) {
            return Vector2.Transform(screenPosition, Matrix.Invert(GetViewMatrix(parallax)));
        }

        public bool IsInView(Entity entity) {
            return IsInView(entity.BoundingBox.TopLeft(), entity.BoundingBox);
        }

        public bool IsInView(Vector2 position, Rectangle bounds) {
            // If the object is not within the horizontal bounds of the screen
            var outHorizontal = (position.X + bounds.Width) < (Position.X - Origin.X) || (position.X) > (Position.X + Origin.X);

            // If the object is not within the vertical bounds of the screen
            var outVertical = (position.Y + bounds.Height) < (Position.Y - Origin.Y) || (position.Y) > (Position.Y + Origin.Y);

            return !outHorizontal && !outVertical;
        }

        #region Camera Shake

        public bool Shaking;
        private float _currentShakeTime;
        private FloatInterpolator _magnitudeInterpolator;
        private float _maxShakeTime;
        private FloatInterpolator _postShakeRotationInterpolator;
        private float _shakeMagnitude;
        private float _shakeRotation;
        private float _shakeSavedRotation;
        private Matrix _transform;
        public event EventHandler OnFinishedShaking;

        public void Shake(float duration, float magnitude = 5f, float rotation = 0.005f) {
            if (Shaking) return;
            _currentShakeTime = 0f;
            _maxShakeTime = duration;
            _shakeMagnitude = magnitude;
            _shakeRotation = rotation;
            if (!_postShakeRotationInterpolator.IsActive) _shakeSavedRotation = Rotation;
            Shaking = true;
        }

        private void DoShake(float delta) {
            if (_postShakeRotationInterpolator.IsActive) Rotation = _postShakeRotationInterpolator.Update(delta);
            if (!Shaking) return;
            if (!_magnitudeInterpolator.IsActive) _magnitudeInterpolator.Start(_shakeMagnitude, 0, _maxShakeTime, true);
            _shakeMagnitude = _magnitudeInterpolator.Update(delta);
            _currentShakeTime += delta;

            var off = new Vector2 {
                X = Noise.Generate(_currentShakeTime) * _shakeMagnitude * (Utilities.Chance(0.5f) ? 1 : -1),
                Y = Noise.Generate(_currentShakeTime) * _shakeMagnitude * (Utilities.Chance(0.5f) ? 1 : -1),
            };
            Position += off;

            var rotamount = Noise.Generate(Position.X, Math.Abs(Rotation)) * (Utilities.Chance(0.5f) ? 1 : -1);
            Rotation += MathHelper.Clamp(rotamount,
                                         _shakeSavedRotation - _shakeRotation,
                                         _shakeSavedRotation + _shakeRotation);

            if (_currentShakeTime > _maxShakeTime) {
                Shaking = false;
                if (OnFinishedShaking != null) OnFinishedShaking.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnCameraFinishedShaking(object sender, EventArgs e) {
            if (!_postShakeRotationInterpolator.IsActive) _postShakeRotationInterpolator.Start(Rotation, _shakeSavedRotation, 0.25f);
        }

        #endregion
    }
}
