using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [SerializeField] MonsterPool monsterPool;                       // 몬스터를 관리하고 있는 몬스터 풀
    [SerializeField] TextMeshProUGUI waveLevelText;                 // 현재 웨이브 단계를 보여줄 텍스트
    [SerializeField] TextMeshProUGUI remainingMonsterCountText;     // 남은 몬스터들의 수를 보여줄 텍스트

    [SerializeField] int totalMonsterCount;         // 전체 몬스터의 수
    [SerializeField] int currentMonsterCount;     // 남은 몬스터들의 수

    [SerializeField] Button startButton;        // 시작 버튼
    [SerializeField] int currentWave;           // 현재 웨이브 단계

    private StringBuilder sb;

    public int CurrentMonsterCount { get { return currentMonsterCount; } set { currentMonsterCount = value; UpdateWaveText(); } }

    private void Awake()
    {
        // WaveManager는 씬에 하나만 존재하므로 -> 싱글톤 사용
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        sb = new StringBuilder();
    }

    public void WaveStart()
    {
        // UI 보여주기
        waveLevelText.gameObject.SetActive(true);
        remainingMonsterCountText.gameObject.SetActive(true);

        // 전체 몬스터의 수와 남은 몬스터의 수를 몬스터 웨이브의 크기많큼 초기화
        totalMonsterCount = currentMonsterCount = monsterPool.WaveSize(currentWave);
        // 몬스터 풀에게 몬스터를 생성 요청
        monsterPool.SpawnMonster();
        // UI 내용 업데이트
        UpdateWaveText();

        // 시작 버튼 비활성화
        startButton.gameObject.SetActive(false);
    }

    public void UpdateWaveText()
    {
        // 현재 남은 적을 표시
        sb.Clear();
        sb.Append($"{currentMonsterCount} / {totalMonsterCount}");
        remainingMonsterCountText.SetText(sb);

        // 현재 웨이브를 표시
        sb.Clear();
        sb.Append($"Wave {currentWave + 1}");
        waveLevelText.SetText(sb);
    }

    public void WaveClear()
    {
        currentWave++;
        // UI 감추기
        remainingMonsterCountText.gameObject.SetActive(false);

        // 다음 웨이브가 있으면 시작 버튼 활성화
        if (!monsterPool.WaveSize(currentWave).Equals(-1))
        {
            waveLevelText.gameObject.SetActive(false);
            startButton.gameObject.SetActive(true);
        }
        else
        {
            sb.Clear();
            sb.Append($"All Clear");
            waveLevelText.SetText(sb);
            waveLevelText.gameObject.SetActive(true);
        }
    }

    public void WaveOver()
    {
        // 웨이브를 실패해서 다시 시작
        Debug.Log("게임 다시 시작!");
        SceneManager.LoadScene(0);
    }

}
