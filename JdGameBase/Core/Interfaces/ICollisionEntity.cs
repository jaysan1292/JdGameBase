using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace JdGameBase.Core.Interfaces {
    public interface ICollisionEntity {
        Rectangle CollisionBox { get; }
    }
}
