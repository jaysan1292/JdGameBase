using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JdGameBase.Core.GameComponents;
using JdGameBase.Core.Interfaces;
using JdGameBase.Extensions;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JdGameBase.Core.Services {
    public class InputManager : JdComponent {
        private readonly GamePadState[] _currentGamePadStates = new GamePadState[4];
        private readonly GamePadState[] _oldGamePadStates = new GamePadState[4];
        private KeyboardState _currentKeyboardState;
        private KeyboardState _oldKeyboardState;
#if !XBOX360
        private MouseState _currentMouseState;
        private MouseState _oldMouseState;
#endif

        public InputManager(JdGame game)
            : base(game) { }

        public event EventHandler<GamePadDisconnectedEventArgs> OnGamePadDisconnected; //TODO: OnGamePadDisconnected

        public void DoUpdate(float delta) {
            _currentKeyboardState = Keyboard.GetState();
#if !XBOX360
            _currentMouseState = Mouse.GetState();
#endif
            _currentGamePadStates[0] = GamePad.GetState(PlayerIndex.One);
            _currentGamePadStates[1] = GamePad.GetState(PlayerIndex.Two);
            _currentGamePadStates[2] = GamePad.GetState(PlayerIndex.Three);
            _currentGamePadStates[3] = GamePad.GetState(PlayerIndex.Four);

            HandleInput(Game, delta);
            Game.Components
                .Where(x => x.GetType().Implements<IInputHandler>())
                .ForEach(x => HandleInput((IInputHandler) x, delta));

            _oldKeyboardState = _currentKeyboardState;
#if !XBOX360
            _oldMouseState = _currentMouseState;
#endif
            _oldGamePadStates[0] = _currentGamePadStates[0];
            _oldGamePadStates[1] = _currentGamePadStates[1];
            _oldGamePadStates[2] = _currentGamePadStates[2];
            _oldGamePadStates[3] = _currentGamePadStates[3];
        }

        public void HandleInput(IInputHandler handler, float delta) {
            HandleInput(delta, handler.HandleKeyboardInput);
#if !XBOX360
            HandleInput(delta, handler.HandleMouseInput);
#endif
            HandleInput(delta, PlayerIndex.One, handler.HandleGamePadInput);
            HandleInput(delta, PlayerIndex.Two, handler.HandleGamePadInput);
            HandleInput(delta, PlayerIndex.Three, handler.HandleGamePadInput);
            HandleInput(delta, PlayerIndex.Four, handler.HandleGamePadInput);
        }

        public void HandleInput(float delta, Action<float, KeyboardState, KeyboardState> action) {
            action.Invoke(delta, _currentKeyboardState, _oldKeyboardState);
        }

#if !XBOX360
        public void HandleInput(float delta, Action<float, MouseState, MouseState> action) {
            action.Invoke(delta, _currentMouseState, _oldMouseState);
        }
#endif

        public void HandleInput(float delta, PlayerIndex idx, Action<float, PlayerIndex, GamePadState, GamePadState> action) {
            GamePadState gps, previous;
            switch (idx) {
                case PlayerIndex.One:
                    gps = _currentGamePadStates[0];
                    previous = _oldGamePadStates[0];
                    break;
                case PlayerIndex.Two:
                    gps = _currentGamePadStates[1];
                    previous = _oldGamePadStates[1];
                    break;
                case PlayerIndex.Three:
                    gps = _currentGamePadStates[2];
                    previous = _oldGamePadStates[2];
                    break;
                case PlayerIndex.Four:
                    gps = _currentGamePadStates[3];
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
        }

        public class GamePadDisconnectedEventArgs : EventArgs {
            public PlayerIndex DisconnectedGamePad;

            public GamePadDisconnectedEventArgs(PlayerIndex idx) {
                DisconnectedGamePad = idx;
            }
        }
    }
}
