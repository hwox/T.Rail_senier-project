using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTest : MonoBehaviour
{

    public GameObject[] Spawn;
   
    void Start()
    {
        for (int i = 0; i <4; i++)
        {
            if (Random.Range(0, 2) % 2 == 0)
            {
                Debug.Log(TrainGameManager.instance.Station_PassengerManager[i] + "dddddddd");
            //Debug.Log(TrainGameManager.instance.Station_PassengerManager[i] + "dddddddd");
                TrainGameManager.instance.Station_PassengerManager[i].SetActive(true);
                TrainGameManager.instance.Station_PassengerManager[i].transform.position = Spawn[i].transform.position;
            }
            
        }

    }
}
	
	
