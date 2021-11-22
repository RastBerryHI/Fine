using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSphere : MonoBehaviour
{
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _healthToRestore;
    void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Viking.s_instance.Heal(_healthToRestore);
            Destroy(gameObject);
        }
    }
}
