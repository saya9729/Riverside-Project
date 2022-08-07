using UnityEngine;

namespace Universal
{
    public static class Smoothing
    {
        public static float LinearSmoothFixedTime(float p_currentValue, float p_startValue, float p_endValue, float p_deltaTime, float p_timeInterval)
        {
            float smoothRate = Mathf.Abs(p_endValue - p_startValue) / p_timeInterval;

            return Mathf.MoveTowards(p_currentValue, p_endValue, smoothRate * p_deltaTime);
        }

        public static float SineWaveSmooth(float p_amplitude, float p_timeElapsed, float p_period)
        {
            return Mathf.Abs(p_amplitude * Mathf.PI * 2 / p_period * Mathf.Sin(p_timeElapsed * Mathf.PI * 2 / p_period));
        }
    }
}