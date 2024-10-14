using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] float hp;                  // 체력
    [SerializeField] float maxHp;               // 최대 체력
    [SerializeField] float attackDamage;        // 공격력
    [SerializeField] int increaseGoldAmount;  // 골드 증가량

    [Header("Attack")]
    [SerializeField] LayerMask layerMask;       // 공격할 레이어

    [Header("UI")]
    [SerializeField] Slider hpBar;              // 체력바
    [SerializeField] TextMeshProUGUI hpText;    // 체력바 텍스트
    [SerializeField] TextMeshProUGUI statText;  // 스텟 텍스트

    public int IncreaseGoldMount { get { return increaseGoldAmount; } }

    private StringBuilder sb;

    private void Awake()
    {
        sb = new StringBuilder();
    }

    private void Start()
    {
        hpBar.maxValue = maxHp;
        hpBar.value = hp;

        sb.Clear();
        sb.Append($"{hp} / {maxHp}");
        hpText.SetText(sb);

        UpdateText();
    }

    private void Update()
    {
        if(Input.touchCount > 0)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, layerMask)) 
            {
                Debug.Log("플레이어 몬스터 공격 중!");
                hit.collider.gameObject.GetComponent<Monster>().TakeDamage(attackDamage); 
            }
        }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("플레이어 공격 받는 중!");
        hp -= damage;

        hpBar.value = hp;

        if (hp <= 0)
        {
            hpBar.value = 0;
            Debug.Log("게임오버!");
            WaveManager.Instance.WaveOver();
        }
    }

    public void UpgradeStat(Store.Type type, int increase)
    {
        switch (type)
        { 
            case Store.Type.Power:
                attackDamage = increase;
                break;
            case Store.Type.Health:
                hp += increase;
                maxHp += increase;
                break;
            case Store.Type.GainGold:
                increaseGoldAmount = increase;
                break;
        }

        UpdateText();
    }

    private void UpdateText()
    {
        sb.Clear();

        sb.AppendLine("[Stat]");
        sb.AppendLine($"Damage : {attackDamage}");
        sb.AppendLine($"GoldGain : x{increaseGoldAmount}");

        statText.SetText(sb);

        hpBar.maxValue = maxHp;
        hpBar.value = hp;

        sb.Clear();
        sb.Append($"{hp} / {maxHp}");
        hpText.SetText(sb);
    }

}
