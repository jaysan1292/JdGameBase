using System;

using JdGameBase.Core.Interfaces;

namespace JdGameBase.Utils {
    public class Timer : IUpdatableEntity {
        private float _currentTime;

        public Timer()
            : this(0, false, () => { }) {
            // Initialize to a default that does nothing
        }

        public Timer(float time, bool startActive, Action callback, bool loop = false) {
            TimeLimit = time;
            Callback = callback;
            Active = startActive;
            IsLooping = loop;
        }

        public bool Active { get; set; }
        public bool IsLooping { get; set; }
        public float TimeLimit { get; set; }
        public Action Callback { get; set; }

        public void Update(float delta) {
            if (!Active) return;

            _currentTime += delta;
            if (_currentTime <= TimeLimit) return;

            Callback();

            if (!IsLooping) Active = false;
            _currentTime = 0f;
        }

        public void Pause() {
            Active = false;
        }

        public void Resume() {
            Active = true;
        }

        public void Toggle() {
            Active = !Active;
        }

        public void Reset() {
            _currentTime = 0;
        }
    }
}
