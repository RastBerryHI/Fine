using System.Collections;
using UnityEngine.Rendering;
using UnityEngine;


public class Dash : MonoBehaviour
{
    private CharacterController _controller;

    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashTime;

    [SerializeField] private AnimationCurve _vignetteValue;
    [SerializeField] private Volume _dashVolume;

    [SerializeField] private AudioSource _dashAudio;
    [SerializeField] private AudioClip _dashClip;

    private IEnumerator PerformDash(Vector3 direction)
    {  
        float startTime = Time.time;
        

        while(Time.time < startTime + _dashTime)
        {
            _controller.Move(direction * _dashSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void ShowFatigue()
    {
        _dashVolume.weight = Mathf.Lerp(_dashVolume.weight, 1, 0.2f);

        //currentTime += Time.deltaTime * 3;
        //if (currentTime >= totalTime)
        //    currentTime = 0;
    }

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void ToDash()
    {
        if (Input.GetKeyDown(KeyCode.Space) == true && Input.GetAxisRaw("Horizontal") == 1)
        {
            StartCoroutine(PerformDash(Viking.s_instance.transform.right));
            Viking.s_instance._animator.SetFloat("DodgeAxis", 1);

            _dashAudio.PlayOneShot(_dashClip);
        }
        else if (Input.GetKeyDown(KeyCode.Space) == true && Input.GetAxisRaw("Horizontal") == -1)
        {
            StartCoroutine(PerformDash(-Viking.s_instance.transform.right));
            Viking.s_instance._animator.SetFloat("DodgeAxis", -1);

            _dashAudio.PlayOneShot(_dashClip);
        }
        else if (Input.GetKeyDown(KeyCode.Space) == true && Input.GetAxisRaw("Vertical") == -1)
        {
            StartCoroutine(PerformDash(-Viking.s_instance.transform.forward));
            Viking.s_instance._animator.SetFloat("DodgeAxisY", -1);

            _dashAudio.PlayOneShot(_dashClip);
        }
    }

    void Update()
    {
        if( Viking.s_instance.IsAttacking == true || Viking.s_instance.IsBeingDamaged == true )
            return;

        if ( Viking.s_instance._animator.GetCurrentAnimatorStateInfo(0).IsName("LeftDodge") == false && Viking.s_instance._animator.GetCurrentAnimatorStateInfo(0).IsName("RightDodge") == false && Viking.s_instance._animator.GetCurrentAnimatorStateInfo(0).IsName("BackDodge") == false )
        {
            _dashVolume.weight = 0;
            ToDash();
        }   
        else
            ShowFatigue();
    }
}
