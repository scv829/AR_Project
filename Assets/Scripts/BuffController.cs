using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    [Header("Buffer")]
    [SerializeField] Buffer bufferPrefab;
    [SerializeField] Buffer buffer;
    [SerializeField] Transform spawnPoint; 
    [SerializeField] Transform movePoint;


    private void Awake()
    {
        buffer = Instantiate(bufferPrefab);
        buffer.gameObject.SetActive(false);
        buffer.SpawnPoint = spawnPoint;
        buffer.MovePoint = movePoint;
    }

    public void BuffStart(int type)
    {
        buffer.MoveForward(type);
    }

}
