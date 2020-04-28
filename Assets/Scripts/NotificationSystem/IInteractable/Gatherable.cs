using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Gatherable : MonoBehaviour, IInteractable
{
    public ResourceType resourceType;
    public List<GameObject> RegisteredGatherers = new List<GameObject>();
    public List<GameObject> GathererQueue = new List<GameObject>();
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
        RegisteredGatherers.Add(gatherer);
    }

    /// <summary>
    /// find all that are queuing to gather.
    /// if queue is empty, gather
    /// otherwise, find a nearest mine within area.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {

        if (RegisteredGatherers.Contains(other.gameObject))
        {
            GameObject gatherer = other.gameObject;
            if (!GathererQueue.Contains(gatherer) && GathererQueue.Count < maxQueue)
            {
                EnQueue(gatherer);
            }
            if (GathererQueue.Contains(gatherer))
            {
                if (GathererQueue.IndexOf(gatherer) < maxGatherers && gatherer.GetComponent<Gatherer>().currentTask != Tasks.Gather)
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
                ExecuteEvents.Execute<Gatherer>(gatherer, null, (x, y) => x.LookingForResourceNode());                
            }
        }
    }

    private void EnQueue(GameObject gatherer)
    {
        Debug.Log("Adding " + gatherer.name + " to gather queue of " + gameObject.name);
        GathererQueue.Add(gatherer.gameObject);       
    }

    private void ExecuteGathering(GameObject gatherer)
    {
        ExecuteEvents.Execute<Gatherer>(gatherer, null, (x, y) => x.Gathering(this));
    }

    private void ExecuteQueuing(GameObject gatherer)
    {
        ExecuteEvents.Execute<Gatherer>(gatherer, null, (x, y) => x.Queuing(this));
    }


}
