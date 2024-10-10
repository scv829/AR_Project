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
            // ī�޶�� ���� 20 ������ ���� ����
            gate.transform.position = Camera.main.transform.position + (Vector3.forward * 10);
            // ����Ʈ�� ī�޶� �ٶ󺸰� ����
            gate.transform.LookAt(Camera.main.transform.position);
            // ����Ʈ Ȱ��ȭ
            gate.SetActive(true);
            // ����Ʈ�� ������ٰ� ����
            isSpawned = true;
        }
    }

}
