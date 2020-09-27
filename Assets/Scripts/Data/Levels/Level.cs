using System;
using System.Collections.Generic;
using System.Diagnostics;
using TD.Data.Entities;
using TD.Data.Entities.Towers;
using TD.Logic.RuntimeState;
using UnityEngine;

namespace TD.Data.Levels
{
    [Serializable, CreateAssetMenu(menuName = "TD/Levels/Level")]
    public class Level : ScriptableObject
    {
        public SlotsGrid Slots => m_slots;
        public GameObject SlotPrefab => m_slotPrefab;
        public GameObject CanConstructPrefab => m_canConstructPrefab;
        public GameObject PathPrefab => m_pathPrefab;
        public uint StartingMoney => m_startingMoney;
        public List<Tower> AvailableTowers => m_availableTowers;
        public List<Enemy> AvailableEnemies => m_availableEnemies;
        public uint NumberOfWaves => m_numberOfWaves;
        public float TimeBetweenWaves => m_timeBetweenWaves;
        public float WavesDuration => m_wavesDuration;
        public float InitialEnemySpawnInterval => m_initialEnemySpawnInterval;
        public float SpawnRateTimeDecreasePerWave => m_spawnRateTimeDecreasePerWave;
        public List<EnemyPathList> EnemyPaths => m_enemyPaths;

        public LevelState BuildState()
        {
            return new LevelState(this);
        }

        public List<Vector2Int> GetSlotsThatGenerateEnemies()
        {
            List<Vector2Int> slots = new List<Vector2Int>();

            foreach (var slot in Slots)
            {
                if (slot.CanGenerateEnemies)
                {
                    slots.Add(slot.Coords);
                }
            }

            return slots;
        }

        [Conditional("UNITY_EDITOR")]
        public void ResizeGrid(int width, int height)
        {
            var oldGrid = m_slots;

            if (null != oldGrid)
            {
                InstantiateGrid((uint)width, (uint)height);
                InitializeSlots();

                foreach (var slot in m_slots)
                {
                    var oldSlot = oldGrid[slot.Coords];

                    if (null != oldSlot)
                    {
                        m_slots[slot.Coords] = oldSlot;
                    }
                }
            }
        }

        [Conditional("UNITY_EDITOR")]
        public void Reset()
        {
            InstantiateGrid(m_defaultWidth, m_defaultHeight);
            InitializeSlots();
            m_enemyPaths.Clear();
        }

        private void OnEnable()
        {
            if (null == m_slots)
            {
                InstantiateGrid(m_defaultWidth, m_defaultHeight);
                InitializeSlots();
            }

            if (null == m_availableTowers)
            {
                m_availableTowers = new List<Tower>();
            }
        }

        private void InstantiateGrid(uint width, uint height)
        {
            m_slots = new SlotsGrid(width, height);
        }

        private void InitializeSlots()
        {
            for (int x = 0; x < m_slots.Width; x++)
            {
                for (int y = 0; y < m_slots.Height; y++)
                {
                    m_slots[x, y] = new Slot(x, y);
                }
            }
        }

        [SerializeField]
        private SlotsGrid m_slots = default;

        [SerializeField]
        private GameObject m_slotPrefab = default;

        [SerializeField]
        private GameObject m_canConstructPrefab = default;

        [SerializeField]
        private GameObject m_pathPrefab = default;

        [SerializeField]
        private uint m_startingMoney = default;

        [SerializeField]
        private List<Tower> m_availableTowers = new List<Tower>();

        [SerializeField]
        private List<Enemy> m_availableEnemies = new List<Enemy>();

        [SerializeField]
        private uint m_numberOfWaves = 1;

        [SerializeField]
        private float m_timeBetweenWaves = 5f;

        [SerializeField]
        private float m_wavesDuration = 5f;

        [SerializeField]
        private float m_initialEnemySpawnInterval = default;

        [SerializeField]
        private float m_spawnRateTimeDecreasePerWave = default;

        [SerializeField]
        private List<EnemyPathList> m_enemyPaths = new List<EnemyPathList>();

        private const uint m_defaultHeight = 15;
        private const uint m_defaultWidth = 15;
    }
}