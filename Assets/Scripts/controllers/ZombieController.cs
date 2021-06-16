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
    [SerializeField] private Vector2 minMAXEating; //заглушка
    [SerializeField] private Transform[] posSpawnZombi;
    [SerializeField] private float timeSpawnZombi;
    [SerializeField] private int[] waveCont = new[] {3, 6, 9, 12, 15}; // заглушка - временная реализация - заменить автолевеленгом
    [SerializeField] private string[] nameZombi;//заглушка - сюда словарь имен

    private List<int> _zombiesInWaves = new List<int>();
    private readonly List<string> _nameZombi = new List<string>();

    private List<Transform> posRandom = new List<Transform>();
    private int countWave = 0;

    private bool ManagerState;

    private List<ZombieBase> zombiClons = new List<ZombieBase>();

    private IEnumerator _timerSpawnZombie;


    private void Start()
    {
        _zombiesInWaves.AddRange(waveCont);
        _nameZombi.AddRange(nameZombi);
        ResetRandom();
    }

    public void StopGame()
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

        _zombiesInWaves.Clear();
        _zombiesInWaves.AddRange(waveCont);

        _timerSpawnZombie = null;

        ResetRandom();
    }

    private IEnumerator CooldownSpawnZombie()
    {
        while (_zombiesInWaves[countWave] > 0)
        {
            yield return new WaitForSeconds(timeSpawnZombi);
            _zombiesInWaves[countWave] -= 1;
            SpawnZombie();
        }

        while (zombiClons.Count > 0)
        {
            yield return null;
        }

        OnCurrentWaveIsOut?.Invoke();
        _timerSpawnZombie = null;
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
        zombieBase.CharacterName = tempName;
        zombieBase.CharacterGameObject = zombieBase.gameObject;
        zombieBase.health = Random.Range(3,7);
        zombieBase.targetMove = moveTarget.position;
        zombieBase.walkSpeed = 2f;
        zombieBase.IDead += ZombiIsDead;
        zombieBase.IGoal += ZombiGoal;
        zombieBase.hunger = (int) Random.Range(minMAXEating.x, minMAXEating.y);
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
        //zombi is it my city
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
        if (countWave > _zombiesInWaves.Count - 1)
        {
            if (_timerSpawnZombie != null) StopCoroutine(_timerSpawnZombie);
            ManagerState = false;
            return; //all wave end}
        }

        if (_timerSpawnZombie == null)
        {
            _timerSpawnZombie = CooldownSpawnZombie();
            StartCoroutine(_timerSpawnZombie);
        }
    }

    #region IZombieProvider

    public ZombieBase FindZombieByName(string name)
    {
        foreach (var zombi in zombiClons)
        {
            if (string.Equals(zombi.CharacterName, name, StringComparison.CurrentCultureIgnoreCase))
            {
                return zombi;
            }
        }

        return null;
    }

    #endregion
}