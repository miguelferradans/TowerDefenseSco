using TD.Logic;
using UnityEngine;

namespace TD.MonoBehaviours.UI
{
    public abstract class UIComponent : MonoBehaviour
    {
        public virtual void Initialize(LevelComponents levelComponents)
        {
            m_levelComponents = levelComponents;
        }

        protected LevelComponents m_levelComponents = default;
    }
}