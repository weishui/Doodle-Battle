using UnityEngine;
using UnityEngine.EventSystems;

public interface IInteracter : IEventSystemHandler
{
    void Register(GameObject target);
}
