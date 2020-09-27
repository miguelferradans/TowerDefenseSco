using System;
using TD.Logic.RuntimeState;
using UnityEngine;

namespace TD.Data.Entities.Towers
{
    [Serializable, CreateAssetMenu(menuName = "TD/Entities/Towers/Projectile")]
    public class ProjectileTower : Tower
    {
        public GameObject ProjectilePrefab => m_projectilePrefab;
        public uint DamagePerProjectile => m_damagePerProjectile;
        public float ProjectileSpeed => m_projectileSpeed;
        public float RateOfFire => m_rateOfFire;
        public bool AttackRareEnemies => m_attackRareEnemies;

        public override EntityState BuildState()
        {
            return new TowerState(this);
        }

        [SerializeField]
        private GameObject m_projectilePrefab = default;

        [SerializeField]
        private uint m_damagePerProjectile = default;

        [SerializeField]
        private float m_projectileSpeed = default;

        [SerializeField]
        private float m_rateOfFire = default;

        [SerializeField]
        private bool m_attackRareEnemies = default;
    }
}