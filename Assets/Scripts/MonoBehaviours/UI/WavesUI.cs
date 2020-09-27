using System.Text;
using TD.Logic;
using TMPro;
using UnityEngine;

namespace TD.MonoBehaviours.UI
{
    public class WavesUI : UIComponent
    {
        public override void Initialize(LevelComponents levelComponents)
        {
            base.Initialize(levelComponents);

            m_levelComponents.Events.Waves.InBetweenWavesStarted += OnInBetweenWavesStarted;
            m_levelComponents.Events.Waves.InBetweenWavesTimerUpdated += OnInBetweenWavesTimerUpdated;
            m_levelComponents.Events.Waves.WaveStarted += OnWaveStarted;
            m_levelComponents.Events.Waves.WaveTimerUpdated += OnWaveTimerUpdated;
            OnInBetweenWavesStarted(1);
        }

        private void OnWaveTimerUpdated(float timer)
        {
            UpdateDurationText(timer);
        }

        private void OnWaveStarted(int wave, int maxWaves)
        {
            m_waveLabel.text = "Wave: ";
            m_waveText.text = wave.ToString() + "/" + maxWaves.ToString();
        }

        private void OnInBetweenWavesTimerUpdated(float timer)
        {
            UpdateDurationText(timer);
        }

        private void OnInBetweenWavesStarted(int nextWave)
        {
            m_waveLabel.text = "Next Wave: ";
            m_waveText.text = nextWave.ToString();
        }

        private void UpdateDurationText(float timer)
        {
            int timerInInteger = Mathf.FloorToInt(timer);
            m_stringBuilder.Clear();
            m_stringBuilder.Append(timerInInteger.ToString());

            m_waveDuration.text = m_stringBuilder.ToString();
        }

        private void OnDestroy()
        {
            m_levelComponents.Events.Waves.InBetweenWavesStarted -= OnInBetweenWavesStarted;
            m_levelComponents.Events.Waves.InBetweenWavesTimerUpdated -= OnInBetweenWavesTimerUpdated;
            m_levelComponents.Events.Waves.WaveStarted -= OnWaveStarted;
            m_levelComponents.Events.Waves.WaveTimerUpdated -= OnWaveTimerUpdated;
        }

        private StringBuilder m_stringBuilder = new StringBuilder();

        [SerializeField]
        private TextMeshProUGUI m_waveLabel = default;
        [SerializeField]
        private TextMeshProUGUI m_waveText = default;
        [SerializeField]
        private TextMeshProUGUI m_waveDuration = default;
    }
}