using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteMySelf : MonoBehaviour {

    public int destroySecond;


	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, destroySecond);
	}
}
