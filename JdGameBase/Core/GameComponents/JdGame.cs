// Project: JdGameBase
// Filename: JdGame.cs
// 
// Author: Jason Recillo

using System;

using JdGameBase.Core.Interfaces;
using JdGameBase.Core.Services;
using JdGameBase.Extensions;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JdGameBase.Core.GameComponents {
    public abstract class JdGame : Game, IInputHandler {
        public static Rectangle ScreenSize = new Rectangle(0, 0, 800, 480);
        private readonly FrameRateCounter _frameRateCounter;
        private readonly GraphicsDeviceManager _graphics;
        public EntityManager EntityManager;
        protected InputManager InputManager;
        protected SpriteBatch SpriteBatch;
        protected TimeScaleManager TimeScaleManager;

        public JdGame() {
            _graphics = new GraphicsDeviceManager(this);
            InputManager = new InputManager();
            TimeScaleManager = new TimeScaleManager();
            _frameRateCounter = new FrameRateCounter();
        }

        protected int FramesPerSecond { get { return _frameRateCounter.FramesPerSecond; } }

        public virtual void HandleKeyboardInput(float delta, KeyboardState ks, KeyboardState old) { }
        public virtual void HandleMouseInput(float delta, MouseState mouseState, MouseState old) { }
        public virtual void HandleGamePadInput(float delta, PlayerIndex player, GamePadState gamePadState, GamePadState old) { }

        protected override void Initialize() {
            _graphics.PreferredBackBufferWidth = ScreenSize.Width;
            _graphics.PreferredBackBufferHeight = ScreenSize.Height;
            _graphics.ApplyChanges();
            EntityManager = new EntityManager(this);
            this.AddService(InputManager);
            this.AddService(TimeScaleManager);
            this.AddService(_frameRateCounter);
            base.Initialize();
        }

        protected override void LoadContent() {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime) {
            _frameRateCounter.Update(gameTime);
            var delta = TimeScaleManager.UpdateTimescale(gameTime);

            InputManager.HandleInput(delta, HandleKeyboardInput);
            InputManager.HandleInput(delta, HandleMouseInput);
            InputManager.HandleInput(delta, PlayerIndex.One, HandleGamePadInput);
            InputManager.HandleInput(delta, PlayerIndex.Two, HandleGamePadInput);
            InputManager.HandleInput(delta, PlayerIndex.Three, HandleGamePadInput);
            InputManager.HandleInput(delta, PlayerIndex.Four, HandleGamePadInput);

            Update(delta);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            _frameRateCounter.UpdateFrameCount();
            base.Draw(gameTime);
        }

        public virtual void Update(float delta) { }
    }
}
