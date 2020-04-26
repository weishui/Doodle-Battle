using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movable : MonoBehaviour
{
    #region properties
    private NavMeshAgent navMeshAgent;
    #endregion

    #region MonoBehaviour
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    #endregion

    #region Handlers
    void GoTo(object sender, object args)
    {
        OnRightClickArgs onRightClickArgs = args as OnRightClickArgs;
        List<GameObject> units = onRightClickArgs.units;
        Vector3 dest = onRightClickArgs.dest;
        if (units.Contains(gameObject))
        {
            navMeshAgent.destination = dest;
        }
    }
    #endregion

    #region OnEnable & OnDisable
    private void OnEnable()
    {
        this.AddObserver(GoTo, InputManager.GoTo);
    }

    private void OnDisable()
    {
        this.RemoveObserver(GoTo, InputManager.GoTo);
    }
    #endregion

}
