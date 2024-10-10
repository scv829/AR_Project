using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] GameObject player;
    [Header("Property")]
    [SerializeField] float moveSpeed;
    [SerializeField] bool isAttack;
    [SerializeField] int hp;
    [SerializeField] int maxHp;

    [Header("Pool")]
    [SerializeField] int type;
    [SerializeField] MonsterPool returnPool;

    public MonsterPool ReturnPool { set { returnPool = value; } }
    public int Type{ set { type = value; } }

    private void Start()
    {
        // 목표 대상을 플레이어로 설정
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        // 우선 공격중이면 멈추기
        if (isAttack) { return; }

        // 플레이어를 향해 추적하는 로직
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        transform.LookAt(player.transform.position);
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
        if (other.CompareTag("Player"))
        {
            // 그래서 추격
            isAttack = false;
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            returnPool.ReturnPool(type, this);
            hp = maxHp;
        }
    }

}
