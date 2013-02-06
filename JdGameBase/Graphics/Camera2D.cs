﻿using System;
using System.Diagnostics;

using JdGameBase.Core;
using JdGameBase.Core.GameComponents;
using JdGameBase.Core.Interfaces;
using JdGameBase.Extensions;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Graphics {
    // Slightly modified from http://stackoverflow.com/questions/712296/xna-2d-camera-engine-that-follows-sprite
    public class Camera2D : JdComponent, ICamera2D {
        private static readonly Random Random = new Random();
        private Viewport _viewport;
        private Vector2 _position;
        private Rectangle? _limits;
        private Game _game;
        private float _zoom;
        private IFocusable _focus;
        

        public Camera2D(Game game)
            : base(game) {
            _game = game;
        }

        #region Properties

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
                _zoom = value;
                if (!Limits.HasValue) return;
                var minZoomX = _viewport.Width / Limits.Value.Width;
                var minZoomY = _viewport.Height / Limits.Value.Height;
                _zoom = MathHelper.Max(Zoom, MathHelper.Max(minZoomX, minZoomY));
            }
        }

        public Vector2 ScreenCenter { get; protected set; }

        public Matrix Transform { get; private set; }

        public IFocusable Focus { get { return _focus; } set {
            _focus = value;
            _position = _focus.FocusPosition;
        } }

        public float MoveSpeed { get; set; }

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
//                return;
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

        #endregion

        public override void Initialize() {
            _viewport = _game.GraphicsDevice.Viewport;
            ScreenCenter = new Vector2(_viewport.Width / 2f, _viewport.Height / 2f);
            Zoom = 1f;
            MoveSpeed = 1.25f;

            base.Initialize();
        }

        public void Shake(float duration, float magnitude = 500f, float rotation = 0.5f) {
            _currentShakeTime = 0f;
            _maxShakeTime = duration;
            _shakeMagnitude = magnitude;
            _shakeRotation = rotation;
            if (!_shaking) _shakeSavedRotation = Rotation;
            _shaking = true;
        }

        private float _shakeMagnitude;
        private float _shakeRotation;
        private bool _shaking;
        private float _currentShakeTime;
        private float _maxShakeTime;
        private float _shakeSavedRotation;
        public override void Update(float delta) {
            //TODO: Dynamically zoom to keep all entities on screen
            var focus = Focus as FocusPoint;
            if (focus != null) focus.Update(delta);

            if (_shaking) {
                _currentShakeTime += delta;

                Position += new Vector2 {
                    X = (float) ((Random.NextDouble() * 2) - 1) * _shakeMagnitude * delta,
                    Y = (float) ((Random.NextDouble() * 2) - 1) * _shakeMagnitude * delta,
                };
                var rotamount = Utilities.RandomWithinRange(_shakeSavedRotation - _shakeRotation, _shakeSavedRotation + _shakeRotation);// (float) ((Random.NextDouble() * 2) - 1) * _shakeRotation;
                Rotation += MathHelper.Clamp(rotamount * delta, _shakeSavedRotation - _shakeRotation, _shakeSavedRotation + _shakeRotation);
                if (_currentShakeTime > _maxShakeTime) _shaking = false;
                if (!_shaking) {
                    Rotation = _shakeSavedRotation;
                }
            }

            Origin = ScreenCenter / Zoom;

            // Create the Transform used by any
            // SpriteBatch process
            Transform = Matrix.Identity *
                        Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                        Matrix.CreateRotationZ(Rotation) *
                        Matrix.CreateTranslation(Origin.X, Origin.Y, 0) *
                        Matrix.CreateScale(Zoom);

            // Move the camera to the position that it needs to go
            // use the property setter, to limit the camera if needed
            Position = new Vector2(_position.X + (Focus.FocusPosition.X - Position.X) * MoveSpeed * delta,
                                   _position.Y + (Focus.FocusPosition.Y - Position.Y) * MoveSpeed * delta);

            base.Update(delta);
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

        public bool IsInView(Vector2 position, Texture2D texture) {
            return IsInView(position, texture.Bounds);
        }

        public bool IsInView(Vector2 position, Rectangle bounds) {
            // If the object is not within the horizontal bounds of the screen
            var outHorizontal = (position.X + bounds.Width) < (Position.X - Origin.X) || (position.X) > (Position.X + Origin.X);

            // If the object is not within the vertical bounds of the screen
            var outVertical = (position.Y + bounds.Height) < (Position.Y - Origin.Y) || (position.Y) > (Position.Y + Origin.Y);

            return !outHorizontal && !outVertical;
        }
    }
}