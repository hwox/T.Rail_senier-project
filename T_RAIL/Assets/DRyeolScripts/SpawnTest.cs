﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTest : MonoBehaviour
{

    public GameObject[] Spawn;
   
    void Start()
    {
        for (int i = 0; i <4; i++)
        {
            GameObject temp = TrainGameManager.instance.GetObject(2);
            temp.SetActive(true);
            temp.transform.position = Spawn[i].transform.position;

        }

    }
}
	
	
