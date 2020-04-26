using UnityEngine.EventSystems;

public interface IInteracter : IEventSystemHandler
{
    void OnDestination(UnityEngine.RaycastHit hit);
}
