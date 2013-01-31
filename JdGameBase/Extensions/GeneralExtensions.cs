// Project: JdGameBase
// Filename: GeneralExtensions.cs
// 
// Author: Jason Recillo

using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JdGameBase.Extensions {
    public static class GeneralExtensions {
        private static readonly Random Random = new Random();

        [StringFormatMethod("str")]
        public static string Fmt(this string str, params object[] args) {
            return String.Format(str, args);
        }

        public static T GetRandom<T>(this IEnumerable<T> enumerable) {
            var values = enumerable as T[] ?? enumerable.ToArray();
            var idx = Random.Next(0, values.Count() - 1);
            return values.ElementAt(idx);
        }

        public static bool IsDerivedFrom<TBaseType>(this Type type) {
            if (type.BaseType == null) return false;
            return type == typeof(TBaseType) || IsDerivedFrom<TBaseType>(type.BaseType);
        }

        public static Vector2 GetPosition(this MouseState mouse) {
            return new Vector2(mouse.X, mouse.Y);
        }

        public static int ToNearestInt(this float f) {
            return (int) (f > 0 ? (f + 0.5f) : (f - 0.5f));
        }

        #region Geometry

        public static Vector2 Center(this Rectangle rect) {
            return new Vector2(rect.Center.X, rect.Center.Y);
        }

        #endregion
    }
}
