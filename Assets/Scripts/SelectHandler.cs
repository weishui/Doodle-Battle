using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SelectHandler : MonoBehaviour, ISelectHandler
{
    [SerializeField]
    private GameObject selectedIndicator;

    private void Start()
    {
        selectedIndicator.SetActive(false);
    }

    public void OnSelected(bool isSelected)
    {
        selectedIndicator.SetActive(isSelected);
    }
}
