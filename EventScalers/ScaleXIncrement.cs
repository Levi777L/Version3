using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleXIncrement : MonoBehaviour, IEventScale
{

    public Transform trigger;
    private int _scale = 0;

    public int GetScale()
    {
        return _scale;
    }

    public void ScaleDown()
    {
        if (trigger.localScale.x > 2f) {
            Vector3 scale = trigger.localScale;
            scale.x -= 2f;
            trigger.localScale = scale;
            _scale--;
        }
    }

    public void ScaleUp()
    {
        Vector3 scale = trigger.localScale;
        scale.x += 2f;
        trigger.localScale = scale;
        _scale++;
    }
}
