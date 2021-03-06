﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JdGameBase.Extensions;

namespace JdGameBase.Utils {
    public static class MathUtils {
        public static int Average(int i, params int[] args) {
            return (args.Aggregate(i, (a, x) => {
                a += x;
                return a;
            }) / (float) args.Length + 1).ToNearestInt();
        }
    }
}
