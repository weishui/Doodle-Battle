using UnityEngine.EventSystems;

public interface ISelectEventHandler : IEventSystemHandler
{
    void SwitchSelected(bool isSelected);
}
