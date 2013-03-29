using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JdGameBase.Core.Interfaces {
    public interface ICommand {
        void Execute();
        void Undo();
    }
}
