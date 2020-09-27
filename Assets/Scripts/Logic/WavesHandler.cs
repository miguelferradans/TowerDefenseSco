using System.Collections;
using System.Collections.Generic;
using TD.Data.Entities;
using TD.Logic;
using TD.Logic.RuntimeState;
using TD.MonoBehaviours.Interactions;
using UnityEngine;

namespace TD.MonoBehaviours
{
    public class WavesHandler : MonoBehaviour
    {
        public void Initialize(LevelComponents levelComponents)
        {
            m_levelComponents = levelComponents;
            m_currentSpawnInterval = levelComponents.LevelConfig.InitialEnemySpawnInterval;
            m_slotsThatGenerateEnemies = levelComponents.LevelConfig.GetSlotsThatGenerateEnemies();

            var waitForWaveRoutine = WaitForNewWave();
            StartCoroutine(waitForWaveRoutine);
        }

        private IEnumerator WaitForNewWave()
        {
            float timeElapsed = 0f;

            var timeBetweenWaves = m_levelComponents.LevelConfig.TimeBetweenWaves;
            m_levelComponents.Events.Waves.OnInBetweenWavesStarted(m_currentWave + 1);

            while (timeElapsed < timeBetweenWaves)
            {
                timeElapsed += Time.deltaTime;
                m_levelComponents.Events.Waves.OnInBetweenWavesTimerUpdated(timeBetweenWaves - timeElapsed);

                yield return null;
            }

            m_currentWave++;
            
            if (m_currentWave > 1)
            {
                m_currentSpawnInterval -= m_levelComponents.LevelConfig.SpawnRateTimeDecreasePerWave;
            }

            var waveRoutine = WaveRoutine();
            StartCoroutine(waveRoutine);
        }

        private IEnumerator WaveRoutine()
        {
            float timeElapsed = 0f;
            float timeSinceLastGeneration = 0f;

            var waveDuration = m_levelComponents.LevelConfig.WavesDuration;

            m_levelComponents.Events.Waves.OnWaveStarted(m_currentWave, (int)m_levelComponents.LevelConfig.NumberOfWaves);

            while (timeElapsed <= waveDuration || m_levelComponents.State.AnySlotHasEnemies())
            {
                if ((timeElapsed <= waveDuration) 
                    && timeElapsed - timeSinceLastGeneration >= m_currentSpawnInterval)
                {
                    SpawnEnemy();
                    timeSinceLastGeneration = timeElapsed;
                }

                timeElapsed += Time.deltaTime;

                float timeLeft = waveDuration - timeElapsed;

                if (timeLeft > 0f)
                {
                    m_levelComponents.Events.Waves.OnWaveTimerUpdated(timeLeft);
                }

                yield return null;
            }

            if (m_currentWave < m_levelComponents.LevelConfig.NumberOfWaves)
            {
                StartCoroutine(WaitForNewWave());
            }
            else
            {
                m_levelComponents.Events.Level.OnWon();
            }
        }

        private void SpawnEnemy()
        {
            System.Random rng = new System.Random();
            var chosenSlot = rng.Next(0, m_slotsThatGenerateEnemies.Count);
            var enemyData = PickWeightedEnemy(rng);

            EnemyState enemyState = enemyData.BuildState() as EnemyState;
            RuntimeSlot slot = m_levelComponents.State[m_slotsThatGenerateEnemies[chosenSlot]];
            slot.Enemies.Add(enemyState);
            m_levelComponents.View.InstantiateEnemy(enemyState, m_levelComponents, slot, enemyData.Prefab, m_levelComponents.Events);
        }

        private Enemy PickWeightedEnemy(System.Random rng)
        {
            int total = 0;

            foreach (var enemy in m_levelComponents.LevelConfig.AvailableEnemies)
            {
                total += enemy.Weight;
            }

            int randomNumber = rng.Next(0, total);

            foreach (var enemy in m_levelComponents.LevelConfig.AvailableEnemies)
            {
                if (randomNumber < enemy.Weight)
                {
                    return enemy;
                }

                randomNumber = randomNumber - enemy.Weight;
            }

            return null;
        }

        private LevelComponents m_levelComponents = default;
        private List<Vector2Int> m_slotsThatGenerateEnemies = default;

        private int m_currentWave = 0;
        private float m_currentSpawnInterval = default;
    }
}