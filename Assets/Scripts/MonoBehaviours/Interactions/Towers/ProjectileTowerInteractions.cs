using System.Collections;
using TD.Data.Entities;
using TD.Data.Entities.Towers;
using TD.Logic;
using TD.Logic.Events;
using TD.Logic.RuntimeState;
using TD.Utils;
using UnityEngine;

namespace TD.MonoBehaviours.Interactions.Towers
{
    public class ProjectileTowerInteractions : InteractionComponent
    {
        public override void Initialize(LevelComponents levelComponents, RuntimeSlot slot, EventsFacade events)
        {
            base.Initialize(levelComponents, slot, events);

            m_tower = slot.Tower.Data as ProjectileTower;
            events.Entity.EnemyEnteredSlot += OnEnemyEnteredSlot;
            events.Entity.EnemyDestroyed += OnEnemyDestroyed;
        }

        private void OnEnemyDestroyed(EnemyState state)
        {
            if (null != m_currentTarget && m_currentTarget == state)
            {
                StopAllCoroutines();
                m_currentTarget = null;
            }
        }

        private void OnEnemyEnteredSlot(RuntimeSlot slot, EnemyState enemy)
        {
            if (slot.Coords.Distance(m_slot.Coords) <= m_tower.Range)
            {
                if (null == m_currentTarget || WillSwitchTarget(enemy))
                {
                    m_currentTarget = enemy;
                    StartCoroutine(AttackTarget());
                }
            }
        }

        private bool WillSwitchTarget(EnemyState state)
        {
            if (m_tower.AttackRareEnemies 
                && (state.Data as Enemy).Weight < (m_currentTarget.Data as Enemy).Weight)
            {
                StopAllCoroutines();
                return true;
            }

            return false;
        }

        private IEnumerator AttackTarget()
        {
            float timeElapsed = 0f;
            float lastTimeAttacked = 0f;
            var enemyObj = m_levelComponents.View.GetEnemyObject(m_currentTarget);

            while (m_currentTarget.CurrentHP > 0)
            {
                LookAtTarget(enemyObj);

                if (timeElapsed - lastTimeAttacked > m_tower.RateOfFire)
                {
                    SpawnProjectile(enemyObj);
                    lastTimeAttacked = timeElapsed;
                }

                timeElapsed += Time.deltaTime;

                yield return null;
            }

            m_currentTarget = null;
        }

        private void LookAtTarget(GameObject enemyObj)
        {
            Vector3 dir = enemyObj.transform.position - transform.position;
            Vector3 axis = (dir.x < 0) ? Vector3.back : Vector3.forward;

            Quaternion rotation = Quaternion.LookRotation(axis, dir) * Quaternion.Euler(0, 0, 90);

            rotation.eulerAngles = new Vector3(
                    rotation.eulerAngles.x != 0 ? rotation.eulerAngles.x : transform.rotation.x,
                    rotation.eulerAngles.y != 0 ? rotation.eulerAngles.y : transform.rotation.y,
                    rotation.eulerAngles.z != 0 ? rotation.eulerAngles.z : transform.rotation.z);

            transform.rotation = rotation;
        }

        private void SpawnProjectile(GameObject target)
        {
            var projectileInstance = m_levelComponents.View.InstantiateEntity(m_levelComponents, m_slot, m_tower.ProjectilePrefab, m_levelComponents.Events);

            var projectileInteractions = projectileInstance.GetComponentsInChildren<ProjectileInteractions>();

            foreach (var component in projectileInteractions)
            {
                component.AttackTarget(target, m_currentTarget, m_levelComponents.Events.Entity, m_tower.DamagePerProjectile, m_tower.ProjectileSpeed);
            }
        }

        private void OnDestroy()
        {
            m_events.Entity.EnemyEnteredSlot -= OnEnemyEnteredSlot;
            m_events.Entity.EnemyDestroyed -= OnEnemyDestroyed;
        }

        private EnemyState m_currentTarget = default;
        private ProjectileTower m_tower = default;
    }
}