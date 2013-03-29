using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework.Input;

namespace JdGameBase.Core.Interfaces {
    public interface IKeyboardHandler {
        /// <summary>
        /// Handles keyboard input.
        /// </summary>
        /// <param name="delta">The amount of time since the last update.</param>
        /// <param name="ks">The current keyboard state.</param>
        /// <param name="old">The keyboard state from the last update.</param>
        void HandleKeyboardInput(float delta, KeyboardState ks, KeyboardState old);
    }
}
