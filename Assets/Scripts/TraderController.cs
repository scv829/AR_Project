using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class TraderController : MonoBehaviour
{
    [Header("Store")]
    [SerializeField] Store store;

    [Header("Trader")]
    [SerializeField] Trader trader;             // 상인 오브젝트
    [SerializeField] Trader TraderPrefab;       // 상인 프리팹
    [SerializeField] Transform spawnPoint;      // 상인 등장 지점     
    [SerializeField] Transform movePoint;    // 등장할 때 목표로 갈 위치

    [Header("Event")]
    [SerializeField] UnityEvent OpenEvent;
    [SerializeField] UnityEvent CloseEvent;

    private StringBuilder sb;

    private void Awake()
    {
        trader = Instantiate(TraderPrefab);
        trader.gameObject.SetActive(false);
        trader.SpawnPoint = spawnPoint;
        trader.MovePoint = movePoint;

        sb = new StringBuilder();
    }

    private void Start()
    {
        OpenEvent.AddListener(store.OpenStore);
        OpenEvent.AddListener(trader.MoveForward);

        CloseEvent.AddListener(store.CloseStore);
        CloseEvent.AddListener(trader.MoveReturn);
    }

    public void OpenStore()
    {
        OpenEvent?.Invoke();
    }

    public void CloseStore()
    {
        CloseEvent?.Invoke();
    }

}
