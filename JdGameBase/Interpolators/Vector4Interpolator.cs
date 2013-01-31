// Project: JdGameBase
// Filename: Vector4Interpolator.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework;

namespace JdGameBase.Interpolators {
    public class Vector4Interpolator : Interpolator<Vector4> {
        protected override Vector4 Interpolate() {
            return SmoothStep
                       ? Vector4.SmoothStep(Value1, Value2, CurrentDuration / TotalDuration)
                       : Vector4.Lerp(Value1, Value2, CurrentDuration / TotalDuration);
        }
    }
}
