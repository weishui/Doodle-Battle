using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Selectable : MonoBehaviour
{
    [SerializeField]
    private GameObject selectionIndicator;
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        selectionIndicator.SetActive(false);
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
            Debug.Log("OnSelected: " + args.ToString() + " is passed to the Unit.");
            selectionIndicator.SetActive(true);
            this.AddObserver(GoTo, InputManager.GoTo);
        }
        else if(selectionIndicator.activeSelf)
        {          
            Debug.Log(args.ToString() + " is deselected");
            selectionIndicator.SetActive(false);
            this.RemoveObserver(GoTo, InputManager.GoTo);
        }        
    }

    void GoTo(object sender, object args)
    {
        Vector3 dest = (Vector3)args;
        Debug.Log("Going to " + args);
        navMeshAgent.destination = dest;
    }

}
