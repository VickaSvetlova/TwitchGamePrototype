using System;
using System.Collections;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    public float AutoCD { get; }
    public float AimingCD { get; }
    public float HeadShootCD { get; }

    //CD= cooldown
    public BaseBullet BaseBullet { get; }

    private float FireRate;
    private float ReloadTime;
    private float Accuracy;

    private IEnumerator _timer;

    [Range(0, 1)] protected float LevelProficiency;


    public BaseWeapon(float auto, float aiming, float headShoot, float fireRate)
    {
        this.AutoCD = auto;
        this.AimingCD = aiming;
        this.HeadShootCD = headShoot;
        this.FireRate = fireRate;
    }

    public IEnumerator Shoot()
    {
        yield return new WaitForSeconds(FireRate);
        Fire();
    }

    private void Fire()
    {
        Debug.Log("fire");
    }

    public void Reload()
    {
    }

    public void Change()
    {
    }
}