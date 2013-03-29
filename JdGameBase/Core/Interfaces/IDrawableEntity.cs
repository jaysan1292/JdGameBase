using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Core.Interfaces {
    public interface IDrawableEntity : IEntity {
        void Draw(SpriteBatch spriteBatch);
    }
}
