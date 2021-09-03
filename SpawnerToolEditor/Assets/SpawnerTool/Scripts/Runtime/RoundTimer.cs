using System;

namespace SpawnerTool
{
    public class RoundTimer
    {
        private float _duration;
        private float _maxDuration;

        public event Action OnTimerEnd;
        
        public RoundTimer(float duration)
        {
            _duration = duration;
            _maxDuration = duration;
        }

        /// <summary>
        /// Advances timer.
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Tick(float deltaTime)
        {
            _duration -= deltaTime;

            if (_duration <= 0.0f)
            {
                WhenTimerEnds();
            }
        }

        private void WhenTimerEnds()
        {
            OnTimerEnd?.Invoke();
        }

        /// <summary>
        /// Get current time counting from MaxRoundTime towards 0.
        /// </summary>
        /// <returns></returns>
        public float GetCountDownTimer()
        {
            return _duration;
        }
        
        /// <summary>
        /// Get current time counting from 0 to MaxRoundTime.
        /// </summary>
        /// <returns></returns>
        public float GetCountUpTimer()
        {
            return _maxDuration - _duration;
        }
    }
}