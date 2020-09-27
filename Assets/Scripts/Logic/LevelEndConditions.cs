using TD.Logic.Events;
using TD.MonoBehaviours.Data;
using UnityEngine.SceneManagement;

namespace TD.Logic
{
    public class LevelEndConditions
    {
        public LevelEndConditions(LevelEvents events)
        {
            m_events = events;
            m_events.Lost += OnLost;
            m_events.Won += OnWon;
        }

        public void UnsubscribeEvents()
        {
            m_events.Lost -= OnLost;
            m_events.Won -= OnWon;
        }

        private void OnWon()
        {
            GameDataLoader.Instance.AdvanceLevel();
            GameDataLoader.Instance.LoadCurrentLevel(OnLevelLoaded);
        }

        private void OnLost()
        {
            GameDataLoader.Instance.LoadCurrentLevel(OnLevelLoaded);
        }

        private void OnLevelLoaded()
        {
            SceneManager.LoadScene(kLevelScene, LoadSceneMode.Single);
        }

        private LevelEvents m_events;

        private const string kLevelScene = "LevelScene";
    }
}