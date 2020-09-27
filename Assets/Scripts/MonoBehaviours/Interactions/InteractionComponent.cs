using TD.Logic;
using TD.Logic.Events;
using TD.Logic.RuntimeState;
using UnityEngine;

namespace TD.MonoBehaviours.Interactions
{
    public abstract class InteractionComponent : MonoBehaviour
    {
        public virtual void Initialize(LevelComponents levelComponents, RuntimeSlot slot, EventsFacade events)
        {
            m_levelComponents = levelComponents;
            m_slot = slot;
            m_events = events;
        }

        protected LevelComponents m_levelComponents = default;
        protected RuntimeSlot m_slot = default;
        protected EventsFacade m_events = default;
    }
}