// Project: JdGameBase
// Filename: ColorInterpolator.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework;

namespace JdGameBase.Interpolators {
    public class ColorInterpolator : Interpolator<Color> {
        protected override Color Interpolate() {
            return Color.Lerp(Value1, Value2, CurrentDuration / TotalDuration);
        }
    }
}
