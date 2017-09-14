using LPWAsset;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlaceObjectMode : IMode
{
    private static PlaceObjectMode instance;
    private static GameManager manager;
    private static IVRControl control;

    private static bool poseable = false;

    private static float savedAniFrame = 0f;


    public static PosLock posLock = PosLock.Free;
    public enum PosLock
    {
        Free,
        Grid,
        RotLock,
    }


    public static PlaceObjectMode Instance()
    {
        if (instance == null)
        {
            instance = new PlaceObjectMode();
            manager = SL.Get<GameManager>();
            control = SL.Get<IVRControl>();
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

        manager.mainPointer.GetComponent<Renderer>().enabled = false;
        poseable = WorldBuilderMain.selectedBI.HasAnimation;

        if (poseable)
        {
            manager.controlList.text = Globals.PLACE_CONTROL_2;
        }
        else {
            manager.controlList.text = Globals.PLACE_CONTROL;
        }
    }

    public void SoftUnload()
    {
        manager.mainPointer.GetComponent<Renderer>().enabled = true;
        manager.controlList.text = "";

    }

    public void IUpdate()
    {
        if (manager.Diorama.activeInHierarchy && WorldBuilderMain.selectedObject)
        {
            UpdateRotation();
            UpdatePosition();
        }
    }

    public void IControlUpdate()
    {
        manager.modeStringEnum = GameManager.ModeString.Place_Object;

        if (control.RB() && !poseable)
        {
            StampCopy();
            return;
        }

        if (control.RB() && poseable) {
            PoserMode.Instance().SetupMode();
            return;
        }
        
        if (control.LB())
        {
            DestroySelectedObject();
            return;
        }

        if (control.RB2()) {
            DropObject();
            return;
        }

        if (!poseable && control.RHR())
        {
            RotateObjectCW();
            return;
        }

        if (!poseable && posLock == PosLock.Grid && control.RHL())
        {
            RotateObjectCCW();
            return;
        }

        if (control.LB2()) {
            UndoPickup();
            return;
        }

        if (control.RHY() != 0) { 
            UpdateScale();  
        }

        AnimFramSelect();
    }

    private void UpdateRotation()
    {
        if (posLock == PosLock.Grid)
        {
            WorldBuilderMain.selectedObject.transform.localRotation = Quaternion.Euler(new Vector3(WorldBuilderMain.objAngleX, WorldBuilderMain.objAngleY, WorldBuilderMain.objAngleZ));
        }
        else if (posLock == PosLock.RotLock)
        {
            var forward = manager.mhObjectPoint.transform.forward;
            forward.y = 0;
            manager.mhObjectPoint.transform.forward = forward;
            WorldBuilderMain.selectedObject.transform.rotation = manager.mhObjectPoint.transform.rotation * Quaternion.Euler(new Vector3(0, WorldBuilderMain.objAngleY, 0));
        }
        else
        {
            WorldBuilderMain.selectedObject.transform.localRotation =
                Quaternion.Inverse(manager.Diorama.transform.rotation) *
                manager.mhObjectPoint.rotation *
                Quaternion.Euler(new Vector3(0, WorldBuilderMain.objAngleY, 0));
        }

    }

    public void UpdatePosition()
    {
        Vector3 endPos;
        if (posLock == PosLock.Grid)
        {
            float multi = WorldBuilderMain.gridSize * WorldBuilderMain.objectScale;
            endPos = manager.Diorama.transform.InverseTransformPoint(manager.mhObjectPoint.position);
            endPos.x = (Mathf.RoundToInt(endPos.x / multi) * multi);
            endPos.z = (Mathf.RoundToInt(endPos.z / multi) * multi);

            if (WorldBuilderMain.savedYPos == float.MaxValue)
            {
                //    endPos.y = (Mathf.RoundToInt(endPos.y / (multi / 4)) * (multi / 4));
                float multiGuess = (multi <= 5f ? 0.5f : (multi / 4f));
                endPos.y = (Mathf.RoundToInt(endPos.y / multiGuess) * multiGuess);
            }
            else
            {
                endPos.y = WorldBuilderMain.savedYPos;
            }

        }
        else
        {
            endPos = manager.Diorama.transform.InverseTransformPoint(manager.mhObjectPoint.position);
            if (WorldBuilderMain.savedYPos != float.MaxValue)
            {
                endPos.y = WorldBuilderMain.savedYPos;
            }
        }
        
        WorldBuilderMain.selectedObject.transform.localPosition = endPos;
        WorldBuilderMain.selectedObject.transform.position += WorldBuilderMain.offsetPosition;

    }

    private void UpdateScale() {
        IEventScale ies = WorldBuilderMain.selectedObject.GetComponent<IEventScale>();
        if (ies != null)
        {
            if (control.RHU())
            {
                ies.ScaleUp();
            }

            if (control.RHD())
            {
                ies.ScaleDown();
            }
        }
        else { UpdateObjectScaleV3(); }
    }

    private void UpdateObjectScaleV3()
    {

            WorldBuilderMain.objectScale = Mathf.Abs(WorldBuilderMain.objectScale + ((manager.lastDeltaTime) * WorldBuilderMain.objectScale * control.RHY()));
            if (Mathf.Round(WorldBuilderMain.objectScale * 100) / 100f > 2)
                WorldBuilderMain.objectScale = Mathf.Round(WorldBuilderMain.objectScale * 100) / 100f;

            WorldBuilderMain.selectedObject.transform.localScale = new Vector3(WorldBuilderMain.objectScale, WorldBuilderMain.objectScale, WorldBuilderMain.objectScale);
        manager.SetToolTip("Object Scale: " + WorldBuilderMain.objectScale.ToString("0%"));
    }

    private void UndoPickup() {
        if (WorldBuilderMain.lastObjectPosition != Vector3.zero)
        {
            WorldBuilderMain.selectedObject.transform.localPosition = WorldBuilderMain.lastObjectPosition;
            WorldBuilderMain.selectedObject.transform.localRotation = WorldBuilderMain.lastObjectRotation;
            DropObject();
        }
    }

    private void StampCopy()
    {
        TextMeshPro tmp = WorldBuilderMain.selectedObject.GetComponentInChildren<TextMeshPro>();
        if (tmp)
        {
            DropObject();
            return;
        }

        LowPolyWaterScript lpws = WorldBuilderMain.selectedObject.GetComponentInChildren<LowPolyWaterScript>();
        if (lpws != null)
        {
            //lpws.sun = manager.dioramaLight.GetComponent<Light>();
        }

        GameObject go = Common.CopyGo(WorldBuilderMain.selectedObject, new Vector3(WorldBuilderMain.objectScale, WorldBuilderMain.objectScale, WorldBuilderMain.objectScale), manager.currentModeParent, false, false);
        DioramaObject d = go.GetComponent<DioramaObject>();
        if (WorldBuilderMain.currentLayer != 0)
        {
            d.layer = WorldBuilderMain.currentLayer;
        }
        else {
            d.layer = WorldBuilderMain.selectedObject.GetComponent<DioramaObject>().layer;
        }

        DestroyAnimator();
    }

    private void DropObject()
    {
        UnlockY();
        DestroyAnimator();

        WorldBuilderMain.ChooseSelectionModeType();
    }

    private void DestroyAnimator() {
        Animator ani = WorldBuilderMain.selectedObject.GetComponent<Animator>();
        if (ani)
        {
            ani.speed = 0;
            ani.Play(ani.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, savedAniFrame);
            manager.GMDestroy(ani);
        }
    }

    //Not Used
    private void LockYPos()
    {
        WorldBuilderMain.savedYPos = WorldBuilderMain.selectedObject.transform.localPosition.y;
    }

    private void UnlockY() {
        WorldBuilderMain.savedYPos = float.MaxValue;
    }


    private void DestroySelectedObject()
    {
        DioramaObject d = WorldBuilderMain.selectedObject.GetComponent<DioramaObject>();
        manager.DestoryObject(WorldBuilderMain.selectedObject);
        WorldBuilderMain.ChooseSelectionModeType();
    }

    private void RotateObjectCW()
    {
        WorldBuilderMain.objAngleY += 90;
        if (WorldBuilderMain.objAngleY > 360)
            WorldBuilderMain.objAngleY -= 360;
        WorldBuilderMain.selectedObject.transform.localRotation *= Quaternion.Euler(new Vector3(0, 90, 0));
    }

    private void RotateObjectCCW()
    {
        WorldBuilderMain.objAngleY -= 90;
        if (WorldBuilderMain.objAngleY < 0)
            WorldBuilderMain.objAngleY += 360;
        WorldBuilderMain.selectedObject.transform.localRotation *= Quaternion.Euler(new Vector3(0, -90, 0));
    }

    private void AnimFramSelect() {
        if (poseable)
        {
            Animator myAnimator = WorldBuilderMain.selectedObject.GetComponent<Animator>();
            if (myAnimator)
            {
                var test = myAnimator.GetCurrentAnimatorClipInfo(0);
                savedAniFrame += ((manager.lastDeltaTime / test[0].clip.length) * 3 * control.RHX() * Mathf.Abs(control.RHX()));
                if (savedAniFrame > 1)
                {
                    savedAniFrame = 0;
                }
                else if (savedAniFrame < 0)
                {
                    savedAniFrame = 1f;
                }

                myAnimator.speed = 0;
                myAnimator.Play(myAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, savedAniFrame);
                return;
            }
        }
    }
   

}
