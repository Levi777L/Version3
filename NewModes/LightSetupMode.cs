using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSetupMode : IMode
{
    private static LightSetupMode instance;
    private static GameManager manager;
    private static IVRControl control;
    private static WorldBuilderMain shared;

    public static LightSetupMode Instance()
    {
        if (instance == null)
        {
            instance = new LightSetupMode();
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
        manager.mainPointer.GetComponent<Renderer>().enabled = false;

    }

    public void SoftUnload()
    {
        manager.mainPointer.GetComponent<Renderer>().enabled = true;
        manager.KeepActive(manager.builderObjects);

        PlayerPrefs.SetFloat("DayIntensity", TOD_Sky.Instance.Day.LightIntensity);
        PlayerPrefs.SetFloat("NightIntensity", TOD_Sky.Instance.Night.LightIntensity);
        PlayerPrefs.SetFloat("TimeOfDay", TOD_Sky.Instance.Cycle.Hour);
        PlayerPrefs.Save();
    }

    public void IUpdate()
    {
        manager.modeStringEnum = GameManager.ModeString.View_Mode;

    }

    public void IControlUpdate()
    {
        manager.modeStringEnum = GameManager.ModeString.Camera_Control;

        if (control.AL())
        {
            SelectObjectMode.Instance().SetupMode();
        }

        if (control.RHX() != 0)
        {
            float time = TOD_Sky.Instance.Cycle.Hour;

            time += control.RHX() * manager.lastDeltaTime * 3;
            if (time > 24)
                time -= 24;
            if (time < 0)
                time += 24;

            TOD_Sky.Instance.Cycle.Hour = time;
        }

        if (control.RHY() != 0)
        {
            float intensity = control.RHY() * manager.lastDeltaTime;
            if (TOD_Sky.Instance.IsDay)
            {
                intensity += TOD_Sky.Instance.Day.LightIntensity;
            }
            else
            {
                intensity += TOD_Sky.Instance.Night.LightIntensity;
            }

            if (intensity > 8)
                intensity = 8;
            else if (intensity < 0)
                intensity = 0;

            if (TOD_Sky.Instance.IsDay)
            {
                TOD_Sky.Instance.Day.LightIntensity = intensity;
                manager.SetToolTip("Day Light Intensity: " + intensity.ToString("0.00"));
            }
            else
            {
                TOD_Sky.Instance.Night.LightIntensity = intensity;
                manager.SetToolTip("Night Light Intensity: " + intensity.ToString("0.00"));
            }
        }
    }
}