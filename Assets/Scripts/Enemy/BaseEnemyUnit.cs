using UnityEngine;
using UniRx;
using Ami.BroAudio;
using System.Collections;

public abstract class BaseEnemyUnit : MonoBehaviour
{
    protected enum State { Move, Attack, Hit, Death }

    protected State currentState;
    private Subject<State> stateChanged = new Subject<State>();

    protected Transform player;
    protected Animator animator;
    protected bool isDead = false;

    [Header("Enemy stats")]
    [SerializeField] protected EnemySO enemyData;


    private CompositeDisposable disposables = new CompositeDisposable();
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected int attackDmg;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float fovAngle;
    public float health;

    [Header("Sounds")]
    public SoundID attackSound;
    public SoundID damageSound;
    protected virtual void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    protected virtual void OnEnable()
    {
        if (player == null) return;
        if (TryGetComponent<Animator>(out animator))
        {
            Debug.Log("Animator found.");
        }
        isDead = false;

        if (enemyData != null)
        {
            moveSpeed = enemyData.moveSpeed;
            attackDmg = enemyData.attakDmg;
            attackRange = enemyData.attackRange;
            fovAngle = enemyData.fovAngle;
            health = enemyData.health;
        }

        Observable.EveryUpdate()
.Where(_ => currentState == State.Move && !isDead && player != null)
.Subscribe(_ =>
{
    MoveTowardsPlayer();
    CheckAttackRange();
})
.AddTo(this);

        currentState = State.Move;
        stateChanged.OnNext(currentState);
    }
    protected virtual void OnDisable()
    {
        disposables.Clear();
    }
    protected virtual void MoveTowardsPlayer()
    {
        //   if (currentState != State.Move) return;
        animator.Play("Walk");
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }
    protected virtual void CheckAttackRange()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            currentState = State.Attack;
            stateChanged.OnNext(currentState);
            animator.Play("Attack");
            BroAudio.Play(attackSound);
        }
    }
    public virtual void TryHitPlayer() // Call in animation frame
    {
        float distance = Vector2.Distance(transform.position, player.position);
        // float angle = Vector2.Angle(transform.right, player.position - transform.position);

        if (distance <= attackRange /*&& angle <= fovAngle / 2f*/)
        {
            GameManager.Instance.OnPlayerDamaged.OnNext(attackDmg);
        }
    }
    public virtual void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;

        if (health <= 0)
        {
            Die();
        }
        else
        {
            currentState = State.Hit;
            stateChanged.OnNext(currentState);
            animator.Play("Hit");

            StartCoroutine(Recover());
        }
    }
    protected virtual void RecoverNMove()  //Assign in animation 
    {
        currentState = State.Move;
        stateChanged.OnNext(State.Move);
    }
    protected virtual IEnumerator Recover()
    {
        yield return new WaitForSeconds(.2f);
        if (!isDead)
        {
            Debug.Log("Transitioning to Move state.");
            currentState = State.Move;
            stateChanged.OnNext(State.Move);
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        currentState = State.Death;
        stateChanged.OnNext(State.Death);
        animator.Play("Death");
        BroAudio.Play(damageSound);
        GameManager.Instance.OnEnemyDied.OnNext(Unit.Default);

    }
    protected virtual void ReturnObject()
    {
        ObjectPoolSystem.ReturnObjectToPool(gameObject);
    }
    // Call this via Animation Event during attack animation

    private void OnDrawGizmosSelected()
    {
        if (player == null) return;

        // Draw attack range as a circle
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


}
