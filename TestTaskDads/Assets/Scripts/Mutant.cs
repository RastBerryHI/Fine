using UnityEngine.AI;
using UnityEngine.VFX;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Mutant : MonoBehaviour, IPawn
{
    [SerializeField] private float _health;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _damage;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _smoothing;
    [SerializeField] private bool b_isAttacking;
    [SerializeField] private bool b_isAlive = true;

    [SerializeField] private VisualEffect _bloodFx;

    private NavMeshAgent _agent;
    private Animator _animator;
    public float Health
    {
        get => _health;
        set
        {
            if (value < 0)
            {
                _health = 0;
            }
            else if (value > 5)
            {
                _health = 5;
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
            if (value != 1)
                _damage = 1;
        } 
    }
    public float AttackSpeed 
    { 
        get => _attackSpeed;
        set
        {
            if(_attackSpeed < 0.5f)
            {
                _attackSpeed = 0.5f;
            }
            else if(_attackSpeed > 2)
            {
                _attackSpeed = 2;
            }
            else
            {
                _attackSpeed = value;
            }
        }
    }

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void DealDamage(IPawn pawn, float damage)
    {
        pawn.EarnDamage(damage);
    }

    public void Die()
    {
        if (this != null)
        {
            //Spawner.s_instance.AddEnemy(Spawner.s_instance.GetRandomPosition());
            transform.DOLocalMoveY(-100, 500);
        }
        _animator.SetBool("isDead", true);
        b_isAlive = false;

        _agent.enabled = false;
        Destroy(gameObject, 3f);
    }

    public void EarnDamage(float damage)
    {
        Health -= damage;
        _bloodFx.Play();
        StartCoroutine(TurnOffDamage());
    }

    private IEnumerator TurnOffDamage()
    {
        yield return new WaitForEndOfFrame();
        _bloodFx.Stop();
    }

    public void Move(Vector3 direction, float speed)
    {
        _agent.speed = speed;
        _agent.SetDestination(Viking.s_instance.transform.position);

        _animator.SetFloat("Velocity", _agent.velocity.magnitude);
        _animator.SetBool("isAttack", false);
    }

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (_agent.velocity.magnitude <= 0.1f)
        {
            transform.LookAt(Viking.s_instance.transform);
            if ( Viking.s_instance != null )
            {
                _animator.SetBool("isAttack", true);
            }
        }
    }

    void FixedUpdate()
    {
        if (Health == 0)
        {
            Die();
            return;
        }


        if( _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == true )
        {
            Move(Viking.s_instance.transform.position, Mathf.Lerp(MoveSpeed, 0, _smoothing));
        }
        else if( _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false)
        {
            Move(Viking.s_instance.transform.position, MoveSpeed);
        }
    }

    void OnDestroy()
    {
        Spawner.s_instance.OnEnemyDeath();
    }
}
