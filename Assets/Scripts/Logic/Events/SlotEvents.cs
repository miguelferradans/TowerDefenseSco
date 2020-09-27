using System;
using TD.Logic.RuntimeState;

namespace TD.Logic.Events
{
    public class SlotEvents
    {
        public event Action<RuntimeSlot> SelectedSlot = delegate { };

        public void OnSelectedSlot(RuntimeSlot slot)
        {
            SelectedSlot.Invoke(slot);
        }
    }
}