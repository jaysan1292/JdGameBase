using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.ThreeDimensional.Interfaces {
    public interface IDrawableEntity3D {
        void Draw(GraphicsDevice device, ICamera3D camera);
    }
}
