using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float moveSpeed;
    [SerializeField] bool isAttack;

    [Header("Pool")]
    [SerializeField] MonsterPool returnPool;

    public MonsterPool ReturnPool { set { returnPool = value; } }

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

    private void OnTriggerEixt(Collider other)
    {
        // 그게 플레이어면 공격 범위에 들어왔다
        if (other.CompareTag("Player"))
        {
            // 그래서 공격
            isAttack = false;
        }
    }
}
