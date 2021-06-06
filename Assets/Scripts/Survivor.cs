using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;


public class Survivor : MonoBehaviour

{
    [SerializeField] private float lookRadius;
    private BaseWeapon _weapon;
    public GameObject TargetAim { get; set; }
    public User user { private get; set; }

    private List<CommandFactory> CommandFactories = new List<CommandFactory>();

    private Queue<ICommand> SurvivorStates = new Queue<ICommand>();
    private Queue<ICommand> WeaponsStates = new Queue<ICommand>();

    private IEnumerator _survivorStateCoroutine;
    private IEnumerator _weaponsStateCoroutine;

    private ZombieBase lastZomby;

    private ICommand CurrentSurvivorCommand;
    private ICommand CurrentWeaponCommand;
    [SerializeField] public Transform _gunpoint;


    public BaseWeapon Weapon
    {
        get => _weapon;
        private set => _weapon = value;
    }

    public float LookRadius => lookRadius;

    public Transform Gunpoint
    {
        get => _gunpoint;
        set => _gunpoint = value;
    }

    private void Awake()
    {
        CommandFactories.Add(new AimedShotFactory());
        CommandFactories.Add(new HeadshotShotFactory());

        SurvivorStates.Enqueue(new IdleCommand());
        WeaponsStates.Enqueue(new IdleCommand());
    }

    private void Start()
    {
        Weapon = new BaseWeapon(this, 0.5f, 1f, 2f, 0.2f);

        _survivorStateCoroutine = SurvivorStatesCoroutine();
        _weaponsStateCoroutine = WeaponsStatesCoroutine();

        StartCoroutine(_survivorStateCoroutine);
        StartCoroutine(_weaponsStateCoroutine);
    }

    private IEnumerator WeaponsStatesCoroutine()
    {
        while (true)
        {
            CurrentWeaponCommand = WeaponsStates.Dequeue();
            yield return CurrentWeaponCommand.Execute();
            if (WeaponsStates.Count == 0) WeaponsStates.Enqueue(new AutoShootCommand(TargetAim, this));
            yield return null;
        }
    }

    private IEnumerator SurvivorStatesCoroutine()
    {
        while (true)
        {
            CurrentSurvivorCommand = SurvivorStates.Dequeue();
            yield return CurrentSurvivorCommand.Execute();
            if (SurvivorStates.Count == 0) SurvivorStates.Enqueue(CurrentSurvivorCommand);
            yield return null;
        }
    }

    public void TakeCommand(string command)
    {
        var tempZombi = user.ZombieProvider.FindZombieByName(command);
        if (tempZombi)
        {
            if (lastZomby != null) lastZomby.LookAtMy(false);
            tempZombi.LookAtMy(true);
            TargetAim = tempZombi.gameObject;
            lastZomby = tempZombi;

            CurrentSurvivorCommand?.Interrupt(); //stop previos command
            SurvivorStates.Clear();
            SurvivorStates.Enqueue(new AimingCommand(TargetAim, this));

            CurrentWeaponCommand?.Interrupt();
            WeaponsStates.Clear();
            WeaponsStates.Enqueue(new AutoShootCommand(TargetAim, this));
        }

        else
        {
            foreach (var factory in CommandFactories)
            {
                if (factory.Match(command))
                {
                    WeaponsStates.Enqueue(factory.CreateCommand(this));
                    break;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, LookRadius);
    }
}