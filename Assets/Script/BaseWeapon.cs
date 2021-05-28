using System;
using System.Collections;
using UnityEngine;

public class BaseWeapon
{
    private Survivor owner { get; set; }
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


    public BaseWeapon(Survivor owner, float auto, float aiming, float headShoot, float fireRate)
    {
        this.AutoCD = auto;
        this.AimingCD = aiming;
        this.HeadShootCD = headShoot;
        this.FireRate = fireRate;
        this.owner = owner;
    }

    public IEnumerator Shoot()
    {
        yield return new WaitForSeconds(FireRate);

        Fire();
    }

    private void Fire()
    {
        Debug.Log("fire");
        Debug.DrawLine(owner.gameObject.transform.position, owner.TargetAim.transform.position, Color.red);
    }

    public void Reload()
    {
    }

    public void Change()
    {
    }
}