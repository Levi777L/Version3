using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class MRWDiorama : MonoBehaviour
{ //This goes on the base

    public MRWDiorama(int id)
    {
        this.id = id;
        //Create new Gameobject, add component this
    }

    [SerializeField]
    private int id;

    [SerializeField]
    public string dioramaName;

    [SerializeField]
    public List<MRWObject> objects = new List<MRWObject>();

    [SerializeField]
    public List<MRWViewPoint> viewpoints = new List<MRWViewPoint>();

    public void Save()
    {
        objects = this.gameObject.GetComponentsInChildren<MRWObject>().ToList();
        foreach (MRWObject m in objects)
        {
            m.Save();
        }
    }

    public void AddViewpoint(MRWViewPoint v)
    {
        viewpoints.Add(v);
    }

}