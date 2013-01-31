// Project: JdGameBase
// Filename: Vector2Interpolator.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework;

namespace JdGameBase.Interpolators {
    public class Vector2Interpolator : Interpolator<Vector2> {
        protected override Vector2 Interpolate() {
            return SmoothStep
                       ? Vector2.SmoothStep(Value1, Value2, CurrentDuration / TotalDuration)
                       : Vector2.Lerp(Value1, Value2, CurrentDuration / TotalDuration);
        }
    }
}
