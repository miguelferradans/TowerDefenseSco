using System.Collections.Generic;
using TD.InputHandling;
using TD.Logic;
using TD.MonoBehaviours.Data;
using TD.MonoBehaviours.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TD.MonoBehaviours
{
    public class LevelUnityFacade : MonoBehaviour
    {
        private void Awake()
        {
            var loadedLevel = GameDataLoader.Instance.LevelList.LoadedLevel;

            m_levelComponents = new LevelComponents();
            m_levelComponents.Build(loadedLevel, gameObject, m_graphicRaycaster, m_eventSystem);

            m_inputListener = new InputListener(m_levelComponents);

            InitializeUIComponents();
        }

        private void OnDestroy()
        {
            m_levelComponents.UnsubscribeEvents();
        }

        private void Update()
        {
            m_inputListener.Update();
        }

        private void InitializeUIComponents()
        {
            foreach (var component in m_uiComponents)
            {
                component.Initialize(m_levelComponents);
            }
        }

        private LevelComponents m_levelComponents = default;
        private InputListener m_inputListener = default;

        [SerializeField]
        private List<UIComponent> m_uiComponents = default;

        [SerializeField]
        private GraphicRaycaster m_graphicRaycaster = default;

        [SerializeField]
        private EventSystem m_eventSystem = default;
    }
}