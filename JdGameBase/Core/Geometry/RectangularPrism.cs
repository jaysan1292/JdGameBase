using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace JdGameBase.Core.Geometry {
    public struct RectangularPrism {
        public float Height;
        public float Length;
        public Vector3 Position;
        public Quaternion Rotation;
        public float Width;

        public RectangularPrism(Vector3 pos, float l, float w, float h) {
            Position = pos;
            Length = l;
            Width = w;
            Height = h;
            Rotation = Quaternion.Identity;
        }

        public bool Intersects(RectangularPrism other) {
            // TODO: Cube/Rectangular prism intersection
            return false;
        }
    }
}
