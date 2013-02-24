﻿// Project: JdGameBase
// Filename: Entity.cs
// 
// Author: Jason Recillo

using System;

using JdGameBase.Core.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Core {
    public abstract class Entity : IUpdatableEntity, IDrawableEntity {
        public virtual bool AlwaysDraw { get { return false; } }
        public virtual Rectangle BoundingBox { get { return Rectangle.Empty; } }
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(float delta, GameTime gameTime);
    }
}
