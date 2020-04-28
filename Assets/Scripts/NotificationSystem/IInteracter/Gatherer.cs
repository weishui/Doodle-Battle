using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gatherer : MonoBehaviour, IInteracter
{

    public GameObject registeredResourceNode;
    public ResourceType resourceType;
    public int maxResourceHolding;
    public int resourceHolding;
    public bool isGathering;

    public void Register(GameObject target)
    {
        if (target == null)
        {
            if (registeredResourceNode == null)
            {
                registeredResourceNode = FindDefaultResourceNode();
                Debug.Log("Registered found: " + registeredResourceNode.name);
            }
        }
        else
        {
            registeredResourceNode = target;
        }
        if (registeredResourceNode != null)
        {
            Debug.Log(gameObject.name + " is On Destination of Gathering");
            ExecuteEvents.Execute<TaskManager>(gameObject, null, (x, y) => x.RegisterTask(Tasks.Gather));
            ExecuteEvents.Execute<TaskManager>(gameObject, null, (x, y) => x.Move(registeredResourceNode.transform.position));
            ExecuteEvents.Execute<Gatherable>(registeredResourceNode, null, (x, y) => x.OnInteracted(gameObject));
        }
    }

    public void Gather(Gatherable target)
    {
        isGathering = true;
        ResetResource(target);
        StartCoroutine(ResourceTick(target));
    }
    
    public void Queuing(Gatherable target)
    {

    }

    IEnumerator ResourceTick(Gatherable target)
    {
        while (target.currentResource > 0 && resourceHolding < maxResourceHolding)
        {
            yield return new WaitForSeconds(1);
            target.currentResource--;
            resourceHolding++;
        }
        isGathering = false;
        ExecuteEvents.Execute<Deliverer>(gameObject, null, (x, y) => x.Register(null));
    }


    private void ResetResource(Gatherable target)
    {
        resourceType = target.resourceType;
        Debug.Log("Reset holding resource Type to " + resourceType.ToString());
        resourceHolding = 0;
    }

    public void GivingOutResource(out ResourceType type, out int holding)
    {
        type = resourceType;        
        holding = resourceHolding;
        Debug.Log("Giving out resource type: " + type.ToString() + " of " + holding);
        resourceHolding = 0;
    }
    public GameObject FindAlternateResourceNode()
    {
        Debug.Log("Looking for ResourceNode");
        Gatherable[] nodes = GameObject.FindObjectsOfType<Gatherable>();
        Debug.Log("Nodes[0]: " + nodes[0].name);
        float minDistance = float.MaxValue;
        GameObject validTarget = null;
        foreach (var node in nodes)
        {
            float distance = (node.transform.position - transform.position).magnitude;
            if (distance < minDistance && node != registeredResourceNode)
            {
                minDistance = distance;
                validTarget = node.gameObject;
            }
        }
        if (validTarget != null)
        {
            Debug.Log("Find a target: " + validTarget.name);
        }
        return validTarget;
    }
    public GameObject FindDefaultResourceNode()
    {
        if (registeredResourceNode != null)
            return registeredResourceNode;
        Debug.Log("Looking for ResourceNode");
        Gatherable[] nodes = GameObject.FindObjectsOfType<Gatherable>();
        Debug.Log("Nodes[0]: " + nodes[0].name);
        float minDistance = float.MaxValue;
        GameObject validTarget = null;
        foreach (var node in nodes)
        {
            float distance = (node.transform.position - transform.position).magnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                validTarget = node.gameObject;
            }
        }
        if (validTarget != null)
        {
            Debug.Log("Find a target: " + validTarget.name);
        }
        return validTarget;
    }

}
