using UnityEngine.EventSystems;

public interface ISelectHandler : IEventSystemHandler
{
    void OnSelected(bool isSelected);
}
