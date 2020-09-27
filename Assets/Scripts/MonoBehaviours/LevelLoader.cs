using TD.MonoBehaviours.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TD.MonoBehaviours
{
    public class LevelLoader : MonoBehaviour
    {
        public void NewGame()
        {
            GameDataLoader.Instance.CreateNewSaveData();
            GameDataLoader.Instance.LoadCurrentLevel(OnLevelLoaded);
        }

        public void LoadGame()
        {
            GameDataLoader.Instance.LoadCurrentLevel(OnLevelLoaded);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        private void OnLevelLoaded()
        {
            SceneManager.LoadScene(kLevelScene, LoadSceneMode.Single);
        }

        private const string kLevelScene = "LevelScene";
    }
}