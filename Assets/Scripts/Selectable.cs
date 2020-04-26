using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Selectable : MonoBehaviour
{
    #region properties
    [SerializeField]
    private GameObject selectionIndicator;
    private NavMeshAgent navMeshAgent;

    private bool IsSelected 
    { 
        get { return selectionIndicator.activeSelf; } 
        set 
        { 
            selectionIndicator.SetActive(value); 
/*            if (value)
                this.AddObserver(GoTo, InputManager.GoTo);
            else
                this.RemoveObserver(GoTo, InputManager.GoTo);*/
        } 
    }
    #endregion

    #region MonoBehaviour
    private void Start()
    {
        IsSelected = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    #endregion

    #region Handlers
    void OnSelected(object sender, object args)
    {
        List<GameObject> units = (List<GameObject>)args;
        if (units.Contains(gameObject))
        {
            IsSelected = true;
            Debug.Log(transform.name + " is selected");
        }
        else
            IsSelected = false;      
    }


    #endregion

    #region OnEnable & OnDisable
    private void OnEnable()
    {
        this.AddObserver(OnSelected, InputManager.Selected);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnSelected, InputManager.Selected);
    }
    #endregion

}
