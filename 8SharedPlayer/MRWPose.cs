using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class MRWPose : MonoBehaviour, IMRWObjectComponent
{

    [SerializeField]
    public List<SerializableQuaternion> transformRotations = new List<SerializableQuaternion>();


    public void Save()
    {

    }
}