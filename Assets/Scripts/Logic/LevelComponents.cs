using TD.Data.Levels;
using TD.Logic.Events;
using TD.Logic.RuntimeState;
using TD.MonoBehaviours;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TD.Logic
{
    public class LevelComponents
    {
        public Level LevelConfig => m_levelConfig;
        public LevelState State => m_levelState;
        public LevelView View => m_levelView;
        public EventsFacade Events => m_events;
        public WavesHandler WavesHandler => m_wavesHandler;

        public void Build(Level level, GameObject parent, GraphicRaycaster graphicRaycast, EventSystem eventSystem)
        {
            m_levelConfig = level;
            m_levelState = level.BuildState();
            m_events = new EventsFacade();

            if (null != parent)
            {
                m_slotSize = level.SlotPrefab.GetComponent<Renderer>().bounds.size;
                m_levelView = parent.GetComponent<LevelView>() ?? parent.AddComponent<LevelView>();
                parent.gameObject.transform.localPosition = new Vector3(-(level.Slots.Width * 0.5f), -(level.Slots.Height * 0.5f), 0f);
                m_levelView.CreateLevelView(this, m_events, level.SlotPrefab, graphicRaycast, eventSystem);
            }

            m_wavesHandler = parent.GetComponent<WavesHandler>() ?? parent.AddComponent<WavesHandler>();
            m_wavesHandler.Initialize(this);

            m_levelEndConditions = new LevelEndConditions(m_events.Level);
        }

        public void UnsubscribeEvents()
        {
            m_levelEndConditions.UnsubscribeEvents();
        }

        public RuntimeSlot GetSlotInScreenPosition(Vector3 screenSpaceCoords)
        {
            if (m_levelView.IsClickingOnUI())
            {
                return null;
            }

            return GetSlotInBoardPosition(ScreenToBoardSpace(screenSpaceCoords));
        }

        private RuntimeSlot GetSlotInBoardPosition(Vector2 position)
        {
            float horizontal = (position.x + (m_slotSize.x / 2)) / m_slotSize.x;
            float vertical = (position.y + (m_slotSize.y / 2)) / m_slotSize.y;

            if (horizontal >= 0 && vertical >= 0 && horizontal <= m_levelState.Width && vertical <= m_levelState.Height)
            {
                return m_levelState[(int)horizontal, (int)vertical];
            }

            return null;
        }

        private Vector3 ScreenToBoardSpace(Vector3 screenSpaceCoords)
        {
            Plane plane = new Plane(-m_levelView.transform.forward, m_levelView.transform.position);
            Ray ray = Camera.main.ScreenPointToRay(screenSpaceCoords);
            float distance = 0f;

            if (plane.Raycast(ray, out distance))
            {
                Vector3 point = ray.GetPoint(distance);
                Vector3 boardSpacePosition = m_levelView.transform.InverseTransformPoint(point);

                return boardSpacePosition;
            }

            return Vector3.zero;
        }

        private Level m_levelConfig = default;
        private LevelState m_levelState = default;
        private LevelView m_levelView = default;
        private EventsFacade m_events = default;
        private WavesHandler m_wavesHandler = default;
        private LevelEndConditions m_levelEndConditions = default;
        private Vector2 m_slotSize = default;
    }
}