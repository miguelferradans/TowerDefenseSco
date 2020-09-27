using TD.Logic;
using TD.Logic.Events;
using TD.Logic.RuntimeState;

namespace TD.MonoBehaviours.Interactions
{
    public class BaseInteraction : InteractionComponent
    {
        public override void Initialize(LevelComponents levelComponents, RuntimeSlot slot, EventsFacade events)
        {
            base.Initialize(levelComponents, slot, events);

            events.Entity.EnemyEnteredSlot += OnEnemyEnteredSlot;
        }

        private void OnEnemyEnteredSlot(RuntimeSlot slot, EnemyState enemy)
        {
            if (slot == m_slot)
            {
                slot.Base.Hit(enemy.CurrentAttackDamage, m_events.Entity);

                if (slot.Base.CurrentHP == 0)
                {
                    m_events.Level.OnLost();
                }
            }
        }

        private void OnDestroy()
        {
            m_events.Entity.EnemyEnteredSlot -= OnEnemyEnteredSlot;
        }
    }
}