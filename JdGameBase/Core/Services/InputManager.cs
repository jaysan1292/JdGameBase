// Project: JdGameBase
// Filename: InputManager.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JdGameBase.Core.Services {
    public class InputManager {
        private readonly GamePadState[] _oldGamePadStates = new GamePadState[4];
        private KeyboardState _oldKeyboardState;
        private MouseState _oldMouseState;

        public void HandleInput(float delta, Action<float, KeyboardState, KeyboardState> action) {
            var ks = Keyboard.GetState();
            action.Invoke(delta, Keyboard.GetState(), _oldKeyboardState);
            _oldKeyboardState = ks;
        }

        public void HandleInput(float delta, Action<float, MouseState, MouseState> action) {
            var ms = Mouse.GetState();
            action.Invoke(delta, ms, _oldMouseState);
            _oldMouseState = ms;
        }

        public void HandleInput(float delta, PlayerIndex idx, Action<float, PlayerIndex, GamePadState, GamePadState> action) {
            var gps = GamePad.GetState(idx);
            if (!gps.IsConnected) return;
            switch (idx) {
                case PlayerIndex.One:
                    HandleGamePadInput(delta, idx, action, gps, ref _oldGamePadStates[0]);
                    break;
                case PlayerIndex.Two:
                    HandleGamePadInput(delta, idx, action, gps, ref _oldGamePadStates[1]);
                    break;
                case PlayerIndex.Three:
                    HandleGamePadInput(delta, idx, action, gps, ref _oldGamePadStates[2]);
                    break;
                case PlayerIndex.Four:
                    HandleGamePadInput(delta, idx, action, gps, ref _oldGamePadStates[3]);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("idx");
            }
        }

        private static void HandleGamePadInput(float delta, PlayerIndex idx, Action<float, PlayerIndex, GamePadState, GamePadState> action, GamePadState gamePadState, ref GamePadState oldGamePadState) {
            action.Invoke(delta, idx, gamePadState, oldGamePadState);
            oldGamePadState = gamePadState;
        }
    }
}
