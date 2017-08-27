using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScale : MonoBehaviour, IEventScale
{
    public int GetScale()
    {
        return 0;
    }

    void IEventScale.ScaleDown()
    {
    }

    void IEventScale.ScaleUp()
    {
    }

}
