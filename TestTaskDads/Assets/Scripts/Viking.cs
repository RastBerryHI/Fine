using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

enum CommonStates : byte 
{
    Patrol,
    Combat
}

public class Viking : MonoBehaviour, IPawn, IEnumerable
{
    public static Viking s_instance;

    [SerializeField] private float _health;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _damage;
    [SerializeField] private float _attackSpeed;

    [SerializeField] private bool b_isAttacking;
    [SerializeField] private bool b_isBeingDamaged;
    [SerializeField] private float _smoothing;

    [SerializeField] private VisualEffect _bloodFx;
    [SerializeField] private Volume _damageVolume; 
    [SerializeField] private GameObject _attentionPointsParent;

    public Animator _animator;
    private CharacterController _characterController;
    private float _baseSpeed;

    private List<AttentionPoint> _attentionPoints;

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
    public bool IsBeingDamaged
    {
        get => b_isBeingDamaged;
    }
    public List<AttentionPoint> AttentionPoints
    {
        get => _attentionPoints;
    }
    public AttentionPoint this[int index]
    {
        get => _attentionPoints[index];
        set
        {
            _attentionPoints[index] = value;
        }
    }
    public void DealDamage(IPawn pawn , float damage)
    {
        pawn.EarnDamage(damage);
    }

    public void Attack()
    {
        _animator.SetBool("isPrimary", Input.GetMouseButtonDown(0));

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Primary") == true)
        {
            b_isAttacking = true;
        }
        else
        {
            b_isAttacking = false;
        }
    }

    public void Die()
    {
        Debug.LogError("Viking has died");
        _characterController.enabled = false;
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

    void SetDamage()
    {
        if ( _animator.GetCurrentAnimatorStateInfo(0).IsName("VikingDamage") == true )
        {
            b_isBeingDamaged = true;
            _damageVolume.weight = Mathf.Lerp(_damageVolume.weight, 1, 0.1f);
        }
        else if ( _animator.GetCurrentAnimatorStateInfo(0).IsName("VikingDamage") == false)
        {
             b_isBeingDamaged = false;
            _damageVolume.weight = Mathf.Lerp(_damageVolume.weight, 0, 0.1f);
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

        if (s_instance == null)
            s_instance = this;
        else
            s_instance = null;
        _attentionPoints = _attentionPointsParent.GetComponentsInChildren<AttentionPoint>().ToList<AttentionPoint>();
        _attentionPoints.RemoveAt(0);
    }

    void Start()
    {
        _baseSpeed = MoveSpeed;
    }

    void Update()
    {
        if (Health == 0)
            Die();
        Attack();
        SetDamage();
    }

    void FixedUpdate()
    {
        float x, z;
        Vector3 direction;

        GetInput(out x, out z);
        direction = new Vector3(x, 0, z).normalized;

        SetStrafe(x, z);
        
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

    public IEnumerator GetEnumerator()
    {
        for(int i = 0; i < _attentionPoints.Count; i++)
        {
            yield return _attentionPoints[i];
        }
    }
}
