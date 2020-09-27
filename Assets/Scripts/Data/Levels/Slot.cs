using System;
using TD.Data.Entities;
using UnityEngine;

namespace TD.Data.Levels
{
    [Serializable]
    public class Slot
    {
        public Vector2Int Coords => m_coords;
        public Base Base { get => m_base; set => m_base = value; }
        public bool CanConstruct { get => m_canConstruct; set => m_canConstruct = value; }
        public bool CanGenerateEnemies { get => m_canGenerateEnemies; set => m_canGenerateEnemies = value; }

        public Slot(int x, int y)
        {
            m_coords = new Vector2Int(x, y);
        }

        [SerializeField]
        private Vector2Int m_coords = default;

        [SerializeField]
        private Base m_base = default;

        [SerializeField]
        private bool m_canConstruct = default;

        [SerializeField]
        private bool m_canGenerateEnemies = default;
    }
}