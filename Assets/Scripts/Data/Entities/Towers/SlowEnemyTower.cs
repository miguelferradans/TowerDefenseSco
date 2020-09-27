using System;
using TD.Logic.RuntimeState;
using UnityEngine;

namespace TD.Data.Entities.Towers
{
    [Serializable, CreateAssetMenu(menuName = "TD/Entities/Towers/Slow Debuff")]
    public class SlowEnemyTower : Tower
    {
        public float SlowPercentage => m_slowPercentage;
        public float DebuffDuration => m_debuffDuration;

        public override EntityState BuildState()
        {
            return new TowerState(this);
        }

        [SerializeField]
        private float m_slowPercentage = 0.25f;

        [SerializeField]
        private float m_debuffDuration = 1f;
    }
}