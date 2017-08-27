//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class DungeonCrawlExperiment : IGameMode
//{
//    GameManager manager;
//    IVRControl control;

//    public bool moveCamera = false;

//    private Vector3 dioStartPos;
//    private Vector3 startPos;
//    private Vector3 endPos;
//    private Vector3 lastPos = Vector3.one;
//    DungeonGrid dungeon;
//    int dungeonSize = 100;
//    int wrongPaths = 1;
//    int seed = 55;
//    int maxPath = 100;

//    Vector3 camOffset = Vector3.zero;
//    private CamType ct = CamType.Char;
//    public enum CamType {
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

//        if (manager.unload)
//        {
//            m.ClearDiorama();
//        }

//        //Only show grid on static size scenes
//        m.grid.SetActive(true);
//        manager.quickRoom = manager.grid;

//        System.Random r = new System.Random();
        
//        dungeon = new DungeonGrid(dungeonSize, seed, wrongPaths, maxPath);

//        GameObject go = MonoBehaviour.Instantiate(manager.characters[0]);
//        manager.cc = go.GetComponent<RPGCharacterControllerFREE>();

//       // manager.quickRoom.SetActive(true);
//        manager.cc.gameObject.SetActive(true);
//        manager.cc.enabled = true;
//        manager.cc.gameObject.GetComponent<Animator>().enabled = true;

//        //manager.quickRoom.transform.parent = manager.diorama.transform;
//        //manager.quickRoom.transform.localScale = new Vector3(.2f, .2f, .2f);
//        //manager.quickRoom.transform.localPosition = Vector3.zero;

//        manager.cc.gameObject.transform.parent = manager.diorama.transform;
//        //manager.cc.gameObject.transform.localScale = new Vector3(.3f, .3f, .3f);
//        manager.cc.gameObject.transform.localPosition = new Vector3(0, .45f, 0);
//        manager.cc.control = control;
//        manager.cc.manager = manager;

//        SetTag(manager.cc.transform, "TableObject", LayerMask.NameToLayer("PlayTable"));
//        SetTag(manager.grid.transform, "TableObject", LayerMask.NameToLayer("PlayTable"));

//        manager.fileManager.gameObject.SetActive(false);
//        HelpText();

//        camOffset = manager.cc.gameObject.transform.position - manager.rig.transform.position;
//    }

//    private void HelpText()
//    {
//        manager.rightControls.text = Globals.BuildString(manager.rb1, "Punch", manager.rb2, "Kick", "Hold Grip + Analog", "Move/Zoom/Rotate Map");
//        manager.leftControls.text = Globals.BuildString(manager.lb1, "Exit Mode", "Analog", "Move");
//    }


//    public void IUpdate()
//    {
//        if (control.OH_TriggerDown()) {
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

//        if (control.OH_BtnDown()) {
//            manager.ReturnToLastMode();
//        }

//        //ScalePanDio();
//        ZoomToChar();

//        float multix = 4 * 16;
//        float multiy = 4 * 9;
//        float multiz = 4; 

//        //endPos = manager.diorama.transform.InverseTransformPoint(manager.mainPointer.position);
//        endPos = manager.cc.gameObject.transform.localPosition;
//        endPos.x = (Mathf.RoundToInt(endPos.x / multix) * multix) ;
//        endPos.z = (Mathf.RoundToInt(endPos.z / multiy) * multiy) ;
//        //endPos.y = (Mathf.RoundToInt(endPos.y));
//        endPos.y = 0;

//        Vector3 camRelPos = manager.quickRoom.transform.InverseTransformPoint(manager.rig.transform.position);
//        manager.quickRoom.transform.localPosition = endPos;
        
//        endPos.x /= multix;
//        endPos.z /= multiy;
//        endPos.x = (1000000 + endPos.x) % dungeonSize;
//        endPos.y = (1000000 + endPos.y) % dungeonSize;
//        endPos.z = (1000000 + endPos.z) % dungeonSize;

       

//        if (endPos != lastPos)
//        {
//            if (ct == CamType.Locked)
//            {
//                manager.rig.transform.position = manager.quickRoom.transform.TransformPoint(camRelPos);
//            }

//            DungeonBlock db = dungeon.GetBlock(endPos);

//            /*
//            if (db != null)
//            {
//                QuickRoom qr = manager.quickRoom.GetComponent<QuickRoom>();
//                qr.n.SetActive(!db.GetNorth());
//                qr.e.SetActive(!db.GetEast());
//                qr.s.SetActive(!db.GetSouth());
//                qr.w.SetActive(!db.GetWest());
//                qr.n1.SetActive(db.GetNorth());
//                qr.e1.SetActive(db.GetEast());
//                qr.s1.SetActive(db.GetSouth());
//                qr.w1.SetActive(db.GetWest());
//                qr.u1.SetActive(db.GetUp());
//                qr.d1.SetActive(db.GetDown());
//                qr.d2.SetActive(!db.GetDown());
//                qr.d3.SetActive(!db.GetDown());
//                qr.end.SetActive(!db.GetEnd());
//                qr.start.SetActive((db.GetStart() || db.GetEnd()));
//                qr.deadend.SetActive(db.GetDeadEnd());

//                if (endPos.y % 2 == 0)
//                {
//                    qr.u1.transform.localPosition = new Vector3(-4f, 0f, 2f);
//                    qr.u2.transform.localPosition = new Vector3(-4f, 0f, 0f);
//                    qr.u3.transform.localPosition = new Vector3(-4f, 0f, 4f);

//                    qr.d1.transform.localPosition = new Vector3(4f, -4f, 2f);
//                    qr.d2.transform.localPosition = new Vector3(4f, 0f, 0f);
//                    qr.d3.transform.localPosition = new Vector3(4f, 0f, 4f);
//                }
//                else
//                {
//                    qr.d1.transform.localPosition = new Vector3(-4f, -4f, 2f);
//                    qr.d2.transform.localPosition = new Vector3(-4f, 0f, 0f);
//                    qr.d3.transform.localPosition = new Vector3(-4f, 0f, 4f);

//                    qr.u1.transform.localPosition = new Vector3(4f, 0f, 2f);
//                    qr.u2.transform.localPosition = new Vector3(4f, 0f, 0f);
//                    qr.u3.transform.localPosition = new Vector3(4f, 0f, 4f);
//                }
//            }
//            */
//            lastPos = endPos;
//        }
        
//        manager.currentLayer.text = endPos.ToString();
//        //manager.UpdateLog(manager.cc.gameObject.transform.localPosition.y.ToString());
//        //manager.UpdateLog(test);
//    }

//    private void SetTag(Transform t, string tag, int layer)
//    {
//        t.gameObject.tag = tag;
//        t.gameObject.layer = layer;
//        foreach (Transform c in t)
//        {
//            SetTag(c, tag, layer);
//        }
//    }

//    private void ZoomToChar() {
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
//}
