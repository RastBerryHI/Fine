using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DamageZone : MonoBehaviour
{
    public float _damage = 0;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Viking.s_instance.EarnDamage(_damage);
        }
    }
}
