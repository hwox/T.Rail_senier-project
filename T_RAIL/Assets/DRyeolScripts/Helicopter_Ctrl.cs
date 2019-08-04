using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter_Ctrl : MonoBehaviour {

    public GameObject[] propeller;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        for(int i=0;i<propeller.Length;i++)
        {
            propeller[i].transform.Rotate(0, 0.0f, 10.0f);
        }
	}
}
