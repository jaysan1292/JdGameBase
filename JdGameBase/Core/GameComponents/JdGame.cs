// Project: JdGameBase
// Filename: JdGame.cs
// 
// Author: Jason Recillo

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JdGameBase.Core.Interfaces;
using JdGameBase.Core.Services;
using JdGameBase.Extensions;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JdGameBase.Core.GameComponents {
    public abstract class JdGame : Game, IInputHandler {
        public readonly InputManager InputManager;
        protected readonly TimeScaleManager TimeScaleManager;
        private readonly FrameRateCounter _frameRateCounter;
        private readonly GraphicsDeviceManager _graphics;
        protected EntityManager EntityManager;
        protected SpriteBatch SpriteBatch;

        public JdGame() {
            _graphics = new GraphicsDeviceManager(this) {
                SynchronizeWithVerticalRetrace = false
            };
            ScreenBounds = new Rectangle(0, 0, 800, 480);
            IsFixedTimeStep = false;

            InputManager = new InputManager(this);
            TimeScaleManager = new TimeScaleManager();
            _frameRateCounter = new FrameRateCounter();

            Content.RootDirectory = "Content";
        }

        protected int FramesPerSecond { get { return _frameRateCounter.FramesPerSecond; } }
        public float AspectRatio { get { return GraphicsDevice.Viewport.AspectRatio; } }

        public Rectangle ScreenBounds {
            get { return GraphicsDevice.Viewport.Bounds; }
            set {
                _graphics.PreferredBackBufferWidth = value.Width;
                _graphics.PreferredBackBufferHeight = value.Height;
                _graphics.ApplyChanges();
            }
        }

        public virtual void HandleKeyboardInput(float delta, KeyboardState ks, KeyboardState old) { }
        public virtual void HandleMouseInput(float delta, MouseState mouseState, MouseState old) { }
        public virtual void HandleGamePadInput(float delta, PlayerIndex player, GamePadState gamePadState, GamePadState old) { }

        protected override void Initialize() {
            EntityManager = new EntityManager(this);
            this.AddComponent(InputManager);
            this.AddService(TimeScaleManager);
            this.AddService(_frameRateCounter);
            base.Initialize();
        }

        protected override void LoadContent() {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        protected override void UnloadContent() {
            SpriteBatch.Dispose();
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime) {
            _frameRateCounter.Update(gameTime);
            var delta = TimeScaleManager.UpdateTimescale(gameTime);

            InputManager.DoUpdate(delta);

            Update(delta, gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            _frameRateCounter.UpdateFrameCount();
            base.Draw(gameTime);
        }

        /// <param name="delta">Time passed since the last call to Update (affected by TimeScale).</param>
        /// <param name="gameTime">Time passed since the last call to Update (unaffected by TimeScale).</param>
        public virtual void Update(float delta, GameTime gameTime) { }
    }
}
