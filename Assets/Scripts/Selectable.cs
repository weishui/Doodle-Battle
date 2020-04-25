using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Selectable : MonoBehaviour
{
    [SerializeField]
    private GameObject selectionIndicator;
    private NavMeshAgent navMeshAgent;

    [System.Obsolete("Should put this to private after debugging")]
    public bool IsSelected 
    { 
        get { return selectionIndicator.activeSelf; } 
        set 
        { 
            selectionIndicator.SetActive(value); 
            if (value)
            {
                this.AddObserver(GoTo, InputManager.GoTo);
            }
            else
            {
                this.RemoveObserver(GoTo, InputManager.GoTo);
            }
        } 
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
        GameObject[] selectedObjects = (GameObject[])args;
        if (selectedObjects.Contains(gameObject) && !IsSelected)
        {
            IsSelected = true;
            Debug.Log(transform.name + " is selected");
        }
        else if (IsSelected)
            IsSelected = false;        
    }

    void GoTo(object sender, object args)
    {
        //if (IsSelected)
            navMeshAgent.destination = (Vector3)args;
    }

}
