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


    public IEnumerator LoadDioAsync(List<DioramaObjectSaver> dos, Transform attachment)
    {
        loading = true;

        for (int i = 0; i < dos.Count; i++)
        {
           StartCoroutine(RebuildGo(dos[i], attachment, true));
            yield return null;

        }

        loading = false;
    }

    IEnumerator RebuildGo(DioramaObjectSaver dos, Transform transform, bool addCollider = true)
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

}
