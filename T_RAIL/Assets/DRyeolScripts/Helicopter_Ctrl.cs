using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter_Ctrl : MonoBehaviour
{

    public GameObject[] propeller;
    public GameObject door;

    // Use this for initialization
    void Start()
    {
       // StartCoroutine("closeDoor");
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < propeller.Length; i++)
        {
            propeller[i].transform.Rotate(0, 0.0f, 10.0f);
        }
    }
    IEnumerator closeDoor()
    {
        door.GetComponent<Animator>().SetBool("close", true);
        yield return new WaitForSeconds(15.0f);
    }
}
