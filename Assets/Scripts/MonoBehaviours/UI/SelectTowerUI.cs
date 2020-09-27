using TD.Data.Entities.Towers;
using TD.Logic;
using TD.Logic.RuntimeState;
using UnityEngine;

namespace TD.MonoBehaviours.UI
{
    public class SelectTowerUI : UIComponent
    {
        public override void Initialize(LevelComponents levelComponents)
        {
            base.Initialize(levelComponents);

            BuildUI();
            m_towersUIContainer.SetActive(false);

            m_levelComponents.Events.Slot.SelectedSlot += OnSelectedSlot;
            m_levelComponents.Events.Tower.BoughtTower += OnBoughtTower; ;
        }

        private void OnBoughtTower(RuntimeSlot slot, Tower tower)
        {
            m_towersUIContainer.SetActive(false);
        }

        private void OnSelectedSlot(RuntimeSlot slot)
        {
            m_towersUIContainer.SetActive(slot.SlotConfig.CanConstruct && null == slot.Tower);
        }

        private void BuildUI()
        {
            foreach (var tower in m_levelComponents.LevelConfig.AvailableTowers)
            {
                var towerUIInstance = Instantiate(tower.UIPrefab, m_towersUIContainer.transform);

                var uiComponents = towerUIInstance.GetComponentsInChildren<UIComponent>();

                foreach (var component in uiComponents)
                {
                    component.Initialize(m_levelComponents);
                }
            }
        }

        private void OnDestroy()
        {
            m_levelComponents.Events.Slot.SelectedSlot -= OnSelectedSlot;
            m_levelComponents.Events.Tower.BoughtTower -= OnBoughtTower; ;
        }

        [SerializeField]
        private GameObject m_towersUIContainer = default;
    }
}