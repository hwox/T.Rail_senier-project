using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTest : MonoBehaviour
{

    public GameObject[] Spawn;
   
    void Start()
    {
        
        for (int i = 0; i <5 ; i++)
        {
            Debug.Log(TrainGameManager.instance.Station_PassengerManager[i] + "dddddddd");
                TrainGameManager.instance.Station_PassengerManager[i].SetActive(true);
                TrainGameManager.instance.Station_PassengerManager[i].transform.position = Spawn[i].transform.position;
            
        }

    }
}
	
	
