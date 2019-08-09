using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conParticleScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.transform.localPosition = Vector3.zero;
        this.gameObject.SetActive(false);
        this.transform.parent = TrainGameManager.instance.gameObject.transform.GetChild(8);
        TrainGameManager.instance.CoinParticle.Add(this.gameObject);
    }
    
}
