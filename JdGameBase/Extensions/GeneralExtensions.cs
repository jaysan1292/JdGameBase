﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using JetBrains.Annotations;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JdGameBase.Extensions {
    public static class GeneralExtensions {
        private static readonly Random Random = new Random();

        [DebuggerHidden]
        [StringFormatMethod("str")]
        public static string Fmt(this string str, params object[] args) {
            return String.Format(str, args);
        }

        [DebuggerHidden]
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action) {
            foreach (var x in enumerable) action(x);
        }

        [DebuggerHidden]
        public static bool Contains<T>(this IEnumerable<T> enumerable, IEnumerable<T> items) {
            return items.Aggregate(true, (b, x) => b && enumerable.Contains(x));
        }

        [DebuggerHidden]
        public static T GetRandom<T>(this IEnumerable<T> enumerable) {
            var values = enumerable as T[] ?? enumerable.ToArray();
            var idx = Random.Next(0, values.Count() - 1);
            return values.ElementAt(idx);
        }

        [DebuggerHidden]
        public static bool IsType<T>(this object o) {
            return IsType(o, typeof(T));
        }

        [DebuggerHidden]
        public static bool IsType(this object o, Type t) {
            return o.GetType() == t;
        }

        [DebuggerHidden]
        public static bool IsDerivedFrom<TBaseType>(this Type type) {
            if (type.BaseType == null) return false;
            return type == typeof(TBaseType) || IsDerivedFrom<TBaseType>(type.BaseType);
        }

        [DebuggerHidden]
        public static Vector2 GetPosition(this MouseState mouse) {
            return new Vector2(mouse.X, mouse.Y);
        }

        [DebuggerHidden]
        public static int ToNearestInt(this float f) {
            return (int) (f > 0 ? (f + 0.5f) : (f - 0.5f));
        }

        [DebuggerHidden]
        public static void Swap<T>(this IList<T> list, int firstIdx, int secondIdx) {
            if (firstIdx < 0 || firstIdx > list.Count) throw new ArgumentOutOfRangeException("firstIdx");
            if (secondIdx < 0 || secondIdx > list.Count) throw new ArgumentOutOfRangeException("secondIdx");

            var temp = list[firstIdx];
            list[firstIdx] = list[secondIdx];
            list[secondIdx] = temp;
        }

        [DebuggerHidden]
        public static void Swap<T>(this IList<T> list, T first, T second) {
            Swap(list, list.IndexOf(first), list.IndexOf(second));
        }
    }
}
