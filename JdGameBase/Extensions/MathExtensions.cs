// Project: JdGameBase
// Filename: MathExtensions.cs
// 
// Author: Jason Recillo

using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;

namespace JdGameBase.Extensions {
    public static class MathExtensions {
        private static readonly Random Random = new Random();

        #region Quaternion

        #region Yaw

        public static Quaternion Yaw(this Quaternion q, float value) {
            return q * Quaternion.CreateFromYawPitchRoll(value, 0, 0);
        }

        public static void Yaw(this Quaternion q, float value, out Quaternion result) {
            result = Yaw(q, value);
        }

        public static Quaternion Yaw(this Quaternion q, float value, bool normalize) {
            var ret = Yaw(q, value);
            if (normalize) ret.Normalize();
            return ret;
        }

        public static void Yaw(this Quaternion q, float value, bool normalize, out Quaternion result) {
            result = Yaw(q, value, normalize);
        }

        #endregion

        #region Pitch

        public static Quaternion Pitch(this Quaternion q, float value) {
            return q * Quaternion.CreateFromYawPitchRoll(0, value, 0);
        }

        public static void Pitch(this Quaternion q, float value, out Quaternion result) {
            result = Pitch(q, value);
        }

        public static Quaternion Pitch(this Quaternion q, float value, bool normalize) {
            var ret = Pitch(q, value);
            if (normalize) ret.Normalize();
            return ret;
        }

        public static void Pitch(this Quaternion q, float value, bool normalize, out Quaternion result) {
            result = Pitch(q, value, normalize);
        }

        #endregion

        #region Roll

        public static Quaternion Roll(this Quaternion q, float value) {
            return q * Quaternion.CreateFromYawPitchRoll(0, value, 0);
        }

        public static void Roll(this Quaternion q, float value, out Quaternion result) {
            result = Roll(q, value);
        }

        public static Quaternion Roll(this Quaternion q, float value, bool normalize) {
            var ret = Roll(q, value);
            if (normalize) ret.Normalize();
            return ret;
        }

        public static void Roll(this Quaternion q, float value, bool normalize, out Quaternion result) {
            result = Roll(q, value, normalize);
        }

        #endregion

        public static void ToYawPitchRoll(this Quaternion q, out float yaw, out float pitch, out float roll) {
            var x = q.X;
            var y = q.Y;
            var z = q.Z;
            var w = q.W;

            yaw = (float) Math.Asin(-2f * (x * z - w * y));
            pitch = (float) Math.Atan2(2f * (y * z + w * x), w * w - x * x - y * y + z * z);
            roll = (float) Math.Atan2(2f * (x * y + w * z), w * w + x * x - y * y - z * z);
        }

        #endregion

        #region Rectangles

        [DebuggerHidden]
        public static int Area(this Rectangle rectangle) {
            return rectangle.Width * rectangle.Height;
        }

        [DebuggerHidden]
        public static int Perimeter(this Rectangle rectangle) {
            return rectangle.Width * 2 + rectangle.Height * 2;
        }

        [DebuggerHidden]
        public static Rectangle Shrink(this Rectangle rect, int amount) {
            return Shrink(rect, amount, amount);
        }

        [DebuggerHidden]
        public static Rectangle Shrink(this Rectangle rect, int xAmount, int yAmount) {
            return new Rectangle {
                X = rect.X + xAmount,
                Y = rect.Y + yAmount,
                Width = rect.Width - (xAmount * 2),
                Height = rect.Height - (yAmount * 2)
            };
        }

        [DebuggerHidden]
        public static Vector2 CenterVector(this Rectangle rect) {
            return new Vector2(rect.Center.X, rect.Center.Y);
        }

        [DebuggerHidden]
        public static Vector2 ConstrainWithin(this Vector2 position, Rectangle rect) {
            if (position.X < rect.X) position.X = rect.X;
            if (position.Y < rect.Y) position.Y = rect.Y;
            if (position.X > rect.X + rect.Width) position.X = rect.X + rect.Width;
            if (position.Y > rect.Y + rect.Height) position.Y = rect.Y + rect.Height;
            return position;
        }

        [DebuggerHidden]
        public static Rectangle ToRectangle(this Vector2 vector, int width, int height) {
            return new Rectangle((int) vector.X, (int) vector.Y, width, height);
        }

        [DebuggerHidden]
        public static Vector2 TopLeft(this Rectangle rect) {
            return new Vector2(rect.Left, rect.Top);
        }

        [DebuggerHidden]
        public static Vector2 TopRight(this Rectangle rect) {
            return new Vector2(rect.Right, rect.Top);
        }

        [DebuggerHidden]
        public static Vector2 BottomLeft(this Rectangle rect) {
            return new Vector2(rect.Left, rect.Bottom);
        }

        [DebuggerHidden]
        public static Vector2 BottomRight(this Rectangle rect) {
            return new Vector2(rect.Right, rect.Bottom);
        }

        #endregion
    }
}
