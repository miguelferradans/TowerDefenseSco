using TD.Data.Entities.Towers;
using TD.Logic;
using TD.Logic.RuntimeState;
using TMPro;
using UnityEngine;

namespace TD.MonoBehaviours.UI
{
    public class BuyProjectileTowerButton : UIComponent
    {
        public override void Initialize(LevelComponents levelComponents)
        {
            base.Initialize(levelComponents);

            m_nameText.text = m_tower.name;
            m_dpsText.text = "DPS " + Mathf.CeilToInt(m_tower.DamagePerProjectile * (1 / m_tower.RateOfFire)).ToString();
            m_costText.text = "Cost " + m_tower.Cost.ToString();
            m_rangeText.text = "Range " + m_tower.Range.ToString();
        }

        public void BuyTower()
        {
            var selectedSlot = m_levelComponents.State.SelectedSlot;

            if (null != selectedSlot && null == selectedSlot.Tower 
                && m_levelComponents.State.CurrentMoney >= m_tower.Cost)
            {
                selectedSlot.Tower = m_tower.BuildState() as TowerState;

                m_levelComponents.View.InstantiateEntity(m_levelComponents, selectedSlot, m_tower.Prefab, m_levelComponents.Events);

                m_levelComponents.Events.Tower.OnBoughtTower(selectedSlot, m_tower);
                m_levelComponents.State.ChangeMoney(-m_tower.Cost, m_levelComponents.Events.Level);
            }
        }

        [SerializeField]
        private ProjectileTower m_tower = default;

        [SerializeField]
        private TextMeshProUGUI m_nameText = default;

        [SerializeField]
        private TextMeshProUGUI m_dpsText = default;

        [SerializeField]
        private TextMeshProUGUI m_costText = default;

        [SerializeField]
        private TextMeshProUGUI m_rangeText = default;
    }
}