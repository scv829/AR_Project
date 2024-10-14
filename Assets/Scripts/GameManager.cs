using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] int gold;                  // 플레이어의 소지 골드
    [SerializeField] TextMeshProUGUI goldText;

    [SerializeField] Toggle spawnToggle;
    [SerializeField] Button nextButton;

    public int Gold { get { return gold; } set { gold = value; UpdateText(); } }

    private StringBuilder sb;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        sb = new StringBuilder();
    }

    private void Start()
    {
        UpdateText();
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
}
