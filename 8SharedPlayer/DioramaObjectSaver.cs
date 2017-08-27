using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DioramaObjectSaver
{
    [SerializeField]
    public int todID = -1;
    [SerializeField]
    public List<CharPose> savedCharPose;
    public bool addCollider = false;
    [SerializeField]
    public SerializableVector3 savedPos;
    [SerializeField]
    public SerializableQuaternion savedRot;
    [SerializeField]
    public SerializableVector3 savedScale;
    [SerializeField]
    public SerializableVector3 savedParameters1;
    [SerializeField]
    public SerializableVector3 savedParameters2;
    [SerializeField]
    public int layer;
    [SerializeField]
    public int groupNumber;
    [SerializeField]
    public string text = string.Empty;
}
