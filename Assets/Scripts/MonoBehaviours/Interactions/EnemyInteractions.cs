using System.Collections;
using System.Collections.Generic;
using TD.Data.Entities;
using TD.Logic;
using TD.Logic.Events;
using TD.Logic.RuntimeState;
using TMPro;
using UnityEngine;

namespace TD.MonoBehaviours.Interactions
{
    public class EnemyInteractions : InteractionComponent
    {
        public override void Initialize(LevelComponents levelComponents, RuntimeSlot slot, EventsFacade events)
        {
            base.Initialize(levelComponents, slot, events);

            m_events.Entity.Hit += OnHit;
            m_events.Entity.EnemySpeedUpdated += OnEnemySpeedUpdated; ;
        }

        private void OnEnemySpeedUpdated(EnemyState state)
        {
            if (state == m_state)
            {
                StopAllCoroutines();
                StartCoroutine(FollowPath());
            }
        }

        private void OnHit(EntityState state)
        {
            if (state == m_state)
            {
                if (m_state.CurrentHP == 0)
                {
                    DestroyEnemy();
                    m_levelComponents.State.ChangeMoney((m_state.Data as Enemy).MoneyOnKill, m_levelComponents.Events.Level);
                }
                else
                {
                    m_hpText.text = m_state.CurrentHP.ToString();
                }
            }
        }

        public void SetState(EnemyState state)
        {
            m_state = state;
            m_hpText.text = m_state.CurrentHP.ToString();

            m_path = GetPath();

            StartCoroutine(FollowPath());
        }

        private List<Vector2Int> GetPath()
        {
            foreach (var path in m_levelComponents.LevelConfig.EnemyPaths)
            {
                foreach (var coords in path.Path)
                {
                    if (coords == m_slot.Coords)
                    {
                        return path.Path;
                    }
                }
            }

            return null;
        }

        private IEnumerator FollowPath()
        {
            int startIndex = m_currentTargetIndex;

            for (int i = startIndex; i < m_path.Count; i++)
            {
                m_currentTargetIndex = i;
                Vector2Int nextSlot = m_path[i];
                Vector3 target = m_levelComponents.View.GetSlotWorldPosition(nextSlot);
                Vector3 origin = transform.position;

                var distance = Mathf.Sqrt(Mathf.Pow(target.x - origin.x, 2) + Mathf.Pow(target.y - origin.y, 2));

                float totalDuration = distance / m_state.CurrentSpeed();

                float timeElapsed = 0f;

                while (timeElapsed < totalDuration)
                {
                    float t = timeElapsed / totalDuration;

                    transform.position = Vector3.Lerp(origin, target, t);

                    timeElapsed += Time.deltaTime;

                    yield return null;
                }

                MoveToNextSlot(nextSlot);
            }
        }

        private void MoveToNextSlot(Vector2Int nextSlot)
        {
            var nextSlotState = m_levelComponents.State[nextSlot];
            m_slot.Enemies.Remove(m_state);
            nextSlotState.Enemies.Add(m_state);
            m_slot = nextSlotState;
            m_levelComponents.Events.Entity.OnEnemyEnteredSlot(m_slot, m_state);

            if (null != m_slot.Base)
            {
                DestroyEnemy();
            }
        }

        private void DestroyEnemy()
        {
            m_levelComponents.Events.Entity.OnEnemyDestroyed(m_state);
            m_events.Entity.Hit -= OnHit;

            m_slot.Enemies.Remove(m_state);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            m_events.Entity.Hit -= OnHit;
        }

        [SerializeField]
        private TextMeshPro m_hpText = default;

        private List<Vector2Int> m_path = default;
        private EnemyState m_state = default;
        private int m_currentTargetIndex = 1;
    }
}