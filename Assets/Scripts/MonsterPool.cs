using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : MonoBehaviour
{
    [SerializeField] int[] sizeArray;               // 생성할 몬스터들의 수
    [SerializeField] Monster[] monsterPrefab;       // 생성할 몬스터들
    [SerializeField] List<Monster>[] monsterList;   // 몬스터를 담을 오브젝트 풀
    [SerializeField] int[] monsters;                // 생성 가능한 각 몬스터들의 수
    [SerializeField] Transform spawnPoint;          // 몬스터를 생성할 위치

    private void Awake()
    {
        monsters = new int[monsterPrefab.Length];
        monsterList = new List<Monster>[monsterPrefab.Length];

        for (int i = 0; i < monsterPrefab.Length; i++)
        {
            monsterList[i] = new List<Monster>();
        }

        for (int i = 0; i < monsterPrefab.Length; i++)
        {
            monsters[i] = sizeArray[i];
            for(int j = 0; j < sizeArray[i]; j++)
            {
                Monster monster = Instantiate(monsterPrefab[i]);
                monster.gameObject.SetActive(false);
                monster.transform.parent = transform;
                monsterList[i].Add(monster);
            }
        }
    }


    public void SpawnMonster(int type)
    {
        if (monsters[type] > 0)
        {
            int count = monsterList[type].Count;
            Monster monster = monsterList[type][count - 1];
            monster.transform.position = spawnPoint.position;
            monster.transform.parent = null;
            monster.ReturnPool = this;

            monster.gameObject.SetActive(true);

            monsters[type]--;
            monsterList[type].RemoveAt(count-1);
        }
    }

}
