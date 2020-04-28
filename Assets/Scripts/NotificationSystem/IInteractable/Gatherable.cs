using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Gatherable : MonoBehaviour, IInteractable
{
    public ResourceType resourceType;
    public List<GameObject> registeredGatherers = new List<GameObject>();
    public List<GameObject> gathererQueue = new List<GameObject>();
    public int maxQueue;
    public int maxGatherers;

    public int maxResource;
    public int currentResource;


    public void OnInteracted(GameObject gatherer)
    {
        Register(gatherer);
    }

    private void Register(GameObject gatherer)
    {
        if (!registeredGatherers.Contains(gatherer))
            registeredGatherers.Add(gatherer);
    }

    /// <summary>
    /// find all that are queuing to gather.
    /// if queue is empty, gather
    /// otherwise, find a nearest mine within area.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {

        if (registeredGatherers.Contains(other.gameObject))
        {
            GameObject gatherer = other.gameObject;
            if (!gathererQueue.Contains(gatherer) && gathererQueue.Count < maxQueue)
            {
                EnQueue(gatherer);
            }
            if (gathererQueue.Contains(gatherer))
            {
                if (gathererQueue.IndexOf(gatherer) < maxGatherers && !gatherer.GetComponent<Gatherer>().isGathering)
                {
                    ExecuteGathering(gatherer);
                }
                else
                {
                    ExecuteQueuing(gatherer);
                }
            }
            else
            {
                ExecuteEvents.Execute<Gatherer>(gatherer, null, (x, y) => x.FindAlternateResourceNode());                
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (registeredGatherers.Contains(other.gameObject))
        {
            GameObject gatherer = other.gameObject;
            if (gathererQueue.Contains(gatherer))
            {
                DeQueue(gatherer);
            }
        }
    }

    private void EnQueue(GameObject gatherer)
    {
        Debug.Log("Adding " + gatherer.name + " to gather queue of " + gameObject.name);
        gathererQueue.Add(gatherer.gameObject);       
    }
    private void DeQueue(GameObject gatherer)
    {
        Debug.Log("Removing " + gatherer.name + " to gather queue of " + gameObject.name);
        gathererQueue.Remove(gatherer.gameObject);       
    }

    private void ExecuteGathering(GameObject gatherer)
    {
        ExecuteEvents.Execute<Gatherer>(gatherer, null, (x, y) => x.Gather(this));
    }

    private void ExecuteQueuing(GameObject gatherer)
    {
        ExecuteEvents.Execute<Gatherer>(gatherer, null, (x, y) => x.Queuing(this));
    }


}
