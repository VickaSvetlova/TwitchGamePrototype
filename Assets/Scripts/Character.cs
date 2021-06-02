using System;
using System.Collections;
using System.Timers;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Action<GameObject> imDeath;
    public Action<GameObject> imLoseCoin;
    public ChatController.User user;
    public int coin = 1;
    private bool timeGetCoin;
    private bool onDamageble;
    private IEnumerator getCoin;
    [SerializeField] private Renderer renderer;
    [SerializeField] private Color damagecolor;
    [SerializeField] private float timerToGetCoin;
    [SerializeField] private ParticleSystem[] fxDamage;

    private void Update()
    {
        if (transform.position.y < -20 || coin <= 0)
        {
            StopAllCoroutines();
            imDeath?.Invoke(gameObject);
        }
    }

    public void GetCoin(bool damage)
    {
        onDamageble = damage;
        if (getCoin != null) return;
        getCoin = TimerGetCoin();
        StartCoroutine(getCoin);
    }

    private IEnumerator TimerGetCoin()
    {
        while (onDamageble)
        {
            yield return new WaitForSeconds(timerToGetCoin);
            coin -= 1;
            imLoseCoin?.Invoke(gameObject);
            StartCoroutine(OnDamageZone());
        }

        getCoin = null;
    }

    private IEnumerator OnDamageZone()
    {
        renderer.material.color = damagecolor;
        foreach (var fx in fxDamage)
        {
            fx.Emit(1000);
        }

        yield return new WaitForSeconds(0.1f);
        renderer.material.color = user != null ? user.color : Color.gray;
    }
}