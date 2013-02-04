using System;
using System.Collections.Generic;
using System.Linq;

using JdGameBase.Core.Primitives;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JdGameBase.Extensions {
    public static class GameExtensions {
        #region Components

        public static void AddComponent<T>(this Game game, T component) where T : IGameComponent {
            game.Components.Add(component);
        }

        public static T GetComponent<T>(this Game game) where T : IGameComponent {
            return (T) GetComponent(game, typeof(T));
        }

        public static void RemoveAllComponents<TComponent>(this Game game) where TComponent : IGameComponent {
            (from component in game.Components
             let componentType = component.GetType()
             where componentType.IsDerivedFrom<TComponent>()
             select component).ToList().ForEach(x => game.Components.Remove(x));
        }

        public static void RemoveComponent<T>(this Game game) where T : IGameComponent {
            game.Components.Remove(game.GetComponent<T>());
        }

        private static object GetComponent(Game game, Type type) {
            if (type.BaseType == null) return null;
            return game.Components.FirstOrDefault(x => type == x.GetType()) ?? GetComponent(game, type.BaseType);
        }

        #endregion

        #region Drawing

        public static void DrawLine(this SpriteBatch spriteBatch, Texture2D texture, Color color, Vector2 start, Vector2 end) {
            spriteBatch.Draw(texture, start, null, color,
                             (float) Math.Atan2(end.Y - start.Y, end.X - start.X),
                             new Vector2(0f, (float) texture.Height / 2),
                             new Vector2(Vector2.Distance(start, end), 1f),
                             SpriteEffects.None, 0f);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Texture2D texture, Color color, Line line) {
            spriteBatch.DrawLine(texture, color, line.Start, line.End);
        }

        public static void DrawRectangle(this SpriteBatch spriteBatch, Texture2D texture, Color color, Rectangle rect) {
            var top = new Line(rect.TopLeft(), rect.TopRight());
            var right = new Line(rect.TopRight(), rect.BottomRight());
            var bottom = new Line(rect.BottomLeft(), rect.BottomRight());
            var left = new Line(rect.TopLeft(), rect.BottomLeft());
            spriteBatch.DrawLine(texture, color, top);
            spriteBatch.DrawLine(texture, color, right);
            spriteBatch.DrawLine(texture, color, bottom);
            spriteBatch.DrawLine(texture, color, left);
        }

        public static void DrawPolygon(this SpriteBatch spriteBatch, Texture2D texture, Color color, Polygon polygon) {
            for (var i = 0; i < polygon.VertexCount - 1; i++) spriteBatch.DrawLine(texture, color, new Line(polygon[i], polygon[i + 1]));
            spriteBatch.DrawLine(texture, color, new Line(polygon[polygon.VertexCount - 1], polygon[0]));
        }

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

        public static Vector2 GetPosition(this MouseState mouse) {
            return new Vector2(mouse.X, mouse.Y);
        }

        public static bool AreKeysDown(this KeyboardState ks, Keys k1, params Keys[] keys) {
            return ks.IsKeyDown(k1) && keys.All(ks.IsKeyDown);
        }

        public static bool AreKeysUp(this KeyboardState ks, Keys k1, params Keys[] keys) {
            return ks.IsKeyUp(k1) && keys.All(ks.IsKeyUp);
        }

        public static bool IsAnyKeyDown(this KeyboardState ks, Keys k1, params Keys[] keys) {
            return ks.IsKeyDown(k1) || keys.Any(ks.IsKeyDown);
        }

        public static bool IsAnyKeyUp(this KeyboardState ks, Keys k1, params Keys[] keys) {
            return ks.IsKeyUp(k1) || keys.Any(ks.IsKeyUp);
        }

        public static bool WasKeyJustPressed(this KeyboardState ks, KeyboardState old, Keys key) {
            return ks.IsKeyDown(key) && old.IsKeyUp(key);
        }

        #endregion
    }
}
