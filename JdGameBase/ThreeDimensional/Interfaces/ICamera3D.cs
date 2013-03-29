using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace JdGameBase.ThreeDimensional.Interfaces {
    public interface ICamera3D {
        float AspectRatio { get; set; }
        Matrix View { get; }
        Matrix Projection { get; }
        IEntity3D Target { get; set; }
        Vector3 Position { get; set; }
        Vector3 UpVector { get; set; }
    }
}
