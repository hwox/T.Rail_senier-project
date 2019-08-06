using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElephantFoot_Ctrl : MonoBehaviour {

    public int step = 1;

    public float moveFloat;
    Vector3 startPosition;
    Vector3 endPosition;
    // Use this for initialization
    void Start () {
        startPosition = new Vector3(93.0f, 88.0f, -8.0f);
        endPosition = new Vector3(63.0f, 35.0f, -8.0f);
    }
	
	// Update is called once per frame
	void Update () {
        if(step==1)
        {
            moveFloat += Time.deltaTime * 3.5f;
            this.transform.position = Vector3.Lerp(startPosition, endPosition, moveFloat);
        }
        else if (step == 2)
        {
            moveFloat += Time.deltaTime * 1.5f;
            this.transform.position = Vector3.Lerp(endPosition, startPosition+new Vector3(-50,0,0), moveFloat);
        }

    }
}
