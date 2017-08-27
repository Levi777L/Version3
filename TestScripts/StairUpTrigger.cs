using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairUpTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        Vector3 pos = other.transform.localPosition;
        pos.y += 1f;
        pos.z -= 1.55f;
        other.transform.localPosition = pos;
    }
}
