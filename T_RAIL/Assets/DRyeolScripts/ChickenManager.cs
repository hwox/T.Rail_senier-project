using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenManager : MonoBehaviour {

    public GameObject[] Spawn;
    public GameObject[] dest;
    


    void Start()
    {

        for (int i = 0; i < Spawn.Length; i++)
        {
            GameObject temp = TrainGameManager.instance.GetObject(6);
            temp.SetActive(true);
            temp.transform.position = Spawn[i].transform.position;
        }


    }


}
