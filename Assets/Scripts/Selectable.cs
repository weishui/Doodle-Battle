using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Selectable : MonoBehaviour
{
    [SerializeField]
    private GameObject selectionIndicator;
    private NavMeshAgent navMeshAgent;
    private bool IsSelected 
    { 
        get { return selectionIndicator.activeSelf; } 
        set { selectionIndicator.SetActive(value); } 
    }

    private void Start()
    {
        IsSelected = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
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
            if (!IsSelected)
            {
                Debug.Log(gameObject.name + " is selected.");
                IsSelected = true;
                this.AddObserver(GoTo, InputManager.GoTo);
            }
        }
        else
        {          
            if (IsSelected)
            {
                Debug.Log(gameObject.name + " is deselected");
                IsSelected = false;
                this.RemoveObserver(GoTo, InputManager.GoTo);
            }
        }        
    }

    void GoTo(object sender, object args)
    {
        if (IsSelected)
        {
            Vector3 dest = (Vector3)args;
            Debug.Log(gameObject.name + "Going to " + args);
            navMeshAgent.destination = dest;
        }
        else
        {
            //Debug.Log(gameObject.name + " is not selected but observing the GoTo note");
        }
    }

}
