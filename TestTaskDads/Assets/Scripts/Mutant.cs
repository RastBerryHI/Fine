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
    [SerializeField] private MutantHealthBar _healthBar;
    [SerializeField] private GameObject _healthSphere;

    private NavMeshAgent _agent;
    private Animator _animator;
    private bool b_isTakenPosition = false;
    private Vector3 _healthSpherePosition;
    public  AttentionPoint _target;
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

    public void Attack()
    {
        transform.LookAt(Viking.s_instance.transform);
        if (Viking.s_instance != null)
        {
            _animator.SetBool("isAttack", true);
        }
    }

    public void DealDamage(IPawn pawn, float damage)
    {
        pawn.EarnDamage(damage);
    }

    public void Die()
    {
        if( b_isTakenPosition == false)
        {
            _healthSpherePosition = transform.position;
            b_isTakenPosition = true;
        }
        if (this != null)
        {
            //Spawner.s_instance.AddEnemy(Spawner.s_instance.GetRandomPosition());
            transform.DOLocalMoveY(-100, 500);
        }
        _animator.SetBool("isDead", true);
        b_isAlive = false;
       

        _target.b_isBusy = false;
        _agent.enabled = false;
        Destroy(gameObject, 3f);
    }

    public void EarnDamage(float damage)
    {
        Health -= damage;
        _healthBar.SetHealth(Health);

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
        _agent.SetDestination(direction);

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
        _healthBar.SetMaxHealth(Health);
    }

    void Update()
    {
        if (b_isAlive == false)
            return;

        if (_agent.velocity.magnitude <= 0.1f)
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        if (Health == 0)
        {
            Die();
            return;
        }

        if (_target == null)
            return;

        if( _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == true )
        {
            Move(_target.transform.position, Mathf.Lerp(MoveSpeed, 0, _smoothing));
        }
        else if( _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") == false )
        {
            Move(_target.transform.position, MoveSpeed);
        }
    }

    void OnDestroy()
    {
        Instantiate<GameObject>(_healthSphere, new Vector3(_healthSpherePosition.x, _healthSpherePosition.y + 0.5f, _healthSpherePosition.z), Quaternion.identity);
        Spawner.s_instance.OnEnemyDeath();
    }
}
