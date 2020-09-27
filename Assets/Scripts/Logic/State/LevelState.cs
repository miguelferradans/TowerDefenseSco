using System;
using System.Collections.Generic;
using System.Collections;
using TD.Utils;
using UnityEngine;
using TD.Data.Levels;
using TD.Logic.Events;

namespace TD.Logic.RuntimeState
{
    [Serializable]
    public class LevelState
    {
        public uint Width => m_grid.Width;
        public uint Height => m_grid.Height;
        public int CurrentMoney => m_currentMoney;
        public RuntimeSlot SelectedSlot { get => m_selectedSlot; set => m_selectedSlot = value; }

        public LevelState(Level data)
        {
            m_grid = new Grid<RuntimeSlot>(data.Slots.Width, data.Slots.Height);
            m_currentMoney = (int)data.StartingMoney;
            InitializeSlots(data);
        }

        public RuntimeSlot this[Vector2Int coords] { get => m_grid[coords]; set => m_grid[coords] = value; }

        public RuntimeSlot this[int x, int y] { get => m_grid[x, y]; set => m_grid[x, y] = value; }

        public void ChangeMoney(int amount, LevelEvents events)
        {
            m_currentMoney += amount;

            m_currentMoney = (m_currentMoney < 0) ? 0 : m_currentMoney;

            events.OnChangedMoney(m_currentMoney);
        }
        
        public bool AnySlotHasEnemies()
        {
            foreach (var slot in this)
            {
                if (slot.Enemies.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(m_grid);
        }

        private void InitializeSlots(Level data)
        {
            foreach (var slot in data.Slots)
            {
                var runtimeSlot = new RuntimeSlot(slot.Coords, slot);
                m_grid[slot.Coords] = runtimeSlot;

                if (null != slot.Base)
                {
                    runtimeSlot.Base = slot.Base.BuildState();
                }
            }
        }

        [SerializeField]
        private Grid<RuntimeSlot> m_grid = default;

        [SerializeField]
        private RuntimeSlot m_selectedSlot = default;

        [SerializeField]
        private int m_currentMoney = default;

        public struct Enumerator : IEnumerator<RuntimeSlot>, IEnumerator
        {
            private Grid<RuntimeSlot> grid;
            private RuntimeSlot current;
            private uint index;

            internal Enumerator(Grid<RuntimeSlot> grid)
            {
                this.grid = grid;
                current = null;
                index = 0;
            }

            public RuntimeSlot Current => current;
            object IEnumerator.Current => current;

            public bool MoveNext()
            {
                if (index < grid.Size)
                {
                    current = GetCurrent(index);
                    index++;

                    return true;
                }

                return false;
            }
            public void Dispose()
            {

            }

            void IEnumerator.Reset()
            {
                current = null;
                index = 0;
            }

            private RuntimeSlot GetCurrent(uint index)
            {
                return grid[grid.ToCoords(index)];
            }
        }
    }
}