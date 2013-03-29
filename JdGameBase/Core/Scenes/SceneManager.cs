using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JdGameBase.Core.GameComponents;
using JdGameBase.Core.Interfaces;
using JdGameBase.Extensions;
using JdGameBase.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JdGameBase.Core.Scenes {
    /// <summary>
    /// Manages scenes and updates the currently active scene.
    /// </summary>
    public class SceneManager : JdDrawableComponent, IInputHandler {
        #region Fields

        private List<IScene> _allScenes;
        private Stack<IScene> _activeScenes;
        private SpriteBatch _spriteBatch;
        private Texture2D _blank;
        private bool _initialized;

        #endregion

        public string ActiveScene { get { return _activeScenes.Peek().Name; } }

        public SceneManager(JdGame game)
            : base(game) {
            _allScenes = new List<IScene>();
            _activeScenes = new Stack<IScene>();
        }

        public override void Initialize() {
            _allScenes.ForEach(x => x.Initialize());
            base.Initialize();
            _initialized = true;
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            _blank = ColorTexture.Create(GraphicsDevice, Color.White);
            _allScenes.ForEach(x => x.LoadContent(Game.Content));
            base.LoadContent();
        }

        protected override void UnloadContent() {
            _allScenes.ForEach(x => x.UnloadContent());
            base.UnloadContent();
        }

        public void Push(string sceneName) {
            var scene = GetScene(sceneName);
            if (scene == null)
                throw new InvalidOperationException("The scene '{0}' does not exist in this SceneManager.".Fmt(sceneName));

            _activeScenes.Push(scene);
        }

        public void Pop() {
            _activeScenes.Pop();
        }

        public override void Update(float delta, GameTime gameTime) {
            _activeScenes.ForEach(x => {
                Game.InputManager.HandleInput(x, delta);
                x.Update(delta, gameTime);
            });
        }

        public override void Draw(GameTime gameTime) {
            _activeScenes.ForEach(x => x.Draw(gameTime));
        }

        public void DrawFadeToBlack(float alpha) {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_blank, Game.ScreenBounds, Color.Black * alpha);
            _spriteBatch.End();
        }

        public void AddScene(IScene scene) {
            scene.Parent = this;
            //            if (!_initialized) {
            //                scene.Initialize();
            //                scene.LoadContent(Game.Content);
            //            }
            _allScenes.Add(scene);
            if (_activeScenes.Empty()) _activeScenes.Push(scene);
        }

        public void RemoveScene(string sceneName) {
            var scene = GetScene(sceneName);
            scene.UnloadContent();
            _allScenes.Remove(scene);
            _activeScenes.ToList().Remove(scene);
        }

        private IScene GetScene(string sceneName) {
            if (string.IsNullOrWhiteSpace(sceneName)) throw new ArgumentNullException("sceneName");
            return _allScenes.Find(x => x.Name == sceneName);
        }

        public void HandleGamePadInput(float delta, PlayerIndex player, GamePadState gps, GamePadState old) {
            _activeScenes.ForEach(x => x.HandleGamePadInput(delta, player, gps, old));
        }

        public void HandleKeyboardInput(float delta, KeyboardState ks, KeyboardState old) {
            _activeScenes.ForEach(x => x.HandleKeyboardInput(delta, ks, old));
        }

        public void HandleMouseInput(float delta, MouseState ms, MouseState old) {
            _activeScenes.ForEach(x => x.HandleMouseInput(delta, ms, old));
        }
    }
}
