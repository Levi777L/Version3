//******//
//*Vive*//
//******//

using UnityEngine;
using System.Collections;
using System;

public class Wand : MonoBehaviour, IVRControl
{
    private Transform mhAnchor, ohAnchor;
    private ulong trigger = SteamVR_Controller.ButtonMask.Trigger;
    private ulong grip = SteamVR_Controller.ButtonMask.Grip;

    public void Init(GameManager m)
    {
        mhAnchor = m.mhAnchor.transform;
        ohAnchor = m.ohAnchor.transform;
    }

    public ControlerStyle GetControlStyle()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            return ControlerStyle.RiftTouch;
        }
        return ControlerStyle.ViveWand;
    }



    public Vector2 MH_Axis() { return GetMH().GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0); }
    public Vector2 OH_Axis() { return GetOH().GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0); }

    public bool MH_BtnDown()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            return GetMH().GetPressDown((1ul << (int)Valve.VR.EVRButtonId.k_EButton_A));
        }
        return (MH_PressDown() && MHAngleBetween(180f, 270f)) || GetMH().GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu);
    }
    public bool MH_BtnUp()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            return GetMH().GetPressUp((1ul << (int)Valve.VR.EVRButtonId.k_EButton_A));
        }
        return (MH_PressUp() && MHAngleBetween(180f, 270f)) || GetMH().GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu);
    }
    public bool MH_Btn()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            return GetMH().GetPress((1ul << (int)Valve.VR.EVRButtonId.k_EButton_A));
        }
        return (MH_Press() && MHAngleBetween(180f, 270f)) || GetMH().GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu);
    }

    public bool OH_BtnDown()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            return GetOH().GetPressDown((1ul << (int)Valve.VR.EVRButtonId.k_EButton_A));
        }
        return (OH_PressDown() && OHAngleBetween(180f, 270f)) || GetOH().GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu);
    }
    public bool OH_BtnUp()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            return GetOH().GetPressUp((1ul << (int)Valve.VR.EVRButtonId.k_EButton_A));
        }
        return (OH_PressUp() && OHAngleBetween(180f, 270f)) || GetOH().GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu);
    }
    public bool OH_Btn()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            return GetOH().GetPress((1ul << (int)Valve.VR.EVRButtonId.k_EButton_A));
        }
        return (OH_Press() && OHAngleBetween(180f, 270f)) || GetOH().GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu);
    }

    public bool MH_BtnTwoDown()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            return GetMH().GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu);
        }
        return MH_PressDown() && MHAngleBetween(0f, 90f);
    }
    public bool MH_BtnTwoUp()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            return GetMH().GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu);
        }
        return MH_PressUp() && MHAngleBetween(0f, 90f);
    }
    public bool MH_BtnTwo()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            return GetMH().GetPress(SteamVR_Controller.ButtonMask.ApplicationMenu);
        }
        return MH_Press() && MHAngleBetween(0f, 90f);
    }

    public bool OH_BtnTwoDown()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            return GetOH().GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu);
        }
        return OH_PressDown() && OHAngleBetween(0f, 90f);
    }
    public bool OH_BtnTwoUp()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            return GetOH().GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu);
        }
        return OH_PressUp() && OHAngleBetween(0f, 90f);
    }
    public bool OH_BtnTwo()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            return GetOH().GetPress(SteamVR_Controller.ButtonMask.ApplicationMenu);
        }
        return OH_Press() && OHAngleBetween(0f, 90f);
    }

    

    public bool MH_TriggerDown() { return GetMH().GetPressDown(trigger); }
    public bool MH_TriggerUp() { return GetMH().GetPressUp(trigger); }
    public bool MH_Trigger() { return GetMH().GetPress(trigger); }

    public bool OH_ShiftDown() { return GetOH().GetPressDown(grip); }
    public bool OH_ShiftUp() { return GetOH().GetPressUp(grip); }
    

    public bool MH_ShiftDown() { return GetMH().GetPressDown(grip); }
    public bool MH_ShiftUp() { return GetMH().GetPressUp(grip); }
    
    public bool OH_TriggerDown() { return GetOH().GetPressDown(trigger); }
    public bool OH_TriggerUp() { return GetOH().GetPressUp(trigger); }
    public bool OH_Trigger() { return GetOH().GetPress(trigger); }



    private bool mhuDown = false;
    public bool MH_UpBtnDown()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            if (MH_Axis().y > 0.65 && !mhuDown)
            {
                mhuDown = true;
                mhdDown = false;
                timer = 0f;
                return true;
            }
            return false;
        }
        return MH_PressDown() && MHAngleBetween(0f, 90f);
    }

    private bool mhrDown = false;
    public bool MH_RightBtnDown()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            if (MH_Axis().x > 0.65 && !mhrDown)
            {
                mhrDown = true;
                mhlDown = false;
                timer = 0f;
                return true;
            }
            return false;
        }
        return MH_PressDown() && MHAngleBetween(90f, 180f);
    }



    public bool MH_RightBtn()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            return MH_Axis().x > 0.65;
        }
        return MH_Press() && MHAngleBetween(90f, 180f);
    }

    private bool mhdDown = false;
    public bool MH_DownBtnDown()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            if (MH_Axis().y < -0.65 && !mhdDown)
            {
                mhdDown = true;
                mhuDown = false;
                timer = 0f;
                return true;
            }
            return false;
        }
        return MH_PressDown() && MHAngleBetween(180f, 270f);
    }

    private bool mhlDown = false;
    public bool MH_LeftBtnDown()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            if (MH_Axis().x < -0.65 && !mhlDown)
            {
                mhlDown = true;
                mhrDown = false;
                timer = 0f;
                return true;
            }
            return false;
        }
        return MH_PressDown() && MHAngleBetween(270f, 360f);
    }

    public bool MH_LeftBtn()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            return MH_Axis().x < -0.65;
        }
        return MH_Press() && MHAngleBetween(270f, 360f);
    }

    private bool ohuDown = false;
    public bool OH_UpBtnDown()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            if (OH_Axis().y > 0.65 && !ohuDown)
            {
                ohuDown = true;
                ohdDown = false;
                timer = 0f;
                return true;
            }
            return false;
        }
        return OH_PressDown() && OHAngleBetween(0f, 90f);
    }

    private bool ohrDown = false;
    public bool OH_RightBtnDown()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            if (OH_Axis().x > 0.65 && !ohrDown)
            {
                ohrDown = true;
                ohlDown = false;
                timer = 0f;
                return true;
            }
            return false;
        }
        return OH_PressDown() && OHAngleBetween(90f, 180f);
    }

    

    public bool OH_RightBtn()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            return OH_Axis().x > 0.65;
        }
        return OH_Press() && OHAngleBetween(90f, 180f);
    }

    private bool ohdDown = false;
    public bool OH_DownBtnDown()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            if (OH_Axis().y < -0.65 && !ohdDown)
            {
                ohdDown = true;
                ohuDown = false;
                timer = 0f;
                return true;
            }
            return false;
        }
        return OH_PressDown() && OHAngleBetween(180f, 270f);
    }

    private bool ohlDown = false;
    public bool OH_LeftBtnDown()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            if (OH_Axis().x < -0.65 && !ohlDown)
            {
                ohlDown = true;
                ohrDown = false;
                timer = 0f;
                return true;
            }
            return false;
        }
        return OH_PressDown() && OHAngleBetween(270f, 360f);
    }

    public bool OH_LeftBtn()
    {
        if (SteamVR.instance.hmd_TrackingSystemName == "oculus")
        {
            return OH_Axis().x < -0.65;
        }
        return OH_Press() && OHAngleBetween(270f, 360f);
    }

    private float timer = 0f;
    public void RefreshControl()
    {
        timer += Time.deltaTime;
        if (timer > 2f)
        {
            ohlDown = false;
            ohrDown = false;
            ohuDown = false;
            ohdDown = false;
            mhlDown = false;
            mhrDown = false;
            mhuDown = false;
            mhdDown = false;
        }

        try
        {
            if (OH_Axis().x == 0)
            {
                ohlDown = false;
                ohrDown = false;
            }

            if (MH_Axis().x == 0)
            {
                mhlDown = false;
                mhrDown = false;
            }
        }
        catch {
            ohlDown = false;
            ohrDown = false;
            mhlDown = false;
            mhrDown = false;
        }

        try
        {
            if (OH_Axis().y == 0)
            {
                ohdDown = false;
                ohuDown = false;
            }

            if (MH_Axis().y == 0)
            {
                mhuDown = false;
                mhdDown = false;
            }
        }
        catch
        {
            ohdDown = false;
            ohuDown = false;
            mhuDown = false;
            mhdDown = false;
        }
    }

    //Helper Fuctions
    private SteamVR_Controller.Device GetMH()
    {
        SteamVR_TrackedObject to;
        to = mhAnchor.gameObject.GetComponent<SteamVR_TrackedObject>();
        return SteamVR_Controller.Input((int)to.index);
    }

    private SteamVR_Controller.Device GetOH()
    {
        SteamVR_TrackedObject to;
        to = ohAnchor.gameObject.GetComponent<SteamVR_TrackedObject>();
        return SteamVR_Controller.Input((int)to.index);
    }

    private bool MH_PressDown()
    {
        return GetMH().GetPressDown(SteamVR_Controller.ButtonMask.Touchpad);
    }


    private bool MH_PressUp()
    {
        return GetMH().GetPressUp(SteamVR_Controller.ButtonMask.Touchpad);
    }

    private bool MH_Press()
    {
        if (GetControlStyle() == ControlerStyle.RiftTouch)
        {
            return true;
        }
        return GetMH().GetPress(SteamVR_Controller.ButtonMask.Touchpad);
    }

    private bool OH_PressDown()
    {
        return GetOH().GetPressDown(SteamVR_Controller.ButtonMask.Touchpad);
    }

    private bool OH_PressUp()
    {
        return GetOH().GetPressUp(SteamVR_Controller.ButtonMask.Touchpad);
    }

    private bool OH_Press()
    {
        if (GetControlStyle() == ControlerStyle.RiftTouch)
        {
            return true;
        }
        return GetOH().GetPress(SteamVR_Controller.ButtonMask.Touchpad);
    }

    private bool MHAngleBetween(float min, float max)
    {
        Vector2 fromVector2 = new Vector2(-1, 1);
        Vector2 toVector2 = MH_Axis();
        float ang = Vector2.Angle(fromVector2, toVector2);
        Vector3 cross = Vector3.Cross(fromVector2, toVector2);

        if (cross.z > 0)
            ang = 360 - ang;

        return ang >= min && ang < max;
    }

    private bool OHAngleBetween(float min, float max)
    {
        Vector2 fromVector2 = new Vector2(-1, 1);
        Vector2 toVector2 = OH_Axis();
        float ang = Vector2.Angle(fromVector2, toVector2);
        Vector3 cross = Vector3.Cross(fromVector2, toVector2);

        if (cross.z > 0)
            ang = 360 - ang;

        return ang >= min && ang < max;
    }

    //V2

    public bool RB()
    {
        if (LSH())
        {
            return false;
        }
        if (GetControlStyle() == ControlerStyle.RiftTouch)
        {
            return MH_BtnDown() || MH_TriggerDown();
        }
        return MH_TriggerDown();
    }

    public bool RBHold()
    {
        if (LSH())
        {
            return false;
        }
        if (GetControlStyle() == ControlerStyle.RiftTouch)
        {
            return MH_Btn() || MH_Trigger();
        }
        return MH_Trigger();
    }

    public bool LB()
    {
        if (LSH()) {
            return false;
        }
        if (GetControlStyle() == ControlerStyle.RiftTouch)
        {
            return OH_BtnDown() || OH_TriggerDown();
        }
        return OH_TriggerDown();
    }

    public bool RB2()
    {
        if (GetControlStyle() == ControlerStyle.RiftTouch)
        {
            return MH_BtnTwoDown() || ((MH_TriggerDown() || MH_BtnDown()) && LSH()) ;
        }
        return MH_TriggerDown() && LSH();
    }

    public bool RB2Lite()
    {
        if (GetControlStyle() == ControlerStyle.RiftTouch)
        {
            return MH_BtnTwoDown() || LSD() || OH_BtnTwoDown();
        }
        return LSD();
    }

    public bool LB2()
    {
        if (GetControlStyle() == ControlerStyle.RiftTouch)
        {
            return OH_BtnTwoDown() || ((OH_TriggerDown() || OH_BtnDown()) && LSH());
        }
        return OH_TriggerDown() && LSH();
    }

    public bool AR()
    {
        if (GetControlStyle() == ControlerStyle.RiftTouch)
        {
            return MH_BtnDown() || MH_BtnTwoDown() || MH_TriggerDown() ;
        }
        return MH_TriggerDown() || MH_PressDown();
    }

    public bool AL()
    {
        if (GetControlStyle() == ControlerStyle.RiftTouch)
        {
            return OH_BtnDown() || OH_BtnTwoDown() || OH_TriggerDown();
        }
        return OH_TriggerDown() || OH_PressDown();
    }
       

    public bool FMU()
    {
        return OH_UpBtnDown();
    }

    public bool FMR()
    {
        return OH_RightBtnDown();
    }

    public bool FMD()
    {
        return OH_DownBtnDown();

    }

    public bool FML()
    {
        return OH_LeftBtnDown();
    }

    public bool RHU()
    {
        return MH_UpBtnDown();
    }

    public bool RHR()
    {
        return MH_RightBtnDown();
    }

    public bool RHD()
    {
        return MH_DownBtnDown();
    }

    public bool RHL()
    {
        return MH_LeftBtnDown();
    }

    public float RHY()
    {
        if (Math.Abs(MH_Axis().y) < 0.65f || !MH_Press())
            return 0;
        return MH_Axis().y;
    }

    public float RHX()
    {
        if (Math.Abs(MH_Axis().x) < 0.65f || !MH_Press())
            return 0;
        return MH_Axis().x;
    }

    public Vector2 PMAxis()
    {
        return OH_Axis();
    }

    public bool PMExit()
    {
        if (GetControlStyle() == ControlerStyle.RiftTouch)
        {
            return OH_BtnDown() || OH_TriggerDown() || OH_BtnTwoDown();
        }
        return OH_TriggerDown();
    }

    public bool PMJump()
    {
        if (GetControlStyle() == ControlerStyle.RiftTouch)
        {
            return MH_BtnTwoDown();
        }
        return MH_TriggerDown();
    }

    public bool PMAction()
    {
        if (GetControlStyle() == ControlerStyle.RiftTouch)
        {
            return MH_BtnDown();
        }
        return MH_PressDown();
    }

    public bool RSH() { return GetMH().GetPress(grip); }
    public bool LSH() { return GetOH().GetPress(grip); }
    public bool RSD() { return MH_ShiftDown(); }
    public bool LSD() { return OH_ShiftDown(); }

    
}
