using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace JdGameBase.Interpolators {
    public class FloatInterpolator : Interpolator<float> {
        protected override float Interpolate() {
            return SmoothStep
                       ? MathHelper.SmoothStep(Value1, Value2, CurrentDuration / TotalDuration)
                       : MathHelper.Lerp(Value1, Value2, CurrentDuration / TotalDuration);
        }
    }
}
