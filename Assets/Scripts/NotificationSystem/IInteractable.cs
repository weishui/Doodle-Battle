using UnityEngine;
using UnityEngine.EventSystems;

public interface IInteractable : IEventSystemHandler
{
    void OnInteracted(GameObject interacter);
}
