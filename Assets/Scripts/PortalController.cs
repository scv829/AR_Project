using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    [SerializeField] List<GameObject> portals;
    [SerializeField] TraderController traderController;
    [SerializeField] BuffController buffController;

    public void OpenPortal()
    {
        float num = Random.Range(0.8f, 1f);

        // Wave 포탈
        if (num < 0.6f)
        {
            Debug.Log("전투 On!");
            portals[0].gameObject.SetActive(true);
            WaveManager.Instance.WaveStart();
            GameManager.Instance.EnterPortal();
        }
        // 업그레이드 상점
        else if (0.6f <= num && num < 0.8f)
        {
            Debug.Log("업그레이드 상점 On!");
            portals[1].gameObject.SetActive(true);
            traderController.OpenStore();
            GameManager.Instance.EnterPortal();
        }
        // 히든 - 체력 회복
        else if (0.8f <= num && num < 0.9f)
        {
            Debug.Log("체력회복 포탈 On!");
            portals[2].gameObject.SetActive(true);
            buffController.BuffStart(0);
            GameManager.Instance.EnterPortal();
        }
        // 히든 - 공격력 버프
        else
        {
            Debug.Log("공격력 버프 포탈 On!");
            portals[3].gameObject.SetActive(true);
            buffController.BuffStart(1);
            GameManager.Instance.EnterPortal();
        }
    }

    public void TurnOffPortal()
    {
        foreach(GameObject portal in portals) portal.gameObject.SetActive(false);
    }
}
