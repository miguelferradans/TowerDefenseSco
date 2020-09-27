using System;

namespace TD.Logic.Events
{
    public class LevelEvents
    {
        public event Action Lost = delegate { };
        public event Action Won = delegate { };
        public event Action<int> ChangedMoney = delegate { };

        public void OnLost()
        {
            Lost.Invoke();
        }

        public void OnWon()
        {
            Won.Invoke();
        }

        public void OnChangedMoney(int currentMoney)
        {
            ChangedMoney.Invoke(currentMoney);
        }
    }
}