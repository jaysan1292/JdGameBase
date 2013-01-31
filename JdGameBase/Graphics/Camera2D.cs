// Project: JdGameBase
// Filename: Camera2D.cs
// 
// Author: Jason Recillo

using System;

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
        protected float ViewportHeight;
        protected float ViewportWidth;
        private Game _game;
        private Vector2 _position;

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="game">Game that the game component should be attached to.</param>
        public Camera2D(Game game)
            : base(game) {
            _game = game;
        }

        #region Properties

        //        public Rectangle Viewport { get { return Matrix.tr } }
        //        public Rectangle Viewport { get { return new Rectangle(0, 0, (int) ViewportWidth, (int) ViewportHeight); } }

        /// <summary>
        /// Gets or sets the position of the camera.
        /// </summary>
        public Vector2 Position { get { return _position; } set { _position = value; } }

        /// <summary>
        /// Gets or sets the rotation of the camera.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Gets the origin of the viewport (accounts for Scale).
        /// </summary>
        public Vector2 Origin { get; private set; }

        /// <summary>
        /// Gets or sets the scale of the camera.
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// Gets the screen center (does not account for Scale).
        /// </summary>
        public Vector2 ScreenCenter { get; protected set; }

        /// <summary>
        /// Gets the transform that can be applied to 
        /// the SpriteBatch class.
        /// </summary>
        /// <see cref="SpriteBatch"/>
        public Matrix Transform { get; private set; }

        /// <summary>
        /// Gets or sets the focus of the Camera.
        /// </summary>
        public IFocusable Focus { get; set; }

        /// <summary>
        /// Gets or sets the move speed of the camera.
        /// The camera will tween to its destination.
        /// </summary>
        public float MoveSpeed { get; set; }

        #endregion

        /// <summary>
        /// Determines whether the target is in view given the specified position.
        /// This can be used to increase performance by not drawing objects 
        /// directly in the viewport.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="texture"></param>
        /// <returns>True if the target is in view at the specified position. False otherwise.</returns>
        public bool IsInView(Vector2 position, Texture2D texture) {
            return IsInView(position, texture.Bounds);
        }

        public override void Initialize() {
            ViewportWidth = Game.GraphicsDevice.Viewport.Width;
            ViewportHeight = Game.GraphicsDevice.Viewport.Height;

            ScreenCenter = new Vector2(ViewportWidth / 2f, ViewportHeight / 2f);
            Scale = 1;
            MoveSpeed = 1.25f;

            base.Initialize();
        }

        public override void Update(float delta) {
            //TODO: Dynamically zoom to keep all entities on screen
            Origin = ScreenCenter / Scale;

            // Create the Transform used by any
            // SpriteBatch process
            Transform = Matrix.Identity *
                        Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                        Matrix.CreateRotationZ(Rotation) *
                        Matrix.CreateTranslation(Origin.X, Origin.Y, 0) *
                        Matrix.CreateScale(Scale);

            // Move the camera to the position that it needs to go
            _position.X += (Focus.FocusPosition.X - Position.X) * MoveSpeed * delta;
            _position.Y += (Focus.FocusPosition.Y - Position.Y) * MoveSpeed * delta;

            base.Update(delta);
        }

        /// <summary>
        /// Determines whether the target is in view given the specified position.
        /// This can be used to increase performance by not drawing objects 
        /// directly in the viewport.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>True if the target is in view at the specified position. False otherwise.</returns>
        public bool IsInView(Entity entity) {
            return IsInView(entity.BoundingBox.TopLeft(), entity.BoundingBox);
        }

        /// <summary>
        /// Determines whether the target is in view given the specified position.
        /// This can be used to increase performance by not drawing objects 
        /// directly in the viewport.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="bounds"></param>
        /// <returns>True if the target is in view at the specified position. False otherwise.</returns>
        public bool IsInView(Vector2 position, Rectangle bounds) {
            // If the object is not within the horizontal bounds of the screen
            var outHorizontal = (position.X + bounds.Width) < (Position.X - Origin.X) || (position.X) > (Position.X + Origin.X);

            // If the object is not within the vertical bounds of the screen
            var outVertical = (position.Y + bounds.Height) < (Position.Y - Origin.Y) || (position.Y) > (Position.Y + Origin.Y);

            return !outHorizontal && !outVertical;
        }
    }
}
