// Project: JdGameBase
// Filename: Quadtree.cs
// 
// Author: Jason Recillo

using System;
using System.Collections.Generic;

using JdGameBase.Core;
using JdGameBase.Core.Interfaces;
using JdGameBase.Extensions;
using JdGameBase.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Utils {
    //TODO: Test this class
    public class Quadtree<TEntity> where TEntity : IQuadObject {
        private readonly int _level;
        private readonly Quadtree<TEntity>[] _nodes;
        private readonly List<TEntity> _objects;
        private Rectangle _bounds;
        private int _maxLevels;
        private int _maxObjects;
        private bool _initialized;

        public Quadtree(int level, Rectangle bounds) {
            _level = level;
            _bounds = bounds;
            _objects = new List<TEntity>();
            _nodes = new Quadtree<TEntity>[4];
        }

        public void Initialize(int maxLevels = 5, int maxObjects = 10) {
            if (_initialized) return;
            _maxLevels = maxLevels;
            _maxObjects = maxObjects;
            _initialized = true;
        }

        /// <summary>
        /// Clears the quadtree.
        /// </summary>
        public void Clear() {
            _objects.Clear();
            for (var i = 0; i < _nodes.Length; i++) {
                if (_nodes[i] != null) {
                    _nodes[i].Clear();
                    _nodes[i] = null;
                }
            }
        }

        /// <summary>
        /// Splits the node into four subnodes.
        /// </summary>
        public void Split() {
            var subWidth = _bounds.Width / 2;
            var subHeight = _bounds.Height / 2;
            var x = _bounds.X;
            var y = _bounds.Y;

            _nodes[0] = new Quadtree<TEntity>(_level + 1, new Rectangle(x + subWidth, y, subWidth, subHeight));
            _nodes[1] = new Quadtree<TEntity>(_level + 1, new Rectangle(x, y, subWidth, subHeight));
            _nodes[2] = new Quadtree<TEntity>(_level + 1, new Rectangle(x, y + subHeight, subWidth, subHeight));
            _nodes[3] = new Quadtree<TEntity>(_level + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight));
            foreach (var node in _nodes) node.Initialize(_maxLevels, _maxObjects);
        }

        /// <summary>
        /// Inserts each all given entities into the quadtree.
        /// </summary>
        /// <see cref="Insert(TEntity)"/>
        public void Insert(IEnumerable<TEntity> ents) {
            foreach (var ent in ents) Insert(ent);
        }

        /// <summary>
        /// Insert the object into the quadtree. If the node 
        /// exceeds the capacity, it will split and add all 
        /// objects to their corresponding nodes.
        /// </summary>
        public void Insert(TEntity ent) {
            if (_nodes[0] != null) {
                var index = GetIndex(ent);

                if (index != -1) {
                    _nodes[index].Insert(ent);
                    return;
                }
            }
            _objects.Add(ent);

            if (!_initialized) Initialize();
            if (_objects.Count > _maxObjects && _level < _maxLevels) {
                if (_nodes[0] == null) Split();

                var i = 0;
                while (i < _objects.Count) {
                    var index = GetIndex(_objects[i]);
                    if (index != -1) {
                        _nodes[index].Insert(_objects[i]);
                        _objects.RemoveAt(i);
                    } else i++;
                }
            }
        }

        public List<TEntity> Retrieve(TEntity ent) {
            var returnObject = new List<TEntity>();
            return Retrieve(returnObject, ent);
        }

        /// <summary>
        /// Returns all objects that could collide with the given object.
        /// </summary>
        public List<TEntity> Retrieve(List<TEntity> returnObjects, TEntity ent) {
            var index = GetIndex(ent);
            if (index != -1 && _nodes[0] != null) _nodes[index].Retrieve(returnObjects, ent);
            returnObjects.AddRange(_objects);

            return returnObjects;
        }

        /// <summary>
        /// Determine which node the object belongs to. -1 means
        /// object cannot completely fit within a child node and is part
        /// of the parent node
        /// </summary>
        private int GetIndex(TEntity ent) {
            var rect = ent.Rect;
            var index = -1;
            var verticalMidpoint = _bounds.X + (_bounds.Width / 2f);
            var horizontalMidpoint = _bounds.Y + (_bounds.Height / 2f);

            // Object can completely fit within the top quadrants
            var topQuadrant = (rect.Y < horizontalMidpoint && rect.Y + rect.Height < horizontalMidpoint);
            // Object can completely fit within the bottom quadrants
            var bottomQuadrant = (rect.Y > horizontalMidpoint);

            if (rect.X < verticalMidpoint && rect.X + rect.Width < verticalMidpoint) {
                // Object can completely fit within the left quadrants
                if (topQuadrant) index = 1;
                else if (bottomQuadrant) index = 2;
            } else if (rect.X > verticalMidpoint) {
                // Object can completely fit within the right quadrants
                if (topQuadrant) index = 0;
                else if (bottomQuadrant) index = 3;
            }

            return index;
        }

        #region Debug Drawing

        /// <summary>
        /// Debug method to draw the boundaries of all of the nodes in the quadtree.
        /// </summary>
        
        public void Draw(SpriteBatch spriteBatch) {
            Draw(spriteBatch, Color.Lime);
        }

        public void Draw(SpriteBatch spriteBatch, Color color) {
            var tex = ColorTexture.Create(spriteBatch.GraphicsDevice, Color.White);
            Draw(spriteBatch, tex, color);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font) {
            Draw(spriteBatch, font, Color.Lime);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Color color) {
            var tex = ColorTexture.Create(spriteBatch.GraphicsDevice, Color.White);
            Draw(spriteBatch, tex, font, color);
        }

        private void Draw(SpriteBatch spriteBatch, Texture2D tex, Color color) {
            spriteBatch.DrawRectangle(tex, color, _bounds);
            foreach (var node in _nodes) if (node != null) node.Draw(spriteBatch, tex, color);
        }

        private void Draw(SpriteBatch spriteBatch, Texture2D tex, SpriteFont font, Color color) {
            spriteBatch.DrawRectangle(tex, color, _bounds);
            var str = "Objects: {0}".Fmt(_objects.Count);
            var dim = font.MeasureString(str);
            spriteBatch.DrawString(font, str, new Vector2(_bounds.X, _bounds.Y + _bounds.Height - dim.Y), color);
            foreach (var node in _nodes) if (node != null) node.Draw(spriteBatch, tex, font, color);
        }

        #endregion
    }
}
