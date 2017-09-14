//******************//
//Game Manager Class//
//******************//

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using TMPro;
using System.Threading;

public class GameManager : MonoBehaviour
{
    private volatile bool workInProgress = false;
    private Thread t1;
    private Thread t2;
    private volatile bool t1PausedFlag = false;
    private volatile bool t2PausedFlag = false;
    private volatile bool cancelFlag = false;

    public void RunMethodOnT1(Func<int> myFunction) {

    }

    private void OnDestroy()
    {
        KeyboardMode.Unhook();
        Debug.Log("Unhooked Keyboard");
    }

    #region New Objects
    [HideInInspector]
    public int quality = 0;
    [HideInInspector]
    public bool titleActive = false;

    public Transform pointerGroup;
    public DateTime? lastSaveTime = null;
    [HideInInspector]
    public ModeString modeStringEnum = ModeString.Main_Menu;
    private ModeString[] builderModes = { ModeString.Camera_Control, ModeString.View_Mode, ModeString.Cycle_Object, ModeString.Pose_Detail, ModeString.Place_Object};
    public enum ModeString {
        Calibration = 0,
        Main_Menu = 1,
        Camera_Control = 2,
        View_Mode = 3,
        Cycle_Object = 4,
        Pose_Detail = 5,
        Place_Object = 6,
        Play_Test = 7,
        Keyboard = 8,
    }

    public GameObject calibration;
    public TitleObjects titleScreen;
    public GameObject builderObjects;
    public Keyboard keyboard;
    [HideInInspector]
    public string keyboardString = String.Empty;
    [HideInInspector]
    public string loadedWorld = null;

    public void KeepActive(object o) {
        calibration.SetActive(o == calibration);
        titleScreen.go.SetActive(o == titleScreen);
        builderObjects.SetActive(o == builderObjects);
    }

    public void EnableKeyboard() {
        keyboard.gameObject.SetActive(true);
        KeyboardMode.Instance().SetupMode();
    }

    public void DisableKeyboard()
    {
        keyboard.gameObject.SetActive(false);
        KeyboardMode.Instance().SoftUnload();
    }

    public GameObject leftTextRot, layerTextRot;
    public TextMeshPro leftText, rightText, toolTip, controlList;
    private Color textColor;
    public Transform WristBottom;
    public Transform WristTop;
    public Transform ControlMenu;
    public Transform FrontMenu;
    public GameObject WristBottomObj, WristTopObj, ControlMenuObj, FrontMenuObj;
    public Material nodeU, nodeS;

    private float toolTipTimer = 0f;

    private bool moveDioToggle = false;
    private bool moveDio = false;

    [HideInInspector]
    public float lastDeltaTime = 0f;

    public Transform mhObjectPoint;

    [HideInInspector]
    public bool createBackups = true;

    [HideInInspector]
    public List<int> favorites = new List<int>();

    #endregion

    #region InspectorVars
    public Loader loader;
    public Transform fileManager;
    public FileManagerV2 activeFM;
    public GameObject testCharPrefab;
    public GameObject poserWidget;
    public GameObject gridPrefab;
    public Transform mhAnchor;
    public Transform ohAnchor;
    public GameObject nodePrefab;
    
    public Transform startPoint;
    #endregion

    #region AwakeVars
    [HideInInspector]
    public Transform mhPointGroup;
    [HideInInspector]
    public GameObject rig;
    [HideInInspector]
    public Transform hmdAnchor;
    [HideInInspector]
    public Transform mainPointer;
    [HideInInspector]
    public Transform hmdPointer;
    #endregion

    #region Preferences

    [HideInInspector]
    public bool showShadows = true;
    [HideInInspector]
    public bool showParticles = true;
    [HideInInspector]
    public bool showControls = true;

    #endregion

    #region OtherVars
    [HideInInspector]
    public GameObject activeNode = null;
    [HideInInspector]
    public GameObject testChar;

    [HideInInspector]
    public GameObject grid;

    [HideInInspector]
    public string lb1, lb2, rb1, rb2, shiftButton, triggerButton;

    private bool lastLoading = false;

    private int currentTip = 0;
    private float textTimer = 0f;

    
    public IVRHmd hmd;
    public IVRControl control;

    
    

    private GameObject diorama;
    [HideInInspector]
    public GameObject Diorama { get { return diorama; } }
    private GameObject dioramaObjects;
    [HideInInspector]
    public Transform currentModeParent;

    [HideInInspector]
    public float dioramaScale = 0.1f;
    [HideInInspector]
    public Vector3 dioramaVScale { get { return new Vector3(dioramaScale, dioramaScale, dioramaScale); } }

    [HideInInspector]
    public Vector3 savedFilePos = Vector3.zero;
    [HideInInspector]
    public IMode mode;
     
    private string lastLogs;

    
    private string uploadName = "";
    
    #endregion




    private void Awake()
    {
        titleActive = false;
        Shader.EnableKeyword("TBT_LINEAR_TARGET");

        mhAnchor.gameObject.SetActive(true);
        ohAnchor.gameObject.SetActive(true);

        rig = GameObject.FindWithTag("VRCameraRig");
        hmdAnchor = GameObject.FindWithTag("CenterEyeAnchor").transform;
        mainPointer = GameObject.FindWithTag("pointer").transform;
        hmdPointer = GameObject.FindWithTag("HMDPointer").transform;

        textColor = toolTip.color;
    }

    void Start()
    {
        KeyboardMode.Hook();

        int i = Application.dataPath.LastIndexOf("/");

        Globals.WORLDPATH = Application.dataPath.Substring(0, i) + "/Worlds/";
        Directory.CreateDirectory(Globals.WORLDPATH);

        Globals.BACKUPS = Application.dataPath.Substring(0, i) + "/BackupFiles/";
        Directory.CreateDirectory(Globals.BACKUPS);

        Globals.BUNDLEPATH = Application.dataPath.Substring(0, i) + "/ArtPacks/";
        Directory.CreateDirectory(Globals.BUNDLEPATH);

        Globals.TEMPFOLDER = Application.dataPath.Substring(0, i) + "/DWTemp/";
        Directory.CreateDirectory(Globals.TEMPFOLDER);

        Globals.TODPATH = Application.dataPath.Substring(0, i) + "/tod2.tbl";

        Globals.LEGACYPATH = Application.dataPath.Substring(0, i) + "/all.tbl";

        Globals.FAVPATH = Application.dataPath.Substring(0, i) + "/dw.fav";
        favorites = TODV2.LoadGeneric<List<int>>(Globals.FAVPATH);
        if (favorites == default(List<int>))
            favorites = new List<int>();

        if (Application.isEditor)
        {
            AssetBundleCreateRequest a = AssetBundleManager.Polygon_Adventure;
        }
        else
        {
            AssetBundleManager.Prewarm();
        }

        SL.Add<GameManager>(this);
        SL.Add<Keyboard>(keyboard);
        
        hmd = rig.GetComponent<IVRHmd>();
        hmd.Init();
        SL.Add<IVRHmd>(hmd);

        control = rig.GetComponent<IVRControl>();
        control.Init(this);
        SL.Add<IVRControl>(control);

        poserWidget.SetActive(false);

        if (control.GetControlStyle() == ControlerStyle.ViveWand)
        {
            lb1 = "Down";
            lb2 = "Up";
            rb1 = "Down";
            rb2 = "Up";
        }
        else if (control.GetControlStyle() == ControlerStyle.RiftTouch)
        {
            lb1 = "X";
            lb2 = "Y";
            rb1 = "A";
            rb2 = "B";
        }

        activeFM.Init(this);
        SL.Add<FileManagerV2>(activeFM);

        SetPrefs();
        DeactivateStarter();

        mode = WorldBuilderMain.Instance();

        if (startPoint.position == Vector3.zero)
        {
            CalibrationMode.Instance().SetupMode();
        }
        else {
            TitleScreenMode.Instance().SetupMode();
        }

    }

    private void DeactivateStarter() {
        activeFM.gameObject.SetActive(false);
        builderObjects.SetActive(false);
        titleScreen.gameObject.SetActive(false);
        keyboard.gameObject.SetActive(false);
    }

    void Update()
    {
        SetMoveDioToggle();


        if (!loader.LoaderRunning)
        {

            if (Time.deltaTime > 0) lastDeltaTime = Time.deltaTime;

            RotateTextsToHMD();
            ToolTipTimer();

            if (control != null) control.RefreshControl();
            if (Input.GetKey("escape")) Application.Quit();

            LookMenuActivate();
            CheckForActiveButton();

            SelectObjectMode.SoftReset();

            mode.IUpdate();
            FMControlUpdate();


            if (moveDio)
            {
                PoserMode.DropObject();
                ScalePanMode.Instance().IControlUpdate();
            }
            else if (activeNode)
            {
                if (control.AR())
                {
                    ActivateNode();
                    return;
                }
                if (keyboard.gameObject.activeInHierarchy)
                {
                    KeyboardMode.Instance().IControlUpdate();
                }
            }
            else if (keyboard.gameObject.activeInHierarchy)
            {
                KeyboardMode.Instance().IControlUpdate();
            }
            else if (mode != null)
            {
                mode.IControlUpdate();
            }
            RightDetailText();
        }
        else
        {
            rightText.text = "Loading";
            if (moveDio)
            {
                ScalePanMode.Instance().IControlUpdate();
            }
        }

        
    }

    private void SetMoveDioToggle() {
        if (control != null && diorama)
        {
            if (control.GetControlStyle() != ControlerStyle.RiftTouch)
            {
                if (control.RSD())
                {
                    moveDio = !moveDio;
                }
            }
            else
            {
                moveDio = control.RSH();
            }
        }
        else
        {
            moveDio = false;
        }
    }

    private void FMControlUpdate() {
        if (activeFM.gameObject.activeInHierarchy)
        {
            if (control.FML())
            {
                activeFM.LastPage();
            }
            if (control.FMR())
            {
                activeFM.NextPage();
            }
            if (control.FMU() || control.FMD())
            {
                activeFM.CloseFolder();
            }
        }
    }

    private void RightDetailText()
    {
        if (mode == ExploreMode.Instance()) {
            string timeText = "Viewer Mode\nLeft and Right change the Time of Day\n\n";
            TimeSpan timeSpan = TimeSpan.FromHours(TOD_Sky.Instance.Cycle.Hour);
            timeText += string.Format("{0:D2}:{1:D2}", (timeSpan.Hours % 12) + 1, timeSpan.Minutes) + (((int)timeSpan.Hours / 12) == 1 ? " PM" : " AM");

            timeText += "\n\nLeft Hand Trigger to Exit ";

            rightText.text = timeText;
            return;
        }

        string text = "Current Mode: " + modeStringEnum.ToString().Replace("_", " ") + "\n";

        if (mode == TitleScreenMode.Instance()) {
            text += "\n\nQuality Level: " + QualitySettings.names[quality] + "\n";
        }

        if (WorldBuilderMain.currentZone != null)
        {
            if (WorldBuilderMain.currentZone.mapType != MapTag.None)
            {
                text += "Tag: " + WorldBuilderMain.currentZone.mapType.ToString() + "\n";
            }
            else
            {
                text += "\n";
            }

            if (WorldBuilderMain.currentZone.name != "")
            {
                text += "Name: " + WorldBuilderMain.currentZone.name + "\n";
            }
            else
            {
                text += "\n";
            }
        }
        else {
            text += "\n";
        }

        if (builderModes.Contains(modeStringEnum))
        {
            text += "ZoneID: " + WorldBuilderMain.currentZoneId + "\n";
            text += "Working Layer: " + WorldBuilderMain.currentLayer.ToString() + "\n";
            text += "Lock Mode: ";
            if (PlaceObjectMode.posLock == PlaceObjectMode.PosLock.Grid)
            {
                text += "Grid Locked\n";
            }
            else if (PlaceObjectMode.posLock == PlaceObjectMode.PosLock.RotLock)
            {
                text += "Rotation Locked\n";
            }
            else
            {
                text += "Unlocked\n";
            }
        }

        text += DateTime.Now.ToString("hh:mm tt") + " - Last Save: " + (lastSaveTime == null ? "NA" : ((DateTime)lastSaveTime).ToString("hh:mm tt"));

        rightText.text = text;
    }

    private void ActivateNode() {
        Node n = activeNode.GetComponent<Node>();
        if (keyboard.gameObject.activeInHierarchy) {
            KeyboardMode.Instance().ButtonActivate(n, control.LSH());
        }
        else
        {
            mode.ButtonActivate(n, control.LSH());
        }
    }

    private void RotateTextsToHMD() {
        leftTextRot.transform.forward = hmdAnchor.forward;
        layerTextRot.transform.forward = hmdAnchor.forward;
    }

    private void ToolTipTimer() {
        toolTipTimer += (lastDeltaTime / 3);
        if (toolTipTimer < 1f)
        {
            toolTip.color = Color.Lerp(textColor, Color.clear, toolTipTimer);
        }
        else {
            toolTip.text = "";
        }
    }

    public void SetToolTip(string s) {
        toolTip.text = s;
        toolTipTimer = 0;
        toolTip.color = textColor;
    }

    private void LookMenuActivate() {
        if (CheckLookMatch(WristBottom))
        {
            FrontMenuObj.SetActive(false);
            ControlMenuObj.SetActive(false);
            WristBottomObj.SetActive(true);
            WristTopObj.SetActive(false);
            activeFM.gameObject.SetActive(false);
        }
        else if (CheckLookMatch(WristTop))
        {
            FrontMenuObj.SetActive(false);
            ControlMenuObj.SetActive(false);
            WristBottomObj.SetActive(false);
            WristTopObj.SetActive(true);
            activeFM.gameObject.SetActive(false);
        }
        else if (CheckLookMatch(ControlMenu))
        {
            FrontMenuObj.SetActive(false);
            ControlMenuObj.SetActive(true);
            WristBottomObj.SetActive(false);
            WristTopObj.SetActive(false);
            activeFM.gameObject.SetActive(false);
        }
        else if (CheckLookMatch(FrontMenu))
        {
            FrontMenuObj.SetActive(true);
            ControlMenuObj.SetActive(false);
            WristBottomObj.SetActive(false);
            WristTopObj.SetActive(false);
            activeFM.gameObject.SetActive(false);
        }
        else
        {
            FrontMenuObj.SetActive(false);
            ControlMenuObj.SetActive(false);
            WristBottomObj.SetActive(false);
            WristTopObj.SetActive(false);
            activeFM.gameObject.SetActive(mode == SelectObjectMode.Instance());
        }
    }

    private void CheckForActiveButton() {
        activeNode = null;
        Collider[] hits = Physics.OverlapSphere(mainPointer.position, dioramaScale/100);
        
        GameObject[] allNodes = GameObject.FindGameObjectsWithTag("node");
        if (allNodes.Length > 0)
        {
            foreach (GameObject go in allNodes)
            {
                Renderer r = go.GetComponent<Renderer>();
                if (!r)
                    continue;

                r.material = nodeU;

                float nearest = float.MaxValue;
                float check = Vector3.Distance(mainPointer.position, go.transform.position);

                foreach (Collider c in hits)
                {
                    if (c.gameObject == go)
                    {
                        if (check < nearest)
                        {
                            nearest = Vector3.Distance(mainPointer.position, go.transform.position);
                            activeNode = go;

                        }
                    }
                }
            }
            if (activeNode)
            {
                Renderer rend = activeNode.GetComponent<Renderer>();
                if (rend)
                    rend.material = nodeS;
            }
        }
    }

    public void ReturnToLastMode() {
        mode = CalibrationMode.Instance();
    }

    private bool CheckLookMatch(Transform b) {
        Vector3 forward = hmdPointer.TransformDirection(Vector3.forward);
        Vector3 toOther = b.position - hmdPointer.position;
        float hmdLookAtOther = Vector3.Dot(forward.normalized, toOther.normalized);

        forward = b.TransformDirection(Vector3.forward);
        toOther = hmdPointer.position - b.position;
        float otherLookAtHmd = Vector3.Dot(forward.normalized, toOther.normalized);

        return (hmdLookAtOther + otherLookAtHmd) > 1.85f;
    }

    public void UploadDemoFile(string filename)
    {
        StartCoroutine(UploadFileAsync(filename, true));
    }

    public void UploadFile(string filename) {
         StartCoroutine(UploadFileAsync(filename));
    }

    public void UploadWorld(string filename)
    {
        StartCoroutine(UploadWorldAsync(filename));
    }

    public void UploadFileBest(string filename)
    {
        StartCoroutine(SaveToBest(filename));
    }

    IEnumerator UploadFileAsync(string filename, bool demo = false) {
        try
        {
            ftp ftpClient;
            if (demo)
            {
                ftpClient = new ftp("ftp://xo2.x10hosting.com/public_html/DemoVersion/", "dioramaw", "1qaz@WSX3edc$RFV");
            }
            else {
                ftpClient = new ftp("ftp://xo2.x10hosting.com/public_html/Uploads/", "dioramaw", "1qaz@WSX3edc$RFV");
            }
            if(uploadName == "")
                uploadName = DateTime.Now.ToUniversalTime().ToString("yyyyMMddhhmmss_") + Path.GetRandomFileName() + ".dio";
            ftpClient.upload(uploadName, filename);
            ftpClient = null;
        }
        catch {
            Debug.Log("Failed");
        }

        yield break;
    }

    IEnumerator UploadWorldAsync(string filename, bool demo = false)
    {
        try
        {
            ftp ftpClient;
            ftpClient = new ftp("ftp://xo2.x10hosting.com/public_html/TestWorld/", "dioramaw", "1qaz@WSX3edc$RFV");
            if (uploadName == "")
                uploadName = DateTime.Now.ToUniversalTime().ToString("yyyyMMddhhmmss_") + Path.GetRandomFileName() + ".world";
            ftpClient.upload(uploadName, filename);
            ftpClient = null;
        }
        catch
        {
            Debug.Log("Failed");
        }

        yield break;
    }

    IEnumerator SaveToBest(string filename)
    {
        try
        {
            ftp ftpClient;
            ftpClient = new ftp("ftp://xo2.x10hosting.com/public_html/Best/", "dioramaw", "1qaz@WSX3edc$RFV");
            uploadName = DateTime.Now.ToUniversalTime().ToString("yyyyMMddhhmmss_") + Path.GetRandomFileName() + ".dio";
            ftpClient.upload(uploadName, filename);
            ftpClient = null;
        }
        catch
        {
            Debug.Log("Failed");
        }

        yield break;
    }



    public List<string> GetBestFiles() {
        List<string> files = new List<string>();
        ftp ftpClient = new ftp("ftp://xo2.x10hosting.com/public_html/Best/", "dioramaw", "1qaz@WSX3edc$RFV");
        /* Get Contents of a Directory (Names Only) */
        string[] simpleDirectoryListing = ftpClient.directoryListSimple("/");
        for (int i = 0; i < simpleDirectoryListing.Count(); i++)
        {
            if (simpleDirectoryListing[i].LastIndexOf(".dio") > 0)
                files.Add(simpleDirectoryListing[i]);
        }
        ftpClient = null;
        
        files = files.OrderByDescending(q => q).ToList();
        
        return files;
    }

    public List<string> GetAllFiles()
    {
        List<string> files = new List<string>();
        ftp ftpClient = new ftp("ftp://xo2.x10hosting.com/public_html/Uploads/", "dioramaw", "1qaz@WSX3edc$RFV");
        /* Get Contents of a Directory (Names Only) */
        string[] simpleDirectoryListing = ftpClient.directoryListSimple("/");
        for (int i = 0; i < simpleDirectoryListing.Count(); i++)
        {
            if (simpleDirectoryListing[i].LastIndexOf(".dio") > 0)
                files.Add(simpleDirectoryListing[i]);
        }
        ftpClient = null;

        files = files.OrderByDescending(q => q).ToList();

        return files;
    }

    public void DownloadFile(string filename) {
        ftp ftpClient = new ftp("ftp://xo2.x10hosting.com/public_html/Best/", "dioramaw", "1qaz@WSX3edc$RFV");
        ftpClient.download(filename, Globals.TEMPFOLDER + filename);
        ftpClient = null;
    }

    public void DownloadFileFromUploads(string filename) {
        ftp ftpClient = new ftp("ftp://xo2.x10hosting.com/public_html/Uploads/", "dioramaw", "1qaz@WSX3edc$RFV");
        ftpClient.download(filename, Globals.TEMPFOLDER + filename);
        ftpClient = null;
    }

    public void DeleteFileFromUploads(string filename) {
        ftp ftpClient = new ftp("ftp://xo2.x10hosting.com/public_html/Uploads/", "dioramaw", "1qaz@WSX3edc$RFV");
        ftpClient.delete(filename);
        ftpClient = null;
    }

    public void DownloadRandomFile() {
        ftp ftpClient = new ftp("ftp://xo2.x10hosting.com/public_html/Uploads/", "dioramaw", "1qaz@WSX3edc$RFV");
        string[] simpleDirectoryListing = ftpClient.directoryListSimple("/");
        //UpdateLog("Number of uploaded Dioramas: " + (simpleDirectoryListing.Length - 2));
        int i = UnityEngine.Random.Range(2, simpleDirectoryListing.Length - 1);
        ftpClient.download(simpleDirectoryListing[i], Globals.TEMPFOLDER + "temp.dio");
        ftpClient = null;
    }

    //public void SetTextColor(float f) {
    //    Color next = Common.GetColor(f);
    //    fileManager.GetComponent<FileManagerV2>().SetColor(next);
    //    toolTip.color = next;
        
    //    mainPointer.GetComponent<Renderer>().material.SetColor("_Color_Tint", next);
    //    PlayerPrefs.SetFloat("Font Color", f);
    //    PlayerPrefs.Save();
    //}
   
    private void SetPrefs() {

        quality = PlayerPrefs.GetInt("Quality");

        if (quality < 1)
            quality = QualitySettings.names.Length - 1;
        QualitySettings.SetQualityLevel(quality, true);

        int nextInt;
        
        nextInt = PlayerPrefs.GetInt("BM Show Controls");
        showControls = (nextInt == 0);
 
        nextInt = PlayerPrefs.GetInt("Shadows");
        showShadows = (nextInt == 0);
        //Light l = dioramaLight.GetComponentInChildren<Light>();
        //if (showShadows)
        //{
        //    l.shadows = LightShadows.Hard;
        //}
        //else {
        //    l.shadows = LightShadows.None;
        //}

        nextInt = PlayerPrefs.GetInt("Particles");
        showParticles = (nextInt == 0);

        startPoint.position = new Vector3(PlayerPrefs.GetFloat("StartPosX"), PlayerPrefs.GetFloat("StartPosY"), PlayerPrefs.GetFloat("StartPosZ"));
        startPoint.rotation = new Quaternion(PlayerPrefs.GetFloat("StartRotX"), PlayerPrefs.GetFloat("StartRotY"), PlayerPrefs.GetFloat("StartRotZ"), PlayerPrefs.GetFloat("StartRotW"));

        float f;

        f = PlayerPrefs.GetFloat("DayIntensity");
        TOD_Sky.Instance.Day.LightIntensity = (f == 0 ? 1 : f);
        f = PlayerPrefs.GetFloat("NightIntensity");
        TOD_Sky.Instance.Night.LightIntensity = (f == 0 ? 5 : f);
        f = PlayerPrefs.GetFloat("TimeOfDay");
        TOD_Sky.Instance.Cycle.Hour = (f == 0 ? 12 : f);


    }

    //Helpers
    public void SwapAnchor(Transform point, Transform parent)
    {
        if (point.parent == null)
        {
            point.parent = parent;
        }
        else
        {
            point.parent = null;
        }
    }

    public void UpdateLog(string s, bool clear = false)
    {

        textTimer = 0;
    }
    

    public GameObject BuildNode()
    {
        return Instantiate(nodePrefab) as GameObject;
    }

    public void DestoryObject(GameObject go)
    {
        Destroy(go);
    }

    internal void DestoryNodeScript(GameObject node)
    {
        Destroy(node.GetComponent<Node>());
    }

  
    public void GMDestroy(UnityEngine.Object c) {
        Destroy(c);
    }

    public void DestroyNow(UnityEngine.Object c) {
        DestroyImmediate(c);
    }

    public void ScaleDiorama(float newScale) {
        rig.transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    public void ClearZone()
    {
        GMDestroy(dioramaObjects);
        dioramaObjects = new GameObject("DioramaObjects");
        dioramaObjects.transform.parent = diorama.transform;
        dioramaObjects.transform.localPosition = Vector3.zero;
        dioramaObjects.transform.localRotation = Quaternion.identity;
        currentModeParent = dioramaObjects.transform;
    }

    public void ClearDioAddZone() {
        ClearDiorama();
        ClearZone();
    }

    private void ClearDiorama() {
        uploadName = "";
        GMDestroy(diorama);
        diorama = new GameObject("diorama");

        grid = Instantiate(gridPrefab);
        grid.transform.parent = diorama.transform;
        grid.transform.localPosition = Vector3.zero;
        
        ScaleDiorama(35f);
        dioramaScale = 35f;

        //DioramaObject dioObj = diorama.AddComponent<DioramaObject>();
        //dioObj.todID = -1;

        diorama.transform.position = startPoint.position;
        diorama.transform.rotation = startPoint.rotation;
    }

    public void DestroyDioReset() {
        uploadName = "";
        GMDestroy(diorama);
        ScaleDiorama(100f);
    }

    public void CameraSky(bool on) {
        TOD_Sky.Instance.Components.Billboards.SetActive(on);
        TOD_Sky.Instance.Components.Stars.SetActive(on);
        TOD_Sky.Instance.Components.Clouds.SetActive(on);
    }
}
