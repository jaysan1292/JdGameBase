// Project: JdGameBase
// Filename: FocusPoint.cs
// 
// Author: Jason Recillo

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using JdGameBase.Core;
using JdGameBase.Core.Geometry;
using JdGameBase.Core.Interfaces;
using JdGameBase.Extensions;

using Microsoft.Xna.Framework;

namespace JdGameBase.Graphics {
    public class FocusPoint : IFocusable {
        public readonly List<Entity> Points;
        private Camera2D _camera;

        public FocusPoint(Camera2D camera, params Entity[] entities) {
            _camera = camera;
            Points = new List<Entity>(entities);
        }

        /// <summary>
        /// Gets the points this FocusPoint is keeping track of in a Polygon.
        /// </summary>
        [DebuggerHidden]
        public Polygon FocusPolygon { get { return new Polygon(Points.Select(ent => ent.BoundingBox.Center())); } }

        /// <summary>
        /// Gets the position that the camera should focus on.
        /// </summary>
        [DebuggerHidden]
        public Vector2 FocusPosition { get { return Points.Count != 0 ? FocusPolygon.Center : Vector2.Zero; } }
    }
}
