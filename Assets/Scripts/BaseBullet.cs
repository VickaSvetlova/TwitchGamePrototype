using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BaseBullet : MonoBehaviour
{
    //statistic
    public event Action<HitInfo> OnHit;
    public WeaponMode WeaponMode { get; set; }
    public Survivor Owner { get; set; }

    //setting
    // public float damage{get;set;}
    // public float stanEffect{get;set;}
    private Rigidbody _rigidbody => GetComponent<Rigidbody>();

    //костыль
    float Speed = 7000f;
    float Damage = 1f;
    float DestroyTime = 5f;

    public float stanEffect = 0.5f;
    //

    private void Awake()
    {
        Destroy(gameObject, DestroyTime);
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _rigidbody.velocity = transform.forward * Time.deltaTime * Speed;
    }

    private void OnCollisionEnter(Collision other)
    {
        var tempOther = other.collider.GetComponent<IDestroyer>();
        var tempOtherDamge = other.collider.GetComponent<ITakeDamage>();

        if (tempOtherDamge != null)
        {
            var HitStruct = tempOtherDamge.TakeDamage(this, Damage);

            OnHit?.Invoke(HitStruct);
        }

        if (tempOther != null)
        {
            Destroy(gameObject);
        }
    }
}

public struct HitInfo
{
    public BaseBullet BaseBullet;
    public bool Dead { get; set; }
    public bool Head { get; set; }
    public bool ShootAiming { get; set; }
    public bool AutoShoot { get; set; }
}