// Project: JdGameBase
// Filename: Vector3Interpolator.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework;

namespace JdGameBase.Interpolators {
    public class Vector3Interpolator : Interpolator<Vector3> {
        protected override Vector3 Interpolate() {
            return SmoothStep
                       ? Vector3.SmoothStep(Value1, Value2, CurrentDuration / TotalDuration)
                       : Vector3.Lerp(Value1, Value2, CurrentDuration / TotalDuration);
        }
    }
}
