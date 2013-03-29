using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JdGameBase.Core.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JdGameBase.Core {
    public abstract class InputHandler : IInputHandler {
        /// <summary>
        /// Handles gamepad input.
        /// </summary>
        /// <param name="delta">The amount of time since the last update.</param>
        /// <param name="player">The player associated with the current gamepad.</param>
        /// <param name="gps">The current gamepad state.</param>
        /// <param name="old">The gamepad state from the last update.</param>
        public virtual void HandleGamePadInput(float delta, PlayerIndex player, GamePadState gps, GamePadState old) { }

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
