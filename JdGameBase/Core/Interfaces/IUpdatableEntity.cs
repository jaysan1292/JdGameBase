// Project: JdGameBase
// Filename: IUpdatableEntity.cs
// 
// Author: Jason Recillo

using System;

namespace JdGameBase.Core.Interfaces {
    public interface IUpdatableEntity : IEntity {
        void Update(float delta);
    }
}
