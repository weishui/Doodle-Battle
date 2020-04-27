using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Interacter : MonoBehaviour, IInteracter
{
    [SerializeField]
    private Tasks[] myTasks;
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void OnDestination(RaycastHit hit)
    {
        GameObject target = hit.collider.gameObject;
        
        if (target.GetComponent<Walkable>() != null)
        {
            navMeshAgent.destination = hit.point;
        }
        else
        {
            navMeshAgent.destination = hit.transform.position;
        }
        if (target.GetComponent<Gatherable>() != null && myTasks.Contains(Tasks.Gather))
        {
            ExecuteEvents.Execute<Gatherer>(gameObject, null, (x, y) => x.OnDestination(hit));
        }
        /*        if (hit.collider.gameObject.GetComponent<Harvestable>() != null)
                {
                    ExecuteEvents.Execute<Harvester>(hit.collider.gameObject, null, (x, y) => x.OnDestination(hit));
                }*/
    }
}
