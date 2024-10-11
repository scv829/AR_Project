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

    [SerializeField] int totalMonsterCount;

    public int WaveSize(int level) { return sizeArray[level]; }

    private Coroutine spawnCoroutine;

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
                monster.Type = i;
                monsterList[i].Add(monster);
            }
        }
    }

    public void SpawnMonster()
    {
        // 몬스터 생성 함수에서 생성 코루틴을 호출
        StartCoroutine(spawnStart());
    }

    IEnumerator spawnStart()
    {
        // 웨이브 시작하고 바로 나오는건 너무 빨라서 대기 시간 이후 등장
        yield return new WaitForSeconds(3f);
        // 대기 시간 지나고 진짜 스폰 시작
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        // 몬스터들의 종류 만큼 반복
        for(int type = 0; type < monsterPrefab.Length; type++)
        {
            // 풀에 해당 종류의 몬스터가 있으면
            if (monsters[type] > 0)
            {
                // 있는 만큼 반복해서 생성
                for (int index = 0; index < monsters[type]; index++)
                {
                    int count = monsterList[type].Count;
                    Monster monster = monsterList[type][count - 1];
                    monster.transform.position = spawnPoint.position;
                    monster.transform.parent = null;
                    monster.ReturnPool = this;

                    monster.gameObject.SetActive(true);

                    monsters[type]--;
                    monsterList[type].RemoveAt(count - 1);

                    yield return new WaitForSeconds(2f);
                }
            }
        }
    }

    public void ReturnPool(int type, Monster monster)
    {
        monsters[type]++;
        monster.gameObject.SetActive(false);
        monster.transform.parent = transform;
        monsterList[type].Add(monster);

        WaveManager.Instance.CurrentMonsterCount -= 1; 

        if (WaveManager.Instance.CurrentMonsterCount <= 0)
        {
            WaveManager.Instance.WaveClear();
        }
    }
}
