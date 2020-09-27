using System;
using System.Collections.Generic;
using UnityEngine;

namespace TD.Data.Levels
{
    [Serializable]
    public class EnemyPathList
    {
        public List<Vector2Int> Path => m_path;

        public EnemyPathList(List<Vector2Int> path)
        {
            m_path = path;
        }

        [SerializeField]
        private List<Vector2Int> m_path = new List<Vector2Int>();
    }
}