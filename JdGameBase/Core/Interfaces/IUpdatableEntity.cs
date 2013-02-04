// Project: JdGameBase
// Filename: IUpdatableEntity.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework;

namespace JdGameBase.Core.Interfaces {
    public interface IUpdatableEntity :IEntity{
        void Update(float delta);
    }
}
