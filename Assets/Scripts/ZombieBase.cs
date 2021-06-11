using System;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

namespace Script
{
    public class ZombieBase : MonoBehaviour, IDestroyer, ITakeDamage
    {
        enum States
        {
            stay,
            walk,
            dead
        }

        public UIName UIName;
        public string Name;
        public Action<ZombieBase> IDead;
        public Action<ZombieBase> IGoal;
        public Vector3 targetMove;
        public float walkSpeed;
        public float health;
        public int hunger;

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


        public HitInfo TakeDamage(BaseBullet bullet, float damage)
        {
            var tempHitInfo = new HitInfo();
            tempHitInfo.BaseBullet = bullet;

            ShowEffect();
            health -= damage;
            if (health <= 0)
            {
                state = States.dead;
                IDead?.Invoke(this);

                tempHitInfo.Dead = true;
            }
            else
            {
                switch (bullet.WeaponMode)
                {
                    case WeaponMode.auto:
                        tempHitInfo.AutoShoot = true;
                        break;
                    case WeaponMode.aim:
                        tempHitInfo.ShootAiming = true;
                        break;
                    case WeaponMode.headshoot:
                        tempHitInfo.ShootAiming = true;
                        break;
                }

                state = States.stay;
                if (coldownTimerStanEffect != null) StopCoroutine(coldownTimerStanEffect);
                coldownTimerStanEffect = ColdownTimerStanEffect(bullet.GetComponent<BaseBullet>().stanEffect);
                StartCoroutine(coldownTimerStanEffect);
            }

            return tempHitInfo;
        }

        public void DestroyOnCollision(GameObject gameObject)
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (UIName != null) Destroy(UIName.gameObject);
        }

        public void Kill()
        {
            Destroy(gameObject);
        }
    }
}