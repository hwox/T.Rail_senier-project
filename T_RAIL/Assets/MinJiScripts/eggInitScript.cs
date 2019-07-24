using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eggInitScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.transform.localPosition = Vector3.zero;
        this.gameObject.SetActive(false);
        this.transform.parent = TrainGameManager.instance.gameObject.transform.GetChild(7);
        TrainGameManager.instance.EggManager.Add(this.gameObject);
    }
}
