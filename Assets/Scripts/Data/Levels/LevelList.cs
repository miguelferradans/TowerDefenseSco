using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TD.Data.Levels
{
    [Serializable, CreateAssetMenu(menuName = "TD/Levels/Level List")]
    public class LevelList : ScriptableObject
    {
        public List<LevelAssetReference> Levels => m_levels;
        public Level LoadedLevel => m_loadedLevel;

        public void LoadLevelAsync(int index, Action callback = null)
        {
            ReleaseLevel();

            if (index < m_levels.Count)
            {
                Levels[index].LoadAssetAsync().Completed += (handle) =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        m_loadedLevel = handle.Result;
                        callback?.Invoke();
                    }
                };
            }
        }

        public void ReleaseLevel()
        {
            if (null != m_loadedLevel)
            {
                Addressables.Release(m_loadedLevel);
            }
        }

        [SerializeField]
        private List<LevelAssetReference> m_levels = new List<LevelAssetReference>();

        private Level m_loadedLevel;
    }
}