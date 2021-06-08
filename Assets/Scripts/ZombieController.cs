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
    public event Action OnCurrentWaveIsOut;

    [SerializeField] private Transform moveTarget;
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private Transform[] posSpawnZombi;
    [SerializeField] private float timeSpawnZombi;
    [SerializeField] private int[] waveCont = new[] {3, 6, 9, 12, 15};
    private List<int> zombiesInWaves = new List<int>();
    [SerializeField] private string[] nameZombi;
    [SerializeField] private float timerNextWave;

    public GameManager GameManager { private get; set; }
    private List<string> _nameZombi = new List<string>();

    private List<Transform> posRandom = new List<Transform>();
    private int countWave = 0;

    private bool ManagerState;

    List<ZombieBase> zombiClons = new List<ZombieBase>();

    private IEnumerator timerSpawnZombie;
    private IEnumerator cooldownNextWave;


    private void Start()
    {
        zombiesInWaves.AddRange(waveCont);
        _nameZombi.AddRange(nameZombi);
        GameManager.OnNextWave += NextWave;
        GameManager.OnStopGame += StopGame;
        ResetRandom();
    }

    private void StopGame()
    {
        StopAllCoroutines();
    }

    public void Reset()
    {
        foreach (var zombie in zombiClons)
        {
            zombie.Kill();
        }

        zombiClons.Clear();
        countWave = 0;

        zombiesInWaves.Clear();
        zombiesInWaves.AddRange(waveCont);

        timerSpawnZombie = null;
        
        ResetRandom();
    }

    private IEnumerator CooldownSpawnZombie()
    {
        while (zombiesInWaves[countWave] > 0)
        {
            yield return new WaitForSeconds(timeSpawnZombi);
            zombiesInWaves[countWave] -= 1;
            SpawnZombie();
        }

        while (zombiClons.Count > 0)
        {
            yield return null;
        }

        OnCurrentWaveIsOut?.Invoke();
        timerSpawnZombie = null;
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

    private void SetRandomSpawnPosition(GameObject clonZombie)
    {
        var rand = Random.Range(0, posRandom.Count);
        clonZombie.transform.position = posRandom[Random.Range(0, posRandom.Count)].position;
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
        zombieBase.hunger = Random.Range(2, 7);
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
        if (countWave > zombiesInWaves.Count - 1)
        {
            if (timerSpawnZombie != null) StopCoroutine(timerSpawnZombie);
            ManagerState = false;
            return; //all wave end}
        }

        if (timerSpawnZombie == null)
        {
            timerSpawnZombie = CooldownSpawnZombie();
            StartCoroutine(timerSpawnZombie);
        }
    }

    private void OnDestroy()
    {
        GameManager.OnNextWave -= NextWave;
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