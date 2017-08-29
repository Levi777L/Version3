using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using UnityEngine;
using TMPro;
using LPWAsset;

public class WorldBuilderMain : IMode
{
    #region Public Shared Vars

    public static World currentWorld;
    public static Zone currentZone;
    public static int currentZoneId = 0;

    public static Vector3 offsetPosition = Vector3.zero;

    public static GameObject selectedObject; //Shared SelectedObjectMode, PlaceObject, PlaceLightMode
    
    public static float objectScale = 1f; //Shared SelectedObjectMode
    public static int currentLayer = 0; //Shared SelectedObjectMode

    public static DioramaObject selectedTodObj {
        get { if (!selectedObject) return null;  return selectedObject.GetComponent<DioramaObject>(); }
    }
    public static BundleItem selectedBI {
        get { if (!selectedTodObj) return null; return TODV2.GetItemByID(selectedTodObj.todID); }
    }
    public static float gridSize {
        get { if (!selectedTodObj) return 2f; return selectedTodObj.gridScale; }
    }

    public static Vector3 lastObjectPosition = Vector3.zero; //Shared SelectedObjectMode, PlaceObject
    public static Quaternion lastObjectRotation = Quaternion.identity; //Shared SelectedObjectMode, PlaceObject

    public static float objAngleX, objAngleY, objAngleZ = 0; //PlaceObjectMode
    public static float savedYPos = float.MaxValue;

    #endregion

    #region Shared Public Methods

    public void SetupWorldMode(string name, bool newWorld)
    {
        manager.KeepActive(manager.builderObjects);
        if (newWorld)
        {
            StartNewWorld(name);
            manager.SetToolTip("Starting new world.");

        }
        else
        {
            try
            {
                manager.ClearDioAddZone();
                currentWorld = LoadWorld(name);
                currentZoneId = 0;
                LoadZone(false);
                manager.SetToolTip("Loaded " + name);
            }
            catch
            {
                StartNewWorld(name);
                manager.SetToolTip("Failed to Load.  Starting new world.");
            }
        }
    }

    public static void SetTag(Transform t, string tag, int layer) //SelectedObjectMode (Move to common? also floattoV3) 
    {
        if (t.tag != "Untagged")
            return;
        t.gameObject.tag = tag;
        t.gameObject.layer = layer;
        foreach (Transform c in t)
        {
            SetTag(c, tag, layer);
        }
    }

    public void ZoneChangeEvent(int i) //Used in zone change event from SelectedObjectMode.  Refactor?
    {
        currentZoneId = i;
        LoadZone();
    }

    public void ChangeLayer(int i)
    {
        currentLayer += i;

        if (currentLayer < 0)
        {
            currentLayer = 9;
        }
        else if (currentLayer > 9)
        {
            currentLayer = 0;
        }
        if (currentLayer == 0)
        {
            manager.SetToolTip("Layers Unlocked");

        }
        else
        {
            manager.SetToolTip("Layer " + currentLayer + " Selected");
        }
    }

    public static void ChoosePlaceMode()
    {
        objAngleX = 0;
        objAngleY = 0;
        objAngleZ = 0;
        PlaceObjectMode.Instance().SetupMode();
    }

    public static void ChooseSelectionModeType() 
    {
        lastObjectPosition = Vector3.zero;
        SelectObjectMode.Instance().SetupMode();
    }

    #endregion

    public void ButtonActivate(Node n, bool shift = false)
    {
        if (n.nodeType == "prefab")
        {
            if (shift)
            {
                if (!manager.favorites.Contains(n.todID))
                {
                    manager.favorites.Add(n.todID);
                    manager.SetToolTip("Added " + n.fileName + " to favorites.");
                }
                else
                {
                    manager.favorites.Remove(n.todID);
                    manager.SetToolTip("Removed " + n.fileName + " from favorites.");
                }
                TODV2.SaveGeneric<List<int>>(Globals.FAVPATH, manager.favorites);
            }
            else
            {
                SpawnNewObject(n);
                ChoosePlaceMode();
            }
        }
        else if (n.nodeType == "zone")
        {
            int id = -1;
            int lastChar = n.fileName.IndexOf(":");
            if (lastChar < 0 || !int.TryParse(n.fileName.Substring(0, lastChar), out id))
            {
                id = currentWorld.zones.Max(x => x.zoneId) + 1;
            }


            if (shift)
            {
                currentZoneId = id;
                LoadZone();
                manager.activeFM.LoadZoneList();
                return;
            }

            SpawnNewObject(n);
            TextMeshPro tmp = selectedObject.GetComponentInChildren<TextMeshPro>();
            if (tmp)
                tmp.text = id.ToString();
            manager.activeFM.CloseFolder();
            ChoosePlaceMode();
        }
        else if (n.nodeType == "button")
        {
            switch (n.fileName)
            {
                case "QUALITYUP":
                    manager.quality++;
                    if (manager.quality >= QualitySettings.names.Length)
                        manager.quality = QualitySettings.names.Length - 1;
                    QualitySettings.SetQualityLevel(manager.quality, true);
                    manager.SetToolTip(QualitySettings.names[manager.quality] + " Setting Selected (For this session only)");
                    return;
                case "QUALITYDOWN":
                    manager.quality--;
                    if (manager.quality < 0)
                        manager.quality = 0;
                    QualitySettings.SetQualityLevel(manager.quality, true);
                    manager.SetToolTip(QualitySettings.names[manager.quality] + " Setting Selected (For this session only)");
                    return;
                case "EXPLORE":
                    ExploreMode.Instance().SetupMode();
                    return;
                case "LIGHTSETUP":
                    LightSetupMode.Instance().SetupMode();
                    return;
                case "RESETLIGHT":
                    TOD_Sky.Instance.Day.LightIntensity = 1;
                    TOD_Sky.Instance.Night.LightIntensity = 5;
                    TOD_Sky.Instance.Cycle.Hour = 12;
                    PlayerPrefs.SetFloat("DayIntensity", TOD_Sky.Instance.Day.LightIntensity);
                    PlayerPrefs.SetFloat("NightIntensity", TOD_Sky.Instance.Night.LightIntensity);
                    PlayerPrefs.SetFloat("TimeOfDay", TOD_Sky.Instance.Cycle.Hour);
                    PlayerPrefs.Save();
                    return;
                case "VIEWPOINT":
                    manager.SetToolTip("Coming Soon");
                    return;
                case "UpFolder":
                    manager.activeFM.CloseFolder();
                    return;
                case "NextPage":
                    manager.activeFM.NextPage();
                    break;
                case "LastPage":
                    manager.activeFM.LastPage();
                    break;
                case "FavTest":
                    manager.activeFM.LoadFavList(manager.favorites);
                    break;
                case "ZONES":
                    manager.activeFM.LoadZoneList();
                    break;


                //WristTop
                case "SAVE":
                    SaveWorld();
                    break;
                case "ZONENAME":
                    manager.keyboard.gameObject.SetActive(true);
                    manager.builderObjects.SetActive(false);
                    manager.Diorama.SetActive(false);
                    manager.StartCoroutine(ZoneNameWait());
                    break;
                case "ZONETAG":
                    CycleZoneTag();
                    break;
                case "UNLOCKLOCK":
                    PlaceObjectMode.posLock = PlaceObjectMode.PosLock.Free;
                    manager.SetToolTip("Position Unlocked");
                    break;
                case "ROTATIONLOCK":
                    PlaceObjectMode.posLock = PlaceObjectMode.PosLock.RotLock;
                    manager.SetToolTip("Rotation Locked");
                    break;
                case "GRIDLOCK":
                    PlaceObjectMode.posLock = PlaceObjectMode.PosLock.Grid;
                    if (selectedObject)
                    {
                        Vector3 e = selectedObject.transform.localRotation.eulerAngles;
                        objAngleY = Mathf.RoundToInt(e.y / 90) * 90;
                        objAngleX = 0f;
                        objAngleZ = 0f;
                        selectedObject.transform.localRotation = Quaternion.Euler(new Vector3(objAngleX, objAngleY, objAngleZ));
                    }
                    manager.SetToolTip("Grid Mode Activated");
                    break;
                case "LAYERDOWN":
                    ChangeLayer(-1);
                    break;
                case "LAYERVISIBLE":
                    manager.SetToolTip("Coming Soon");
                    break;
                case "LAYERUP":
                    ChangeLayer(1);
                    break;

                //Botton Menu
                case "SAVEEXIT":
                    SaveWorld();
                    manager.DestroyDioReset();
                    TitleScreenMode.Instance().SetupMode();
                    break;
                case "SPAWNMODE":
                    AdventureTester.Instance().SetupMode();
                    manager.SetToolTip("Play Test Mode Activated");
                    break;
                case "TOGGLEGRID":
                    manager.grid.SetActive(!manager.grid.activeInHierarchy);
                    manager.SetToolTip("Grid " + (manager.grid.activeInHierarchy ? "Visible" : "Hidden"));
                    break;

                //Front Menu
                case "EXITNOSAVE":
                    Application.Quit();
                    return;


                //case "Clear All Viewpoints":
                //    currentZone.viewpoints.Clear();
                //    break;

                //case "Show Help Panels On":
                //    manager.showControls = false;
                //    PlayerPrefs.SetInt("BM Show Controls", (manager.showControls ? 0 : 1));
                //    PlayerPrefs.Save();

                //    break;
                //case "Show Help Panels Off":
                //    manager.showControls = true;
                //    PlayerPrefs.SetInt("BM Show Controls", (manager.showControls ? 0 : 1));
                //    PlayerPrefs.Save();

                //    break;

                //case "SetView":
                //    SetView();
                //    //manager.UpdateLog("View Saved");
                //    break;
                //case "LoadView":
                //    LoadView();
                //    break;
                default:
                    n.ActivateNode();
                    break;
            }

        }
        else
        {
            n.ActivateNode();
        }
    }

    #region IModeMethods
    private static WorldBuilderMain instance;
    private static GameManager manager;
    private static IVRControl control;
    public static WorldBuilderMain Instance()
    {
        if (instance == null)
        {
            instance = new WorldBuilderMain();
            manager = SL.sl.Get<GameManager>();
            control = SL.sl.Get<IVRControl>();
            SL.sl.Add<WorldBuilderMain>(instance);
        }
        return instance;
    }

    
    
    

    public void SetupMode()
    {
        //Do Nothing, prob remove
    }

    public void SoftUnload()
    {
        //Don't hard unload here.  Nothing really needs to be unloaded I think
    }

    public void IUpdate()
    {

    }

    public void IControlUpdate()
    {

    }

    #endregion

    #region Helpers
    private void CycleZoneTag()
    {
        switch (currentZone.mapType)
        {
            case MapTag.None:
                currentZone.mapType = MapTag.Environment;
                break;
            case MapTag.Environment:
                currentZone.mapType = MapTag.Adventure_Zone;
                break;
            default:
                currentZone.mapType = MapTag.None;
                break;
        }
        manager.SetToolTip("Tag Set to " + currentZone.mapType.ToString());
    }

    private IEnumerator ZoneNameWait()
    {
        while (manager.keyboardString == string.Empty)
        {
            yield return new WaitForSeconds(0.1f);
        }
        manager.builderObjects.SetActive(true);
        manager.Diorama.SetActive(true);
        currentZone.name = manager.keyboardString;
        manager.SetToolTip("Zone Name Set to: " + currentZone.name);
        manager.keyboardString = string.Empty;
    }

    private void SpawnNewObject(Node n)
    {
        manager.mhObjectPoint.position = manager.mainPointer.position;
        Vector3 scale = new Vector3(objectScale, objectScale, objectScale);
        manager.mhObjectPoint.transform.rotation = n.go.transform.rotation;
        GameObject next = Common.CopyGo(n.go, scale, null, true, true);
        selectedObject = next;
        next.transform.localScale = scale;

        SetTag(next.transform, "TableObject", LayerMask.NameToLayer("PlayTable"));
        DioramaObject d = next.AddComponent<DioramaObject>();
        d.layer = currentLayer;
        selectedTodObj.todID = n.todID;
        selectedTodObj.layer = WorldBuilderMain.currentLayer;
        selectedTodObj.gridScale = n.gridScale;
        n.SoftReset();

        next.transform.parent = manager.currentModeParent;
        next.transform.localScale = scale;
    }
    #endregion

    #region WorldMethods

    public void StartNewWorld(string name)
    {
        manager.ClearDioAddZone();
        objectScale = 1f;

        currentLayer = 1;
        ChangeLayer(0);

        currentWorld = new World(name);
        currentZone = new Zone(currentZoneId);
        currentWorld.zones.Add(currentZone);
    }

    public static void SaveWorldZone()
    {

        currentZone.dos.Clear();
        DioramaObject[] allDioramaObjects = manager.currentModeParent.gameObject.GetComponentsInChildren<DioramaObject>();
        foreach (DioramaObject a in allDioramaObjects)
        {
            try
            {
                a.dos.savedPos = a.gameObject.transform.localPosition;
                a.dos.savedRot = a.gameObject.transform.localRotation;
                a.dos.savedScale = a.gameObject.transform.localScale;
                a.dos.savedCharPose = new List<CharPose>();
                Transform[] allTrans = a.gameObject.GetComponentsInChildren<Transform>();
                for (int i = 0; i < allTrans.Count(); i++)
                {
                    CharPose next = new CharPose();
                    next.savedPos = allTrans[i].localPosition;
                    next.savedRot = allTrans[i].localRotation;
                    a.dos.savedCharPose.Add(next);
                }

                TextMeshPro tmp = a.gameObject.GetComponentInChildren<TextMeshPro>();
                if (tmp)
                {
                    a.text = tmp.text;
                }

                //IEventScale ies = a.gameObject.GetComponentInChildren<IEventScale>();
                //if (ies != null) {
                //    a.dos.savedScale = new Vector3(ies.GetScale(), ies.GetScale(), ies.GetScale());
                //}

                currentZone.dos.Add(a.dos);
            }
            catch { }
        }
    }

    public static void LoadZone(bool save = true)
    {
        Vector3 savedPos = manager.Diorama.transform.position;
        Quaternion savedRot = manager.Diorama.transform.rotation;
        float savedScale = manager.rig.transform.localScale.x;
 
        if (save)
        {
            SaveWorldZone();
            manager.ClearZone();
        }

        manager.grid.SetActive(true);

        currentZone = currentWorld.zones.Find(x => x.zoneId == currentZoneId);
        if (currentZone == null)
        {
            currentZone = new Zone(currentZoneId);
            currentWorld.zones.Add(currentZone);
        }

        if (currentZone.dos.Count > 0)
        {       
            manager.StartCoroutine(manager.loader.LoadDioAsync(currentZone.dos, manager.currentModeParent));
        }

        manager.Diorama.transform.position = savedPos;
        manager.Diorama.transform.rotation = savedRot;
        manager.ScaleDiorama(savedScale);
    }

    public void SaveWorld()
    {
        SaveWorldZone();

        string path = Globals.WORLDPATH;
        path += currentWorld.name + ".mrw";

        try
        {
            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, (object)currentWorld);
                File.WriteAllBytes(path, stream.ToArray());
                if (manager.createBackups)
                {
                    File.WriteAllBytes(Globals.BACKUPS + currentWorld.name + " " + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd hh-mm-ss") + ".mrw", stream.ToArray());
                }

            }

            manager.SetToolTip(currentWorld.name + " Successfully Saved at " + DateTime.Now.ToString("hh:mm:ss"));
            manager.lastSaveTime = DateTime.Now;
        }
        catch {
            manager.SetToolTip("Save Failed");
        }
    }

    public static World LoadWorld(string name)
    {
        string filename = Globals.WORLDPATH + name + ".mrw";
        try
        {
            byte[] file = File.ReadAllBytes(filename);
            using (MemoryStream stream = new MemoryStream(file))
            {
                return (World)new BinaryFormatter().Deserialize(stream);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        return null;
    }

    #endregion
    
}
