using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JdGameBase.ThreeDimensional.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.ThreeDimensional {
    public abstract class Entity3D : IEntity3D {
        public Vector3 Position { get; set; }

        public abstract void Draw(GraphicsDevice device, ICamera3D camera);
    }
}
