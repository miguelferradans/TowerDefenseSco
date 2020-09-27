using System;
using TD.Utils;

namespace TD.Data.Levels
{
    [Serializable]
    public class SlotsGrid : Grid<Slot>
    {
        public SlotsGrid(uint columns, uint rows) : base(columns, rows)
        {
        }
    }
}