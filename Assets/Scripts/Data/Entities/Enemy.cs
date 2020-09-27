using System;
using TD.Logic.RuntimeState;
using UnityEngine;

namespace TD.Data.Entities
{
    [Serializable, CreateAssetMenu(menuName = "TD/Entities/Enemy")]
    public class Enemy : Entity
    {
        public uint AttackDamage => m_attackDamage;
        public float InitialSpeed => m_initialSpeed;
        public int MoneyOnKill => m_moneyOnKill;
        public int Weight => m_weight;

        public override EntityState BuildState()
        {
            return new EnemyState(this);
        }

        [SerializeField]
        private uint m_attackDamage = default;

        [SerializeField]
        private float m_initialSpeed = default;

        [SerializeField]
        private int m_moneyOnKill = default;

        [SerializeField]
        private int m_weight = default;
    }
}