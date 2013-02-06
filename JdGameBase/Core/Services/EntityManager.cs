// Project: JdGameBase
// Filename: EntityManager.cs
// 
// Author: Jason Recillo

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JdGameBase.Core.Interfaces;
using JdGameBase.Extensions;
using JdGameBase.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Core.Services {
    public class EntityManager {
        //TODO: Remove _drawables and _updatables as IDrawableEntity and IUpdatableEntity now inherity IEntity
        private readonly Camera2D _camera;
        private readonly bool _containsCamera;
        private readonly Texture2D _debugTexture;
        private readonly List<IDrawableEntity> _drawables;
        private readonly List<Entity> _entities;
        private readonly Game _game;
        private readonly List<IUpdatableEntity> _updatables;
        public bool DrawBoundingBoxes;

        public EntityManager(Game game) {
            _entities = new List<Entity>();
            _drawables = new List<IDrawableEntity>();
            _updatables = new List<IUpdatableEntity>();
            _debugTexture = ColorTexture.Create(game.GraphicsDevice, Color.White);
            _game = game;

            _camera = game.GetComponent<Camera2D>();
            _containsCamera = _camera != null;
        }

        public int EntityCount { get { return _entities.Count + _drawables.Count + _updatables.Count; } }

        public int VisibleEntities {
            get {
                return _camera == null ?
                           _entities.Count :
                           _entities.FindAll(x => _camera.IsInView(x)).Count;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            Action<Entity> draw = x => {
                x.Draw(spriteBatch);
                if (!DrawBoundingBoxes) return;
                spriteBatch.DrawRectangle(_debugTexture, Color.Lime, x.BoundingBox);
            };

            _drawables.ForEach(x => x.Draw(spriteBatch));
            if (_containsCamera) _entities.FindAll(x => _camera.IsInView(x) || x.AlwaysDraw).ForEach(draw);
            else _entities.ForEach(draw);
        }

        public void Update(float delta) {
            _updatables.ForEach(x => x.Update(delta));
            _entities.ForEach(x => x.Update(delta));
        }

        public List<T> GetAllEntities<T>() where T : Entity {
            return (from entity in _entities
                    where entity.GetType().IsDerivedFrom<T>()
                    select (T) entity).ToList();
        }

        public List<T> GetAllDrawables<T>() where T : IDrawableEntity {
            return (from entity in _drawables
                    where entity.GetType().IsDerivedFrom<T>()
                    select (T) entity).ToList();
        }

        public List<T> GetAllUpdatables<T>() where T : IUpdatableEntity {
            return (from entity in _updatables
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

        #region IEntity

        public void RegisterEntity(Entity entity) {
            _entities.Add(entity);
        }

        public void RegisterEntity(IEnumerable<Entity> ents) {
            _entities.AddRange(ents);
        }

        public void UnregisterEntity(Entity entity) {
            if (_entities.Contains(entity)) _entities.Remove(entity);
        }

        public void UnregisterEntities(IEnumerable<Entity> ents) {
            _entities.RemoveAll(ents.Contains);
        }

        public void UnregisterEntityType<T>() where T : Entity {
            _entities.RemoveAll(x => x.GetType().IsDerivedFrom<T>());
        }

        public void UnregisterAllEntities<T>(Predicate<T> predicate) where T : Entity {
            var ents = from entity in _entities
                       where entity.GetType().IsDerivedFrom<T>()
                       where predicate.Invoke((T) entity)
                       select entity;
            UnregisterEntities(ents);
        }

        #endregion

        #region IDrawableEntity

        public void RegisterEntity(IDrawableEntity entity) {
            _drawables.Add(entity);
        }

        public void RegisterEntity(IEnumerable<IDrawableEntity> ents) {
            _drawables.AddRange(ents);
        }

        public void UnregisterEntity(IDrawableEntity entity) {
            if (_drawables.Contains(entity)) _drawables.Remove(entity);
        }

        public void UnregisterEntities(IEnumerable<IDrawableEntity> ents) {
            _drawables.RemoveAll(ents.Contains);
        }

        public void UnregisterDrawableEntityType<T>() where T : IDrawableEntity {
            _drawables.RemoveAll(x => x.GetType().IsDerivedFrom<T>());
        }

        public void UnregisterAllDrawableEntities<T>(Predicate<T> predicate) where T : IDrawableEntity {
            var ents = from entity in _drawables
                       where entity.GetType().IsDerivedFrom<T>()
                       where predicate.Invoke((T) entity)
                       select entity;
            UnregisterEntities(ents);
        }

        #endregion

        #region IUpdatableEntity

        public void RegisterEntity(IUpdatableEntity entity) {
            _updatables.Add(entity);
        }

        public void RegisterEntity(IEnumerable<IUpdatableEntity> ents) {
            _updatables.AddRange(ents);
        }

        public void UnregisterEntity(IUpdatableEntity entity) {
            if (_updatables.Contains(entity)) _updatables.Remove(entity);
        }

        public void UnregisterEntities(IEnumerable<IUpdatableEntity> ents) {
            _updatables.RemoveAll(ents.Contains);
        }

        public void UnregisterUpdatableEntityType<T>() where T : IUpdatableEntity {
            _updatables.RemoveAll(x => x.GetType().IsDerivedFrom<T>());
        }

        public void UnregisterAllUpdatableEntities<T>(Predicate<T> predicate) where T : IUpdatableEntity {
            var ents = from entity in _updatables
                       where entity.GetType().IsDerivedFrom<T>()
                       where predicate.Invoke((T) entity)
                       select entity;
            UnregisterEntities(ents);
        }

        #endregion

        #endregion
    }
}
