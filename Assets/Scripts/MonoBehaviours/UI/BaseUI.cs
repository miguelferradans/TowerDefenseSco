using TD.Data.Entities;
using TD.Logic;
using TD.Logic.RuntimeState;
using TMPro;
using UnityEngine;

namespace TD.MonoBehaviours.UI
{
    public class BaseUI : UIComponent
    {
        public override void Initialize(LevelComponents levelComponents)
        {
            base.Initialize(levelComponents);

            m_levelComponents.Events.Entity.Hit += OnHit;

            m_currentHP.text = GetBase().MaxHP.ToString();
        }

        private void OnHit(EntityState state)
        {
            if (state.Data is Base)
            {
                m_currentHP.text = state.CurrentHP.ToString();
            }
        }

        private Base GetBase()
        {
            foreach (var slot in m_levelComponents.LevelConfig.Slots)
            {
                if (null != slot.Base)
                {
                    return slot.Base;
                }
            }

            return null;
        }

        private void OnDestroy()
        {
            m_levelComponents.Events.Entity.Hit -= OnHit;
        }

        [SerializeField]
        private TextMeshProUGUI m_currentHP = default;
    }
}