using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JdGameBase.Core.GameComponents;
using JdGameBase.Core.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Core.Scenes {
    public enum SceneState {
        /// <summary>
        /// The scene is currently active.
        /// </summary>
        Active,

        /// <summary>
        /// The scene is currently inactive.
        /// </summary>
        Inactive,

        /// <summary>
        /// The scene is in the process of becoming active.
        /// </summary>
        Entering,

        /// <summary>
        /// The scene is in the process of becoming inactive.
        /// </summary>
        Leaving
    }

    /// <summary>
    /// Manages a single scene's state and resources.
    /// </summary>
    public interface IScene : IGameComponent, IUpdatableEntity, IInputHandler {
        #region Properties

        /// <summary>
        /// A unique name used to identify itself to the SceneManager.
        /// </summary>
        string Name { get; set; }

        SceneManager Parent { get; set; }
        float TransitionDelta { get; }
        SceneState CurrentState { get; }
        GraphicsDevice GraphicsDevice { get; }
        JdGame Game { get; }

        #endregion

        void LoadContent(ContentManager content);
        void UnloadContent();

        /// <summary>
        /// Called when this scene should pause itself.
        /// </summary>
        void OnPause();

        /// <summary>
        /// Called when this scene should resume from its paused state.
        /// </summary>
        void OnResume();

        /// <summary>
        /// Called when this scene will become active.
        /// </summary>
        void OnEnter();

        /// <summary>
        /// Called when this scene will become inactive.
        /// </summary>
        void OnLeave();

        /// <summary>
        /// Called when this scene should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void Draw(GameTime gameTime);
    }
}
