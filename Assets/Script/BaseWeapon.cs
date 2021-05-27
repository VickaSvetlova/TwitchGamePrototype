using System;
using System.Collections;
using UnityEngine;

public enum TimeAiming
{
    auto,
    aim,
    headShoot
};

public class BaseWeapon : MonoBehaviour
{
    private BaseBullet _baseBullet;
    protected float FireRate;
    protected float ReloadTime;

    private float auto;
    private float aiming;
    private float headShoot;

    protected float Accuracy;

    [Range(0, 1)] protected float LevelProficiency;

    public BaseBullet BaseBullet
    {
        get => _baseBullet;
        set => _baseBullet = value;
    }

    public IEnumerator _timer;

    public BaseWeapon(float auto, float aiming, float headShoot)
    {
        this.auto = auto;
        this.aiming = aiming;
        this.headShoot = headShoot;
    }

    public void Shoot(TimeAiming plusTimer)
    {
        if (_timer != null) return;
        _timer = CooldownFireRate(plusTimer);
        StartCoroutine(_timer);
    }

    private void Fire()
    {
    }

    public void Reload()
    {
    }

    public void Change()
    {
    }

   private IEnumerator CooldownFireRate(TimeAiming timeAiming)
    {
        switch (timeAiming)
        {
            case TimeAiming.auto:
                yield return new WaitForSeconds(auto);
                break;
            case TimeAiming.aim:
                break;
            case TimeAiming.headShoot:
                break;
        }

        Fire();
    }
}