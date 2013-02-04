using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JdGameBase.Core.Interfaces;
using JdGameBase.Extensions;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Graphics {
    public class SpriteSheet : IEnumerable<ISprite>, IUpdatableEntity {
        private readonly Texture2D _baseTexture;
        private readonly Dictionary<string, ISprite> _textures;
        public string Name;

        public SpriteSheet(ContentManager content, string textureName) {
            _baseTexture = content.Load<Texture2D>(textureName);
            _textures = new Dictionary<string, ISprite>();
            Name = textureName;
        }

        public int SpriteCount { get { return _textures.Count; } }

        public ISprite this[string name] { get { return _textures[name]; } set { _textures[name] = value; } }

        public dynamic this[string name, Type type]{get {
            return (from sprite in _textures
                    where sprite.Value.IsType(type) &&
                          sprite.Key == name
                    select sprite.Value).Single();
        }}

        public IEnumerator<ISprite> GetEnumerator() {
            return _textures.Values.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Update(float delta) {
            // Update all animated sprites
            (from sprite in this
             where sprite.IsType<AnimatedSprite>()
             select (AnimatedSprite) sprite)
                .ToList().ForEach(x => x.Update(delta));
        }

        public void Add(string name, ISprite sprite) {
            sprite.Texture = _baseTexture;
            sprite.Texture.Name = name;
            _textures.Add(name, sprite);
        }
    }
}
