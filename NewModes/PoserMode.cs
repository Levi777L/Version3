using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoserMode : IMode
{
    private static PoserMode instance;
    private static GameManager manager;
    private static IVRControl control;

    private static GameObject poserWidget;
    private static Transform[] allPoints;
    private static bool held = false;
    private static Transform parent;

    public static PoserMode Instance()
    {
        if (instance == null)
        {
            instance = new PoserMode();
            manager = SL.Get<GameManager>();
            control = SL.Get<IVRControl>();
            poserWidget = manager.poserWidget;
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

        held = false;
        poserWidget.SetActive(true);

        allPoints = WorldBuilderMain.selectedObject.GetComponentsInChildren<Transform>();
        Animator ani = WorldBuilderMain.selectedObject.GetComponent<Animator>();
        if (ani)
        {
            manager.GMDestroy(ani);
        }

        manager.controlList.text = Globals.POSE_CONTROL;

        manager.SetToolTip("Entering Pose Mode");
    }

    public void SoftUnload()
    {
        manager.SetToolTip("Leaving Pose Mode");
        poserWidget.transform.parent = null;
        poserWidget.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        poserWidget.SetActive(false);
        manager.controlList.text = "";
    }

    public void IUpdate()
    {
        ResetWidgetScale();
        CycleSoftSelected();
    }

    public void IControlUpdate()
    {
        manager.modeStringEnum = GameManager.ModeString.Pose_Detail;

        if (control.LB())
        {
            DropObject();
            SelectObjectMode.Instance().SetupMode();
            return;
        }

        if (control.RB())
        {
            SetPointerRotOnStart();
            return;
        }

        if (control.RB2Lite() && held)
        {
            DropObject();
            return;
        }

        if (control.RB2Lite() && !held)
        {
            PickupObjectLeft();
            return;
        }

        if (control.RBHold() && poserWidget.transform.parent)
        {
            RotateSelectedJoint();
            return;
        }
    }

    public static void DropObject()
    {
        if (held)
        {
            WorldBuilderMain.selectedObject.transform.parent = parent;
            held = false;
        }
    }

    private static void PickupObjectLeft()
    {
        parent = WorldBuilderMain.selectedObject.transform.parent;
        WorldBuilderMain.selectedObject.transform.parent = manager.ohAnchor;
        held = true;
    }

    private static void ResetWidgetScale()
    {
        poserWidget.transform.localScale = Vector3.one;
        poserWidget.transform.localScale = new Vector3(2.5f / poserWidget.transform.lossyScale.x, 2.5f / poserWidget.transform.lossyScale.y, 2.5f / poserWidget.transform.lossyScale.z);
    }

    private void SetPointerRotOnStart()
    {
        manager.mainPointer.rotation = poserWidget.transform.parent.rotation;
    }

    private void RotateSelectedJoint()
    {

        poserWidget.transform.parent.rotation = manager.mainPointer.rotation;

    }

    private void CycleSoftSelected()
    {
        if (!control.RBHold())
        {
            Vector3 main = manager.mainPointer.position;
            Transform closest;
            float dist = float.MaxValue;
            foreach (Transform t in allPoints)
            {
                if (Vector3.Distance(main, t.position) < dist)
                {
                    closest = t;
                    dist = Vector3.Distance(main, t.position);
                    poserWidget.transform.parent = t;
                    poserWidget.transform.localPosition = Vector3.zero;
                }


            }
        }
    }


}
