using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JdGameBase.Camera;
using JdGameBase.Core.GameComponents;
using JdGameBase.Core.Interfaces;
using JdGameBase.Extensions;
using JdGameBase.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Core.Services {
    public class EntityManager : JdComponent, IDrawableEntity {
        private readonly Camera2D _camera;
        private readonly bool _containsCamera;
        private readonly Texture2D _debugTexture;
        private readonly List<IEntity> _entities;
        public bool DrawBoundingBoxes;

        public EntityManager(JdGame game)
            : base(game) {
            _entities = new List<IEntity>();
            _debugTexture = ColorTexture.Create(game.GraphicsDevice, Color.White);
            DebugColor = Color.Lime;

            _camera = game.GetComponent<Camera2D>();
            _containsCamera = _camera != null;
        }

        public Color DebugColor { get; set; }

        public int EntityCount { get { return _entities.Count /*+ _drawables.Count + _updatables.Count*/; } }

        public int VisibleEntities {
            get {
                return _camera == null ?
                           _entities.Count :
                           _entities.OfType<Entity>().Count(x => _camera.IsInView(x));
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            Action<Entity> draw = x => {
                x.Draw(spriteBatch);
                if (!DrawBoundingBoxes) return;
                spriteBatch.DrawRectangle(_debugTexture, DebugColor, x.BoundingBox);
            };

            _entities.OfType<IDrawableEntity>().ForEach(x => x.Draw(spriteBatch));
            if (_containsCamera) _entities.OfType<Entity>().Where(x => _camera.IsInView(x) || x.AlwaysDraw).ForEach(draw);
            else _entities.OfType<Entity>().ForEach(draw);
        }

        public override void Update(float delta, GameTime gameTime) {
            //            _updatables.ForEach(x => x.Update(delta));
            _entities.OfType<IUpdatableEntity>().ToList().ForEach(x => x.Update(delta, gameTime));
        }

        public List<T> GetEntitiesOfType<T>() where T : IEntity {
            return (from entity in _entities
                    where entity.GetType().IsDerivedFrom<T>()
                    select (T) entity).ToList();
        }

        #region Entity Manipulation

        public void MoveUp(Entity entity) {
            if (!_entities.Contains(entity)) return;
            var idx = _entities.IndexOf(entity);

            if (idx == 0) return;
            _entities.Swap(idx, idx - 1);
        }

        public void MoveDown(Entity entity) {
            if (!_entities.Contains(entity)) return;
            var idx = _entities.IndexOf(entity);

            if (idx == 0) return;
            _entities.Swap(idx, idx + 1);
        }

        public void MoveBefore(Entity ent, Entity target) {
            if (!_entities.Contains(ent) || !_entities.Contains(target)) return;
            var entIdx = _entities.IndexOf(ent);
            var targetIdx = _entities.IndexOf(target);

            if (entIdx < targetIdx) return;
            _entities.Swap(entIdx, targetIdx);
        }

        public void MoveAfter(Entity ent, Entity target) {
            if (!_entities.Contains(ent) || !_entities.Contains(target)) return;
            var entIdx = _entities.IndexOf(ent);
            var targetIdx = _entities.IndexOf(target);

            if (entIdx > targetIdx) return;
            _entities.Swap(entIdx, targetIdx);
        }

        public void Swap(Entity one, Entity two) {
            if (!_entities.Contains(one) || !_entities.Contains(two)) return;
            _entities.Swap(one, two);
        }

        #endregion

        #region Entity Registration/Unregistration

        public void RegisterEntity(IEntity entity) {
            _entities.Add(entity);
        }

        public void RegisterEntity(IEnumerable<IEntity> ents) {
            _entities.AddRange(ents);
        }

        public void UnregisterEntity(IEntity entity) {
            if (_entities.Contains(entity)) _entities.Remove(entity);
        }

        public void UnregisterEntities(IEnumerable<IEntity> ents) {
            _entities.RemoveAll(ents.Contains);
        }

        public void UnregisterEntitiesOfType<T>() where T : IEntity {
            _entities.RemoveAll(x => x.GetType().IsDerivedFrom<T>());
        }

        public void UnregisterAllEntities<T>(Predicate<T> predicate) where T : IEntity {
            var ents = from entity in _entities
                       where entity.GetType().IsDerivedFrom<T>()
                       where predicate.Invoke((T) entity)
                       select entity;
            UnregisterEntities(ents);
        }

        #endregion
    }
}
