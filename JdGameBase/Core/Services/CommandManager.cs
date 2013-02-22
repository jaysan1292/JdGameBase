// Project: JdGameBase
// Filename: CommandManager.cs
// 
// Author: Jason Recillo

using System;
using System.Collections.Generic;
using System.Diagnostics;

using JdGameBase.Core.Interfaces;
using JdGameBase.Extensions;

namespace JdGameBase.Core.Services {
    // TODO: Test this class
    public class CommandManager<T> where T : ICommand {
        private readonly Stack<T> _redos = new Stack<T>();
        private readonly Stack<T> _undos = new Stack<T>();

        public bool UndoAvailable { get { return !_undos.Empty(); } }
        public bool RedoAvailable { get { return !_redos.Empty(); } }

        public void ExecuteCommand(T c) {
            c.Execute();
            _undos.Push(c);
            _redos.Clear();
        }

        public void Undo() {
            Debug.Assert(!_undos.Empty());
            var command = _undos.Pop();
            command.Undo();
            _redos.Push(command);
        }

        public void Redo() {
            Debug.Assert(!_redos.Empty());
            var command = _redos.Pop();
            command.Execute();
            _undos.Push(command);
        }
    }
}
