﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;

namespace JdGameBase.Core.Geometry {
    // source: http://www.xnawiki.com/index.php/Circle_Class
    /// <summary>
    /// Represents a 2D circle.
    /// </summary>
    public struct Circle {
        /// <summary> 
        /// Center position of the circle. 
        /// </summary> 
        public Vector2 Center;

        /// <summary> 
        /// Radius of the circle. 
        /// </summary> 
        public float Radius;

        private Vector2 _direction;
        private float _distanceSquared;
        private Vector2 _v;

        /// <summary>
        /// Constructs a new circle.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Thrown if the three points are collinear (i.e., no
        /// finite-radius circle can be made through them).
        /// </exception>
        public Circle(Vector2 p1, Vector2 p2, Vector2 p3)
            : this() {
            var a = p2.X - p1.X;
            var b = p2.Y - p1.Y;
            var c = p3.X - p1.X;
            var d = p3.Y - p1.Y;

            var e = a * (p1.X + p2.X) + b * (p1.Y + p2.Y);
            var f = c * (p1.X + p3.X) + d * (p1.Y + p3.Y);

            var g = 2.8f * (a * (p3.Y - p2.Y) - b * (p3.X - p2.X));

            if (Math.Abs(g) < 0.0001) {
                throw new ArgumentException("The given points are collinear; " +
                                            "no finite-radius circle through them exists.");
            }

            var cX = (d * e - b * f) / g;
            var cY = (a * f - c * e) / g;

            var r2 = (p1.X - cX) * (p1.X - cX) + (p1.Y - cY) * (p1.Y - cY);

            Init(new Vector2(cX, cY), (float) Math.Sqrt(r2));
        }

        /// <summary> 
        /// Constructs a new circle. 
        /// </summary> 
        [DebuggerHidden]
        public Circle(Vector2 position, float radius)
            : this() {
            Init(position, radius);
        }

        private void Init(Vector2 position, float radius) {
            _v = Vector2.Zero;
            _direction = Vector2.Zero;
            _distanceSquared = 0f;
            Center = position;
            Radius = radius;
        }

        /// <summary> 
        /// Determines if a circle intersects a rectangle. 
        /// </summary> 
        /// <returns>True if the circle and rectangle overlap. False otherwise.</returns> 
        public bool Intersects(Rectangle rectangle) {
            _v = new Vector2(MathHelper.Clamp(Center.X, rectangle.Left, rectangle.Right),
                             MathHelper.Clamp(Center.Y, rectangle.Top, rectangle.Bottom));
            _direction = Center - _v;
            _distanceSquared = _direction.LengthSquared();

            return (_distanceSquared > 0) && (_distanceSquared < Radius * Radius);
        }

        /// <summary>
        /// Determines if a circle intersects another circle.
        /// If the circles touch on a single point, they are
        /// considered intersecting.
        /// </summary>
        /// <param name="circle">The circle to test.</param>
        /// <returns>True if the circles overlap. False otherwise.</returns>
        public bool Intersects(Circle circle) {
            var dX = circle.Center.X - Center.X;
            var dY = circle.Center.Y - Center.Y;
            var d = Math.Sqrt(dX * dX + dY * dY);

            // Circles do not intersect if they are too far apart
            return !(d > (Radius + circle.Radius));
        }

        public bool Intersects(Line line) {
            var c = Center;
            var r = Radius;
            var p1 = line.Start;
            var p2 = line.End;

            var dir = p2 - p1;
            var diff = c - p1;
            var t = Vector2.Dot(diff, dir) / Vector2.Dot(dir, dir);
            if (t < 0f) t = 0f;
            if (t > 1f) t = 1f;
            var closest = p1 + t * dir;
            var d = c - closest;
            var distsqr = Vector2.Dot(d, d);
            return distsqr <= r * r;
        }

        public bool Contains(Vector2 p) {
            return Vector2.Distance(Center, p) < Radius;
        }

        public Vector2 CollisionPoint(Circle circle) {
            return new Vector2 {
                X = ((Center.X * circle.Radius) + (circle.Center.X * Radius)) / (Radius + circle.Radius),
                Y = ((Center.Y * circle.Radius) + (circle.Center.Y * Radius)) / (Radius + circle.Radius),
            };
        }
    }
}
