using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GateController : MonoBehaviour
{
    [SerializeField] List<GameObject> portals;
    [SerializeField] int currentPortal;


    public void ChangeState(Toggle toggle)
    {
        portals[currentPortal].SetActive(toggle.isOn);
    }

    public void ChangePortal(Toggle toggle)
    {
        if(portals.Count < 1) { return; }

        portals[currentPortal].SetActive(false);
        currentPortal = (currentPortal + 1) % (portals.Count);
        portals[currentPortal].SetActive(toggle.isOn);
    }

}
