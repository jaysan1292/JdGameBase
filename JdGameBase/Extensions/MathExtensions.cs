// Project: JdGameBase
// Filename: MathExtensions.cs
// 
// Author: Jason Recillo

using System;

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

        public static Rectangle ToRectangle(this Vector2 vector, int width, int height) {
            return new Rectangle((int) vector.X, (int) vector.Y, width, height);
        }

        public static Vector2 TopLeft(this Rectangle rect) {
            return new Vector2(rect.Left, rect.Top);
        }

        public static Vector2 TopRight(this Rectangle rect) {
            return new Vector2(rect.Right, rect.Top);
        }

        public static Vector2 BottomLeft(this Rectangle rect) {
            return new Vector2(rect.Left, rect.Bottom);
        }

        public static Vector2 BottomRight(this Rectangle rect) {
            return new Vector2(rect.Right, rect.Bottom);
        }

        #endregion
    }
}
