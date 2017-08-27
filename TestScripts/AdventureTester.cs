//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using TMPro;
//using UnityEngine;

//public class AdventureTester : IGameMode
//{
//    private GameObject chatBubble;
//    int eventID = 0;

////    public Adventure adventure;
//    public NewBuilderMode abm;

//    GameManager manager;
//    IVRControl control;

//    public bool moveCamera = false;

//    private Vector3 dioStartPos;
//    private Vector3 startPos;
//    private Vector3 endPos;
//    private Vector3 lastPos = Vector3.one;
//    int dungeonSize = 100;

//    private GameObject character;

//    Vector3 camOffset = Vector3.zero;
//    private CamType ct = CamType.Char;
//    public enum CamType
//    {
//        Unlocked,
//        Locked,
//        Char,
//    }

//    public void Unload()
//    {

//        manager.quickRoom.SetActive(false);
//        manager.cc.gameObject.SetActive(false);
//        manager.fileManager.gameObject.SetActive(true);
//    }

//    public void Init(GameManager m, int submode = 0)
//    {
//        manager = m;
//        control = m.control;

//        //Only show grid on static size scenes
//        m.grid.SetActive(true);
//        manager.quickRoom = new GameObject();
//        manager.quickRoom.transform.parent = manager.grid.transform;
//        manager.quickRoom.transform.localPosition = Vector3.zero;
       
//        GameObject go = MonoBehaviour.Instantiate(manager.characters[0]);
//        manager.cc = go.GetComponent<RPGCharacterControllerFREE>();

//        manager.cc.gameObject.SetActive(true);
//        manager.cc.enabled = true;
//        manager.cc.gameObject.GetComponent<Animator>().enabled = true;


//        manager.cc.gameObject.transform.parent = manager.diorama.transform;
//        manager.cc.gameObject.transform.localPosition = new Vector3(0, .45f, 0);
//        manager.cc.control = control;
//        manager.cc.manager = manager;

//        SetTag(manager.cc.transform, "TableObject", LayerMask.NameToLayer("PlayTable"));
//        SetTag(manager.grid.transform, "TableObject", LayerMask.NameToLayer("PlayTable"));

//        manager.fileManager.gameObject.SetActive(false);
//        HelpText();

//        camOffset = manager.cc.gameObject.transform.position - manager.rig.transform.position;

//        character = go;

//        chatBubble = MonoBehaviour.Instantiate(Resources.Load("ChatBubble", typeof(GameObject))) as GameObject;
//        chatBubble.transform.parent = manager.diorama.transform;
//    }

//    private void HelpText()
//    {
//        manager.rightControls.text = Globals.BuildString(manager.rb1, "Punch", manager.rb2, "Kick", "Hold Grip + Analog", "Move/Zoom/Rotate Map");
//        manager.leftControls.text = Globals.BuildString(manager.lb1, "Exit Mode", "Analog", "Move");
//    }


//    public void IUpdate()
//    {
//        if (control.OH_TriggerDown())
//        {
//            if (ct == CamType.Unlocked)
//                ct = CamType.Locked;
//            else if (ct == CamType.Locked)
//                ct = CamType.Char;
//            else
//                ct = CamType.Unlocked;
//        }


//        if (ct == CamType.Char && !control.MH_Shift())
//        {
//            manager.rig.transform.position = manager.cc.gameObject.transform.position - camOffset;
//        }

//        if (control.OH_BtnDown())
//        {
//            manager.mode = abm;
//            manager.GMDestroy(character);
//            manager.grid.SetActive(true);
//            manager.cc.gameObject.SetActive(false);
//            manager.fileManager.gameObject.SetActive(true);
//            manager.ClearDiorama();

//        }

//        //ScalePanDio();
//        ZoomToChar();

//        float multix = 36;
//        float multiz = 28;
//        float multiy = 1;

//        //endPos = manager.diorama.transform.InverseTransformPoint(manager.mainPointer.position);
//        endPos = manager.cc.gameObject.transform.localPosition;
//        endPos.x = (Mathf.RoundToInt(endPos.x / multix) * multix);
//        endPos.z = (Mathf.RoundToInt(endPos.z / multiz) * multiz);

//        //To Do: Zone up needs thinking
//        //endPos.y = (Mathf.RoundToInt(endPos.y / multiy) * multiy);
//        endPos.y = 0;

//        Vector3 camRelPos = manager.grid.transform.InverseTransformPoint(manager.rig.transform.position);
//        manager.grid.transform.localPosition = endPos;

//        endPos.x /= multix;
//        endPos.z /= multiz;

//        if (endPos != lastPos)
//        {
//            LoadZoneById((int)endPos.x, (int)endPos.y, (int)endPos.z);
//            if (ct == CamType.Locked)
//            {
//                manager.rig.transform.position = manager.grid.transform.TransformPoint(camRelPos);
//            }
//            lastPos = endPos;
//        }

//        manager.currentLayer.text = endPos.ToString();

//    }

//    private void SetTag(Transform t, string tag, int layer)
//    {
//        if (t.tag == "Skip")
//            return;

//        t.gameObject.tag = tag;
//        t.gameObject.layer = layer;
//        foreach (Transform c in t)
//        {
//            SetTag(c, tag, layer);
//        }
//    }

//    private void ZoomToChar()
//    {
//        Vector3 startPointWorld = manager.cc.transform.position;
//        Vector3 startPointLocal = manager.diorama.transform.InverseTransformPoint(startPointWorld);


//        if (control.MH_ShiftDown() && !control.OH_Shift())
//        {
//            startPos = manager.mhSphereCastPoint.position;
//            dioStartPos = manager.diorama.transform.position;
//        }

//        if (control.MH_Shift() && !control.MH_Trigger())
//        {
//            manager.ScaleDiorama(manager.dioramaScale);

//            Vector3 moved = manager.mhSphereCastPoint.position - startPos;


//            manager.dioramaScale = Mathf.Abs(manager.dioramaScale + (-control.MH_Axis().y));


//            //foreach (LightObject lo in dioramaLights)
//            //{
//            //    lo.Scale(manager.dioramaScale);
//            //}


//            manager.diorama.transform.localRotation *= Quaternion.Euler(new Vector3(0, .45f * control.MH_Axis().x, 0));


//            Vector3 endPointLocalToWorld = manager.diorama.transform.TransformPoint(startPointLocal);
//            Vector3 difference = startPointWorld - endPointLocalToWorld;
//            manager.diorama.transform.position = manager.diorama.transform.position + difference + moved;
//            startPos = manager.mhSphereCastPoint.position;
//        }

//        if (control.MH_ShiftUp() && !control.MH_Trigger())
//        {

//        }
//        camOffset = manager.cc.gameObject.transform.position - manager.rig.transform.position;
//    }

//    private void ScalePanDio()
//    {
//        Vector3 startPointWorld = manager.mhSphereCastPoint.position;
//        Vector3 startPointLocal = manager.diorama.transform.InverseTransformPoint(startPointWorld);


//        if (control.MH_ShiftDown() && !control.OH_Shift())
//        {
//            startPos = manager.mhSphereCastPoint.position;
//            dioStartPos = manager.diorama.transform.position;
//        }

//        if (control.MH_Shift() && !control.MH_Trigger())
//        {
//            manager.ScaleDiorama(manager.dioramaScale);

//            Vector3 moved = manager.mhSphereCastPoint.position - startPos;

//            manager.dioramaScale = Mathf.Abs(manager.dioramaScale + ((Time.deltaTime) * manager.dioramaScale * -control.MH_Axis().y));


//            //foreach (LightObject lo in dioramaLights)
//            //{
//            //    lo.Scale(manager.dioramaScale);
//            //}


//            manager.diorama.transform.localRotation *= Quaternion.Euler(new Vector3(0, 45 * Time.deltaTime * control.MH_Axis().x, 0));


//            Vector3 endPointLocalToWorld = manager.diorama.transform.TransformPoint(startPointLocal);
//            Vector3 difference = startPointWorld - endPointLocalToWorld;
//            manager.diorama.transform.position = manager.diorama.transform.position + difference + moved;
//            startPos = manager.mhSphereCastPoint.position;
//        }

//        if (control.MH_ShiftUp() && !control.MH_Trigger())
//        {
//        }

//    }

////    Zone currentZone;
//    public void LoadZoneById(int x, int y, int z, int w = 0)
//    {
//        manager.GMDestroy(manager.quickRoom);
//        manager.quickRoom = new GameObject();
//        manager.quickRoom.transform.parent = manager.grid.transform;
//        manager.quickRoom.transform.localPosition = Vector3.zero;

////        currentZone = adventure.zones.Find(zone => zone.w == w && zone.x == x && zone.y == y && zone.z == z);

//        //if (currentZone != null)
//        //{
//        //    manager.StartCoroutine(LoadDioAsync());
//        //    manager.leftHandLoger.text = "Zone Loaded";
//        //}
//        //else {
//        //    currentZone = new Zone(w, x, y, z);
//        //}
//    }


//}
