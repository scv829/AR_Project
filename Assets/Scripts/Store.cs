using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] PlayerController player;

    [Header("UpgradeButton")]
    [SerializeField] List<Button> buttonList;

    [Header("UpgradeButtonText")]
    [SerializeField] List<TextMeshProUGUI> textList;

    [Header("Level")]
    [SerializeField] int[] levels;
    [SerializeField] int powerLevel;           // 공격력 현재 레벨
    [SerializeField] int healthLevel;          // 체력 현재 레벨
    [SerializeField] int gainGoldAmountLevel;  // 골드 획득량 현재 레벨

    [Header("UpgradeCosts")]
    [SerializeField] int[,] costArray;
    public enum Type { Power, Health, GainGold, Size }

    [Header("UpgradeStat")]
    [SerializeField] int[,] statArray;

    private StringBuilder sb;

    private Coroutine openCoroutine;
    private Coroutine closeCoroutine;

    private void Awake()
    {
        costArray = new int[4, (int)Type.Size]
        {
            { 100, 100, 100 },    // 첫 번째 업그레이드 비용
            { 200, 200, 200 },    // 두 번째 업그레이드 비용
            { 300, 300, 300 },    // 세 번째 업그레이드 비용
            { 500, 500, 500 },    // 마지막 업그레이드 비용
        };

        statArray = new int[4, (int)Type.Size]
{         //  ATK,  HP, GAIN
            {   2,   2,   2 },    // 첫 번째 업그레이드 스텟
            {   4,   2,   3 },    // 두 번째 업그레이드 스텟
            {   6,   3,   4 },    // 세 번째 업그레이드 스텟
            {   8,   5,   5 },    // 마지막 업그레이드 스텟
        };

        levels = new int[(int)Type.Size];
        sb = new StringBuilder();

        openCoroutine = null;
        closeCoroutine = null;
    }

    private void Start()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            textList[i] = buttonList[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    public void OpenStore()
    {
        Debug.Log("상점 버튼 오픈!");
        if (closeCoroutine != null) { StopCoroutine(closeCoroutine); closeCoroutine = null; }
        openCoroutine = StartCoroutine(OpenStoreCorotine());

        // 버튼들이 업글 가능한지 확인 -> 골드에 변화량이 생길 때 UI 변경
        CheckUpgrade();
    }

    IEnumerator OpenStoreCorotine()
    {
        canvas.gameObject.GetComponent<Animator>().SetTrigger("OpenTrigger");
        while (true)
        {
            if (canvas.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) break;
            yield return null;
        }

        foreach(Button button in buttonList) button.gameObject.SetActive(true);
        GameManager.Instance.ChangeStoreCanvas();

        openCoroutine = null;
    }

    public void CloseStore()
    {
        Debug.Log("상점 버튼 클로즈!");
        if (openCoroutine != null) { StopCoroutine(openCoroutine); openCoroutine = null; }
        closeCoroutine = StartCoroutine(CloseStoreCorotine());
    }

    IEnumerator CloseStoreCorotine()
    {
        canvas.gameObject.GetComponent<Animator>().SetTrigger("CloseTrigger");
        while (true)
        {
            if (canvas.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) break;
            yield return null;
        }

        foreach(Button button in buttonList) button.gameObject.SetActive(false);
        GameManager.Instance.ChangeMainCanvas();

        closeCoroutine = null;
    }

    public void Upgrade(int index)
    {
        if (!Enum.IsDefined(typeof(Type), index)) new NullReferenceException("해당하는 값이 Store.Type에 없습니다");
        
        Type type = (Type)index;

        Debug.Log($"{type} 업그레이드!");

        GameManager.Instance.Gold -= costArray[levels[(int)type], (int)type];
        player.UpgradeStat(type, statArray[levels[(int)type], (int)type]);

        levels[(int)type] += 1;


        // 업그레이드를 했는데 최고 레벨일 경우 Text 설정
        if (levels[(int)type] >= costArray.GetLength(0))
        {
            sb.Clear();
            sb.AppendLine($"{type}");
            sb.Append("Max");

            textList[(int)type].SetText(sb);

            // 더이상 업그레이드 못하게 비활성화
            buttonList[(int)type].interactable = true;
        }

        // 업그레이드 이후 다시 버튼들을 확인
        CheckUpgrade();
    }

    // 업그레이드 가능 여부
    private void CheckUpgrade()
    {
        for (int index = 0; index < (int)Type.Size; index++)
        {
            // 업그레이드를 모두 완료한 버튼은 변경할 게 없으니 그냥 넘어가기
            if (levels[index] >= costArray.GetLength(0)) continue;

            // 업그레이브 부위의 현재 레벨이 다음 레벨의 업그레이드 비용보다 내 소지 골드가 더 많으면
            if (costArray[levels[index], index] <= GameManager.Instance.Gold)
            {
                // 강화 버튼을 활성화
                buttonList[index].interactable = true;
                // 버튼의 자식에 있는 Text의 색상 변경
                textList[index].color = new Color(0, 0, 0);
            }
            // 비용이 없다면
            else
            {

                // 강화 버튼을 비활성화
                buttonList[index].interactable = false;

                // 버튼의 자식에 있는 Text의 색상 변경
                textList[index].color = new Color(255, 0, 0);
            }

            // 버튼의 내용 설정
            sb.Clear();
            sb.AppendLine($"{(Type)index}");
            sb.Append($"Cost : {costArray[levels[index], index]}");

            textList[index].SetText(sb);
        }
    }
}
