using System;
using UnityEngine;

namespace SpawnerTool.Runtime
{
    [HelpURL("https://joanorba.gitbook.io/spawnertool/v/api/runtime/timer")]
    [Serializable]
    public class Timer
    {
        [SerializeField] private float _timer;
        [SerializeField] private float _maxDuration;
        private event Action _onTimerEnd;

        /// <summary>
        /// When timer ends event is raised
        /// </summary>
        public Action OnTimerEnd
        {
            get => _onTimerEnd;
            set => _onTimerEnd = value;
        }

        /// <summary>
        /// Constructor of the timer
        /// </summary>
        /// <param name="duration">The length in seconds of the timer</param>
        public Timer(float duration)
        {
            _timer = duration;
            _maxDuration = duration;
        }

        /// <summary>
        /// Advances timer.
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Tick(float deltaTime)
        {
            if (_timer <= 0.0) 
                return;
            
            _timer -= deltaTime;

            if (_timer <= 0.0f)
            {
                _timer = 0.0f;
                WhenTimerEnds();
            }
        }

        private void WhenTimerEnds()
        {
            OnTimerEnd?.Invoke();
        }

        /// <summary>
        /// Get current time counting from Duration towards 0.
        /// </summary>
        /// <returns></returns>
        public float GetCountDownTimer()
        {
            return _timer;
        }

        /// <summary>
        /// Get current time counting from 0 to Duration.
        /// </summary>
        /// <returns></returns>
        public float GetCountUpTimer()
        {
            return _maxDuration - _timer;
        }
    }
}