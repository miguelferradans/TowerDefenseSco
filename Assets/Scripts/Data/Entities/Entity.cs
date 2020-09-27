using System;
using TD.Logic.RuntimeState;
using UnityEngine;

namespace TD.Data.Entities
{
    [Serializable]
    public abstract class Entity : ScriptableObject
    {
        public GameObject Prefab => m_prefab;
        public uint MaxHP => m_maxHP;

        public abstract EntityState BuildState();

        [SerializeField]
        private GameObject m_prefab = default;

        [SerializeField]
        private uint m_maxHP = default;
    }
}