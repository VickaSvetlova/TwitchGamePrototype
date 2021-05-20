using System;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

namespace Script
{
    public class ZombieBase : MonoBehaviour
    {
        enum States
        {
            stay,
            walk,
            dead
        }

        public string Name;
        public Action<ZombieBase> IDead;
        public Action<ZombieBase> IGoal;
        public Vector3 targetMove;
        public float walkSpeed;
        public float timeEffectStan;
        public float health;
        public float timeToDestroy;
        private GameObject _targetMarcker;

        private IEnumerator coldownTimerStanEffect;

        private States state = States.walk;

        public void LookAtMy(bool stateTragetMarcker)
        {
            if (_targetMarcker == null) return;
            _targetMarcker.SetActive(stateTragetMarcker);
        }

        private void Awake()
        {
            _targetMarcker = GetComponentInChildren<TargetAim>().gameObject;
            _targetMarcker.SetActive(false);
            
        }

        private void Update()
        {
            switch (state)
            {
                case States.stay:
                    break;
                case States.walk:
                    Walk();
                    break;
                case States.dead:
                    break;
            }
        }

        private void Walk()
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(targetMove.x, transform.position.y, transform.position.z), Time.deltaTime * walkSpeed);
        }

        private void Dead()
        {
            //анимациия смерти.
            //timeToDie
        }

        public void TakeDamage(float damage, Bullet bullet)
        {
            ShowEffect();
            health -= damage;
            if (health <= 0)
            {
                state = States.dead;
            }
            else
            {
                state = States.stay;
                if (coldownTimerStanEffect != null) StopCoroutine(coldownTimerStanEffect);
                coldownTimerStanEffect = ColdownTimerStanEffect(bullet.stanEffect);
                StartCoroutine(coldownTimerStanEffect);
            }
        }

        private IEnumerator ColdownTimerStanEffect(float bulletStanEffect)
        {
            yield return new WaitForSeconds(bulletStanEffect);
            coldownTimerStanEffect = null;
            state = States.walk;
        }

        private void ShowEffect()
        {
            //еффекты попадания
        }

        public void Goal()
        {
            IGoal?.Invoke(gameObject.GetComponent<ZombieBase>());
        }
    }
}