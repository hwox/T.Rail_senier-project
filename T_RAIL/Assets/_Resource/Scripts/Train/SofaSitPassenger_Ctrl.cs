using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SofaSitPassenger_Ctrl : MonoBehaviour
{

    // 이 스크립트는 현재 소파에 앉아있는 승객을 '총'관리하는 스크립트임
    // 각각 개별의 승객 관리는 Passenger_Ctrl에서 따로 함
   

    // 현재 소파에 앉아있는 승객을 총관리
    public List<InSofaPassenger> passengers = new List<InSofaPassenger>();

    int sofaNubmer; // 소파 총 몇개인지


    // Use this for initialization
    void Start()
    {

    }



    public void AddedSofa(InSofaPassenger insofa)
    {
        passengers.Add(insofa);
        sofaNubmer += 1;
    }
    public void DeletedSofa(int index)
    {
        passengers.RemoveAt(index);
        sofaNubmer -= 1;
    }

    public void PassengerRideInTrain()
    {
        // int temp = TrainGameManager.instance.GetPassengerCount;
        //int temp = 1;
        //if (temp >= 0)
        //{
        //    for (int i = 0; i < temp; i++)
        //    {

        //        for (int j = 0; j < passengers.Count; j++)
        //        {
        //            if (!passengers[j].NowSit)
        //            {
        //                passengers[j].SitPassenger(TrainGameManager.instance.GetObject(1));
        //                break;
        //            }
        //        }


        //    }

        //    TrainGameManager.instance.GetPassengerCount = 0; // 다시 0으로 
        //}
        int temp = TrainGameManager.instance.GetPassengerCount;
        if (temp >= 0)
        {
            for (int i = 0; i < temp; i++)
            {

                for (int j = 0; j < passengers.Count; j++)
                {
                    if (!passengers[j].NowSit)
                    {
                        passengers[j].SitPassenger(TrainGameManager.instance.GetObject(1));
                        break;
                    }
                }
            }
            TrainGameManager.instance.GetPassengerCount = 0; // 다시 0으로 
        }
    }

}
