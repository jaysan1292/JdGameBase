using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace JdGameBase.Core.Interfaces {
    public interface IFocusable {
        /// <summary>
        /// Gets the position that the camera should focus on.
        /// </summary>
        Vector2 FocusPosition { get; }
    }
}
