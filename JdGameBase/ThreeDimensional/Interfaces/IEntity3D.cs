using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace JdGameBase.ThreeDimensional.Interfaces {
    public interface IEntity3D : IDrawableEntity3D {
        Vector3 Position { get; set; }
    }
}
