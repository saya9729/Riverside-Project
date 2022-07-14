using UnityEngine;

namespace Universal
{
    public static class Smoothing
    {
        public static float LinearSmoothFixedRate(float p_currentValue, float p_targetValue, float p_smoothRate)
        {
            float difference = p_targetValue - p_currentValue;

            if (Mathf.Abs(difference) <= p_smoothRate)
            {
                return p_targetValue;
            }
            else if (difference > 0)
            {
                return p_currentValue + p_smoothRate;
            }
            else
            {
                return p_currentValue - p_smoothRate;
            }
        }

        public static float LinearSmoothFixedTime(float p_currentValue, float p_startValue, float p_endValue, float p_deltaTime, float p_timeInterval)
        {
            float smoothRate = Mathf.Abs(p_endValue - p_startValue) / p_timeInterval;

            return LinearSmoothFixedRate(p_currentValue, p_endValue, smoothRate * p_deltaTime);
        }

        public static Vector3 LinearSmoothFixedTime(Vector3 p_currentValue, Vector3 p_startValue, Vector3 p_endValue, float p_deltaTime, float p_timeInterval)
        {
            float smoothRateX = Mathf.Abs(p_endValue.x - p_startValue.x) / p_timeInterval;
            float smoothRateY = Mathf.Abs(p_endValue.y - p_startValue.y) / p_timeInterval;
            float smoothRateZ = Mathf.Abs(p_endValue.z - p_startValue.z) / p_timeInterval;

            return new Vector3(
                LinearSmoothFixedRate(p_currentValue.x, p_endValue.x, smoothRateX * p_deltaTime),
                LinearSmoothFixedRate(p_currentValue.y, p_endValue.y, smoothRateY * p_deltaTime),
                LinearSmoothFixedRate(p_currentValue.z, p_endValue.z, smoothRateZ * p_deltaTime));
        }
    }
}