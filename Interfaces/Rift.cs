//******//
//*Rift*//
//******//

using UnityEngine;
using System.Collections;
using System;

public class Rift : MonoBehaviour, IVRHmd
{
    public void Init()
    {

    }

    public void Recenter()
    {
        OVRManager.display.RecenterPose();
    }


}