using UnityEngine.EventSystems;

public interface IMoveEventHandler : IEventSystemHandler
{
    void OnDestination(UnityEngine.Vector3 dest);
}
