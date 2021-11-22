using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantAnimationEvents : MonoBehaviour
{
    [Header("Mutant properties")]
    [SerializeField] Mutant _mutant;
    [SerializeField] Collider _claw;
    [SerializeField] DamageZone _damageZone;

    void Start()
    {
        _claw.enabled = false;
        _damageZone._damage = _mutant.Damage;
    }

    void OpenMutantZone()
    {
        _claw.enabled = true;
    }

    void CloseMutantZone()
    {
        _claw.enabled = false;
    }
}
