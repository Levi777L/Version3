using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class MRWObject : MonoBehaviour
{

    [SerializeField]
    public int todID = -1;

    [SerializeField]
    public int uniqueID = -1;

    [SerializeField]
    public SerializableVector3 savedLocalPos;

    [SerializeField]
    public SerializableQuaternion savedLocalRot;

    [SerializeField]
    public SerializableVector3 savedLocalScale;

    [SerializeField]
    public int layer = -1;

    [SerializeField]
    public List<IMRWObjectComponent> components = new List<IMRWObjectComponent>();

    public void Save()
    {
        savedLocalPos = this.gameObject.transform.localPosition;
        savedLocalRot = this.gameObject.transform.localRotation;
        savedLocalScale = this.gameObject.transform.localScale;

        components = this.GetComponentsInChildren<IMRWObjectComponent>().ToList();
        foreach (IMRWObjectComponent c in components)
        {
            c.Save();
            components.Add(c);
        }
    }

}