using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MixedRealityWorld {

  [SerializeField]
  public string name;

  [SerializeField]
  public List<Diorama> dioramas = new List<Diorama>();
  
  private Diorama current;
  
  public void AddNewDiorama() {
    current.Save();
    current = new Diorama(dioramas.Count);
    dioramas.Add(current);
  }
  
  public void Save() {
        string path = Globals.WORLDPATH;
        path += name + ".mrw";

        try
        {
          current.Save();
            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, (object)this);
                File.WriteAllBytes(path, stream.ToArray());
                File.WriteAllBytes(Globals.BACKUPS + currentWorld.name + " " + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd hh-mm-ss") + ".mrw", stream.ToArray());
            }
        }
        catch {
            manager.SetToolTip("Save Failed");
        }
        
        //Get manager from SL
        manager.SetToolTip(currentWorld.name + " Successfully Saved at " + DateTime.Now.ToString("hh:mm:ss"));
        manager.lastSaveTime = DateTime.Now;
  }
}

[Serializable]
public class Diorama : Monobehaviour { //This goes on the base
  
  public Diorama(int id) {
    this.id = id;
    //Create new Gameobject, add component this
  }
  
  [SerializeField]
  private int id;
  
  [SerializeField]
  public string name;
  
  [SerializeField]
  public List<MRWObject> objects = new List<MRWObject>();
  
  [SerializeField]
  public List<ViewPoint> viewpoints = new List<ViewPoint>();
  
  public void Save() {
    objects = FindComponentsInChildren<MRWObject>().ToList();
  }
  
  public void AddViewpoint(ViewPoint v) {
    viewpoints.Add(v);
  }
  
}

[Serializable]
public class ViewPoint {

  [SerializeField]
  public SerializableVector3 relativePos;
  
  [SerializeField]
  public SerializableQuaternion relativeRot;
  
  [SerializeField]
  public SerializableVector3 relativeScale;

}

[Serializable]
public class MRWObject : Monobehaviour { //This goes on each object

  [SerializeField]
  public int todID = -1;
  
  [SerializeField]
  pulbic int uniqueID = -1;
  
  [SerializeField]
  public SerializableVector3 savedLocalPos;
  
  [SerializeField]
  public SerializableQuaternion savedLocalRot;
  
  [SerializeField]
  public SerializableVector3 savedLocalScale;
  
  [SerializeField]
  public int layer = -1;

  [SerializeField]
  public List<object> components = new List<object>();

}

[Serializable]
public class MRWPose {

  [SerializeField]
  public List<SerializableQuaternion> transformRotations = new List<SerializableQuaternion>();
  
}

[Serializable]
public class MRWChat {

  [SerializeField]
  public string chatText = "";
  
  [SerializeField]
  public float colorShift;
  
}


[Serializable]
public class MRWEvent {

}
