using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// Handles spawning enemies logic
/// </summary>
public class Spawner : MonoBehaviour
{
    /// <summary>
    /// Singletone instance
    /// </summary>
    public static Spawner s_instance;

    private static int s_healthCounter = 1;

    [SerializeField] private Transform[] _spawners = new Transform[4];
    [SerializeField] private Mutant _mutant;

    public event System.Action onEnemyDeath;

    public static float Respawned
    {
        get => s_healthCounter;
    }
    public void OnEnemyDeath()
    {
        if( onEnemyDeath != null )
        {
            onEnemyDeath();
        }
    }
    private void SpawnNew()
    {
        AddEnemy(GetRandomPosition());
    }
    private Vector3 GetRandomPosition()
    {
        return _spawners[Random.Range(0, _spawners.Length-1)].position;
    }
    private void AddEnemy(Vector3 pos)
    {
        Mutant cloneMutant = Instantiate<Mutant>(_mutant, pos, Quaternion.identity) as Mutant;
        cloneMutant.name = cloneMutant.name + "clone";
        cloneMutant.MoveSpeed = Random.Range(5f, 8f);
        cloneMutant.Health += ++s_healthCounter;   
        foreach(AttentionPoint at in Viking.s_instance.AttentionPoints)
        {
            if( at.b_isBusy == false )
            {
                cloneMutant._target = at;
                at.b_isBusy = true;
                return;
            }
        }
    }

    void Awake()
    {
        if ( s_instance == null )
            s_instance = this;
        else
            s_instance = null;
        Cleaner.s_instance.CleanAll();
    }

    void Start()
    {

        for(int i = 0; i < Viking.s_instance.AttentionPoints.Count; i++)
        {
            Mutant cloneMutant = Instantiate<Mutant>(_mutant, _spawners[i].position, Quaternion.identity);
            cloneMutant.MoveSpeed = Random.Range(5f, 8f);
            cloneMutant._target = Viking.s_instance.AttentionPoints[i];
            Viking.s_instance.AttentionPoints[i].b_isBusy = true;
        }

        onEnemyDeath += SpawnNew;
    }
}
