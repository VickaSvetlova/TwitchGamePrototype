using System;
using System.Collections;
using UnityEngine;

public enum WeaponMode
{
    auto,
    aim,
    headshoot
}

public class BaseWeapon
{
    public event Action<BaseBullet> OnCreateBullet;
    private Survivor owner { get; set; }
    public float auto { get; }
    public float aiming { get; }
    public float headShoot { get; }
    
    private BaseBullet BaseBullet { get; }

    private float fireRate;
    private float ReloadTime;
    private float Accuracy;

    private IEnumerator _timer;

    [Range(0, 1)] protected float LevelProficiency;
    
    public BaseWeapon(Survivor owner, float auto, float aiming, float headShoot, float fireRate)
    {
        this.auto = auto;
        this.aiming = aiming;
        this.headShoot = headShoot;
        this.fireRate = fireRate;
        this.owner = owner;
    }

    public IEnumerator Shoot()
    {
        yield return new WaitForSeconds(fireRate);

        CreateBullet();
    }

    //костыль - вынести в отдельную сущнось
    private void CreateBullet()
    {
        if (owner.TargetAim == null) return;

        var tempBullet = new GameObject();
        LayerMask.NameToLayer("bullet");
        tempBullet.AddComponent<SphereCollider>().radius = 0.1f;

        var baseBullet = tempBullet.AddComponent<BaseBullet>();
        baseBullet.Owner = owner;
        baseBullet.WeaponMode = WeaponMode.auto;

        var direction = owner.TargetAim.transform.position - owner.gameObject.transform.position;
        var trail = tempBullet.AddComponent<TrailRenderer>();
        trail.time = 0.04f;
        trail.startWidth = 0.01f;
        trail.endWidth = 0.1f;

        tempBullet.transform.position = owner.Gunpoint.position;
        tempBullet.transform.forward = direction;


        OnCreateBullet?.Invoke(baseBullet);
        

        Debug.Log("fire mode "+baseBullet.WeaponMode);
        Debug.DrawLine(owner.gameObject.transform.position, owner.TargetAim.transform.position, Color.red);
    }

//
    public void Reload()
    {
    }

    public void Change()
    {
    }
}