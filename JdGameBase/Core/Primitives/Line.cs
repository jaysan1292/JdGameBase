// Project: JdGameBase
// Filename: Line.cs
// 
// Author: Jason Recillo

using System;
using System.Diagnostics;

using JdGameBase.Extensions;

using Microsoft.Xna.Framework;

namespace JdGameBase.Core.Primitives {
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

        [DebuggerHidden]
        public override string ToString() {
            return "{{Start:{0} End:{1}}}".Fmt(Start, End);
        }
    }
}
