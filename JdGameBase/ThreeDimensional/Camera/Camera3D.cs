using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JdGameBase.Core.GameComponents;
using JdGameBase.ThreeDimensional.Interfaces;

using Microsoft.Xna.Framework;

namespace JdGameBase.ThreeDimensional.Camera {
    public class Camera3D : JdComponent, ICamera3D {
        private Matrix _projection;
        private Matrix _view;

        public Camera3D(JdGame game)
            : base(game) { }

        public Vector3 Position { get; set; }
        public float AspectRatio { get; set; }
        public Matrix View { get { return _view; } }
        public Matrix Projection { get { return _projection; } }
        public IEntity3D Target { get; set; }
        public Vector3 UpVector { get; set; }
    }
}
