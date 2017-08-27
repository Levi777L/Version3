using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZoneChangeEvent : MonoBehaviour, IWorldEvent
{
    private int _eventId = 0;
    public EndType endType;
    public int nextZone;
    public TextMeshPro tmp;
    IMode mode;

    public int EventID { get { return _eventId; } set { _eventId = value; } }

    public void StartEvent(IMode mode)
    {
        this.mode = mode;
        int i = 0;
        int.TryParse(tmp.text, out i);
        if (mode.GetType() == typeof(WorldBuilderMain))
        {
            (mode as WorldBuilderMain).ZoneChangeEvent(i);
        }
    }

    public void EndEvent()
    {
    }

    public void NextEvent()
    {
    }

    public void PlayerAction()
    {
        int i = 0;
        int.TryParse(tmp.text, out i);
        AdventureTester.Instance().PlayerZoneChangeEvent(i);
    }
}
