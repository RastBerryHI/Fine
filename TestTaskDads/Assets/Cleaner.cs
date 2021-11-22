using UnityEngine;

public class Cleaner : MonoBehaviour
{
    public static Cleaner s_instance;

    void Awake()
    {
        if ( s_instance == null )
        {
            s_instance = this;
        }
        else if( s_instance != null )
        {
            Destroy(s_instance.gameObject);
        }
    }
    public void CleanAll()
    {
        Mutant[] mutants = GameObject.FindObjectsOfType<Mutant>();
        HealthSphere[] spheres = GameObject.FindObjectsOfType<HealthSphere>();
        for (int i = 0; i < mutants.Length; i++)
        {
            Destroy(mutants[i].gameObject);
        }
        for (int i = 0; i < spheres.Length; i++)
        {
            Destroy(spheres[i].gameObject);
        }
    }

    public void CleanSpheares()
    {
        HealthSphere[] spheres = GameObject.FindObjectsOfType<HealthSphere>();

        for (int i = 0; i < spheres.Length; i++)
        {
            Destroy(spheres[i].gameObject);
        }
    }

    public void CleanMutants()
    {
        Mutant[] mutants = GameObject.FindObjectsOfType<Mutant>();

        for (int i = 0; i < mutants.Length; i++)
        {
            Destroy(mutants[i].gameObject);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        CleanAll();
    }

    private void OnApplicationQuit()
    {
        CleanAll();
    }
}
