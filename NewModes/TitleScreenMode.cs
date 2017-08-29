using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class TitleScreenMode : IMode
{
    private static TitleScreenMode instance;
    private static GameManager manager;
    private static IVRControl control;
    private static WorldBuilderMain shared;
    private static List<string> allWorlds = new List<string>();
    private static int fileId = 0;
    private static List<string> dioArtist = new List<string>();
    private static int dioArtistId = 0;
    private static int selectedZoneID = 0;
    private static List<Zone> z;

    public static TitleScreenMode Instance()
    {
        if (instance == null)
        {
            instance = new TitleScreenMode();
            manager = SL.sl.Get<GameManager>();
            control = SL.sl.Get<IVRControl>();
            shared = SL.sl.Get<WorldBuilderMain>();
        }
        return instance;
    }

    public void IUpdate()
    {
        if(!manager.keyboard.gameObject.activeInHierarchy)
            manager.titleScreen.gameObject.SetActive(true);

        if (manager.keyboardString != string.Empty) {
            
            string s = manager.keyboardString.Trim().ToLower();
            if (s.Length < 2)
            {
                manager.SetToolTip("File names must be longer then 2 characters.");
            }
            else if (allWorlds.Contains(s))
            {
                manager.SetToolTip("World with that name already exists.");
            }
            else if (s.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0) {
                manager.SetToolTip(@"Filename contains invalid characters.  ? / \ | * : '' < >");
            }
            else
            {
                allWorlds.Add(s);
                fileId = allWorlds.Count - 1;
                manager.titleScreen.mainText.text = s;
            }
            manager.keyboardString = string.Empty;
            manager.DisableKeyboard();
        }

        Vector3 forward = manager.hmdAnchor.forward;
        forward.y = 0;
        manager.titleScreen.rotator.forward = forward;
        
    }

    public void IControlUpdate()
    {
        manager.modeStringEnum = GameManager.ModeString.Main_Menu;

        if (control.AL())
        {
            CalibrationMode.Instance().SetupMode();
        }
    }

    public void ButtonActivate(Node n, bool shift = false)
    {
        switch (n.fileName) {
            case "Builder":
                SetupFileSelectMode();
                break;
            case "Exit":
                Application.Quit();
                break;
            case "Viewer":
                SetupArtistSelection();
                break;
            case "Options":
                manager.quality--;
                if (manager.quality < 1)
                    manager.quality = QualitySettings.names.Length - 1;
                QualitySettings.SetQualityLevel(manager.quality, true);
                manager.SetToolTip(QualitySettings.names[manager.quality] + " Setting Selected");
                PlayerPrefs.SetInt("Quality", manager.quality);
                PlayerPrefs.Save();
                break;
            case "TitleNext":
                fileId++;
                if (fileId >= allWorlds.Count)
                    fileId = 0;
                manager.titleScreen.mainText.text = allWorlds[fileId];
                break;
            case "NextGroup":
                dioArtistId++;
                if (dioArtistId >= dioArtist.Count)
                    dioArtistId = 0;
                manager.titleScreen.mainText.text = dioArtist[dioArtistId];
                break;
            case "BackTitle":
                SetupTitleSelectMode();
                break;
            case "TitleLoad":
                if (fileId == 0)
                {
                    manager.EnableKeyboard();
                    manager.titleScreen.gameObject.SetActive(false);
                }
                else {
                    LoadWorld();
                }
                break;
            case "DioListLoad":
                z = LoadWorld(Globals.LEGACYPATH).zones.FindAll(x => x.name.IndexOf(dioArtist[dioArtistId].Substring(0, 4)) >= 0);
                selectedZoneID = 0;
                SetupDioramaSelection();
                break;
            case "DioLoad":
                LoadZone();
                ExploreMode.Instance().SetupMode();
                break;
            case "NextDiorama":
                selectedZoneID += 1;
                if (selectedZoneID >= z.Count)
                    selectedZoneID = 0;
                manager.titleScreen.mainText.text = GetZoneName();
                break;
            default:
                manager.SetToolTip("Coming Soon");
                break;
        }
    }

    public static World LoadWorld(string name)
    {
        try
        {
            byte[] file = File.ReadAllBytes(name);
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

    public void SetupMode()
    {
        manager.mode.SoftUnload();
        manager.mode = instance;

        string[] files = Directory.GetFiles(Globals.WORLDPATH + @"\", "*.mrw");
        allWorlds.Clear();
        allWorlds.Add("Start New World");
        foreach (string s in files) {
            string str = s.Substring(s.LastIndexOf("/") + 1).Replace(".mrw", "");
            allWorlds.Add(str);
        }
        fileId = 0;

        //Dynamic Add based on folders?
        dioArtist.Add("Levi's Experiments");
        dioArtist.Add("Baron's Dioramas");
        dioArtist.Add("Pigeon's Dioramas");
        dioArtist.Add("Other Dioramas");

        manager.KeepActive(manager.titleScreen);
        manager.titleScreen.transform.position = manager.startPoint.transform.position;
        manager.titleScreen.transform.rotation = manager.startPoint.transform.rotation;
        TOD_Sky.Instance.gameObject.transform.rotation = manager.startPoint.transform.rotation;

        if (manager.titleActive)
        {
            SetupDioramaSelection();
        }
        else
        {
            manager.titleActive = true;
            SetupTitleSelectMode();
            dioArtistId = 0;

        }
    }

    public void SoftUnload()
    {
        manager.leftText.text = "";
    }

    private void SetupTitleSelectMode() {
        manager.titleScreen.leftText.text = "Viewer";
        manager.titleScreen.left.fileName = "Viewer";
        manager.titleScreen.rightText.text = "Quality Level";
        manager.titleScreen.right.fileName = "Options";
        manager.titleScreen.mainText.text = "Diorama World Builder";
        manager.titleScreen.main.fileName = "Builder";

        manager.leftText.text = "Welcome to Diorama Worlds!\n" +
                                "------ Mixed Reality Worlds, Building Tools ------\n" +
                                "Visit www.MR-Worlds.com for Tutorials and Information\n" +
                                "on becomming a writer for Mixed Reality Worlds!\n" +
                                "-- Press the Left Trigger to recalibrate --";
    }

    private void SetupFileSelectMode() {
        manager.titleScreen.leftText.text = "Main Menu";
        manager.titleScreen.left.fileName = "BackTitle";
        manager.titleScreen.rightText.text = "Next World";
        manager.titleScreen.right.fileName = "TitleNext";

        int index = allWorlds.FindIndex(x => x == PlayerPrefs.GetString("LastWorld"));
        if (index > 0)
        {
            fileId = index;
        }
        else {
            fileId = 0;
        }
        manager.titleScreen.mainText.text = allWorlds[fileId];

        manager.titleScreen.main.fileName = "TitleLoad";

    }

    
    private void SetupArtistSelection()
    {
        manager.titleScreen.leftText.text = "Main Menu";
        manager.titleScreen.left.fileName = "BackTitle";

        manager.titleScreen.rightText.text = "Next Builder";
        manager.titleScreen.right.fileName = "NextGroup";

        manager.titleScreen.mainText.text = dioArtist[dioArtistId];
        manager.titleScreen.main.fileName = "DioListLoad";

    }

    private void SetupDioramaSelection()
    {
        manager.titleScreen.leftText.text = "Main Menu";
        manager.titleScreen.left.fileName = "BackTitle";

        manager.titleScreen.rightText.text = "Next Diorama";
        manager.titleScreen.right.fileName = "NextDiorama";
        
        manager.titleScreen.mainText.text = GetZoneName();
        manager.titleScreen.main.fileName = "DioLoad";

    }

    private string GetZoneName() {
        string s = z[selectedZoneID].name;
        s = s.Replace("Levi", "");
        s = s.Replace("Baron", "");
        s = s.Replace("Pigeon", "");
        s = s.Replace("Other", "");
        return s;
    }

    private void LoadZone() {
        manager.ClearDioAddZone();
        manager.grid.SetActive(false);

        if (z[selectedZoneID].dos.Count > 0)
        {
            manager.StartCoroutine(manager.loader.LoadDioAsync(z[selectedZoneID].dos, manager.currentModeParent));
        }

    }

    private void SetupOptionsMode()
    {
        manager.titleScreen.leftText.text = "ph";
        manager.titleScreen.left.fileName = "ph";
        manager.titleScreen.rightText.text = "ph";
        manager.titleScreen.right.fileName = "ph";
        manager.titleScreen.mainText.text = "ph";
        manager.titleScreen.main.fileName = "ph";

    }

    private void LoadWorld() {
        bool fileExists = File.Exists(Globals.WORLDPATH + allWorlds[fileId] + ".mrw");

        manager.loadedWorld = allWorlds[fileId];
        PlayerPrefs.SetString("LastWorld", allWorlds[fileId]);
        PlayerPrefs.Save();

        WorldBuilderMain.Instance().SetupWorldMode(allWorlds[fileId], !fileExists);
        SelectObjectMode.Instance().SetupMode();
        manager.titleActive = false;
        if(z != null)
            z.Clear();
        allWorlds.Clear();
        dioArtist.Clear();
    }



}
