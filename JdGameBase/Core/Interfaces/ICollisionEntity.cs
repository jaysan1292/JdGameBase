// Project: JdGameBase
// Filename: ICollisionEntity.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework;

namespace JdGameBase.Core.Interfaces {
    public interface ICollisionEntity {
        Rectangle CollisionBox { get; }
    }
}
