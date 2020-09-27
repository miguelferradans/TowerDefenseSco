using System;
using UnityEngine;

namespace TD.Data.Persistent
{
    [Serializable]
    public class SaveData
    {
        public int CurrentLevelIndex => m_currentLevelIndex;

        public void AdvanceLevel()
        {
            m_currentLevelIndex++;
            SaveDataLoader.SaveData(this);
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        [SerializeField]
        private int m_currentLevelIndex = default;
    }
}