using System;
using TD.Data.Entities.Towers;
using TD.Logic.RuntimeState;

namespace TD.Logic.Events
{
    public class TowerEvents
    {
        public event Action<RuntimeSlot, Tower> BoughtTower = delegate { };

        public void OnBoughtTower(RuntimeSlot slot, Tower boughtTower)
        {
            BoughtTower.Invoke(slot, boughtTower);
        }
    }
}