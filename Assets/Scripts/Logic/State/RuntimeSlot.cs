using System;
using System.Collections.Generic;
using TD.Data.Levels;
using UnityEngine;

namespace TD.Logic.RuntimeState
{
    [Serializable]
    public class RuntimeSlot
    {
        public Vector2Int Coords => m_coords;
        public EntityState Base { get => m_base; set => m_base = value; }
        public TowerState Tower { get => m_tower; set => m_tower = value; }
        public List<EnemyState> Enemies => m_enemies;
        public Slot SlotConfig => m_slotConfig;

        public RuntimeSlot(Vector2Int coords, Slot slotConfig)
        {
            m_coords = coords;
            m_slotConfig = slotConfig;
        }

        [SerializeField]
        private Vector2Int m_coords = default;

        [SerializeField]
        private EntityState m_base = default;

        [SerializeField]
        private TowerState m_tower = default;

        [SerializeField]
        private List<EnemyState> m_enemies = new List<EnemyState>();

        [SerializeField]
        private Slot m_slotConfig = default;
    }
}