using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] int gold;                  // 플레이어의 소지 골드
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI gameoverUI;
    [SerializeField] bool isOver;
    [SerializeField] bool isClear;
    [SerializeField] bool isUseStore;
    [SerializeField] Button openPortalButton;
    [SerializeField] PortalController portalController;


    [Header("Manager")]
    [SerializeField] Toggle spawnToggle;
    [SerializeField] Button nextButton;
    [SerializeField] Button OpenStore;

    public int Gold { get { return gold; } set { gold = value; UpdateText(); } }
    public void GameOver() => isOver = true;
    public void GameClear() => isClear = true;

    public void EnterStore() => isUseStore = true;
    public void ExitStore() => isUseStore = false;

    private StringBuilder sb;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        sb = new StringBuilder();
        isOver = false;
        isClear = false;
    }

    private void Start()
    {
        UpdateText();
    }

    private void Update()
    {
        if(isOver)
        {
            gameoverUI.gameObject.SetActive(true);
            if(Input.touchCount > 0) SceneManager.LoadScene(0);
        }
        else if(isClear && !isUseStore)
        {
            // 포탈 열고 닫기 토글 활성화
            spawnToggle.gameObject.SetActive(true);
            // 다음 포탈 설정 버튼 활성화
            nextButton.gameObject.SetActive(true);
            // 상점 포탈 자유 이동 활성화
            OpenStore.gameObject.SetActive(true);
            // 랜덤 포탈 버튼 비활성화
            openPortalButton.gameObject.SetActive(false);
        }
            
    }

    private void UpdateText()
    {
        sb.Clear();
        sb.AppendLine("Gold");
        sb.Append($"{gold}");

        goldText.SetText(sb);
    }

    public void ChangeStoreCanvas()
    {
        Debug.Log("상점 캠버스");
        spawnToggle.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
    }

    public void ChangeMainCanvas()
    {
        Debug.Log(" 캠버스");
        spawnToggle.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(true);
    }

    public void EnterPortal()
    {
        openPortalButton.gameObject.SetActive(false);
    }


    public void ExitPortal()
    {
        openPortalButton.gameObject.SetActive(true);
        portalController.TurnOffPortal();
    }

}
