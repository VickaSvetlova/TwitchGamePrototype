using System;
using System.Collections;
using UnityEngine;

public enum TimeAiming
{
    auto,
    aiming,
    headShoot
};

public class BaseWeapon : MonoBehaviour
{
    private BaseBullet _baseBullet;
    protected float FireRate;
    protected float ReloadTime;

    protected float auto;
    protected float aiming;
    protected float headShoot;

    protected float Accuracy;
    
    [Range(0, 1)] protected float LevelProficiency;

    public BaseBullet BaseBullet
    {
        get => _baseBullet;
        set => _baseBullet = value;
    }

    private IEnumerator _timer;

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

    IEnumerator CooldownFireRate(TimeAiming timeAiming)
    {
        switch (timeAiming)
        {
            case TimeAiming.auto:
                yield return new WaitForSeconds(auto);
                break;
            case TimeAiming.aiming:
                break;
            case TimeAiming.headShoot:
                break;
        }

        Fire();
    }
}