// Project: JdGameBase
// Filename: MathExtensions.cs
// 
// Author: Jason Recillo

using System;
using System.Diagnostics;

using JdGameBase.Core.Primitives;

using Microsoft.Xna.Framework;

namespace JdGameBase.Extensions {
    public static class MathExtensions {
        private static readonly Random Random = new Random();

        #region General

        [DebuggerHidden]
        public static float ToDegrees(this float f) {
            return MathHelper.ToDegrees(f);
        }

        [DebuggerHidden]
        public static float ToRadians(this float f) {
            return MathHelper.ToRadians(f);
        }

        #endregion

        #region Quaternion

        #region Yaw

        [DebuggerHidden]
        public static Quaternion Yaw(this Quaternion q, float value) {
            return q * Quaternion.CreateFromYawPitchRoll(value, 0, 0);
        }

        [DebuggerHidden]
        public static void Yaw(this Quaternion q, float value, out Quaternion result) {
            result = Yaw(q, value);
        }

        [DebuggerHidden]
        public static Quaternion Yaw(this Quaternion q, float value, bool normalize) {
            var ret = Yaw(q, value);
            if (normalize) ret.Normalize();
            return ret;
        }

        [DebuggerHidden]
        public static void Yaw(this Quaternion q, float value, bool normalize, out Quaternion result) {
            result = Yaw(q, value, normalize);
        }

        #endregion

        #region Pitch

        [DebuggerHidden]
        public static Quaternion Pitch(this Quaternion q, float value) {
            return q * Quaternion.CreateFromYawPitchRoll(0, value, 0);
        }

        [DebuggerHidden]
        public static void Pitch(this Quaternion q, float value, out Quaternion result) {
            result = Pitch(q, value);
        }

        [DebuggerHidden]
        public static Quaternion Pitch(this Quaternion q, float value, bool normalize) {
            var ret = Pitch(q, value);
            if (normalize) ret.Normalize();
            return ret;
        }

        [DebuggerHidden]
        public static void Pitch(this Quaternion q, float value, bool normalize, out Quaternion result) {
            result = Pitch(q, value, normalize);
        }

        #endregion

        #region Roll

        [DebuggerHidden]
        public static Quaternion Roll(this Quaternion q, float value) {
            return q * Quaternion.CreateFromYawPitchRoll(0, value, 0);
        }

        [DebuggerHidden]
        public static void Roll(this Quaternion q, float value, out Quaternion result) {
            result = Roll(q, value);
        }

        [DebuggerHidden]
        public static Quaternion Roll(this Quaternion q, float value, bool normalize) {
            var ret = Roll(q, value);
            if (normalize) ret.Normalize();
            return ret;
        }

        [DebuggerHidden]
        public static void Roll(this Quaternion q, float value, bool normalize, out Quaternion result) {
            result = Roll(q, value, normalize);
        }

        #endregion

        [DebuggerHidden]
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
        public static bool Contains(this Rectangle rect, Vector2 point) {
            return rect.Contains(point.X.ToNearestInt(), point.Y.ToNearestInt());
        }

        [DebuggerHidden]
        public static Rectangle Shrink(this Rectangle rect, int amount) {
            return Shrink(rect, amount, amount);
        }

        [DebuggerHidden]
        public static Rectangle Grow(this Rectangle rect, int amount) {
            return Shrink(rect, -amount, -amount);
        }

        [DebuggerHidden]
        public static Rectangle Grow(this Rectangle rect, int xAmount, int yAmount) {
            return Shrink(rect, -xAmount, -yAmount);
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
            position.X = MathHelper.Clamp(position.X, rect.X, rect.X + rect.Width);
            position.Y = MathHelper.Clamp(position.Y, rect.Y, rect.Y + rect.Height);
            return position;
        }

        [DebuggerHidden]
        public static Rectangle OffsetBy(this Rectangle rect, Vector2 offset) {
            return new Rectangle {
                X = (int) (rect.X + offset.X),
                Y = (int) (rect.Y + offset.Y),
                Width = rect.Width,
                Height = rect.Height
            };
        }

        [DebuggerHidden]
        public static bool IntersectsTop(this Rectangle rect, Rectangle test) {
            var line = new Line(rect.CenterVector(), test.CenterVector());
            return line.Intersects(test.TopLine());
        }

        [DebuggerHidden]
        public static bool IntersectsBottom(this Rectangle rect, Rectangle test) {
            var line = new Line(rect.CenterVector(), test.CenterVector());
            return line.Intersects(test.BottomLine());
        }

        [DebuggerHidden]
        public static bool IntersectsLeft(this Rectangle rect, Rectangle test) {
            var line = new Line(rect.CenterVector(), test.CenterVector());
            return line.Intersects(test.LeftLine());
        }

        [DebuggerHidden]
        public static bool IntersectsRight(this Rectangle rect, Rectangle test) {
            var line = new Line(rect.CenterVector(), test.CenterVector());
            return line.Intersects(test.RightLine());
        }

        [DebuggerHidden]
        public static Rectangle ToRectangle(this Vector2 vector, int width, int height) {
            return new Rectangle((int) vector.X, (int) vector.Y, width, height);
        }

        [DebuggerHidden]
        public static Line TopLine(this Rectangle rect) {
            return new Line(rect.TopLeft(), rect.TopRight());
        }

        [DebuggerHidden]
        public static Line BottomLine(this Rectangle rect) {
            return new Line(rect.BottomLeft(), rect.BottomRight());
        }

        [DebuggerHidden]
        public static Line LeftLine(this Rectangle rect) {
            return new Line(rect.TopLeft(), rect.BottomLeft());
        }

        [DebuggerHidden]
        public static Line RightLine(this Rectangle rect) {
            return new Line(rect.TopRight(), rect.BottomRight());
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

        #region Vector2

        [DebuggerHidden]
        public static float ToAngle(this Vector2 v) {
            return (float) Math.Atan2(v.Y, v.X);
        }

        /// <summary>
        /// Converts an angle into a Vector2 with the given length.
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="length"></param>
        /// <param name="radians">Whether the angle is in radians. Defaults to true.</param>
        /// <returns></returns>
        [DebuggerHidden]
        public static Vector2 ToVector2(this float angle, float length = 1f, bool radians = true) {
            if (!radians) angle = MathHelper.ToRadians(angle);
            return new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle)) * length;
        }

        /// <summary>
        /// Rotates a vector to have the specified angle.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="angle">The angle, in degrees.</param>
        /// <returns></returns>
        [DebuggerHidden]
        public static Vector2 RotateVector(this Vector2 vector, float angle) {
            return Vector2.Transform(vector, Matrix.CreateRotationZ(MathHelper.ToRadians(angle)));
        }

        [DebuggerHidden]
        public static Vector2 Offset(this Vector2 vector, Vector2 offset) {
            return Vector2.Add(vector, offset);
        }

        [DebuggerHidden]
        public static Vector2 Offset(this Vector2 vector, float offsetX, float offsetY) {
            return Offset(vector, new Vector2(offsetX, offsetY));
        }

        [DebuggerHidden]
        public static Vector2 Offset(this Vector2 vector, float offset) {
            return Offset(vector, new Vector2(offset));
        }

        #endregion
    }
}
