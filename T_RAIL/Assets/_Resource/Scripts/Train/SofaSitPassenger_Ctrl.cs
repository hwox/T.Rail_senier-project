using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SofaSitPassenger_Ctrl : MonoBehaviour
{



    // 현재 소파에 앉아있는 승객을 총관리
    public List<InSofaPassenger> passengers = new List<InSofaPassenger>();

    int sofaNubmer; // 소파 총 몇개인지


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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
        int temp = 2;
        if (temp >= 0)
        {
            for (int i = 0; i < temp; i++)
            {
                // 다음 해야될일?
                // 승객 클릭하면 승객 속성 고나리같은거?
                // 그리고 승객 hp 줄어들고 질병ㅇ수치 관리하는거

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
