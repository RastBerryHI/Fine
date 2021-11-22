using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingAnimationEvents : MonoBehaviour
{
    [Header("Viking properties")]
    [SerializeField] private Collider _axe;
    [SerializeField] private AxeZone _damageZone;
     private AudioSource _vikingAudio;

    [SerializeField] private AudioClip[] _axeClips;

    void Awake()
    {
        _vikingAudio = GetComponent<AudioSource>();
    }
    void Start()
    {
        _axe.enabled = false;
        _damageZone._damage = Viking.s_instance.Damage;
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
        Viking.s_instance._animator.SetFloat("DodgeAxis", 0);
        Viking.s_instance._animator.SetFloat("DodgeAxisY", 0);
    }

    void PrimarySwing()
    {
        _vikingAudio.PlayOneShot(_axeClips[Random.Range(0, _axeClips.Length - 1)]);
    }
}
