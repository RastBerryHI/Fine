using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingAnimationEvents : MonoBehaviour
{
    [Header("Viking properties")]
    [SerializeField] Collider _axe;
    [SerializeField] AxeZone _damageZone;

    void Start()
    {
        _axe.enabled = false;
        _damageZone._damage = Viking.instance.Damage;
    }

    void OpenVikingZone()
    {
        _axe.enabled = true;
    }

    void CloseVikingZone()
    {
        _axe.enabled = false;
    }

    void EndDodge()
    {
        Viking.instance._animator.SetFloat("DodgeAxis", 0);
        Viking.instance._animator.SetFloat("DodgeAxisY", 0);
    }
}
