using UnityEngine.EventSystems;

public interface ISelectable : IEventSystemHandler
{
    void SwitchSelected(bool isSelected);
}
