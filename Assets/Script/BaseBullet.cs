using System;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class BaseBullet : MonoBehaviour
{
    private Rigidbody _rigidbody => GetComponent<Rigidbody>();
    protected float Speed=7000f;
    protected float Damage;
    protected float DestroyTime=5f;

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
        if (tempOther != null)
        {
            Destroy(gameObject);
        }
    }
}