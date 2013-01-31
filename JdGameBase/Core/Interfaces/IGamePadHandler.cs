// Project: JdGameBase
// Filename: IGamePadHandler.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JdGameBase.Core.Interfaces {
    public interface IGamePadHandler {
        /// <summary>
        /// Handles gamepad input.
        /// </summary>
        /// <param name="delta">The amount of time since the last update.</param>
        /// <param name="player">The player associated with the current gamepad.</param>
        /// <param name="gs">The current gamepad state.</param>
        /// <param name="old">The gamepad state from the last update.</param>
        void HandleGamePadInput(float delta, PlayerIndex player, GamePadState gs, GamePadState old);
    }
}
