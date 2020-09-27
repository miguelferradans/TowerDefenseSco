using System;
using TD.Data.Entities;
using UnityEngine;

namespace TD.Logic.RuntimeState
{
    [Serializable]
    public class EnemyState : EntityState
    {
        public uint CurrentAttackDamage => m_currentAttackDamage;
        public float SlowDebuff { get => m_slowDebuff; set => m_slowDebuff = value; }

        public float CurrentSpeed()
        {
            return m_currentSpeed * ((m_slowDebuff > 0f) ? m_slowDebuff : 1f);
        }

        public EnemyState(Enemy data) : base(data)
        {
            m_currentAttackDamage = data.AttackDamage;
            m_currentSpeed = data.InitialSpeed;
        }

        [SerializeField]
        private uint m_currentAttackDamage = default;

        [SerializeField]
        private float m_currentSpeed = default;

        [SerializeField]
        private float m_slowDebuff = default;
    }
}