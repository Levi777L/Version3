using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DioramaObject : MonoBehaviour
{

    public DioramaObjectSaver dos = new DioramaObjectSaver();

    public List<CharPose> savedCharPose {
        get { return dos.savedCharPose; }
        set { dos.savedCharPose = value; }
    }

    public bool addCollider {
        get { return dos.addCollider; }
        set { dos.addCollider = value; }
    }

    public int todID
    {
        get { return dos.todID; }
        set { dos.todID = value; }
    }

    public int groupNumber
    {
        get { return dos.groupNumber; }
        set { dos.groupNumber = value; }
    }


    public Vector3 savedPos
    {
        get { return dos.savedPos; }
        set { dos.savedPos = value; }
    }

    public Vector3 savedScale
    {
        get { return dos.savedScale; }
        set { dos.savedScale = value; }
    }

    public Quaternion savedRot
    {
        get { return dos.savedRot; }
        set { dos.savedRot = value; }
    }
    
    public Vector3 savedParameters1
    {
        get { return dos.savedParameters1; }
        set { dos.savedParameters1 = value; }
    }
    
    public Vector3 savedParameters2
    {
        get { return dos.savedParameters2; }
        set { dos.savedParameters2 = value; }
    }

    public int layer 
    {
        get { return dos.layer; }
        set { dos.layer = value; }
    }

    public string text {
        get { return dos.text; }
        set { dos.text = value; }
    }
    
    public float gridScale = 1f;

}
