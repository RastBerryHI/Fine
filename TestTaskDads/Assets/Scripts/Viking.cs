using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

enum CommonStates : byte 
{
    Patrol,
    Combat
}

public class Viking : MonoBehaviour, IPawn
{
    public static Viking instance;

    [SerializeField] private float _health;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _damage;
    [SerializeField] private float _attackSpeed;

    [SerializeField] private bool b_isAttacking;
    [SerializeField] private bool b_isBeingDamaged;
    [SerializeField] private float _smoothing;

    [SerializeField] private VisualEffect _bloodFx;

    private CharacterController _characterController;
    public Animator _animator;
    private float _baseSpeed;

    public float Health 
    { 
        get => _health;
        set
        {
            if(value < 0)
            {
                _health = 0;
            }
            else if(value > 20)
            {
                _health = 20;
            }
            else
            {
                _health = value;
            }
        }
    }
    public float MoveSpeed
    {
        get => _moveSpeed;
        set
        {
            if (value < 0)
            {
                _moveSpeed = 0;
            }
            else if (_moveSpeed > 10)
            {
                _moveSpeed = 5;
            }
            else
            {
                _moveSpeed = value;
            }
        }
    }
    public float Damage
    {
        get => _damage;
        set
        {
            if (value > 3)
                _damage = 3;
        }
    }
    public float AttackSpeed
    {
        get => _attackSpeed;
        set
        {
            if (_attackSpeed < 0.5f)
            {
                _attackSpeed = 0.5f;
            }
            else if (_attackSpeed > 2)
            {
                _attackSpeed = 2;
            }
            else
            {
                _attackSpeed = value;
            }
        }
    }
    public bool IsAttacking
    {
        get => b_isAttacking;
    }
    public void DealDamage(IPawn pawn , float damage)
    {
        pawn.EarnDamage(damage);
    }

    public void Attack()
    {

    }

    public void Die()
    {
        Debug.LogError("Viking has died");
    }

    public void EarnDamage(float damage)
    {
        Health -= damage;
        _bloodFx.Play();

        _animator.SetBool("isDamage", true);
        StartCoroutine(TurnOffDamage());
    }

    public void Move(Vector3 direction, float speed)
    {
        direction += Physics.gravity;
        _characterController.Move(direction * speed * Time.fixedDeltaTime);
    }

    private void GetInput(out float x, out float z)
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
    }
#region Animations
    void SetStrafe(float x, float z)
    {
        _animator.SetFloat("VelocityX", x);
        _animator.SetFloat("VelocityZ", z);
    }
    void SetAttack()
    {
        _animator.SetBool("isPrimary", Input.GetMouseButtonDown(0));

        if ( _animator.GetCurrentAnimatorStateInfo(0).IsName("Primary") == true )
        {
            b_isAttacking = true;
        }
        else if ( _animator.GetCurrentAnimatorStateInfo(0).IsName("Locomotion") == true )
        {
            b_isAttacking = false;
        }
    }
    void SetDamage()
    {
        if ( _animator.GetCurrentAnimatorStateInfo(0).IsName("VikingDamage") == true )
        {
           b_isBeingDamaged = true;
        }
        else if ( _animator.GetCurrentAnimatorStateInfo(0).IsName("VikingDamage") == false)
        {
            b_isBeingDamaged = false;
        }
    }
    #endregion

#region Routines
    private IEnumerator TurnOffDamage()
    {
        yield return new WaitForEndOfFrame();
        _animator.SetBool("isDamage", false);
        _bloodFx.Stop();
    }
#endregion
    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();

        if (instance == null)
            instance = this;
        else
            instance = null;
    }

    void Start()
    {
        _baseSpeed = MoveSpeed;
    }

    void FixedUpdate()
    {
        if (Health == 0)
            Die();

        float x, z;
        Vector3 direction;

        GetInput(out x, out z);
        direction = new Vector3(x, 0, z).normalized;

        SetStrafe(x, z);
        SetAttack();
        SetDamage();

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        Vector3 converted = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

        switch( b_isBeingDamaged )
        {
            case true:
                MoveSpeed = 3;
                break;
            case false:
                MoveSpeed = _baseSpeed;
                break;
        }

        if ( direction.magnitude >= 0.1f && b_isAttacking == false )
        {
            Move(converted, MoveSpeed);
        }
        else if( direction.magnitude >= 0.1f && b_isAttacking == true )
        {
            Move(converted, Mathf.Lerp(MoveSpeed, 0, _smoothing));
        }
    }
}
