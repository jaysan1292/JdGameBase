using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using JdGameBase.Extensions;

using Microsoft.Xna.Framework;

namespace JdGameBase.Core.Geometry {
    public struct Line {
        public Vector2 End;
        public Vector2 Start;

        [DebuggerHidden]
        public Line(Vector2 start, Vector2 end) {
            Start = start;
            End = end;
        }

        [DebuggerHidden]
        public float Length { get { return Vector2.Distance(Start, End); } }

        [DebuggerHidden]
        public Vector2 Midpoint {
            get {
                return new Vector2((Start.X + End.X) / 2,
                                   (Start.Y + End.Y) / 2);
            }
        }

        public float Angle { get { return (float) Math.Atan2(End.Y - Start.Y, End.X - Start.X); } }

        public bool Intersects(Line line) {
            var ab = this;
            var cd = line;
            var a = ab.Start;
            var b = ab.End;
            var c = cd.Start;
            var d = cd.End;

            float r, s;
            FindPoints(a, b, c, d, out r, out s);

            return (0 <= r && r <= 1) && (0 <= s && s <= 1);
        }

        public Vector2 IntersectionPoint(Line line) {
            var ab = this;
            var cd = line;
            var a = ab.Start;
            var b = ab.End;
            var c = cd.Start;
            var d = cd.End;

            float r, s;
            FindPoints(a, b, c, d, out r, out s);

            var p = a + r * (b - a);

            return p;
        }

        private static void FindPoints(Vector2 a, Vector2 b, Vector2 c, Vector2 d, out float r, out float s) {
            var denom = ((b.X - a.X) * (d.Y - c.Y) - (b.Y - a.Y) * (d.X - c.X));
            r = ((a.Y - c.Y) * (d.X - c.X) - (a.X - c.X) * (d.Y - c.Y)) / denom;
            s = ((a.Y - c.Y) * (b.X - a.X) - (a.X - c.X) * (b.Y - a.Y)) / denom;
        }

        [DebuggerHidden]
        public override string ToString() {
            return "{{Start:{0} End:{1}}}".Fmt(Start, End);
        }
    }
}
