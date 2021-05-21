using System;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    private Rigidbody _rigidbody => GetComponent<Rigidbody>();
    protected float Speed;
    protected float Damage;
    protected float DestroyTime;

    private void Start()
    {
        Destroy(gameObject, DestroyTime);
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        _rigidbody.velocity = Vector3.forward * Time.deltaTime * Speed;
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