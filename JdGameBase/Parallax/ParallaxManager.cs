// Project: JdGameBase
// Filename: ParallaxManager.cs
// 
// Author: Jason Recillo

using System;
using System.Collections.Generic;
using System.Linq;

using JdGameBase.Camera;
using JdGameBase.Core.GameComponents;
using JdGameBase.Extensions;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Parallax {
    public class ParallaxManager : JdDrawableComponent {
        private readonly Camera2D _camera;
        private readonly List<Layer> _layers;
        private readonly SpriteBatch _spriteBatch;
        private Rectangle _viewport;

        public ParallaxManager(JdGame game)
            : base(game) {
            _layers = new List<Layer>();
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);
            var camera = game.GetComponent<Camera2D>();
            if (camera == null) throw new ArgumentException("Game does not contain a Camera2D component.");
            _camera = camera;
            _viewport = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
        }

        public void Add(Layer layer) {
            _layers.Add(layer);

            _layers.Sort((a, b) => a.Order.CompareTo(b.Order));
        }

        public override void Draw(GameTime gameTime) {
            _layers.ForEach(x => DrawParallaxLayer(_spriteBatch, x));
            base.Draw(gameTime);
        }

        private void DrawParallaxLayer(SpriteBatch sb, Layer layer) {
            //            var samplerstate = layer.IsSeamless ? SamplerState.PointWrap : SamplerState.PointClamp;

            sb.Begin(SpriteSortMode.FrontToBack, null, SamplerState.AnisotropicWrap, null,
                     null, null, _camera.GetViewMatrix(layer.Parallax));

            var wh = new Vector2(_viewport.Width, 0);
            wh = _camera.ScreenToWorld(wh, layer.Parallax);

            var spr = layer.Sprite;
            var rect = layer.IsSeamless ?
                           new Rectangle((int) (-spr.Position.X), 0, (int) wh.X, spr.Texture.Height) :
                           spr.SourceRect;

            sb.Draw(spr.Texture,
                    spr.Position,
                    rect,
                    spr.Color,
                    spr.Rotation,
                    spr.Origin,
                    spr.Scale,
                    spr.Effects,
                    GetLayerSortOrder(layer.Order));

            sb.End();
        }

        private float GetLayerSortOrder(int order) {
            if (_layers.Count == 0) return 0f;
            return (1f / _layers.Count) * order;
        }

        public override string ToString() {
            return "{{{0}}}".Fmt(_layers.Aggregate("", (s, l) => { return s + "{0}".Fmt(l.Order); }));
        }
    }
}
