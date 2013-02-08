// Project: JdGameBase
// Filename: IInputHandler.cs
// 
// Author: Jason Recillo

using System;

namespace JdGameBase.Core.Interfaces {
    public interface IInputHandler : IGamePadHandler, IKeyboardHandler, IMouseHandler { }
}
