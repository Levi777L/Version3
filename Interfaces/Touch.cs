////******//
////*Rift*//
//////******//

//using UnityEngine;
//using System.Collections;
//using System;

//public class Touch : MonoBehaviour, IVRControl
//{
//    private GameManager manager;

//    private OVRInput.Controller mhDevice;
//    private OVRInput.Controller ohDevice;

//    private OVRInput.Axis2D mhThumb;
//    private OVRInput.Axis2D ohThumb;

//    private OVRInput.Button trigger = OVRInput.Button.PrimaryIndexTrigger;
//    private OVRInput.Button grip = OVRInput.Button.PrimaryHandTrigger;
//    private bool swapGrip = false;

//    public void Init(GameManager m)
//    {
//        manager = m;
//        mhDevice = OVRInput.Controller.RTouch;
//        ohDevice = OVRInput.Controller.LTouch;
//    }
    
//    public void RefreshControl()
//    {

//    }

//    public ControlerStyle GetControlStyle() {
//        return ControlerStyle.RiftTouch;
//    }

//    public Vector2 MH_Axis() { return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, mhDevice); }
//    public Vector2 OH_Axis() { return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, ohDevice); }

//    public bool MH_BtnDown() { return OVRInput.GetDown(OVRInput.Button.One, mhDevice); }
//    public bool MH_BtnUp() { return OVRInput.GetUp(OVRInput.Button.One, mhDevice); }
//    public bool MH_Btn() { return OVRInput.Get(OVRInput.Button.One, mhDevice); }

//    public bool OH_BtnDown() { return OVRInput.GetDown(OVRInput.Button.One, ohDevice); }
//    public bool OH_BtnUp() { return OVRInput.GetUp(OVRInput.Button.One, ohDevice); }
//    public bool OH_Btn() { return OVRInput.Get(OVRInput.Button.One, ohDevice); }
       
//    public bool MH_BtnTwoDown() { return OVRInput.GetDown(OVRInput.Button.Two, mhDevice); }
//    public bool MH_BtnTwoUp() { return OVRInput.GetUp(OVRInput.Button.Two, mhDevice); }
//    public bool MH_BtnTwo() { return OVRInput.Get(OVRInput.Button.Two, mhDevice); }

//    public bool OH_BtnTwoDown() { return OVRInput.GetDown(OVRInput.Button.Two, ohDevice); }
//    public bool OH_BtnTwoUp() { return OVRInput.GetUp(OVRInput.Button.Two, ohDevice); }
//    public bool OH_BtnTwo() { return OVRInput.Get(OVRInput.Button.Two, ohDevice); }

//    public bool MH_TriggerDown() { return OVRInput.GetDown(trigger, mhDevice); }
//    public bool MH_TriggerUp() { return OVRInput.GetUp(trigger, mhDevice); }
//    public bool MH_Trigger() { return OVRInput.Get(trigger, mhDevice); }

//    public bool OH_ShiftDown() { return OVRInput.GetDown(grip, ohDevice); }
//    public bool OH_ShiftUp() { return OVRInput.GetUp(grip, ohDevice); }
//    public bool OH_Shift() { return OVRInput.Get(grip, ohDevice); }

//    public bool MH_ShiftDown() { return OVRInput.GetDown(grip, mhDevice); }
//    public bool MH_ShiftUp() { return OVRInput.GetUp(grip, mhDevice); }
//    public bool MH_Shift() { return OVRInput.Get(grip, mhDevice); }

//    public bool OH_TriggerDown() { return OVRInput.GetDown(trigger, ohDevice); }
//    public bool OH_TriggerUp() { return OVRInput.GetUp(trigger, ohDevice); }
//    public bool OH_Trigger() { return OVRInput.Get(trigger, ohDevice); }

//    public bool MH_LeftBtnDown() { return OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft, mhDevice); }
//    public bool MH_LeftBtn() { return OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft, mhDevice); }

//    public bool MH_RightBtnDown() { return OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight, mhDevice); }
//    public bool MH_RightBtn() { return OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight, mhDevice); }

//    public bool OH_LeftBtnDown() { return OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft, ohDevice); }
//    public bool OH_LeftBtn() { return OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft, ohDevice); }

//    public bool OH_RightBtnDown() { return OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight, ohDevice); }
//    public bool OH_RightBtn() { return OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight, ohDevice); }

//    public bool RBDown()
//    {
//        throw new NotImplementedException();
//    }



//    public bool LBDown()
//    {
//        throw new NotImplementedException();
//    }



//    public bool ShiftHeld()
//    {
//        throw new NotImplementedException();
//    }

//    public bool ARDown()
//    {
//        throw new NotImplementedException();
//    }

//    public bool ALDown()
//    {
//        throw new NotImplementedException();
//    }
//}
