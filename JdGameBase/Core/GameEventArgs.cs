﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JdGameBase.Core {
    public class GameEventArgs : EventArgs {
        public new static readonly GameEventArgs Empty = new GameEventArgs(0, null);
        public readonly object Data;
        public readonly float Delta;

        public GameEventArgs(float delta) {
            Delta = delta;
        }

        public GameEventArgs(float delta, object data) {
            Delta = delta;
            Data = data;
        }
    }
}
