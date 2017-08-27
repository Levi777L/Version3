using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeExample : IMode
{
    private static ModeExample instance;
    private static GameManager manager;
    private static IVRControl control;
    private static WorldBuilderMain shared;

    public static ModeExample Instance()
    {
        if (instance == null)
        {
            instance = new ModeExample();
            manager = SL.sl.Get<GameManager>();
            control = SL.sl.Get<IVRControl>();
            shared = SL.sl.Get<WorldBuilderMain>();
        }
        return instance;
    }

    public void ButtonActivate(Node n, bool shift = false)
    {
        WorldBuilderMain.Instance().ButtonActivate(n, shift);
    }

    public void SetupMode()
    {

    }

    public void SoftUnload()
    {

    }

    public void IUpdate()
    {

    }

    public void IControlUpdate()
    {
        manager.modeStringEnum = GameManager.ModeString.Camera_Control;
    }
}