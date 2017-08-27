using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;

public class Loader : MonoBehaviour
{

    private bool loading;
    public bool LoaderRunning {
        get { return loading; }
    }

    #region LoadingAsync


    public IEnumerator LoadDioAsync(GameManager manager, List<DioramaObjectSaver> dos, Transform attachment)
    {
        loading = true;

        for (int i = 0; i < dos.Count; i++)
        {
           manager.StartCoroutine(RebuildGo(manager, dos[i], attachment, true));
            yield return null;

        }

        loading = false;
    }

    IEnumerator RebuildGo(GameManager manager, DioramaObjectSaver dos, Transform transform, bool addCollider = true)
    {
        GameObject go = null;

        if(dos.todID >= 0)
        {
            BundleItem t = TODV2.GetItemByID(dos.todID);

            string path = t.TopFolder.ToString() + @"\";
            path += t.BundleName + @"\" + (t.FolderPath.Replace(",", @"\")) + @"\";
            path += t.prefabName.Replace(".prefab", "");

            if (t.loadFromResources)
            {
                go = GameObject.Instantiate(Resources.Load(path, typeof(GameObject))) as GameObject;
            }
            else
            {
                try
                {
                    AssetBundle ab = AssetBundleManager.GetAssetBundle(t.BundleName).assetBundle;
                    go = Instantiate(ab.LoadAsset(t.PrefabName, typeof(GameObject))) as GameObject;
                    Common.ReassignTextures(go, t.HasLight);
                }
                catch (Exception e)
                {
                    Debug.Log(t.prefabName + " ID: " + t.id + " failed to load -- " + e.StackTrace);
                }
            }
            Common.ReassignTextures(go, t.HasLight);

            go.name = "Object";
            go.transform.parent = transform;
            go.transform.localPosition = dos.savedPos;
            go.transform.localRotation = dos.savedRot;
            go.transform.localScale = dos.savedScale;
            

            DioramaObject newDio = go.AddComponent<DioramaObject>();
            newDio.todID = dos.todID;
            newDio.savedPos = dos.savedPos;
            newDio.savedRot = dos.savedRot;
            newDio.savedScale = dos.savedScale;

            newDio.gridScale = t.gridScale;
            newDio.layer = dos.layer == 0 ? 1 : dos.layer;

            Common.DestroyAll(go);

            CheckAnimation(go);
            List<CharPose> pose = dos.savedCharPose;
            Transform[] allTrans = go.GetComponentsInChildren<Transform>();
            for (int i = 0; i < allTrans.Count(); i++)
            {
                CharPose next = pose[i];
                allTrans[i].localPosition = next.savedPos;
                allTrans[i].localRotation = next.savedRot;
            }

            TextMeshPro tmp = go.GetComponentInChildren<TextMeshPro>();
            if (tmp)
            {
                tmp.text = dos.text;
            }

            if (go.tag == "Untagged")
            {
                go.tag = "TableObject";
            }

            //IEventScale ies = go.GetComponentInChildren<IEventScale>();
            //if (ies != null)
            //{
            //    go.transform.localScale = Vector3.one;
            //    for (int i = 0; i < dos.savedScale.x; i++)
            //    {
            //        ies.ScaleUp();
            //    }
            //}
        }
        yield return null;
    }

    private void CheckAnimation(GameObject go)
    {
        Animation[] ani = go.GetComponentsInChildren<Animation>();
        foreach (Animation a in ani)
            DestroyImmediate(a);

        Animator[] ani2 = go.GetComponentsInChildren<Animator>();
        foreach (Animator a in ani2)
        {
            DestroyImmediate(a);
        }

    }
    #endregion

    #region LegacyLoader
    //public IEnumerator LoadV2(List<DioramaObjectSaver> dos, Transform t)
    //{
    //    loading = true;
        
    //    //Prewarm all bundles used
    //    List<BundleItem.BundleNames> allBundles = new List<BundleItem.BundleNames>();
    //    for (int i = 0; i < dos.Count; i++)
    //    {
    //        BundleItem bi = TODV2.GetItemByID(dos[i].todID);

    //        if (bi != null && bi.id > 0 && !bi.loadFromResources)
    //        {
    //            if (File.Exists(Globals.BUNDLEPATH + bi.bundle.ToString().ToLower()))
    //            {
    //                allBundles.Add(bi.bundle);

    //            }
    //            else
    //            {
    //                loading = false;
    //                yield break;
    //            }
    //        }

    //    }
    //    allBundles = allBundles.Distinct().ToList();
    //    int counter = 1;
    //    foreach (BundleItem.BundleNames BN in allBundles)
    //    {
    //        while (!AssetBundleManager.GetAssetBundle(BN).isDone)
    //        {
    //            yield return new WaitForSeconds(1);
    //        }
    //        counter++;
    //    }



    //    //Build each object  (First object is always the diorama transform
    //    for (int i = 1; i < dos.Count; i++)
    //    {
    //        if (dos[i].todID != -1)
    //        {
    //            yield return StartCoroutine(RebuildGoLegacy(dos[i], t.transform, true, dos[i].groupNumber));
    //        }
    //    }

    //    loading = false;

    //}

    //IEnumerator RebuildGoLegacy(DioramaObjectSaver dos, Transform transform, bool addCollider = true, int thisGroupNumber = 0)
    //{
    //    GameObject go;
    //    Transform groupParent = null;

    //    if (groupParent == null)
    //        groupParent = transform;

    //    if (dos.todID == -2 )
    //    {

    //    }
    //    else if (dos.todID == -1)
    //    {

    //    }
    //    else
    //    {
    //        BundleItem t = TODV2.GetItemByID(dos.todID);

    //        AssetBundleCreateRequest a = AssetBundleManager.GetAssetBundle(t.BundleName);

    //        while (!a.isDone)
    //            yield return new WaitForSeconds(1);

    //        AssetBundle ab = a.assetBundle;

    //        if (t.LoadFromResources)
    //        {
    //            string path = t.TopFolder.ToString() + @"\";
    //            path += t.BundleName + @"\" + (t.FolderPath.Replace(",", @"\")) + @"\";
    //            path += t.prefabName.Replace(".prefab", "");
    //            go = GameObject.Instantiate(Resources.Load(path, typeof(GameObject))) as GameObject;
    //        }
    //        else
    //        {
    //            go = MonoBehaviour.Instantiate(ab.LoadAsset(t.PrefabName, typeof(GameObject))) as GameObject;
    //        }

    //        Common.ReassignTextures(go, t.HasLight);

    //        go.name = "Object";
    //        go.transform.parent = groupParent;
    //        go.transform.localPosition = dos.savedPos;
    //        go.transform.localRotation = dos.savedRot;
    //        go.transform.localScale = dos.savedScale;

    //        DioramaObject newDio = go.AddComponent<DioramaObject>();
    //        newDio.todID = dos.todID;
    //        newDio.savedPos = dos.savedPos;
    //        newDio.savedRot = dos.savedRot;
    //        newDio.savedScale = dos.savedScale;
    //        newDio.groupNumber = thisGroupNumber;
    //        newDio.gridScale = t.gridScale;
    //        newDio.layer = dos.layer == 0 ? 1 : dos.layer;

    //        Common.DestroyAll(go);


    //        CheckAnimation(go);
    //        List<CharPose> pose = dos.savedCharPose;
    //        Transform[] allTrans = go.GetComponentsInChildren<Transform>();
    //        for (int i = 0; i < allTrans.Count(); i++)
    //        {
    //            CharPose next = pose[i];
    //            allTrans[i].localPosition = next.savedPos;
    //            allTrans[i].localRotation = next.savedRot;
    //        }


    //        TextMeshPro tmp = go.GetComponentInChildren<TextMeshPro>();
    //        if (tmp)
    //        {
    //            tmp.text = dos.text;
    //        }
    //    }

    //}


#endregion
}
