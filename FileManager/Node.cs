//******//
//*Node*//
//******//

using System;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{

    public string fileName;
    private Color original = Color.white;
    public string nodeType;
    public GameObject go;
    public int todID = -1;
    public float gridScale = 1f;

    private bool devNode = false;

    void Start()
    {
        if(go)
            gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    public void Init(FileManagerV2 fm, string fileName, string nodeType)
    {
        this.fileName = fileName;
        this.nodeType = nodeType;
        this.todID = -1;
    }

    public void ActivateNode()
    {
        SL.Get<FileManagerV2>().NodeAction(fileName, nodeType, this);
    }

    public void SoftReset() {
        gameObject.GetComponent<Renderer>().material.color = original;
    }

    public void ResetNode()
    {
        if (go != null)
        {
            this.todID = -1;
            Destroy(go);
        }
        gameObject.GetComponent<Renderer>().material.color = original;
    }

    
    
    public void SetObject(BundleItem t, float sizeMod)
    {
        AssetBundle ab = AssetBundleManager.GetAssetBundle(t.BundleName).assetBundle;
        Destroy(go);
        this.todID = t.id;
        gridScale = t.gridScale;

        if (t.LoadFromResources)
        {
            string path = t.TopFolder.ToString() + @"\";
            path += t.BundleName + @"\" + (t.FolderPath.Replace(",", @"\")) + @"\";
            path += t.prefabName.Replace(".prefab", "");
            go = GameObject.Instantiate(Resources.Load(path, typeof(GameObject))) as GameObject;
        }
        else
        {
            try
            {
                go = Instantiate(ab.LoadAsset(t.PrefabName, typeof(GameObject))) as GameObject;
                Common.ReassignTextures(go, t.HasLight);
            }
            catch (Exception e)
            {
                Debug.Log(t.prefabName + " ID: " + t.id + " failed to load -- " + e.StackTrace);
            }
        }


        //if (t.HasAnimation) {
        CheckAnimation(go, t.SavedInt);
        //}

        go.name = "Test";

        go.transform.parent = this.transform;
        go.transform.localPosition = t.offsetPos;
        go.transform.localRotation = t.offsetRot;
        go.transform.localScale = new Vector3(t.offsetScale * sizeMod, t.offsetScale * sizeMod, t.offsetScale * sizeMod);

        Common.DestroyAll(go);
    }

    private void CheckAnimation(GameObject go, int hash) {

        Animation[] ani = go.GetComponentsInChildren<Animation>();
        foreach (Animation a in ani)
            Destroy(a);

        Animator[] ani2 = go.GetComponentsInChildren<Animator>();
        foreach (Animator a in ani2)
        {
            a.StopPlayback();
            //Destroy(a);
        }

    }
}
