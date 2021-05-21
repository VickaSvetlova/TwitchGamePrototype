using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    private BaseBullet _baseBullet;
    protected float FireRate;
    protected float ReloadTime;
    protected float Accuracy;
    [Range(0, 1)] protected float LevelProficiency;

    public BaseBullet BaseBullet
    {
        get => _baseBullet;
        set => _baseBullet = value;
    }

    public void Shoot()
    {
    }

    public void Reload()
    {
    }

    public void Change()
    {
    }
}