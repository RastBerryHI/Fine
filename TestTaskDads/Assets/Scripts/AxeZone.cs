using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeZone : MonoBehaviour
{
    public float _damage;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Mutant")
        {
            other.GetComponent<Mutant>().EarnDamage(_damage);
        }
    }
}
