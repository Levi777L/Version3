using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using System.Collections;
using TMPro;

[Serializable]
public class TempItem { public int key; public string value; }

public class VRFileManager : MonoBehaviour
{

    public static Dictionary<int, string> allFiles = new Dictionary<int, string>();

    void Awake()
    {
        LoadDatabase();
        LoadFolderStructure();        
    }

    private void LoadFolderStructure() {
        
    }

    private void LoadDatabase() {
        int k = Application.dataPath.LastIndexOf("/");
        string path = Application.dataPath.Substring(0, k) + "/fm.xml";

        List<TempItem> temp = new List<TempItem>();
        SL.Load(ref temp, path);
        foreach (TempItem t in temp)
        {
            allFiles.Add(t.key, t.value);
        }
        temp.Clear();

        if (Application.isEditor)
        {
            string resourcepath = Application.dataPath + @"/Resources/";
            //Get All Files, TO DO Add resource path, strip resource path)
            string[] files = Directory.GetFiles(resourcepath, "*.prefab", SearchOption.AllDirectories);

            //Add new files
            foreach (string s in files)
            {
                string cut = s.Replace(resourcepath, "");
                if (!allFiles.ContainsValue(cut))
                {
                    allFiles.Add(allFiles.Count + 1, cut);
                }
            }

            //Find all files to remove
            List<int> removes = new List<int>();
            foreach (var v in allFiles)
            {
                string uncut = resourcepath + v.Value;
                if (!files.Contains(uncut))
                {
                    removes.Add(v.Key);
                }
            }

            //Remove Files
            foreach (int i in removes)
            {
                allFiles.Remove(i);
            }

            //Save
            foreach (var v in allFiles)
            {
                var t = new TempItem();
                t.key = v.Key;
                t.value = v.Value;
                temp.Add(t);
            }
            temp.SaveFile(path);
            temp.Clear();

        }
    }
}
