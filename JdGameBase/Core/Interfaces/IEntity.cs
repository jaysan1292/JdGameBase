// Project: JdGameBase
// Filename: IEntity.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework;

namespace JdGameBase.Core.Interfaces {
    public interface IEntity : IUpdatableEntity, IDrawableEntity {
        Rectangle BoundingBox { get; }
    }
}
