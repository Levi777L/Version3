using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Keyboard : MonoBehaviour {

    public TextMeshPro finalText;
    private bool hasCaret = false;

    public void AddChar(string s) {
        RemoveCaret();
        finalText.text += s;
    }

    public void BackSpace() {
        RemoveCaret();
        if (finalText.text.Length > 0)
            finalText.text = finalText.text.Substring(0, finalText.text.Length - 1);
    }

    public String Enter()
    {
        RemoveCaret();
        string s = finalText.text;
        finalText.text = string.Empty;
        return s;
    }
    public void Space() {
        RemoveCaret();
        finalText.text += " ";
    }

    public void ToggleCaret()
    {
        if (hasCaret)
        {
            RemoveCaret();
        }
        else {
            AddCaret();
        }
    }

    private void AddCaret() {
        AddChar("_");
        hasCaret = true;
    }

    private void RemoveCaret() {
        if (hasCaret) {
            if (finalText.text.Length > 0)
                finalText.text = finalText.text.Substring(0, finalText.text.Length - 1);
            hasCaret = false;
        }
    }
}
