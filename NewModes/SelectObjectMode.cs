using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObjectMode : IMode
{
    private static SelectObjectMode instance;
    private static GameManager manager;
    private static IVRControl control;

    private static Dictionary<Renderer, Material[]> lastMats = new Dictionary<Renderer, Material[]>();
    private static Dictionary<SkinnedMeshRenderer, Material[]> lastMats2 = new Dictionary<SkinnedMeshRenderer, Material[]>();
    private static GameObject softSelected = null;

    private static List<GameObject> ignore = new List<GameObject>();

    public static SelectObjectMode Instance()
    {
        if (instance == null)
        {
            instance = new SelectObjectMode();
            manager = SL.sl.Get<GameManager>();
            control = SL.sl.Get<IVRControl>();
        }
        return instance;
    }

    public void ButtonActivate(Node n, bool shift = false)
    {
        WorldBuilderMain.Instance().ButtonActivate(n, shift);
    }

    public void SetupMode() {
        manager.mode.SoftUnload();
        manager.mode = instance;

        WorldBuilderMain.lastObjectPosition = Vector3.zero;

        manager.activeFM.gameObject.SetActive(true);
        WorldBuilderMain.selectedObject = null;

        manager.controlList.text = Globals.SELECT_CONTROL;
        ignore.Clear();
    }

    public void SoftUnload()
    {
        manager.activeFM.gameObject.SetActive(false);
        manager.controlList.text = "";
        ignore.Clear();

    }

    public void IUpdate()
    {
        SoftSelect();
    }

    public void IControlUpdate() {

        manager.modeStringEnum = GameManager.ModeString.View_Mode;

        if (control.LB() && softSelected) {
            ignore.Add(softSelected);
        }

        if (control.RB())
        {
            if (softSelected)
            {
                PickupSelected();
                WorldBuilderMain.ChoosePlaceMode();
            }
            return;
        }

        if (control.RHY() != 0) {
            Vector3 pos = manager.pointerGroup.localPosition;
            pos.y -= control.RHY() * Time.deltaTime / 4f;
            pos.z += control.RHY() * Time.deltaTime / 4f;
            manager.pointerGroup.localPosition = pos;
        }


        if (control.RB2()) {
            if (softSelected)
            {
                if (TODV2.GetItemByID(softSelected.GetComponent<DioramaObject>().todID).hasAnimation)
                {
                    PickupSelected();
                    PoserMode.Instance().SetupMode();
                    return;
                }

                IWorldEvent iwe = softSelected.GetComponent<IWorldEvent>();
                if (iwe != null)
                {
                    iwe.StartEvent(WorldBuilderMain.Instance());
                    return;
                }
            }
        }
    }

    public static void SoftReset() {
        foreach (KeyValuePair<Renderer, Material[]> entry in lastMats) {
            try
            {
                entry.Key.materials = entry.Value;
            }
            catch {

            }
        }

        foreach (KeyValuePair<SkinnedMeshRenderer, Material[]> entry in lastMats2)
        {
            try
            {
                entry.Key.materials = entry.Value;
            }
            catch
            {

            }
        }

        lastMats.Clear();
        lastMats2.Clear();
        softSelected = null;
    }

    private static DioramaObject GetTopDO(Collider c) {
        Transform t = c.gameObject.transform;
        do
        {
            if (t.gameObject.GetComponent<DioramaObject>())
                return t.gameObject.GetComponent<DioramaObject>();
            t = t.parent;
        } while (t.parent);

        return default(DioramaObject);
    }

    public static void SoftSelect()
    {
        Collider[] hits = Physics.OverlapSphere(manager.mainPointer.position, manager.dioramaScale / 100f);

        float nearest = float.MaxValue; 

        foreach(Collider h in hits)
        {
            DioramaObject d = GetTopDO(h);
            if (d && (WorldBuilderMain.currentLayer == 0 || d.layer == WorldBuilderMain.currentLayer || d.layer == 0))
            {
                GameObject go = d.gameObject;

                if (ignore.Contains(go)) {
                    continue;
                }

                float check = Vector3.Distance(manager.mainPointer.position, go.transform.position);

                if (check < nearest)
                {
                    nearest = Vector3.Distance(manager.mainPointer.position, go.transform.position);
                    softSelected = go;

                }
            }
        }
        if (softSelected)
        {
            SkinnedMeshRenderer[] smr = softSelected.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer sr in smr) {
                lastMats2.Add(sr, sr.materials);
                sr.material = manager.nodeS;
            }

            Renderer[] rend = softSelected.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rend)
            {
                lastMats.Add(r, r.materials);
                r.material = manager.nodeS;
            }
        }
    }

     private void PickupSelected()
    {
       
            DioramaObject d = softSelected.GetComponent<DioramaObject>();

        manager.mhObjectPoint.position = softSelected.transform.position;
            //WorldBuilderMain.offsetPosition = softSelected.transform.position - manager.mhObjectPoint.position;

            WorldBuilderMain.selectedObject = softSelected;
            WorldBuilderMain.lastObjectPosition = WorldBuilderMain.selectedObject.transform.localPosition;
            WorldBuilderMain.lastObjectRotation = WorldBuilderMain.selectedObject.transform.localRotation;

            manager.mhObjectPoint.transform.rotation = WorldBuilderMain.selectedObject.transform.rotation;
            WorldBuilderMain.objectScale = softSelected.transform.localScale.x;

    }
}
