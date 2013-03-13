﻿// Project: JdGameBase
// Filename: MathExtensions.cs
// 
// Author: Jason Recillo

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using JdGameBase.Core.Geometry;

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

        /// <summary>
        /// Adds the specified amount to the given float, but wraps around to zero if
        /// it reaches the specified max value.
        /// </summary>
        /// <param name="value">The initial value.</param>
        /// <param name="amount">The amount to add.</param>
        /// <param name="max">The maximum value for value.</param>
        /// <returns></returns>
        [DebuggerHidden]
        public static float Wrap(this float value, float amount, float max) {
            return (value + amount) % max;
        }

        [DebuggerHidden]
        public static float Clamp(this float value, float amount, float min, float max) {
            return MathHelper.Clamp(value + amount, min, max);
        }

        [DebuggerHidden]
        public static bool WithinRange(this float value, float min, float max) {
            return min <= value && value >= max;
        }

        #endregion

        #region Quaternion

        #region Yaw

        [DebuggerHidden]
        public static float Yaw(this Quaternion q) {
            return q.X;
        }

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
        public static float Pitch(this Quaternion q) {
            return q.Y;
        }

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
        public static float Roll(this Quaternion q) {
            return q.Z;
        }

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

        #region Matrix
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
            return new Vector2(rect.Width / 2f, rect.Height / 2f);
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
        public static Rectangle ToRectangle(this Vector2 vector, int width = 0, int height = 0) {
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

        #region Points

        [DebuggerHidden]
        public static Vector2[] ConvertToVectors(this Point[] points) {
            var vectors = new Vector2[points.Length];
            for (var i = 0; i < points.Length; i++) vectors[i] = new Vector2(points[i].X, points[i].Y);
            return vectors;
        }

        [DebuggerHidden]
        public static Vector2 ToVector2(this Point p) {
            return new Vector2(p.X, p.Y);
        }

        #endregion

        #region Vector2

        [DebuggerHidden]
        public static Vector2 Wrap(this Vector2 v, Vector2 amount, Vector2 max) {
            return new Vector2((v.X + amount.X) % max.X,
                               (v.Y + amount.Y) % max.Y);
        }

        [DebuggerHidden]
        public static Vector2 Wrap(this Vector2 v, float amount, float max) {
            return v.Wrap(new Vector2(amount), new Vector2(max));
        }

        [DebuggerHidden]
        public static Vector3 ToVector3(this Vector2 v, float z = 0f) {
            return new Vector3(v, z);
        }

        [DebuggerHidden]
        public static float ToAngle(this Vector2 v) {
            return (float) Math.Atan2(v.Y, v.X);
        }

        [DebuggerHidden]
        public static Point ToPoint(this Vector2 v) {
            return new Point(v.X.ToNearestInt(), v.Y.ToNearestInt());
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

        #region Vector3

        [DebuggerHidden]
        public static Vector3 Offset(this Vector3 v, Vector3 amount) {
            return Vector3.Add(v, amount);
        }

        [DebuggerHidden]
        public static Vector3 Offset(this Vector3 v, Vector2 amount, float amountZ) {
            return v.Offset(new Vector3(amount, amountZ));
        }

        [DebuggerHidden]
        public static Vector3 Offset(this Vector3 v, float x, float y, float z) {
            return v.Offset(new Vector3(x, y, z));
        }

        #endregion

        #region Vector4

        [DebuggerHidden]
        public static Vector3 ToVector3(this Vector4 v) {
            return new Vector3(v.X, v.Y, v.Z);
        }

        #endregion
    }
}
