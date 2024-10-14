using System.Collections;
using UnityEngine;

public class Trader : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;          // 상인 등장 지점
    [SerializeField] Transform moveFwdPoint;        // 등장할 때 목표로 갈 위치
    [SerializeField] float moveSpeed;               // 상인이 움직이는 속도
    [SerializeField] Animator animator;

    public Transform SpawnPoint { set { spawnPoint = value; } }
    public Transform MovePoint { set { moveFwdPoint = value; } }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void MoveForward()
    {
        transform.position = spawnPoint.position;
        // 활성화 시킨 후
        gameObject.SetActive(true);
        Debug.Log("상인등장!");
        // 이동
        StartCoroutine(MoveCoroutine(moveFwdPoint.position, true));
    }

    public void MoveReturn()
    {
        Debug.Log("상인퇴장!");
        StartCoroutine(MoveCoroutine(spawnPoint.position, false));
    }

    private IEnumerator MoveCoroutine(Vector3 pos, bool isOpen)
    {
        // 갈 방향을 바라보고
        transform.LookAt (pos);
        // 움직이는 애니메이션 시작
        animator.SetBool("IsMove", true);
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime);
            if (transform.position.Equals(pos))
            {
                // 애니메이션 종료
                animator.SetBool("IsMove", false);
                gameObject.SetActive(isOpen);
                break;
            }
            yield return null;
        }
    }
}
