// Project: JdGameBase
// Filename: IMouseHandler.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework.Input;

namespace JdGameBase.Core.Interfaces {
    public interface IMouseHandler {
        /// <summary>
        /// Handles mouse input.
        /// </summary>
        /// <param name="delta">The amount of time since the last update.</param>
        /// <param name="ms">The current mouse state.</param>
        /// <param name="old">The mouse state from the last update.</param>
        void HandleMouseInput(float delta, MouseState ms, MouseState old);
    }
}
