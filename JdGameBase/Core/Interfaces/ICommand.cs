using System;

namespace JdGameBase.Core.Interfaces {
    public interface ICommand {
        void Execute();
        void Undo();
    }
}