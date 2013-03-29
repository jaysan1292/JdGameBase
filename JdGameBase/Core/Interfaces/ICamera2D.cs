using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Core.Interfaces {
    // http://stackoverflow.com/questions/712296/xna-2d-camera-engine-that-follows-sprite
    public interface ICamera2D {
        /// <summary>
        /// Gets or sets the position of the camera.
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the move speed of the camera.
        /// The camera will tween to its destination.
        /// </summary>
        float MoveSpeed { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the camera.
        /// </summary>
        float Rotation { get; set; }

        /// <summary>
        /// Gets the origin of the viewport (accounts for Zoom).
        /// </summary>
        Vector2 Origin { get; }

        /// <summary>
        /// Gets or sets the scale of the camera.
        /// </summary>
        float Zoom { get; set; }

        /// <summary>
        /// Gets the screen center (does not account for Zoom).
        /// </summary>
        Vector2 ScreenCenter { get; }

        /// <summary>
        /// Gets the transform that can be applied to 
        /// the SpriteBatch class.
        /// </summary>
        /// <see cref="SpriteBatch"/>
        Matrix Transform { get; }

        /// <summary>
        /// Gets or sets the focus of the Camera.
        /// </summary>
        IFocusable Focus { get; set; }

        /// <summary>
        /// Determines whether the target is in view given the specified position.
        /// This can be used to increase performance by not drawing objects 
        /// directly in the viewport.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="texture"></param>
        /// <returns>True if the target is in view at the specified position. False otherwise.</returns>
        bool IsInView(Vector2 position, Texture2D texture);
    }
}
