﻿using UnityEngine.EventSystems;

public interface IMoveEventHandler : IEventSystemHandler
{
    void OnDestination(UnityEngine.RaycastHit hit);
}
