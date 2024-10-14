using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] GameObject player;
    [Header("Property")]
    [SerializeField] float moveSpeed;
    [SerializeField] float hp;
    [SerializeField] float maxHp;
    [SerializeField] bool isAttack;
    [SerializeField] int dropGold;

    [Header("Pool")]
    [SerializeField] int type;
    [SerializeField] MonsterPool returnPool;

    public MonsterPool ReturnPool { set { returnPool = value; } }
    public int Type{ set { type = value; } }

    [Header("Animation")]
    [SerializeField] Animator animator;


    [Header("State")]
    [SerializeField] State curState;
    public enum State { Trace, Attack, Die, Size }
    BaseState[] states = new BaseState[(int)State.Size];


    private void Awake()
    {
        animator = GetComponent<Animator>();

        states[(int)State.Trace] = new TraceState(this);
        states[(int)State.Attack] = new AttackState(this);
        states[(int)State.Die] = new DieState(this);
    }

    private void Start()
    {
        // 목표 대상을 플레이어로 설정
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        hp = maxHp;
        curState = State.Trace;
        states[(int)curState].Enter();
    }


    private void Update()
    {
        states[(int)curState].Update();
    }

    // 충돌이 발생했는데
    private void OnTriggerEnter(Collider other)
    {
        // 그게 플레이어면 공격 범위에 들어왔다
        if(other.CompareTag("Player"))
        {
            // 그래서 공격
            isAttack = true;
        }
    }

    // 누가 나갔는데
    private void OnTriggerEixt(Collider other)
    {
        // 그게 플레이어면 공격 범위에서 나갔다
        if (other.CompareTag("Player") )
        {
            // 그래서 추격
            isAttack = false;
        }
    }


    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            ChangeState(State.Die);
        }
    }

    public void ChangeState(State state)
    {
        states[(int)curState].Exit();
        curState = state;
        states[(int)curState].Enter();
    }

    private class TraceState : BaseState
    {
        [SerializeField] Monster monster;

        public TraceState(Monster monster) { this.monster = monster; }

        public override void Update()
        {
            // 플레이어를 향해 추적하는 로직
            monster.transform.position = Vector3.MoveTowards(monster.transform.position, monster.player.transform.position, monster.moveSpeed * Time.deltaTime);
            monster.transform.LookAt(monster.player.transform.position);

            // 공격 범위에 들어 왔으면
            if(monster.isAttack.Equals(true))
            {
                // 공격
                monster.ChangeState(State.Attack);
            }
        }

    }

    private class AttackState : BaseState
    {
        [SerializeField] Monster monster;
        [SerializeField] float currentAttackCoolTime;

        private Coroutine attackCoroutine;

        public AttackState(Monster monster)
        {
            this.monster = monster;
        }

        public override void Enter()
        {
            Debug.Log("enemy Attack Start");
            attackCoroutine = monster.StartCoroutine(AttackCoroutine());

        }

        public override void Update()
        {
            if (monster.isAttack.Equals(false))
            {
                monster.ChangeState(State.Trace);
            }
        }

        public override void Exit()
        {
            Debug.Log("enemy Attack Stop!");
            monster.StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        private IEnumerator AttackCoroutine()
        {
            float currentAttackCoolTime = 1f;
            while (true)
            {
                if (currentAttackCoolTime >= 1)
                {
                    // 쿨타임 초기화
                    currentAttackCoolTime = 0f;
                    // 공격 개시
                    monster.animator.SetTrigger("Attack");

                    monster.player.GetComponent<PlayerController>().TakeDamage(1f);
                }

                currentAttackCoolTime += Time.deltaTime;

                yield return null;
            }
        }

    }

    private class DieState : BaseState
    {
        private Monster monster;

        public DieState(Monster monster) { this.monster = monster; }

        public override void Enter()
        {
            Debug.Log($"{monster.name} is Die!");
            // 풀에 회수
            monster.returnPool.ReturnPool(monster.type, monster);

            // 골드 드랍
            GameManager.Instance.Gold += monster.dropGold * monster.player.GetComponent<PlayerController>().IncreaseGoldMount;
        }
    }

}
