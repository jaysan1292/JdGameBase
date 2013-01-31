// Project: JdGameBase
// Filename: Utilities.cs
// 
// Author: Jason Recillo

using System;
using System.Linq;

using Microsoft.Xna.Framework;

namespace JdGameBase.Core {
    public static class Utilities {
        private static readonly Random Random = new Random();

        #region Random

        public static float RandomWithinRange(float min, float max) {
            return MathHelper.Lerp(min, max, (float) Random.NextDouble());
        }

        public static Vector2 RandomWithinRectangle(Rectangle rect) {
            return new Vector2(Random.Next(rect.X, rect.X + rect.Width),
                               Random.Next(rect.Y, rect.Y + rect.Height));
        }

        public static Vector2 RandomVelocity(Random random, float maxSpeed) {
            return Vector2.Transform(new Vector2((float) random.NextDouble() * maxSpeed, 0),
                                     Matrix.CreateRotationZ(MathHelper.ToRadians((float) (random.NextDouble() * 360))));
        }

        public static bool Chance(int percentage) {
            var chance = percentage + Random.Next(1, 100);
            return chance >= 100;
        }

        #endregion

        #region Vectors

        public static void ConstrainPositionWithinScreen(ref Vector2 position, Rectangle screenSize) {
            if (position.X < 0) position.X = 0;
            if (position.X > screenSize.Width) position.X = screenSize.Width;
            if (position.Y < 0) position.Y = 0;
            if (position.Y > screenSize.Height) position.Y = screenSize.Height;
        }

        public static Vector2 AngleToVector2(float angle, float length = 1f) {
            return new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle)) * length;
        }

        public static float Vector2ToAngle(Vector2 vector) {
            return (float) Math.Atan2(vector.Y, vector.X);
        }

        public static Vector2 RotateVector(Vector2 vector, float angle) {
            return Vector2.Transform(vector, Matrix.CreateRotationZ(MathHelper.ToRadians(angle)));
        }

        #endregion

        #region Colors

        // http://www.cs.rit.edu/~ncs/color/t_convert.html

        public static float[] ColorRgbtoHsv(Color color) {
            return ColorRgbToHsv(color.R, color.G, color.B);
        }

        public static float[] ColorRgbToHsv(float r, float g, float b) {
            const float epsilon = 0.00001f;
            float h = 0, s, v;
            float min, max, delta;
            min = Min(r, g, b);
            max = Max(r, g, b);
            v = max;

            delta = max - min;

            if (Math.Abs(max - 0) > epsilon) s = delta / max;
            else {
                s = 0;
                h = -1;
                return new[] { h, s, v };
            }
            if (Math.Abs(r - max) < epsilon) h = (g - h) / delta;
            else if (Math.Abs(g - max) < epsilon) h = 2 + (b - r) / delta;
            else h = 4 + (r - g) / delta;

            h *= 60;
            if (h < 0) h += 360;

            return new[] { h, s, v };
        }

        /// <summary>
        /// Converts an HSV color into its RGB representation.
        /// </summary>
        /// <param name="h">Hue: 0-360</param>
        /// <param name="s">Saturation: 0-1</param>
        /// <param name="v">Value: 0-1</param>
        /// <returns></returns>
        public static Color ColorHsvToRgb(float h, float s, float v) {
            const float epsilon = 0.00001f;
            float r, g, b;
            int i;
            float f, p, q, t;

            if (Math.Abs(s - 0) < epsilon) return new Color(v, v, v);

            h /= 60;
            i = (int) Math.Floor(h);
            f = h - i;
            p = v * (1 - s);
            q = v * (1 - s * f);
            t = v * (1 - s * (1 - f));

            switch (i) {
                case 0:
                    r = v;
                    g = t;
                    b = p;
                    break;
                case 1:
                    r = q;
                    g = v;
                    b = p;
                    break;
                case 2:
                    r = p;
                    g = v;
                    b = t;
                    break;
                case 3:
                    r = p;
                    g = q;
                    b = v;
                    break;
                case 4:
                    r = t;
                    g = p;
                    b = v;
                    break;
                default:
                    r = v;
                    g = p;
                    b = q;
                    break;
            }
            return new Color(r, g, b);
        }

        private static float Min(params float[] args) {
            if (args == null) throw new ArgumentNullException("args");
            if (args.Length == 0) throw new ArgumentException("args must contain at least one value");
            return args.Min();
        }

        private static float Max(params float[] args) {
            if (args == null) throw new ArgumentNullException("args");
            if (args.Length == 0) throw new ArgumentException("args must contain at least one value");
            return args.Max();
        }

        #endregion
    }
}
