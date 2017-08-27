using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PosedCharacter
{
    [SerializeField]
    public int todID = -1;
    
    [SerializeField]
    public SerializableVector3 savedPos;
    [SerializeField]
    public SerializableQuaternion savedRot;
    [SerializeField]
    public SerializableVector3 savedScale;
    
    [SerializeField]
    public List<CharPose> savedCharPose;
}

[Serializable]
public class CharPose
{
    [SerializeField]
    public SerializableVector3 savedPos;
    [SerializeField]
    public SerializableQuaternion savedRot  = Quaternion.identity;
}
