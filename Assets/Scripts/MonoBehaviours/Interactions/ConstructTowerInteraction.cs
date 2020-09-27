using TD.Data.Entities.Towers;
using TD.Logic;
using TD.Logic.Events;
using TD.Logic.RuntimeState;
using UnityEngine;

namespace TD.MonoBehaviours.Interactions
{
    public class ConstructTowerInteraction : InteractionComponent
    {
        public override void Initialize(LevelComponents levelComponents, RuntimeSlot slot, EventsFacade events)
        {
            base.Initialize(levelComponents, slot, events);

            events.Slot.SelectedSlot += OnSelectedSlot;
            events.Tower.BoughtTower += OnBoughtTower;
        }

        private void OnSelectedSlot(RuntimeSlot slot)
        {
            bool selected = m_slot == slot;

            if (null != m_selectedObject)
            {
                m_selectedObject.SetActive(selected);
            }
        }

        private void OnBoughtTower(RuntimeSlot slot, Tower tower)
        {
            if (slot == m_slot)
            {
                m_events.Slot.SelectedSlot -= OnSelectedSlot;
                m_events.Tower.BoughtTower -= OnBoughtTower;
                Destroy(this.gameObject);
            }
        }

        private void OnDestroy()
        {
            m_events.Slot.SelectedSlot -= OnSelectedSlot;
            m_events.Tower.BoughtTower -= OnBoughtTower;
        }

        [SerializeField]
        private GameObject m_selectedObject = default;
    }
}