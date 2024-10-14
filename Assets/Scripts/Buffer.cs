using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;          // 마법사 등장 지점
    [SerializeField] Transform moveFwdPoint;        // 등장할 때 목표로 갈 위치
    [SerializeField] float moveSpeed;               // 마법사 움직이는 속도
    [SerializeField] Animator animator;

    [Header("Buff")]
    [SerializeField] GameObject[] buffPrefabs;

    [Header("Player")]
    [SerializeField] PlayerController playerController;

    public Transform SpawnPoint { set { spawnPoint = value; } }
    public Transform MovePoint { set { moveFwdPoint = value; } }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    public void MoveForward(int type)
    {
        transform.position = spawnPoint.position;
        // 활성화 시킨 후
        gameObject.SetActive(true);
        Debug.Log("마법사 등장!");
        // 이동
        StartCoroutine(MoveCoroutine(type));
    }

    private IEnumerator MoveCoroutine(int type)
    {
        // 갈 방향을 바라보고
        transform.LookAt(moveFwdPoint.position);
        while (true)
        {
            // 움직이기
            Debug.Log("움직이기!");
            transform.position = Vector3.MoveTowards(transform.position, moveFwdPoint.position, moveSpeed * Time.deltaTime);
            // 정해진 위치에 도달 했으면
            if (transform.position.Equals(moveFwdPoint.position))
            {
                // 버프 애니메이션 실행
                Debug.Log("버프 애니메이션!");
                animator.SetTrigger("BuffTrigger");

                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

                GameObject buff = Instantiate(buffPrefabs[type], transform.position, Quaternion.identity);
                Destroy(buff, 3f);

                if (type == 0)
                {
                    // 힐
                    Debug.Log("힐!");
                    playerController.HealBuff();
                }
                else if (type == 1)
                {
                    // 공격력 증가
                Debug.Log("공격력!");
                    playerController.DamageBuff();
                }

                break;
            }
            yield return null;
        }

        transform.LookAt(spawnPoint.position);
        while (true)
        {
            // 움직이기
            Debug.Log("다시 움직이기!");
            transform.position = Vector3.MoveTowards(transform.position, spawnPoint.position, moveSpeed * Time.deltaTime);
            // 정해진 위치에 도달 했으면
            if (transform.position.Equals(spawnPoint.position))
            {
                gameObject.SetActive(false);
                break;
            }
            yield return null;
        }

        Debug.Log("그 만!");
        GameManager.Instance.ExitPortal();
    }
}
