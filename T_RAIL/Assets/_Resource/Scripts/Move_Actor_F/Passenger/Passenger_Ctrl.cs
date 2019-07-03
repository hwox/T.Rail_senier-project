using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger_Ctrl : MonoBehaviour {


    Passenger_Actor pass;

    public bool Passenger_Live; // 태우고 소파에 앉으면 sitpassenger에서 이거 호출


    private void Awake()
    { 
        pass = new Passenger_Actor();
    }

    private void Update()
    {
        if(Passenger_Live)
        {
            



        }
    }

    public void Passenger_LiveOn()
    {

        pass.HP = 100; // HP
        pass.Disease = 0; // 질병수치


        Passenger_Live = true;

    }

}
