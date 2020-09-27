using TD.Data.Entities;
using TD.Logic.Events;
using UnityEngine;

namespace TD.Logic.RuntimeState
{
    public class EntityState
    {
        public Entity Data => m_data;
        public int CurrentHP => m_currentHP;

        public EntityState(Entity data)
        {
            m_data = data;
            m_currentHP = (int)m_data.MaxHP;
        }

        public void Hit(uint damage, EntityEvents events = null)
        {
            m_currentHP -= (int)damage;

            m_currentHP = (m_currentHP < 0) ? 0 : m_currentHP;

            events?.OnHit(this);
        }

        [SerializeField]
        protected Entity m_data = default;

        [SerializeField]
        protected int m_currentHP = default;
    }
}