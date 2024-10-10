using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class SpawnGate : MonoBehaviour
{
    [SerializeField] GameObject gate;
    
    [SerializeField] bool isSpawned;
    [SerializeField] GameObject spawnButton;

    private void Update()
    {
        if(Input.touchCount > 0 && !isSpawned)
        {
            // 카메라로 부터 10 떨어진 곳에 생성
            gate.transform.position = Camera.main.transform.position + (Camera.main.transform.forward * 10);
            // 게이트는 카메라를 바라보게 설정
            gate.transform.LookAt(Camera.main.transform.position);
            // 게이트 활성화
            gate.SetActive(true);
            // 게이트를 만들었다고 설정
            isSpawned = true;
            spawnButton.SetActive(true);
        }
    }

}
