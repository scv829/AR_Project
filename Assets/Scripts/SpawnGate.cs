using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SpawnGate : MonoBehaviour
{
    [SerializeField] GameObject gate;
    
    [SerializeField] bool isSpawned;

    private void Update()
    {
        if(Input.touchCount > 0 && !isSpawned)
        {
            // 카메라로 부터 20 떨어진 곳에 생성
            gate.transform.position = Camera.main.transform.position + (Vector3.forward * 10);
            // 게이트는 카메라를 바라보게 설정
            gate.transform.LookAt(Camera.main.transform.position);
            // 게이트 활성화
            gate.SetActive(true);
            // 게이트를 만들었다고 설정
            isSpawned = true;
        }
    }

}
