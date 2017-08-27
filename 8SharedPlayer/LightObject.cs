using UnityEngine;
using System.Collections.Generic;

public class LightObject : MonoBehaviour
{
    public Light light;
    public float savedIntensity;
    public float savedRange;
    public Color savedColor;
    public float lastSize = 1f;

    public void Setup(Light l, float i, float r, Vector3 c, int mask, bool shadows)
    {
        light = l;
        l.bounceIntensity = 0;
        if (shadows)
        {
            l.shadows = LightShadows.Hard;
        }
        else
        {
            l.shadows = LightShadows.None;
        }
        savedIntensity = i;
        savedRange = r;
        savedColor = Common.GetColor(c.x);
        light.color = savedColor;
        light.intensity = i;
        light.renderMode = LightRenderMode.ForcePixel;
        light.cullingMask = mask;
    }

    public void Scale(float size)
    {
        lastSize = size;
        light.range = savedRange * size;
    }

    public void SetColor(Color c)
    {
        savedColor = c;
        light.color = c;
    }

    public void SetIntensity(float i)
    {
        light.intensity = i;
    }

    public void SetRange(float r)
    {
        light.range = r * lastSize;
    }

    public void SetMask(int mask)
    {
        light.cullingMask = mask;
    }

}
