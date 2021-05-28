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
    private BaseBullet BaseBullet { get; }

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
        var tempBullet = new GameObject();
        tempBullet.AddComponent<BaseBullet>();
        tempBullet.AddComponent<SphereCollider>().radius=0.1f;

        //temporary solution
        var direction = owner.TargetAim.transform.position - owner.gameObject.transform.position;
        var trail = tempBullet.AddComponent<TrailRenderer>();
        trail.time = 0.04f;
        trail.startWidth = 0.01f;
        trail.endWidth = 0.1f;

        tempBullet.transform.position = owner.Gunpoint.position;
        tempBullet.transform.forward = direction;

        Debug.Log("fire");
        Debug.DrawLine(owner.gameObject.transform.position, owner.TargetAim.transform.position, Color.red);

        //
    }

    public void Reload()
    {
    }

    public void Change()
    {
    }
}