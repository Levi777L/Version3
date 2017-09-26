using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class MixedRealityWorld : MonoBehaviour
{

    [SerializeField]
    public string worldName;

    [SerializeField]
    public List<MRWDiorama> dioramas = new List<MRWDiorama>();

    private MRWDiorama current;

    public void AddNewDiorama()
    {
        current.Save();
        current = new MRWDiorama(dioramas.Count);
        dioramas.Add(current);
    }

    public void LoadDiorama()
    {

    }

    public void Save()
    {
        current.Save();

        string path = Globals.WORLDPATH;
        path += this.worldName + ".mrw";
        string worldName = "Test";
        try
        {
            current.Save();
            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, (object)this);
                File.WriteAllBytes(path, stream.ToArray());
                File.WriteAllBytes(Globals.BACKUPS + worldName + " " + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd hh-mm-ss") + ".mrw", stream.ToArray());
            }
        }
        catch
        {
            //manager.SetToolTip("Save Failed");
        }

        //Get manager from SL
        //manager.SetToolTip(currentWorld.name + " Successfully Saved at " + DateTime.Now.ToString("hh:mm:ss"));
        //manager.lastSaveTime = DateTime.Now;
    }
}

public interface IMRWObjectComponent
{
    void Save();
}

[Serializable]
public class MRWViewPoint
{

    [SerializeField]
    public SerializableVector3 relativePos;

    [SerializeField]
    public SerializableQuaternion relativeRot;

    [SerializeField]
    public SerializableVector3 relativeScale;

}



[Serializable]
public class MRWEvent
{

}
