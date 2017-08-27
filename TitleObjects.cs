using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleObjects : MonoBehaviour {

    public GameObject dwLogo;
    public Transform rotator;
    public Node left, main, right;
    public TextMeshPro leftText, mainText, rightText;

    public GameObject go { get { return this.gameObject; } }
}
