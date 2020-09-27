using System;

namespace TD.Logic.Events
{
    public class WaveEvents 
    {
        public event Action<int, int> WaveStarted = delegate { };
        public event Action<float> WaveTimerUpdated = delegate { };
        public event Action<int> InBetweenWavesStarted = delegate { };
        public event Action<float> InBetweenWavesTimerUpdated = delegate { };

        public void OnWaveStarted(int waveNumber, int maxWaves)
        {
            WaveStarted.Invoke(waveNumber, maxWaves);
        }

        public void OnWaveTimerUpdated(float timer)
        {
            WaveTimerUpdated.Invoke(timer);
        }

        public void OnInBetweenWavesStarted(int nextWave)
        {
            InBetweenWavesStarted.Invoke(nextWave);
        }

        public void OnInBetweenWavesTimerUpdated(float timer)
        {
            InBetweenWavesTimerUpdated.Invoke(timer);
        }
    }
}