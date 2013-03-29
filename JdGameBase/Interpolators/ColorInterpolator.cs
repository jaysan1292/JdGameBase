using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace JdGameBase.Interpolators {
    public class ColorInterpolator : Interpolator<Color> {
        protected override Color Interpolate() {
            return Color.Lerp(Value1, Value2, CurrentDuration / TotalDuration);
        }
    }
}
