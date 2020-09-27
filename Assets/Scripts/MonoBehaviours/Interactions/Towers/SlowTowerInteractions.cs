using System.Collections;
using System.Collections.Generic;
using TD.Data.Entities.Towers;
using TD.Logic;
using TD.Logic.Events;
using TD.Logic.RuntimeState;
using TD.Utils;
using UnityEngine;

namespace TD.MonoBehaviours.Interactions.Towers
{
    public class SlowTowerInteractions : InteractionComponent
    {
        public override void Initialize(LevelComponents levelComponents, RuntimeSlot slot, EventsFacade events)
        {
            base.Initialize(levelComponents, slot, events);

            m_tower = slot.Tower.Data as SlowEnemyTower;
            events.Entity.EnemyEnteredSlot += OnEnemyEnteredSlot;
        }

        private void OnEnemyEnteredSlot(RuntimeSlot slot, EnemyState state)
        {
            if (slot.Coords.Distance(m_slot.Coords) <= m_tower.Range)
            {
                if (!m_enemiesInRange.ContainsKey(state))
                {
                    m_enemiesInRange[state] = slot;
                }

                if (Mathf.Approximately(0f, state.SlowDebuff))
                {
                    StartCoroutine(SlowDebuff(state, true));
                }
            }

            if (m_enemiesInRange.ContainsKey(state))
            {
                m_enemiesInRange[state] = slot;
            }
        }

        private IEnumerator SlowDebuff(EnemyState enemyState, bool refreshSlow)
        {
            float timeElapsed = 0f;

            if (refreshSlow)
            {
                enemyState.SlowDebuff = m_tower.SlowPercentage;
                m_levelComponents.Events.Entity.OnEnemySpeedUpdated(enemyState);
            }
            
            while (timeElapsed < m_tower.DebuffDuration)
            {
                timeElapsed += Time.deltaTime;

                yield return null;
            }

            if (m_enemiesInRange[enemyState].Coords.Distance(m_slot.Coords) <= m_tower.Range)
            {
                StartCoroutine(SlowDebuff(enemyState, false));
            }
            else
            {
                RemoveDebuff(enemyState);
            }
        }

        private void RemoveDebuff(EnemyState enemyState)
        {
            enemyState.SlowDebuff = 0f;
            m_levelComponents.Events.Entity.OnEnemySpeedUpdated(enemyState);
            m_enemiesInRange.Remove(enemyState);
        }

        private SlowEnemyTower m_tower = default;
        private Dictionary<EnemyState, RuntimeSlot> m_enemiesInRange = new Dictionary<EnemyState, RuntimeSlot>();
    }
}