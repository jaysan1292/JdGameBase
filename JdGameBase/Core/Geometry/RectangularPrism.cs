// Project: JdGameBase
// Filename: RectangularPrism.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework;

namespace JdGameBase.Core.Geometry {
    public struct RectangularPrism {
        public float Length;
        public float Width;
        public float Height;
        public Vector3 Position;
        public Quaternion Rotation;

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
