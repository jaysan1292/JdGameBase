// Project: JdGameBase
// Filename: Circle.cs
// 
// Author: Jason Recillo

using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;

namespace JdGameBase.Core.Primitives {
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

        private Vector2 _v;
        private Vector2 _direction;
        private float _distanceSquared;

        /// <summary> 
        /// Constructs a new circle. 
        /// </summary> 
        [DebuggerHidden]
        public Circle(Vector2 position, float radius) {
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
        /// Determines if a circle intersect another circle.
        /// If the circles touch on a single point, they are
        /// considered intersecting.
        /// </summary>
        /// <param name="circle">The circle to test.</param>
        /// <returns>True if the circles overlap. False otherwise.</returns>
        public bool Intersects(Circle circle) {
            var dX = circle.Center.X - Center.X;
            var dY = circle.Center.Y - Center.Y;
            var d = Math.Sqrt(dX * dX + dY * dY);

            // Circles do not interset if they are too far apart
            return !(d > (Radius + circle.Radius));
        }
    }
}
