using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    [SerializeField]
    private GameObject selectionIndicator;

    private void Start()
    {
        selectionIndicator.SetActive(false);
    }

    private void OnEnable()
    {
        this.AddObserver(OnSelected, InputManager.Selected);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnSelected, InputManager.Selected);
    }

    void OnSelected(object sender, object args)
    {
        if(gameObject == args as GameObject)
        {
            Debug.Log("OnSelected: " + args.ToString() + " is passed to the Unit.");
            selectionIndicator.SetActive(true);
        }
        else if(selectionIndicator.activeSelf)
        {          
            Debug.Log(args.ToString() + " is deselected");
            selectionIndicator.SetActive(false);
        }
        
    }

}
