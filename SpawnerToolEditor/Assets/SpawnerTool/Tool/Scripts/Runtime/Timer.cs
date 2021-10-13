using System;
using UnityEngine;

namespace SpawnerTool
{
    [Serializable]
    public class Timer
    {
        [SerializeField] private float _timer;
        [SerializeField] private float _maxDuration;
        public event Action OnTimerEnd;

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