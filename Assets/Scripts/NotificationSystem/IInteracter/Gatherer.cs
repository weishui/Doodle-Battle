using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gatherer : MonoBehaviour, IInteracter
{
    public Tasks currentTask;
    public GameObject registeredTarget;
    public ResourceType resourceType;
    public int maxResourceHolding;
    public int resourceHolding;

    public void Start()
    {
        currentTask = Tasks.None;
    }
    public void OnDestination(RaycastHit hit)
    {
        registeredTarget = hit.collider.gameObject;
        Debug.Log(gameObject.name + " is On Destination of Gathering");
        currentTask = Tasks.Move;
        ExecuteEvents.Execute<Gatherable>(registeredTarget, null, (x, y) => x.OnInteracted(gameObject));
    }

    public void Gathering(Gatherable target)
    {
        ResetResource(target);
        currentTask = Tasks.Gather;
        StartCoroutine(ResourceTick(target));
    }
    
    public void Queuing(Gatherable target)
    {

    }

    IEnumerator ResourceTick(Gatherable target)
    {
        while(target.currentResource > 0 && resourceHolding < maxResourceHolding)
        {
            yield return new WaitForSeconds(1);
            ResourceGather(target);
        }
    }

    public void ResourceGather(Gatherable target)
    {
        target.currentResource--;
        resourceHolding++;
    }

    private void ResetResource(Gatherable target)
    {
        resourceType = target.resourceType;
        resourceHolding = 0;
    }

    public void LookingForResourceNode()
    {
        // Cannot queue a resource node, look for another node. if success, quit register the previous one.
    }

}
