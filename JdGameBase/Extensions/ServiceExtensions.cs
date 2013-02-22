// Project: JdGameBase
// Filename: ServiceExtensions.cs
// 
// Author: Jason Recillo

using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;

namespace JdGameBase.Extensions {
    public static class ServiceExtensions {
        [DebuggerHidden]
        public static void AddService<T>(this Game game, T provider) where T : class {
            game.Services.AddService(typeof(T), provider);
        }

        [DebuggerHidden]
        public static T GetService<T>(this Game game) where T : class {
            return (T) GetService(game, typeof(T));
        }

        [DebuggerHidden]
        public static void RemoveService<TService>(this Game game) where TService : class {
            game.Services.RemoveService(typeof(TService));
        }

        [DebuggerHidden]
        private static object GetService(Game game, Type type) {
            // Recursively look for the service type in the game's 
            // registered services, and if not found, return null

            return type != null ? game.Services.GetService(type) ?? GetService(game, type.BaseType) : null;
        }
    }
}
