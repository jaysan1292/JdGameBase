// Project: JdGameBase
// Filename: InputManager.cs
// 
// Author: Jason Recillo

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JdGameBase.Core.GameComponents;
using JdGameBase.Core.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JdGameBase.Core.Services {
    public class InputManager : JdComponent {
        private readonly GamePadState[] _oldGamePadStates = new GamePadState[4];
        private KeyboardState _oldKeyboardState;
#if !XBOX360
        private MouseState _oldMouseState;
#endif

        public InputManager(Game game)
            : base(game) { }

        public event EventHandler<GamePadDisconnectedEventArgs> OnGamePadDisconnected; //TODO: OnGamePadDisconnected

        public void HandleInput(IInputHandler game, float delta) {
            HandleInput(delta, game.HandleKeyboardInput);
#if !XBOX360
            HandleInput(delta, game.HandleMouseInput);
#endif
            HandleInput(delta, PlayerIndex.One, game.HandleGamePadInput);
            HandleInput(delta, PlayerIndex.Two, game.HandleGamePadInput);
            HandleInput(delta, PlayerIndex.Three, game.HandleGamePadInput);
            HandleInput(delta, PlayerIndex.Four, game.HandleGamePadInput);
        }

        public void HandleInput(float delta, Action<float, KeyboardState, KeyboardState> action) {
            var ks = Keyboard.GetState();
            action.Invoke(delta, Keyboard.GetState(), _oldKeyboardState);
            _oldKeyboardState = ks;
        }

#if !XBOX360
        public void HandleInput(float delta, Action<float, MouseState, MouseState> action) {
            var ms = Mouse.GetState();
            action.Invoke(delta, ms, _oldMouseState);
            _oldMouseState = ms;
        }
#endif

        public void HandleInput(float delta, PlayerIndex idx, Action<float, PlayerIndex, GamePadState, GamePadState> action) {
            var gps = GamePad.GetState(idx);
            GamePadState previous;
            switch (idx) {
                case PlayerIndex.One:
                    previous = _oldGamePadStates[0];
                    break;
                case PlayerIndex.Two:
                    previous = _oldGamePadStates[1];
                    break;
                case PlayerIndex.Three:
                    previous = _oldGamePadStates[2];
                    break;
                case PlayerIndex.Four:
                    previous = _oldGamePadStates[3];
                    break;
                default:
                    throw new ArgumentOutOfRangeException("idx");
            }
            if (!gps.IsConnected && previous.IsConnected) {
                if (OnGamePadDisconnected != null)
                    OnGamePadDisconnected.Invoke(Game, new GamePadDisconnectedEventArgs(idx));
                return;
            }
            HandleGamePadInput(delta, idx, action, gps, ref previous);
        }

        private static void HandleGamePadInput(float delta, PlayerIndex idx, Action<float, PlayerIndex, GamePadState, GamePadState> action, GamePadState gamePadState, ref GamePadState oldGamePadState) {
            action.Invoke(delta, idx, gamePadState, oldGamePadState);
            oldGamePadState = gamePadState;
        }

        public class GamePadDisconnectedEventArgs : EventArgs {
            public PlayerIndex DisconnectedGamePad;

            public GamePadDisconnectedEventArgs(PlayerIndex idx) {
                DisconnectedGamePad = idx;
            }
        }
    }
}
