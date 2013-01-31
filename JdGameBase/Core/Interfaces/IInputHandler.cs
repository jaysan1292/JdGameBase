// Project: JdGameBase
// Filename: IInputHandler.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework;

namespace JdGameBase.Core.Interfaces {
    public interface IInputHandler : IGamePadHandler, IKeyboardHandler, IMouseHandler { }
}
