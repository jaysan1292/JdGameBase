// Project: JdGameBase
// Filename: ParallaxManager.cs
// 
// Author: Jason Recillo

using System;
using System.Collections;
using System.Collections.Generic;

using JdGameBase.Core;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//TODO: Redo using Camera2D class

namespace JdGameBase.Parallax {
    public class ParallaxManager : Entity, IEnumerable<Layer> {
        private readonly List<Layer> _layers;
        public Rectangle RenderingArea;

        public ParallaxManager(Rectangle visibleArea) {
            _layers = new List<Layer>();
            RenderingArea = visibleArea;
        }

        private float BaseLayerSortValue { get { return (float) 1 / _layers.Count; } }

        public float ScrollSpeedX { get; set; }

        public override Rectangle BoundingBox { get { return RenderingArea; } }

        public IEnumerator<Layer> GetEnumerator() {
            return _layers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public override void Draw(SpriteBatch spriteBatch) {
            var sortValue = BaseLayerSortValue;
            var originalSort = sortValue;
            foreach (var l in _layers) {
                var tex = l.Texture;
                var srcRect = new Rectangle((int) (-l.ScrollX), 0, RenderingArea.Width, tex.Height);
                spriteBatch.Draw(tex, new Vector2(0, l.PositionY), srcRect, Color.White, 0, Vector2.Zero, l.Scale, SpriteEffects.None, sortValue);

                sortValue += originalSort;
            }
        }

        public override void Update(float delta) {
            foreach (var layer in _layers) layer.ScrollX += ScrollSpeedX * layer.ParallaxFactor * delta / layer.Scale;
        }

        public void Add(Layer layer) {
            _layers.Add(layer);
        }

        public float GetLayerSortOrder(string name) {
            for (var i = 0; i < _layers.Count; i++) {
                var layer = _layers[i];
                if (layer.Name == name) return BaseLayerSortValue * i;
            }
            throw new ArgumentException("Layer not found", "name");
        }
    }
}
