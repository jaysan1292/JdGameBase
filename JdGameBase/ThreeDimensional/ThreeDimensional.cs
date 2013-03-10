// Project: JdGameBase
// Filename: ThreeDimensional.cs
// 
// Author: Jason Recillo

using System;

using JdGameBase.Core.GameComponents;
using JdGameBase.ThreeDimensional.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.ThreeDimensional {
    namespace Interfaces {
        public interface IEntity3D : IDrawableEntity3D {
            Vector3 Position { get; set; }
        }

        public interface IDrawableEntity3D {
            void Draw(GraphicsDevice device, ICamera3D camera);
        }

        public interface ICamera3D {
            float AspectRatio { get; set; }
            Matrix View { get; }
            Matrix Projection { get; }
            IEntity3D Target { get; set; }
            Vector3 Position { get; set; }
            Vector3 UpVector { get; set; }
        }
    }

    namespace Camera {
        public class Camera3D : JdComponent, ICamera3D {
            private Matrix _view;
            private Matrix _projection;

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

    public abstract class Entity3D : IEntity3D {
        private Vector3 _position;

        public Vector3 Position { get { return _position; } set { _position = value; } }

        public abstract void Draw(GraphicsDevice device, ICamera3D camera);
    }
}
