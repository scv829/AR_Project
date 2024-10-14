using System.Collections;
using UnityEngine;

public class Trader : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;          // 상인 등장 지점
    [SerializeField] Transform moveFwdPoint;        // 등장할 때 목표로 갈 위치
    [SerializeField] float moveSpeed;               // 상인이 움직이는 속도

    [SerializeField] Transform parent;              // 상인을 자식으로 가지고 있을 부모

    public Transform Parent { set { parent = value; } }
    public Transform SpawnPoint { set { spawnPoint = value; } }

    public void MoveForward()
    {
        // 플레이어를 바라보고
        transform.LookAt(Camera.main.transform.position);
        // 활성화 시킨 후
        gameObject.SetActive(true);
        Debug.Log("상인등장!");
        // 10 정도 앞으로 
        Vector3 pos = spawnPoint.position + spawnPoint.forward * 10;
        // 이동
        StartCoroutine(MoveCoroutine(pos, true));
    }

    public void MoveReturn()
    {
        Debug.Log("상인퇴장!");
        StartCoroutine(MoveCoroutine(spawnPoint.position, false));
    }

    private IEnumerator MoveCoroutine(Vector3 pos, bool isOpen)
    {
        // 움직이는 애니메이션 시작
        while (true)
        {
            transform.position = Vector3.MoveTowards(spawnPoint.position, pos, moveSpeed * Time.deltaTime);
            if (transform.position.Equals(pos))
            {
                // 애니메이션 종료
                transform.parent = isOpen ? null : parent;
                gameObject.SetActive(isOpen);
                break;
            }
            yield return null;
        }
    }
}
