using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreMode : IMode
{
    private static ExploreMode instance;
    private static GameManager manager;
    private static IVRControl control;
    private static WorldBuilderMain shared;

    public static ExploreMode Instance()
    {
        if (instance == null)
        {
            instance = new ExploreMode();
            manager = SL.Get<GameManager>();
            control = SL.Get<IVRControl>();
            shared = SL.Get<WorldBuilderMain>();
        }
        return instance;
    }

    public void ButtonActivate(Node n, bool shift = false)
    {
        WorldBuilderMain.Instance().ButtonActivate(n, shift);
    }

    public void SetupMode()
    {
        manager.mode.SoftUnload();
        manager.mode = instance;
        manager.KeepActive(null);
        WorldBuilderMain.selectedObject = null;

    }

    public void SoftUnload()
    {
        manager.KeepActive(manager.builderObjects);

    }

    public void IUpdate()
    {
        manager.modeStringEnum = GameManager.ModeString.View_Mode;

    }

    public void IControlUpdate()
    {
        manager.modeStringEnum = GameManager.ModeString.Camera_Control;

        if (control.AL()) {
            if (manager.titleActive)
            {
                manager.DestroyDioReset();
                TitleScreenMode.Instance().SetupMode();
                return;
            }
            SelectObjectMode.Instance().SetupMode();
        }

        float time = TOD_Sky.Instance.Cycle.Hour;

        time += control.RHX() * manager.lastDeltaTime * 3;
        if (time > 24)
            time -= 24;
        if (time < 0)
            time += 24;

        TOD_Sky.Instance.Cycle.Hour = time;
        
    }
}