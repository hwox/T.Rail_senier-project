using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfShake : MonoBehaviour {


    public float shakedelay = 0.0f;
    public float shaketime = 0.0f;
    public float shakeY = 0.0f;


  
    void shakeDelay()
    {
        iTween.ShakePosition(this.gameObject, iTween.Hash("time", shaketime, "y", shakeY, "easetype", iTween.EaseType.easeInOutBack));

    }

    private void OnEnable()
    {
        InvokeRepeating("shakeDelay", shakedelay, shakedelay);
    }


    private void OnDisable()
    {
        CancelInvoke("shakeDelay");

    }
}

