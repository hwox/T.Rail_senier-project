using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger_Ctrl : MonoBehaviour {


    Passenger_Actor pass;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // 근데 gamemanager밑에 child로 넣으면 얘도 dontdestroy를 따로 해줘야 될 필요가 있나?
        pass = new Passenger_Actor();
    }

    // Use this for initialization
  

    public void Passenger_Add()
    {
        // 승객 추가

    }




    // 승객 추가 
    // 1. 역에서 승객이 보인다.(여기저기 서있음) -> 승객을 클릭하면 태울거냐고 Ui 뜸
    // 2. 
}
