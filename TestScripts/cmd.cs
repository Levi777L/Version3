using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cmd : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            Debug.Log("ARG " + i + ": " + args[i]);
            if (args[i].ToLower() == "-oculus")
            {
                SceneManager.LoadScene("Rift");
                return;
            }
        }
        SceneManager.LoadScene("Vive");
    }

}
