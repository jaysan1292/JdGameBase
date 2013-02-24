// Project: JdGameBase
// Filename: GameExtensions.cs
// 
// Author: Jason Recillo

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using JdGameBase.Core.Geometry;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JdGameBase.Extensions {
    public static class GameExtensions {
        #region Components
        [DebuggerHidden]
        public static float AspectRatio(this GraphicsDevice device) {
            return (float) device.Viewport.Width / device.Viewport.Height;
        }

        [DebuggerHidden]
        public static void AddComponent<T>(this Game game, T component) where T : IGameComponent {
            game.Components.Add(component);
        }

        [DebuggerHidden]
        public static T GetComponent<T>(this Game game) where T : IGameComponent {
            return (T) GetComponent(game, typeof(T));
        }

        [DebuggerHidden]
        public static void RemoveAllComponents<TComponent>(this Game game) where TComponent : IGameComponent {
            (from component in game.Components
             let componentType = component.GetType()
             where componentType.IsDerivedFrom<TComponent>()
             select component).ToList().ForEach(x => game.Components.Remove(x));
        }

        [DebuggerHidden]
        public static void RemoveComponent<T>(this Game game) where T : IGameComponent {
            game.Components.Remove(game.GetComponent<T>());
        }

        [DebuggerHidden]
        private static object GetComponent(Game game, Type type) {
            if (type.BaseType == null) return null;
            return game.Components.FirstOrDefault(x => type == x.GetType()) ?? GetComponent(game, type.BaseType);
        }

        #endregion

        #region Drawing

        #region Colors

        //TODO: Color.Darken() + Color.Lighten()

        [DebuggerHidden]
        public static Color Opacity(this Color c, float opacity) {
            return Color.FromNonPremultiplied(c.R, c.G, c.B, (int) (opacity * byte.MaxValue));
        }

        [DebuggerHidden]
        public static float Opacity(this Color c) {
            return (float) c.A / byte.MaxValue;
        }

        [DebuggerHidden]
        public static Color Add(this Color color, Color add) {
            var c = color.ToVector4();
            var a = add.ToVector4();
            return new Color((c + a) / 2);
        }

        [DebuggerHidden]
        public static Texture2D Add(this Texture2D tex, Texture2D add) {
            if (tex.Width != add.Width || tex.Height != add.Height) throw new ArgumentException();
            var dim = tex.Width * tex.Height;
            var data = new Color[dim];
            var td = new Color[dim];
            var ad = new Color[dim];
            tex.GetData(td);
            add.GetData(ad);
            for (var i = 0; i < data.Length; i++) data[i] = td[i].Add(ad[i]);
            tex.GraphicsDevice.Textures[0] = null;
            tex.SetData(data);

            return tex;
        }

        #endregion

        [DebuggerHidden]
        public static void DrawLine(this SpriteBatch spriteBatch, Texture2D texture, Color color, Vector2 start, Vector2 end) {
            spriteBatch.Draw(texture, start, null, color,
                             (float) Math.Atan2(end.Y - start.Y, end.X - start.X),
                             new Vector2(0f, (float) texture.Height / 2),
                             new Vector2(Vector2.Distance(start, end), 1f),
                             SpriteEffects.None, 0f);
        }

        [DebuggerHidden]
        public static void DrawLine(this SpriteBatch spriteBatch, Texture2D texture, Color color, Line line) {
            spriteBatch.DrawLine(texture, color, line.Start, line.End);
        }

        [DebuggerHidden]
        public static void DrawRectangle(this SpriteBatch spriteBatch, Texture2D texture, Color color, Rectangle rect) {
            spriteBatch.DrawLine(texture, color, rect.TopLine());
            spriteBatch.DrawLine(texture, color, rect.RightLine());
            spriteBatch.DrawLine(texture, color, rect.BottomLine());
            spriteBatch.DrawLine(texture, color, rect.LeftLine());
        }

        [DebuggerHidden]
        public static void DrawPolygon(this SpriteBatch spriteBatch, Texture2D texture, Color color, Polygon polygon) {
            for (var i = 0; i < polygon.VertexCount - 1; i++) spriteBatch.DrawLine(texture, color, new Line(polygon[i], polygon[i + 1]));
            spriteBatch.DrawLine(texture, color, new Line(polygon[polygon.VertexCount - 1], polygon[0]));
        }

        [DebuggerHidden]
        public static void DrawPolygon(this SpriteBatch spriteBatch, Texture2D texture, Color color, Vector2 p1, Vector2 p2, params Vector2[] points) {
            spriteBatch.DrawPolygon(texture, color, new Polygon(p1, p2, points));
        }

        [DebuggerHidden]
        public static void DrawCircle(this SpriteBatch spriteBatch, Texture2D texture, Color color, Vector2 center, float radius) {
            spriteBatch.DrawCircle(texture, color, new Circle(center, radius));
        }

        [DebuggerHidden]
        public static void DrawCircle(this SpriteBatch spriteBatch, Texture2D texture, Color color, Circle circle) {
            var angleStep = 1f / circle.Radius;

            var circlePoly = new Polygon { Vertices = new List<Vector2>() };
            for (var angle = 0f; angle < MathHelper.Pi * 2; angle += angleStep) {
                var x = (float) (circle.Radius + circle.Radius * Math.Cos(angle));
                var y = (float) (circle.Radius + circle.Radius * Math.Sin(angle));

                x += circle.Center.X - circle.Radius;
                y += circle.Center.Y - circle.Radius;

                circlePoly.Vertices.Add(new Vector2(x, y));
            }
            spriteBatch.DrawPolygon(texture, color, circlePoly);
        }

        #endregion

        #region Input

        #region Keyboard

        [DebuggerHidden]
        public static bool AreKeysDown(this KeyboardState ks, Keys k1, params Keys[] keys) {
            return ks.IsKeyDown(k1) && keys.All(ks.IsKeyDown);
        }

        [DebuggerHidden]
        public static bool AreKeysUp(this KeyboardState ks, Keys k1, params Keys[] keys) {
            return ks.IsKeyUp(k1) && keys.All(ks.IsKeyUp);
        }

        [DebuggerHidden]
        public static bool IsAnyKeyDown(this KeyboardState ks, Keys k1, params Keys[] keys) {
            return ks.IsKeyDown(k1) || keys.Any(ks.IsKeyDown);
        }

        [DebuggerHidden]
        public static bool IsAnyKeyUp(this KeyboardState ks, Keys k1, params Keys[] keys) {
            return ks.IsKeyUp(k1) || keys.Any(ks.IsKeyUp);
        }

        [DebuggerHidden]
        public static bool WasKeyJustPressed(this KeyboardState ks, KeyboardState old, Keys key) {
            return ks.IsKeyDown(key) && old.IsKeyUp(key);
        }

        #endregion

        #region Mouse

        [DebuggerHidden]
        public static Vector2 GetPosition(this MouseState mouse) {
            return new Vector2(mouse.X, mouse.Y);
        }

        #endregion

        [DebuggerHidden]
        public static bool WasJustPressed(this ButtonState button, ButtonState old) {
            return button == ButtonState.Pressed && old == ButtonState.Released;
        }

        #endregion
    }
}
