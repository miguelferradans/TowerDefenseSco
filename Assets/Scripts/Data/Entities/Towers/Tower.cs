using System;
using UnityEngine;

namespace TD.Data.Entities.Towers
{
    [Serializable]
    public abstract class Tower : Entity
    {
        public GameObject UIPrefab => m_uiPrefab;
        public int Cost => m_cost;
        public uint Range => m_range;

        [SerializeField]
        protected GameObject m_uiPrefab = default;

        [SerializeField]
        protected int m_cost = default;

        [SerializeField]
        protected uint m_range = default;
    }
}