using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrationMode : IMode
{
    private static CalibrationMode instance;
    private static GameManager manager;
    private static IVRControl control;
    private static WorldBuilderMain shared;

    public static CalibrationMode Instance()
    {
        if (instance == null) {
            instance = new CalibrationMode();
            manager = SL.sl.Get<GameManager>();
            control = SL.sl.Get<IVRControl>();
            shared = SL.sl.Get<WorldBuilderMain>();
        }
        return instance;
    }

    public void IUpdate()
    {
    }

    public void IControlUpdate()
    {
        manager.modeStringEnum = GameManager.ModeString.Calibration;
        //if (control.RB2()) {
        //    if (manager.pointerGroup.parent == null)
        //    {
        //        manager.pointerGroup.parent = manager.mhAnchor;
        //    }
        //    else {
        //        manager.pointerGroup.parent = null;
        //    }
        //}
    }

    public void ButtonActivate(Node n, bool shift = false)
    {
        manager.startPoint.position = n.transform.position;
        manager.startPoint.rotation = n.transform.rotation;
        Vector3 forward = manager.startPoint.forward;
        forward.y = 0;
        manager.startPoint.forward = forward;

        PlayerPrefs.SetFloat("StartPosX", manager.startPoint.position.x);
        PlayerPrefs.SetFloat("StartPosY", manager.startPoint.position.y);
        PlayerPrefs.SetFloat("StartPosZ", manager.startPoint.position.z);

        PlayerPrefs.SetFloat("StartRotX", manager.startPoint.rotation.x);
        PlayerPrefs.SetFloat("StartRotY", manager.startPoint.rotation.y);
        PlayerPrefs.SetFloat("StartRotZ", manager.startPoint.rotation.z);
        PlayerPrefs.SetFloat("StartRotW", manager.startPoint.rotation.w);

        PlayerPrefs.Save();

        TitleScreenMode.Instance().SetupMode();
    }

    public void SetupMode() {
        manager.mode.SoftUnload();
        manager.mode = instance;

        manager.titleActive = false;

        manager.KeepActive(manager.calibration);
        manager.leftText.text = "Welcome to Diorama Worlds.\n" +
                                "--------- Calibrate Steps ---------\n" +
                                "Step 1: Locate the 3d cursor on your right hand.\n" +
                                "Step 2: Stand up straight or sit in a comfortable position.\n" +
                                "Step 3: Move the cursor to the button and press the Right Trigger.";
    }

    public void SoftUnload()
    {
        manager.leftText.text = "";
    }

}
