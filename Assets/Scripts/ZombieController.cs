using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieController : MonoBehaviour, IZombieProvider
{
    public event Action<ZombieBase> OnZombieCreated;
    public event Action<ZombieBase> OnZombieReachedCity;

    [SerializeField] private Transform moveTarget;
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private Transform[] posSpawnZombi;
    [SerializeField] private float timeSpawnZombi;
    [SerializeField] private int[] waveCont = new[] {3, 6, 9, 12, 15};
    [SerializeField] private string[] nameZombi;
    [SerializeField] private float timerNextWave;

    private List<string> _nameZombi = new List<string>();

    private List<Transform> posRandom = new List<Transform>();
    private int countWave = 0;

    private bool ManagerState;

    List<ZombieBase> zombiClons = new List<ZombieBase>();

    private IEnumerator timerSpawnZombie;
    private IEnumerator cooldownNextWave;

    private void Start()
    {
        _nameZombi.AddRange(nameZombi);
        ResetRandom();
    }

    public void StateManager(bool state)
    {
        timerSpawnZombie = CooldownSpawnZombi();

        if (state)
        {
            ManagerState = state;
            StartCoroutine(timerSpawnZombie);
        }
        else
        {
            if (timerSpawnZombie != null)
            {
                ManagerState = state;
                StopCoroutine(timerSpawnZombie);
            }
        }
    }

    private IEnumerator CooldownSpawnZombi()
    {
        while (ManagerState)
        {
            if (waveCont[countWave] > 0)
            {
                yield return new WaitForSeconds(timeSpawnZombi);
                waveCont[countWave] -= 1;
                SpawnZombie();
            }
            else
            {
                yield return new WaitForSeconds(timerNextWave);
                NextWave();
            }
        }
    }

    private void SpawnZombie()
    {
        var clonZomby = Instantiate(zombiePrefab);
        var clon = clonZomby.AddComponent<ZombieBase>();
        clonZomby.AddComponent<CapsuleCollider>();
        zombiClons.Add(clon);
        OnZombieCreated?.Invoke(SetUpZombie(clon));
        SetRandomSpawnPosition(clonZomby);
    }

    private void SetRandomSpawnPosition(GameObject clonZomby)
    {
        var rand = Random.Range(0, posRandom.Count);
        clonZomby.transform.position = posRandom[Random.Range(0, posRandom.Count)].position;
        posRandom.RemoveAt(rand);
        if (posRandom.Count <= 0)
        {
            ResetRandom();
        }
    }

    private void ResetRandom()
    {
        posRandom.AddRange(posSpawnZombi);
    }

    private ZombieBase SetUpZombie(ZombieBase zombieBase)
    {
        var tempName = TakeRandomName(); //TakeRandomName();
        zombieBase.Name = tempName;
        zombieBase.health = 5;
        zombieBase.targetMove = moveTarget.position;
        zombieBase.walkSpeed = 5f;
        zombieBase.IDead += ZombiIsDead;
        zombieBase.IGoal += ZombiGoal;
        zombieBase.hunger = Random.Range(1, 5);
        return zombieBase;
    }

    private string TakeRandomName()
    {
        if (_nameZombi.Count > 0)
        {
            var rand = Random.Range(0, _nameZombi.Count);
            var name = _nameZombi[rand];
            _nameZombi.RemoveAt(rand);
            return name;
        }

        _nameZombi.AddRange(nameZombi);
        return TakeRandomName();
    }

    private void ZombiGoal(ZombieBase zombie)
    {
        //zombi is it my hosbrand
        OnZombieReachedCity?.Invoke(zombie);
        ZombieRemove(zombie);
    }

    private void ZombiIsDead(ZombieBase obj)
    {
        var tempZomby = obj.gameObject;
        zombiClons.Remove(obj);
        Destroy(tempZomby);
    }

    private void ZombieRemove(ZombieBase obj)
    {
        obj.IDead -= ZombiIsDead;
        obj.IGoal -= ZombiGoal;
        zombiClons.Remove(obj);
        Destroy(obj.gameObject);
    }

    public void NextWave()
    {
        countWave += 1;
        if (countWave > waveCont.Length - 1)
        {
            if (timerSpawnZombie != null) StopCoroutine(timerSpawnZombie);
            ManagerState = false;
            return; //all wave end}
        }
    }

    #region IZombieProvider

    public ZombieBase FindZombieByName(string name)
    {
        foreach (var zombi in zombiClons)
        {
            if (string.Equals(zombi.Name, name, StringComparison.CurrentCultureIgnoreCase))
            {
                return zombi;
            }
        }

        return null;
    }

    #endregion
}