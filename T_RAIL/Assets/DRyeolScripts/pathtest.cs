using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathtest : MonoBehaviour {


    iTweenPath itp;
    
	// Use this for initialization
	void Start () {

        itp = GetComponent<iTweenPath>();

        itp.SetNode(0, new Vector3(10.0f, 0.0f, 0.0f));

        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("New Path 1"), "time", 7));
        
     
    }
	
	// Update is called once per frame      
	void Update () {
		
	}
}
