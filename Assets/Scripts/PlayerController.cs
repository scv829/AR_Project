using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] float hp;              // 체력
    [SerializeField] float attackSpeed;     // 공격속도
    [SerializeField] float attackDamage;    // 공격력

    [Header("Attack")]
    [SerializeField] LayerMask layerMask;    // 공격할 레이어

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
        if (hp <= 0)
        {
            Debug.Log("게임오버!");
            WaveManager.Instance.WaveOver();
        }
    }

}
