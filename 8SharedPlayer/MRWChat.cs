using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;

[Serializable]
public class MRWChat : MonoBehaviour, IMRWObjectComponent
{
    private static Color32 CYAN = new Color(0, 1, 1);//cyan
    private static Color YGREEN = new Color(0.5f, 1, 0); //yellow green
    private static Color FIRE = new Color(0.913f, 0.647f,0.301f);
    private static Color PURPLE = new Color(0.913f, 0.301f, 0.913f); //purple

    [SerializeField]
    private string _chatText = "";
    public string ChatText {
        get { return _chatText; }
        set { _chatText = value; }
    } 

    [SerializeField]
    private float _colorShift;
    public float ColorShift {
        get { return _colorShift; }
        set {
            float clamp = value;
            if (clamp > 360)
                clamp -= 360f;
            else if (clamp < 0)
                clamp += 360f;
            _colorShift = clamp;
            ShiftColor();
        }
    }

    public void Save()
    {

    }

    public void ShiftColor()
    {
        TextMeshPro tmp = this.GetComponentInChildren<TextMeshPro>();

        if (tmp)
        {
            if (_colorShift < 90)
            {
                tmp.faceColor = Color.Lerp(CYAN, YGREEN, (_colorShift - 0) / 90f);
            }
            else if (_colorShift < 180)
            {
                tmp.faceColor = Color.Lerp(YGREEN, FIRE, (_colorShift - 90) / 90f);
            }
            else if (_colorShift < 270)
            {
                tmp.faceColor = Color.Lerp(FIRE, PURPLE, (_colorShift - 180) / 90f);
            }
            else
            {
                tmp.faceColor = Color.Lerp(PURPLE, CYAN, (_colorShift - 270) / 90f);
            }
        }
    }


}