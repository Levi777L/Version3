using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePanMode : IMode
{
    private static ScalePanMode instance;
    private static GameManager manager;
    private static IVRControl control;
    private static WorldBuilderMain shared;

    private static Vector3 startPos;

    public static ScalePanMode Instance()
    {
        if (instance == null)
        {
            instance = new ScalePanMode();
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

        if (manager.Diorama)
        {
            Vector3 startPointWorld = manager.mainPointer.position;

            Vector3 startPointLocal = manager.Diorama.transform.InverseTransformPoint(startPointWorld);


            if (control.RSD())
            {
                //set on first frame
                startPos = manager.mainPointer.position;
            }

            manager.ScaleDiorama(manager.dioramaScale);

            Vector3 moved = manager.mainPointer.position - startPos;


            manager.dioramaScale = Mathf.Abs(manager.dioramaScale + ((manager.lastDeltaTime) * manager.dioramaScale * -control.RHY()));

            manager.Diorama.transform.localRotation *= Quaternion.Euler(new Vector3(0, 45 * manager.lastDeltaTime * control.RHX(), 0));

            Vector3 endPointLocalToWorld = manager.Diorama.transform.TransformPoint(startPointLocal);
            Vector3 difference = startPointWorld - endPointLocalToWorld;
            manager.Diorama.transform.position = manager.Diorama.transform.position + difference + moved;
            startPos = manager.mainPointer.position;

            manager.SetToolTip("World Scale: 1/" + manager.rig.transform.localScale.x.ToString("0.0"));
            if (manager.dioramaScale < 0.5f)
                manager.dioramaScale = 0.5f;
            if (manager.dioramaScale > 250f)
                manager.dioramaScale = 250f;
        }


    }

}
