using System;
using TD.Data.Levels;
using TD.Data.Persistent;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace TD.MonoBehaviours.Data
{
    public class GameDataLoader : MonoBehaviour
    {
        public static GameDataLoader Instance => m_instance;
        public LevelList LevelList => m_loadedLevelList;
        public SaveData SaveData => m_saveData;

        public void LoadCurrentLevel(Action callback)
        {
            m_loadedLevelList.LoadLevelAsync(m_saveData.CurrentLevelIndex, callback);
        }

        public void AdvanceLevel()
        {
            m_saveData.AdvanceLevel();
        }

        public void CreateNewSaveData()
        {
            m_saveData = new SaveData();
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);

            m_saveData = SaveDataLoader.LoadData();

            m_levelList.LoadAssetAsync().Completed += OnCompletedLevelListLoad;

            m_instance = this;

            SceneManager.LoadScene(kMainMenuScene, LoadSceneMode.Single);
        }

        private void OnCompletedLevelListLoad(AsyncOperationHandle<LevelList> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                m_loadedLevelList = handle.Result;
            }
        }

        private static GameDataLoader m_instance;

        [SerializeField]
        private LevelListAssetReference m_levelList = default;

        private SaveData m_saveData;
        private LevelList m_loadedLevelList;

        private const string kMainMenuScene = "MainMenu";
    }
}