using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //DontDestroy(this.gameObject); 
        DontDestroyOnLoad(this);
        Screen.fullScreen = true;
        Screen.SetResolution(1280, 720, true);
    }
	
}
