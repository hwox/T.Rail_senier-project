using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tunnel_Ctrl : MonoBehaviour {

    public GameObject Tunnel;
    public GameObject DestroyedTunnel;
    public GameObject ElephantFoot;
    public GameObject camera;
    int PassCount = 0;



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(GameValue.ladder_layer))
        {

            Debug.Log("기차 충돌" + PassCount);
            PassCount++;

        }
    }


    // Use this for initialization
    void Start () {
        camera = GameObject.Find("Ending_Camera");
        //camera = GameObject.Find("Camera");
    }
	
	// Update is called once per frame
	void Update () {
        if (PassCount >= TrainGameManager.instance.trainindex)     
        {

            StartCoroutine("ElephantfootAppear");
            PassCount = 0;
        }
    }

    IEnumerator ElephantfootAppear()
    {
        yield return new WaitForSeconds(2.0f);
        ElephantFoot.SetActive(true);
        StartCoroutine("ElephantfootOut");
        StartCoroutine("StartShakeCamera");
    }

    IEnumerator ElephantfootOut()
    {
        
        yield return new WaitForSeconds(3.0f);


        DestroyedTunnel.SetActive(true);
        Tunnel.SetActive(false);
        ElephantFoot.GetComponent<ElephantFoot_Ctrl>().moveFloat = 0;
        ElephantFoot.GetComponent<ElephantFoot_Ctrl>().step = 2;

    }
    IEnumerator StartShakeCamera()
    {
        yield return new WaitForSeconds(0.30f);
        StartCoroutine("ShakeCamera");
    }


    IEnumerator ShakeCamera()
    {
        while (true)
        {
            iTween.ShakeRotation(camera, iTween.Hash("time", 1.2f, "x", 0.5f));
            //iTween.ShakeRotation(camera, iTween.Hash("time", 1.2f, "z", 0.5f));
            yield return new WaitForSeconds(2.8f);
        }
    }
}
