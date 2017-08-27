using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleLadder : MonoBehaviour, IEventScale {

    public List<GameObject> steps;
    public GameObject firstStep, lastStep;
    private int _scale = 0;

    public int GetScale() {
        return _scale;
    }

    public void ScaleUp() {
        Vector3 pos = steps[steps.Count - 1].transform.localPosition;
        pos.y += 0.3f;
        GameObject nextStep = Instantiate(steps[0], steps[steps.Count - 1].transform.position, steps[steps.Count - 1].transform.rotation, this.transform);
        nextStep.transform.localPosition = pos;
        pos = lastStep.transform.localPosition;
        pos.y += 0.3f;
        lastStep.transform.localPosition = pos;
        steps.Add(nextStep);
        _scale++;
    }

    public void ScaleDown() {
        if (steps.Count > 1)
        {
            GameObject removeStep = steps[steps.Count - 1];
            steps.Remove(removeStep);
            DestroyImmediate(removeStep);
            Vector3 pos = lastStep.transform.localPosition;
            pos.y -= 0.3f;
            lastStep.transform.localPosition = pos;
            _scale--;
        }
    }

}
