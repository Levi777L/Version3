using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System;
using System.Linq;
using System.Collections;
using TMPro;

public class FileManagerV2 : MonoBehaviour
{

    private GameManager manager;
    private IVRControl control;
    public TextMeshPro pageText;
    private TextMeshPro[] nodeText = new TextMeshPro[9];
    public GameObject[] nodes;

    private List<string> AllNodeStrings = new List<string>();
    private List<BundleItem> bundleItems = new List<BundleItem>();
    private List<BundleItem> nodeItems = new List<BundleItem>();

    private BundleItem.TopCategory topCategory;
    private BundleItem.BundleNames bundleName;

    AssetBundle ab;

    private int currentPage;
    private int totalPages;
    private List<int> savedPages = new List<int>();

    private bool initialized = false;
    private bool returnToMenu = false;
    private List<string> MainMenuBackup = new List<string>();

    private int currentFolderIndex = -1;
    private List<string> currentPath = new List<string>();
    private string path = "";

    private FolderLevel folderLevel = FolderLevel.CustomList;
    public enum FolderLevel
    {
        CustomList,
        MainMenu,
        ObjectExplorer,
        CategorySelect,
        PickBundle,
        SubFolder,
        SubFolderLevelOne,
        FileExplorer,
        Drive,
        Folder,


    }

    // Use this for initialization
    private void Awake()
    {
        for (int i = 0; i < 9; i++)
        {
            nodes[i].SetActive(true);
            nodeText[i] = nodes[i].GetComponentInChildren<TextMeshPro>();
        }
    }

    void Start()
    {
        LoadTopCategory();
    }

    public void Init(GameManager m)
    {
        manager = m;
        control = manager.control;
        initialized = true;
    }



    private void BuildNodes()
    {
        int count = (currentPage - 1) * 9;

        for (int i = 0; i < 9; i++)
        {
            Node n = nodes[i].GetComponentInChildren<Node>();
            n.ResetNode();

            if (count >= AllNodeStrings.Count)
            {
                nodes[i].SetActive(false);
            }
            else
            {

                nodes[i].SetActive(true);

                int j = AllNodeStrings[count].LastIndexOf(".");
                string lhs = j < 0 ? AllNodeStrings[count] : AllNodeStrings[count].Substring(0, j);
                string rhs = j < 0 ? "" : AllNodeStrings[count].Substring(j + 1);
                n.Init(this, lhs, rhs);

                //pick node object/icon
                if (AllNodeStrings[count].LastIndexOf(".prefab") > 0 || AllNodeStrings[count].LastIndexOf(".zone") > 0)
                {
                    float sizeMod = 1f;
                    if (AllNodeStrings[count].LastIndexOf(".zone") > 0) {
                        sizeMod = 0.5f;
                    }
                    n.SetObject(nodeItems[count], sizeMod);
                    if(Application.isEditor || Debug.isDebugBuild){ 
                        nodeText[i].text = "" + nodeItems[count].id + ": " + lhs;
                    }
                    else {
                        nodeText[i].text = lhs;
                    }
                     //AllNodeStrings[count];
                }
                else
                {
                    try
                    {
                        int k = Mathf.Max(lhs.LastIndexOf(@"/"), lhs.LastIndexOf(@"\"));
                        nodeText[i].text = k < 0 ? lhs : lhs.Substring(k + 1); //AllNodeStrings[count];
                    }
                    catch { }

                }
            }

            count++;
        }

    }

    private void UpdatePageText()
    {
        if(pageText)
            pageText.text = "Page " + currentPage + " of " + totalPages;
    } 

    private void ResetCurrentPage()
    {
        savedPages.Add(currentPage);
        currentPage = 1;
        totalPages = (AllNodeStrings.Count / 9) + (AllNodeStrings.Count % 9 == 0 ? 0 : 1);
        UpdatePageText();
    }

    private void GetSavedPage()
    {
        try
        {
            int i = savedPages.Count() - 1;
            currentPage = savedPages[i];
            savedPages.Remove(currentPage);
        }
        catch {
            currentPage = 1;
        }
        totalPages = (AllNodeStrings.Count / 9) + (AllNodeStrings.Count % 9 == 0 ? 0 : 1);
        UpdatePageText();
    }

    private void LoadMode(string fileName)
    {
        folderLevel = FolderLevel.MainMenu;

        switch (fileName)
        {
            case "Exit Game":
                Application.Quit();
                return;
            case "Exit Mode":
                manager.ReturnToLastMode();
                return;


        }

        //if (nextMode != null)
        //    manager.NextMode(nextMode, subMode);
    }

    public void ButtonPress(string fileName)
    {
        switch (fileName)
        {
            case "UpFolder":
                CloseFolder();
                return;
            case "NextPage":
                NextPage();
                break;
            case "LastPage":
                LastPage();
                break;
        }
    }

    public void NodeAction(string fileName, string nodeType, Node node)
    {

            switch (nodeType)
            {
                case "drive":
                case "desktop":
                case "folder":
                    OpenFolder(fileName);
                    break;
                case "mode":
                    LoadMode(fileName);
                    break;
                case "button":
                    ButtonPress(fileName);
                    break;
                case "jpg":
                    //manager.UpdateLog("JPG: " + fileName);
                    break;
                case "bundle":
                    currentFolderIndex = 0;
                    //OpenBundle(fileName);
                    OpenBundle(fileName);
                    break;
                case "subfolder":
                    currentFolderIndex++;
                    OpenSubFolder(fileName);
                    break;
                case "category":
                    OpenCategory(fileName);
                    break;
                default:
                    //manager.UpdateLog("Unknown File: " + fileName + "." + nodeType);
                    break;
            }
        
    }

    public void LoadFavList(List<int> fav)
    {
        folderLevel = FolderLevel.CustomList;
        nodeItems.Clear();
        AllNodeStrings.Clear();

        foreach (int i in fav) {
            nodeItems.Add(TODV2.GetItemByID(i));
            AllNodeStrings.Add(i.ToString() + ".prefab");
        }
        ResetCurrentPage();
        BuildNodes();
    }

    public void LoadZoneList() {
        folderLevel = FolderLevel.CustomList;
        nodeItems.Clear();
        AllNodeStrings.Clear();
        nodeItems.Add(TODV2.GetItemByID(14803));
        AllNodeStrings.Add("New Zone.zone");
        foreach (Zone z in WorldBuilderMain.currentWorld.zones) {
            nodeItems.Add(TODV2.GetItemByID(14803));
            string s = z.zoneId + ":" + ((z.name == null || z.name.Length == 0) ? @"<No Name>" : z.name) + ".zone";
            AllNodeStrings.Add(s);
        }

        ResetCurrentPage();
        BuildNodes();
    }

    public void LoadTopCategory(bool forward = true)
    {
        folderLevel = FolderLevel.CategorySelect;
        returnToMenu = true;
        AllNodeStrings.Clear();

        var values = Enum.GetValues(typeof(BundleItem.TopCategory));

            foreach (BundleItem.TopCategory p in values)
            {
                if (p != BundleItem.TopCategory.Staging && p != BundleItem.TopCategory.Environment && p != BundleItem.TopCategory.Other)
                    AllNodeStrings.Add(p.ToString() + ".category");
            }

        AllNodeStrings.Reverse();

        if (forward)
            ResetCurrentPage();
        else
            GetSavedPage();
        BuildNodes();
    }

    public void OpenCategory(string filename, bool forward = true)
    {
        folderLevel = FolderLevel.PickBundle;
        AllNodeStrings.Clear();
        returnToMenu = false;
        this.topCategory = (BundleItem.TopCategory)Enum.Parse(typeof(BundleItem.TopCategory), filename);
        bundleItems = TODV2.GetItemByTopCategory(this.topCategory);



            foreach (BundleItem b in bundleItems)
            {
//                if (File.Exists(Globals.BUNDLEPATH + b.bundle.ToString().ToLower()))

                    AllNodeStrings.Add(b.BundleName.ToString() + ".bundle");
            }

        AllNodeStrings = AllNodeStrings.Distinct().ToList();

        if (forward)
            ResetCurrentPage();
        else
            GetSavedPage();
        BuildNodes();

    }

    private void OpenBundle(string filename, bool forward = true)
    {
        folderLevel = FolderLevel.SubFolderLevelOne;
        AllNodeStrings.Clear();
        currentPath.Add(filename);

        this.bundleName = (BundleItem.BundleNames)Enum.Parse(typeof(BundleItem.BundleNames), filename);
        //  AssetBundleRequest req = AssetBundleManager.RequestBundle(this.bundleName);

        AssetBundleCreateRequest a = AssetBundleManager.GetAssetBundle(this.bundleName);

        ab = a.assetBundle;

        bundleItems = TODV2.GetItemByBundle(this.bundleName);

            foreach (BundleItem b in bundleItems)
            {
                string[] tokens = b.path.Split(',');
                string next;
                if (currentFolderIndex < tokens.Length)
                {
                    next = tokens[currentFolderIndex];
                    if(next.ToLower() != "hidden" || Application.isEditor || Debug.isDebugBuild)
                        AllNodeStrings.Add(next + ".subfolder");
                }
            }
      
        AllNodeStrings = AllNodeStrings.Distinct().ToList();

        if (forward)
            ResetCurrentPage();
        else
            GetSavedPage();

        BuildNodes();
    }

    private void OpenSubFolder(string filename, bool back = false)
    {
        folderLevel = FolderLevel.SubFolder;
        if (!back)
        {
            currentPath.Add(filename);
        }
        nodeItems.Clear();

        AllNodeStrings.Clear();
        foreach (BundleItem b in bundleItems)
        {
            string[] tokens = b.path.Split(',');
            string next;
            try
            {
                string comp = "";
                if (currentFolderIndex < tokens.Length && tokens[currentFolderIndex - 1] == filename)
                {
                    
                    next = tokens[currentFolderIndex];
                    if(next.ToLower() != "hidden" || Application.isEditor || Debug.isDebugBuild) {
                        AllNodeStrings.Add(next + ".subfolder");
                        nodeItems.Add(null);
                    }
                }
                else if (tokens[currentFolderIndex - 1] == filename)
                {
                    AllNodeStrings.Add(b.PrefabName);
                    nodeItems.Add(b);
                }
                
            }
            catch { }
        }
        AllNodeStrings = AllNodeStrings.Distinct().ToList();

        if (!back)
            ResetCurrentPage();
        else
            GetSavedPage();

        BuildNodes();
    }

    public void NextPage()
    {
        currentPage += 1;
        if (currentPage > totalPages)
        {
            currentPage = totalPages;
        }
        else
        {
            BuildNodes();
        }
        UpdatePageText();
    }

    public void LastPage()
    {
        currentPage -= 1;
        if (currentPage < 1)
        {
            currentPage = 1;
        }
        else
        {
            BuildNodes();
        }
        UpdatePageText();

    }

    private void LoadFileExplorer(bool forward = true)
    {
        folderLevel = FolderLevel.FileExplorer;
        AllNodeStrings.Clear();

        path = "";
        string[] drives = Directory.GetLogicalDrives();
        foreach (string d in drives)
        {
            AllNodeStrings.Add(d.Replace(@"\", ".drive"));
        }

        AllNodeStrings.Add(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + ".desktop");
        AllNodeStrings.Add(Application.dataPath + ".folder");

        if (forward)
            ResetCurrentPage();
        else
            GetSavedPage();

        BuildNodes();
    }
    
    public void OpenFolder(string filename, bool addNew = false, bool forward = true)
    {
        folderLevel = FolderLevel.Folder;

        //Check if they passed a full path in, it will have a :\
        if (filename.LastIndexOf(@":") >= 0)
        {
            path = "";
        }

        string startPath = path;
        if (!path.Equals(""))
            path += @"\";
        path += filename;
        try
        {
            LoadFiles(addNew, forward);
        }
        catch
        {
            //manager.UpdateLog("Access denied: " + path);
            path = startPath;

        }

    }

    private void LoadFiles(bool addNew, bool forward)
    {
        AllNodeStrings.Clear();


        if (addNew)
            AllNodeStrings.Add("New.button");

        string[] directories = Directory.GetDirectories(path + @"\");
        foreach (string d in directories)
        {
            int i = d.LastIndexOf(@"\");
            string rhs = i < 0 ? d : d.Substring(i + 1);
            AllNodeStrings.Add(rhs + ".folder");
        }

        string[] files = Directory.GetFiles(path + @"\");
        foreach (string f in files)
        {
            int j = f.LastIndexOf(".meta");
            if (j >= 0)
                continue;
            int i = f.LastIndexOf(@"\");
            string rhs = i < 0 ? f : f.Substring(i + 1);
            AllNodeStrings.Add(rhs);
        }

        if (forward)
            ResetCurrentPage();
        else
            GetSavedPage();

        BuildNodes();
    }

    public void CloseFolder()
    {
        switch (folderLevel)
        {
            case FolderLevel.CustomList:
                LoadTopCategory(false);
                return;
            case FolderLevel.MainMenu:
            case FolderLevel.CategorySelect:
            case FolderLevel.FileExplorer:
                return;
            case FolderLevel.PickBundle:
                LoadTopCategory(false);
                break;
            case FolderLevel.SubFolderLevelOne:
                currentPath.Clear();
                OpenCategory(this.topCategory.ToString(), false);
                break;
            case FolderLevel.SubFolder:
                if (currentFolderIndex > 1)
                {
                    string remove = currentPath[currentFolderIndex];
                    currentFolderIndex--;
                    string next = currentPath[currentFolderIndex];
                    currentPath.Remove(remove);

                    OpenSubFolder(next, true);
                }
                else if (currentFolderIndex == 1)
                {
                    currentFolderIndex--;
                    currentPath.Clear();
                    OpenBundle(this.bundleName.ToString(), false);
                }
                break;
            case FolderLevel.Folder:
                int i = path.LastIndexOf(@"\");
                if (i < 0)
                {
                    LoadFileExplorer(false);
                }
                else
                {
                    path = path.Substring(0, i);
                    LoadFiles(false, false);
                }
                break;
        }
    }
}
