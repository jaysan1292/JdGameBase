// Project: JdGameBase
// Filename: FocusPoint.cs
// 
// Author: Jason Recillo

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using JdGameBase.Core;
using JdGameBase.Core.Interfaces;
using JdGameBase.Core.Primitives;
using JdGameBase.Extensions;

using Microsoft.Xna.Framework;

namespace JdGameBase.Graphics {
    public class FocusPoint : IFocusable, IUpdatableEntity {
        public readonly List<Entity> Points;
        private Camera2D _camera;

        public FocusPoint(Camera2D camera, Entity entity, params Entity[] entities) {
            _camera = camera;
            Points = new List<Entity> { entity };
            Points.AddRange(entities);
        }

        /// <summary>
        /// Whether or not this FocusPoint should attempt to keep all entities on screen
        /// </summary>
        public bool KeepAllEntitiesOnScreen { get; set; }

        /// <summary>
        /// Gets the points this FocusPoint is keeping track of in a Polygon.
        /// </summary>
        [DebuggerHidden]
        public Polygon FocusPolygon { get { return new Polygon(Points.Select(ent => ent.BoundingBox.CenterVector())); } }

        /// <summary>
        /// Gets the position that the camera should focus on.
        /// </summary>
        [DebuggerHidden]
        public Vector2 FocusPosition { get { return Points.Count != 0 ? FocusPolygon.Center : Vector2.Zero; } }

        public void Update(float delta) {
            // Don't update camera zoom if not set, or if there are less than two points
            if (!KeepAllEntitiesOnScreen || Points.Count < 2) return;
            return;
            var screen = _camera.ScreenSize;
            var zoom = Points[0].BoundingBox.CenterVector().Y / (screen.Height * (1f / 4f));
            // position.X += (Ball.Position.X - Player.Position.X - (Player.BoundingBox.Width / 2f)) * 10f * delta;
            zoom = (zoom - _camera.Zoom) * delta;
            _camera.Zoom = Math.Min(zoom, 1);
        }
    }
}
