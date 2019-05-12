using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTest : MonoBehaviour
{

    public GameObject[] Spawn;
   
    void Start()
    {
        int dontactivenum = Random.Range(1, Spawn.Length);
        for (int i = 0; i < Spawn.Length; i++)
        {

            if (i != dontactivenum)
            {
                TrainGameManager.instance.Station_PassengerManager[i].SetActive(true);
                TrainGameManager.instance.Station_PassengerManager[i].transform.position = Spawn[i].transform.position;
            }
        }

    }
}
	
	
