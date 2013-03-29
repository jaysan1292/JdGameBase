using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JdGameBase.Core.Scenes {
    public interface ISceneTransition {
        TimeSpan Duration { get; set; }
        void Update(float delta);
    }
}
