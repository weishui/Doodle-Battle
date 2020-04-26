using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Gatherable : MonoBehaviour, IInteractable
{
    public ResourceType resourceType;
    public List<GameObject> RegisteredGatherers = new List<GameObject>();
    public int currentGatherers;
    public int maxGatherers;

    public int maxResource;
    public int currentResource;


    public void OnInteracted(GameObject gatherer)
    {
        Debug.Log(gatherer.name + " is about to mine me! BTW, I am " + gameObject.name);
        RegisteredGatherers.Add(gatherer);
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collider triggered");
        if (RegisteredGatherers.Contains(other.gameObject))
        {
            if (currentGatherers < maxGatherers)
            {
                Debug.Log("coming in " + other.name);
                ExecuteEvents.Execute<Gatherer>(other.gameObject, null, (x, y) => x.StartGathering(this));
            }
        }
        
    }


}
