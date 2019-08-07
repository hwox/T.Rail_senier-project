using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter_Ctrl : MonoBehaviour
{

    public GameObject[] propeller;
    public GameObject door;
    int PassCount = 0;
    // Use this for initialization
    void Start()
    {
      //  TrainGameManager.instance.SoundManager.
      //  TrainGameManager.instance.Soundmanager.TrainWhistl_Sound_Play();
    }



    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.layer.Equals(GameValue.ladder_layer))
        {

            Debug.Log("기차 충돌"+ PassCount);
            PassCount++;
           
        }
    }

        // Update is called once per frame
    void Update()
    {
        iTween.ShakePosition(gameObject, iTween.Hash("time", 0.5f, "y", 0.05f));
       
        for (int i = 0; i < propeller.Length; i++)
        {
            propeller[i].transform.Rotate(0, 0.0f, 10.0f);
        }
        if (PassCount >= TrainGameManager.instance.trainindex)
        {
           
            StartCoroutine("closeDoor");
            PassCount=0;
        }
    }
    IEnumerator closeDoor()
    {
        door.GetComponent<Animator>().SetBool("close", true);
        yield return new WaitForSeconds(7.0f);
    }
}
