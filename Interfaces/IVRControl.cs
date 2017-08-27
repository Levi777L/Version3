using UnityEngine;
using System.Collections;

public enum ControlerStyle
{
    RiftTouch,
    ViveWand,
}

public interface IVRControl
{
    //Any Left/Right Down Press (Menus)
    bool AR();
    bool AL();

    //Main Button Press (set main button with trigger bool later)
    bool RB();
    bool LB();
    bool RBHold();

    //Is left shift held and Main Button Pressed
    bool RB2();
    bool RB2Lite();
    bool LB2();
    
    //Is shift held
    bool LSH();
    bool LSD();
    bool RSH();
    bool RSD();

    //LeftDpad
    bool FMU();
    bool FMR();
    bool FMD();
    bool FML();

    //Right Axis
    float RHY();
    float RHX();
    bool RHU();
    bool RHR();
    bool RHD();
    bool RHL();


    Vector2 PMAxis();
    bool PMExit();
    bool PMJump();
    bool PMAction();

    ControlerStyle GetControlStyle();

    void Init(GameManager m);

    void RefreshControl();

    //Vector2 MH_Axis();
    //Vector2 OH_Axis();

    //bool MH_BtnDown();
    //bool MH_BtnUp();
    //bool MH_Btn();
    
    //bool OH_BtnDown();
    //bool OH_BtnUp();
    //bool OH_Btn();
    
    //bool MH_BtnTwoDown();
    //bool MH_BtnTwoUp();
    //bool MH_BtnTwo();
    
    //bool OH_BtnTwoDown();
    //bool OH_BtnTwoUp();
    //bool OH_BtnTwo();

    //bool MH_TriggerDown();
    //bool MH_TriggerUp();
    //bool MH_Trigger();
    
    //bool OH_ShiftDown();
    //bool OH_ShiftUp();

    //bool MH_ShiftDown();
    //bool MH_ShiftUp();
    
    //bool OH_TriggerDown();
    //bool OH_TriggerUp();
    //bool OH_Trigger();

    //bool MH_LeftBtnDown();
    //bool MH_LeftBtn();

    //bool MH_RightBtnDown();
    //bool MH_RightBtn();


    //bool OH_LeftBtnDown();
    //bool OH_LeftBtn();

    //bool OH_RightBtnDown();
    //bool OH_RightBtn();
 }
