using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movable : MonoBehaviour, IMoveHandler
{
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void OnDestination(Vector3 dest)
    {
        navMeshAgent.destination = dest;
    }
}
