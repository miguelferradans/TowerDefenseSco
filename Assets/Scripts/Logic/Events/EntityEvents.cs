using System;
using TD.Logic.RuntimeState;

namespace TD.Logic.Events
{
    public class EntityEvents
    {
        public event Action<EntityState> Hit = delegate { };
        public event Action<RuntimeSlot, EnemyState> EnemyEnteredSlot = delegate { };
        public event Action<EnemyState> EnemyDestroyed = delegate { };
        public event Action<EnemyState> EnemySpeedUpdated = delegate { };

        public void OnHit(EntityState entityState)
        {
            Hit.Invoke(entityState);
        }

        public void OnEnemyEnteredSlot(RuntimeSlot slot, EnemyState enemyState)
        {
            EnemyEnteredSlot.Invoke(slot, enemyState);
        }

        public void OnEnemyDestroyed(EnemyState enemyState)
        {
            EnemyDestroyed.Invoke(enemyState);
        }

        public void OnEnemySpeedUpdated(EnemyState enemyState)
        {
            EnemySpeedUpdated.Invoke(enemyState);
        }
    }
}