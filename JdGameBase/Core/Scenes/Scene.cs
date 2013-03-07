// Project: JdGameBase
// Filename: Scene.cs
// 
// Author: Jason Recillo

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JdGameBase.Core.Scenes {
    public class Scene : IScene {
        #region Properties

        /// <summary>
        /// A unique name used to identify itself to the SceneManager.
        /// </summary>
        public string Name { get; set; }

        public SceneManager Parent { get; set; }
        public float TransitionDelta { get; private set; }
        public SceneState CurrentState { get; private set; }

        #endregion

        /// <summary>
        /// Called when this scene should pause itself.
        /// </summary>
        public virtual void OnPause() { }

        /// <summary>
        /// Called when this scene should resume from its paused state.
        /// </summary>
        public virtual void OnResume() { }

        /// <summary>
        /// Called when this scene will become active.
        /// </summary>
        public virtual void OnEnter() {
            CurrentState = SceneState.Entering;
        }

        /// <summary>
        /// Called when this scene will become inactive.
        /// </summary>
        public virtual void OnLeave() {
            CurrentState = SceneState.Leaving;
        }

        /// <summary>
        /// Called when this entity should update itself.
        /// </summary>
        /// <param name="delta">The time passed since the last Update (premultiplied by the current TimeScale).</param>
        /// <param name="gameTime">Provides a snapshot of timing values (unaffected by TimeScale).</param>
        public virtual void Update(float delta, GameTime gameTime) { }

        /// <summary>
        /// Called when this scene should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public virtual void Draw(GameTime gameTime) { }

        /// <summary>
        /// Handles gamepad input.
        /// </summary>
        /// <param name="delta">The amount of time since the last update.</param>
        /// <param name="player">The player associated with the current gamepad.</param>
        /// <param name="gs">The current gamepad state.</param>
        /// <param name="old">The gamepad state from the last update.</param>
        public virtual void HandleGamePadInput(float delta, PlayerIndex player, GamePadState gs, GamePadState old) { }

        /// <summary>
        /// Handles keyboard input.
        /// </summary>
        /// <param name="delta">The amount of time since the last update.</param>
        /// <param name="ks">The current keyboard state.</param>
        /// <param name="old">The keyboard state from the last update.</param>
        public virtual void HandleKeyboardInput(float delta, KeyboardState ks, KeyboardState old) { }

        /// <summary>
        /// Handles mouse input.
        /// </summary>
        /// <param name="delta">The amount of time since the last update.</param>
        /// <param name="ms">The current mouse state.</param>
        /// <param name="old">The mouse state from the last update.</param>
        public virtual void HandleMouseInput(float delta, MouseState ms, MouseState old) { }
    }
}
