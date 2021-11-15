using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    CharacterController _controller;

    [SerializeField] float _dashSpeed;
    [SerializeField] float _dashTime;
    [SerializeField] float _dashDelay;

    private float _nextDash;
    private IEnumerator PerformDash(Vector3 direction)
    {
        float startTime = Time.time;

        while(Time.time < startTime + _dashTime)
        {
            _controller.Move(direction * _dashSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void ToDash()
    {
        if (Input.GetKeyDown(KeyCode.Space) == true && Input.GetAxisRaw("Horizontal") == 1)
        {
            StartCoroutine(PerformDash(Viking.instance.transform.right));
            Viking.instance._animator.SetFloat("DodgeAxis", 1);
        }
        else if (Input.GetKeyDown(KeyCode.Space) == true && Input.GetAxisRaw("Horizontal") == -1)
        {
            StartCoroutine(PerformDash(-Viking.instance.transform.right));
            Viking.instance._animator.SetFloat("DodgeAxis", -1);
        }
        else if (Input.GetKeyDown(KeyCode.Space) == true && Input.GetAxisRaw("Vertical") == -1)
        {
            StartCoroutine(PerformDash(-Viking.instance.transform.forward));
            Viking.instance._animator.SetFloat("DodgeAxisY", -1);
        }
    }

    void Update()
    {
        if( Viking.instance.IsAttacking == true )
        {
            return;
        }
        if( Viking.instance._animator.GetCurrentAnimatorStateInfo(0).IsName("LeftDodge") == false && Viking.instance._animator.GetCurrentAnimatorStateInfo(0).IsName("RightDodge") == false && Viking.instance._animator.GetCurrentAnimatorStateInfo(0).IsName("BackDodge") == false )
            ToDash();
    }
}
