using TD.Logic;
using TMPro;
using UnityEngine;

namespace TD.MonoBehaviours.UI
{
    public class MoneyUI : UIComponent
    {
        public override void Initialize(LevelComponents levelComponents)
        {
            base.Initialize(levelComponents);

            m_levelComponents.Events.Level.ChangedMoney += OnChangedMoney;
            OnChangedMoney((int)m_levelComponents.LevelConfig.StartingMoney);
        }

        private void OnChangedMoney(int amount)
        {
            m_money.text = amount.ToString();
        }

        private void OnDestroy()
        {
            m_levelComponents.Events.Level.ChangedMoney -= OnChangedMoney;
        }

        [SerializeField]
        private TextMeshProUGUI m_money = default;
    }
}