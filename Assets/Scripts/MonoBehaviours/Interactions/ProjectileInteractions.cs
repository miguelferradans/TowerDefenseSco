using System.Collections;
using TD.Logic.Events;
using TD.Logic.RuntimeState;
using UnityEngine;

namespace TD.MonoBehaviours.Interactions
{
    public class ProjectileInteractions : InteractionComponent
    {
        public void AttackTarget(GameObject enemyObj, EnemyState state, EntityEvents events, uint attackDamage, float projectileSpeed)
        {
            StartCoroutine(AttackTargetRoutine(enemyObj, state, events, attackDamage, projectileSpeed));
        }

        private IEnumerator AttackTargetRoutine(GameObject enemyObj, EnemyState state, EntityEvents events, uint attackDamage, float projectileSpeed)
        {
            Vector3 origin = transform.position;
            Vector3 target = enemyObj.transform.position;

            float distance = Mathf.Sqrt(Mathf.Pow(target.x - origin.x, 2) + Mathf.Pow(target.y - origin.y, 2));

            float totalDuration = distance / projectileSpeed;

            float timeElapsed = 0f;

            while (timeElapsed < totalDuration)
            {
                float t = timeElapsed / totalDuration;

                transform.position = Vector3.Lerp(origin, target, t);

                timeElapsed += Time.deltaTime;

                yield return null;
            }

            state.Hit(attackDamage, events);

            Destroy(gameObject);
        }
    }
}