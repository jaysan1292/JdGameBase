// Project: JdGameBase
// Filename: SceneManager.cs
// 
// Author: Jason Recillo

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JdGameBase.Core.GameComponents;
using JdGameBase.Extensions;
using JdGameBase.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Core.Scenes {
    /// <summary>
    /// Manages scenes and updates the currently active scene.
    /// </summary>
    public class SceneManager : JdDrawableComponent {
        #region Fields

        private List<IScene> _allScenes;
        private Stack<IScene> _activeScenes;
        private SpriteBatch _spriteBatch;
        private Texture2D _blank;

        #endregion

        public string ActiveScene { get { return _activeScenes.Peek().Name; } }

        public SceneManager(JdGame game)
            : base(game) {
            _allScenes = new List<IScene>();
            _activeScenes = new Stack<IScene>();
        }

        public override void Initialize() {
            base.Initialize();
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            _blank = ColorTexture.Create(GraphicsDevice, Color.White);
        }

        protected override void LoadContent() {
            _allScenes.ForEach(x => x.LoadContent(Game.Content));
            base.LoadContent();
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
            _allScenes.Add(scene);
        }

        public void RemoveScene(string sceneName) {
            var scene = GetScene(sceneName);
            _allScenes.Remove(scene);
            _activeScenes.ToList().Remove(scene);
        }

        private IScene GetScene(string sceneName) {
            if (string.IsNullOrWhiteSpace(sceneName)) throw new ArgumentNullException("sceneName");
            return _allScenes.Find(x => x.Name == sceneName);
        }
    }
}
