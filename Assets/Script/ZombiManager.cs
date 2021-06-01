using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombiManager : MonoBehaviour
{
    [SerializeField] private UIController _uiController;
    [SerializeField] private Transform moveTarget;
    [SerializeField] private GameObject zombiPrefab;
    [SerializeField] private Transform[] posSpawnZombi;
    [SerializeField] private float timeSpawnZombi;
    [SerializeField] private int[] waveCont = new[] {3, 6, 9, 12, 15};
    [SerializeField] private GameObject UINamePrefab;
    [SerializeField] private string[] nameZombi;

    [SerializeField] private CityManager cityManager;
    private List<string> _nameZombi = new List<string>();

    private List<Transform> posRandom = new List<Transform>();
    private int countWave = 0;

    public bool ManagerState;

    [HideInInspector] public int countZombiInWave;

    List<ZombieBase> zombiClons = new List<ZombieBase>();

    private IEnumerator timerSpawnZombi;
    private IEnumerator cooldownNextWave;
    [SerializeField] private float timerNextWave;

    private void Start()
    {
        _nameZombi.AddRange(nameZombi);
        ResetRandom();
    }

    public void StateManager(bool state)
    {
        timerSpawnZombi = CooldownSpawnZombi();

        if (state)
        {
            StartCoroutine(timerSpawnZombi);
        }
        else
        {
            if (timerSpawnZombi != null)
            {
                StopCoroutine(timerSpawnZombi);
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
                SpawnZombi();
            }
            else
            {
                yield return new WaitForSeconds(timerNextWave);
                NextWave();
            }
        }
    }

    private void SpawnZombi()
    {
        var clonZomby = Instantiate(zombiPrefab);
        var clon = clonZomby.AddComponent<ZombieBase>();
        clonZomby.AddComponent<CapsuleCollider>();
        zombiClons.Add(clon);
        countZombiInWave -= 1;
        ZombiConstructor(clon);
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

    private void ZombiConstructor(ZombieBase zombieBase)
    {
        var tempName = TakeRandomName(); //TakeRandomName();
        zombieBase.Name = tempName;
        zombieBase.health = 5;
        zombieBase.targetMove = moveTarget.position;
        zombieBase.walkSpeed = 1f;
        zombieBase.IDead += ZombiIsDead;
        zombieBase.IGoal += ZombiGoal;
        zombieBase.hunger = Random.Range(1, 5);
        ///
        _uiController.CreateUIName(tempName,zombieBase);
       
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
        return null;
    }

    private void ZombiGoal(ZombieBase obj)
    {
        //zombi is it my hosbrand
        cityManager.CityDamage(obj.hunger);
        ZombiRemove(obj);
    }

    private void ZombiIsDead(ZombieBase obj)
    {
        var tempZomby = obj.gameObject;
        zombiClons.Remove(obj);
        Destroy(tempZomby);
    }

    private void ZombiRemove(ZombieBase obj)
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
            if (timerSpawnZombi != null) StopCoroutine(timerSpawnZombi);
            ManagerState = false;
            return; //all wave end}
        }
    }

    public ZombieBase CheckZombiName(string name)
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
}