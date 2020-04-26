using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SelectEventHandler : MonoBehaviour, ISelectEventHandler
{
    [SerializeField]
    private GameObject selectedIndicator;

    private void Start()
    {
        selectedIndicator.SetActive(false);
    }

    public void SwitchSelected(bool isSelected)
    {
        selectedIndicator.SetActive(isSelected);
    }
}
