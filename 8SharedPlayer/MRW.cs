using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MixedRealityWorld {

  [SerializeField]
  public string name;

  [SerializeField]
  public List<Diorama> dioramas = new List<Diorama>();
  
}

[Serializable]
public class Diorama {
  
  [SerializeField]
  public List<MRWObject> objects = new List<MRWObject>();
  
  [SerializeField]
  public List<ViewPoint> viewpoints = new List<ViewPoint>();
  
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
public class MRWObject : Monobehaviour {

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
