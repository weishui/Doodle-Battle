using UnityEngine.EventSystems;

public interface IMoveHandler : IEventSystemHandler
{
    void OnDestination(UnityEngine.Vector3 dest);
}
