using UnityEngine;

public interface ITakeDamage
{
    HitInfo TakeDamage(BaseBullet bullet, float damage);
}