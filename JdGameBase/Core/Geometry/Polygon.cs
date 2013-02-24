// Project: JdGameBase
// Filename: Polygon.cs
// 
// Author: Jason Recillo

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;

namespace JdGameBase.Core.Geometry {
    // TODO: Polygon area
    public struct Polygon : IEnumerable<Vector2> {
        public List<Vector2> Vertices;

        [DebuggerHidden]
        public Polygon(Vector2 v1, Vector2 v2, params Vector2[] vertices) {
            Vertices = new List<Vector2>(vertices.Length + 2) { v1, v2 };
            foreach (var v in vertices) Vertices.Add(v);
        }

        [DebuggerHidden]
        public Polygon(IEnumerable<Vector2> vertices) {
            Vertices = new List<Vector2>(vertices);
        }

        [DebuggerHidden]
        public Polygon(Polygon p) {
            Vertices = p.Vertices;
        }

        [DebuggerHidden]
        public Vector2 Center {
            get {
                if (Vertices == null || Vertices.Count == 0) return Vector2.Zero;
                var x = Vertices.Aggregate(0f, (am, v) => am + v.X) / Vertices.Count;
                var y = Vertices.Aggregate(0f, (am, v) => am + v.Y) / Vertices.Count;
                return new Vector2(x, y);
            }
        }

        [DebuggerHidden]
        public int VertexCount { get { return Vertices.Count; } }

        [DebuggerHidden]
        public Vector2 this[int idx] { get { return Vertices[idx]; } }

        [DebuggerHidden]
        public IEnumerator<Vector2> GetEnumerator() {
            return Vertices.GetEnumerator();
        }

        [DebuggerHidden]
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
