using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD.Utils
{
    [Serializable]
    public class Grid<T> : ICloneable
    {
        public Grid(uint columns, uint rows)
        {
            m_columns = columns;
            m_rows = rows;
            m_grid = new T[columns * rows];
        }

        public uint Width { get { return m_columns; } }

        public uint Height { get { return m_rows; } }

        public uint Size { get { return m_columns * m_rows; } }

        public T this[int x, int y]
        {
            get
            {
                if (x < Width && y < Height && x >= 0 && y >= 0)
                {
                    return m_grid[x + y * m_columns];
                }
                else
                {
                    return default(T);
                }
            }
            set
            {
                if (x < Width && y < Height && x >= 0 && y >= 0)
                {
                    m_grid[x + y * m_columns] = value;
                }
            }
        }

        public T this[Vector2Int coords]
        {
            get { return this[coords.x, coords.y]; }
            set { this[coords.x, coords.y] = value; }
        }

        public Vector2Int ToCoords(uint flattenedIndex)
        {
            if (flattenedIndex >= m_grid.Length)
            {
                throw new IndexOutOfRangeException();
            }

            uint firstCoord = flattenedIndex / Height;
            uint secondCoord = flattenedIndex % Height;

            return new Vector2Int((int)firstCoord, (int)secondCoord);
        }

        public object Clone()
        {
            Grid<T> table = new Grid<T>(m_columns, m_rows);

            int index = 0;

            foreach (T item in m_grid)
            {
                if (item is ICloneable clone)
                {
                    table.m_grid[index] = (T)clone.Clone();
                }

                index++;
            }

            return table;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        [SerializeField]
        private T[] m_grid;

        [SerializeField]
        private uint m_rows;

        [SerializeField]
        private uint m_columns;

        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private Grid<T> table;
            private T current;
            private int index;

            internal Enumerator(Grid<T> table)
            {
                this.table = table;
                current = default(T);
                index = 0;
            }

            public T Current => current;
            object IEnumerator.Current => current;

            public bool MoveNext()
            {
                if (index < table.m_grid.Length)
                {
                    current = table.m_grid[index];
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
                current = default(T);
                index = 0;
            }
        }
    }
}
