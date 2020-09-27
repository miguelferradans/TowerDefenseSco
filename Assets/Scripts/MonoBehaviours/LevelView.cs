using System.Collections.Generic;
using TD.Data.Levels;
using TD.Logic;
using TD.Logic.Events;
using TD.Logic.RuntimeState;
using TD.MonoBehaviours.Interactions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TD.MonoBehaviours
{
    public class LevelView : MonoBehaviour
    {
        public void CreateLevelView(LevelComponents levelComponents, EventsFacade events, GameObject slotPrefab, GraphicRaycaster graphicRaycast, EventSystem eventSystem)
        {
            m_eventSystem = eventSystem;
            m_graphicRaycaster = graphicRaycast;
            var slotSize = slotPrefab.GetComponent<Renderer>().bounds.size;

            foreach (var slot in levelComponents.State)
            {
                var slotInstance = InstantiateSlot(slotPrefab, slotSize, slot);

                m_slotViews[slot.Coords] = slotInstance;

                if (null != slot.Base)
                {
                    InstantiateEntity(levelComponents, slot, slot.Base.Data.Prefab, events);
                }

                if (slot.SlotConfig.CanConstruct)
                {
                    InstantiateEntity(levelComponents, slot, levelComponents.LevelConfig.CanConstructPrefab, events);
                }
            }

            foreach (var path in levelComponents.LevelConfig.EnemyPaths)
            {
                foreach (var coords in path.Path)
                {
                    InstantiateEntity(levelComponents, levelComponents.State[coords], levelComponents.LevelConfig.PathPrefab, events);
                }
            }
        }

        public bool IsClickingOnUI()
        {
            var pointerEventData = new PointerEventData(m_eventSystem);
            pointerEventData.position = Input.mousePosition;

            m_raycastResults.Clear();

            m_graphicRaycaster.Raycast(pointerEventData, m_raycastResults);

            return m_raycastResults.Count > 0;
        }

        public GameObject GetEnemyObject(EnemyState state)
        {
            GameObject obj;

            if (m_enemyViews.TryGetValue(state, out obj))
            {
                return obj;
            }

            return null;
        }

        public void InstantiateEnemy(EnemyState state, LevelComponents levelComponents, RuntimeSlot slot, GameObject prefab, EventsFacade events)
        {
            var enemyInstance = InstantiateEntity(levelComponents, slot, prefab, events);

            var enemyInteractions = enemyInstance.GetComponentsInChildren<EnemyInteractions>();

            foreach (var component in enemyInteractions)
            {
                component.SetState(state);
            }

            m_enemyViews[state] = enemyInstance;
        }

        public GameObject InstantiateEntity(LevelComponents levelComponents, RuntimeSlot slot, GameObject prefab, EventsFacade events)
        {
            var slotInstance = m_slotViews[slot.Coords];
            var entityInstance = Instantiate(prefab, slotInstance.transform);
            entityInstance.transform.localPosition = Vector3.zero;

            var interactionComponents = entityInstance.GetComponentsInChildren<InteractionComponent>();

            foreach (var component in interactionComponents)
            {
                component.Initialize(levelComponents, slot, events);
            }

            return entityInstance;
        }

        public Vector3 GetSlotWorldPosition(Vector2Int coords)
        {
            return m_slotViews[coords].transform.position;
        }

        private GameObject InstantiateSlot(GameObject slotPrefab, Vector3 slotSize, RuntimeSlot slot)
        {
            Vector2 position = new Vector2(slotSize.x * slot.Coords.x, slotSize.y * slot.Coords.y);

            var slotGameObject = Instantiate(slotPrefab, transform, false);
            slotGameObject.name = "Slot " + slot.Coords;
            slotGameObject.transform.localPosition = position;

            return slotGameObject;
        }

        private EventSystem m_eventSystem;
        private GraphicRaycaster m_graphicRaycaster;
        private List<RaycastResult> m_raycastResults = new List<RaycastResult>();
        private Dictionary<Vector2Int, GameObject> m_slotViews = new Dictionary<Vector2Int, GameObject>();
        private Dictionary<EnemyState, GameObject> m_enemyViews = new Dictionary<EnemyState, GameObject>();
    }
}