using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InSofaPassenger : MonoBehaviourPunCallbacks  {


    //Sofa에 달려있는 스크립트인데 
    // 사실상 소파는 승객이 앉아있는지 아닌지만 관리하면 돼서
    // 이렇게 지었다.
    // 가 아니고 Box랑 맞추다보니까 이렇게 만들어졌는데
    // 죄송함ㅠ

    public bool NowSit; // 현재 승객이 앉아있는지?

    [SerializeField]
    //  AllItem_Ctrl allitem;
    SofaSitPassenger_Ctrl sitpassenger;
    GameObject NowSitPassengerObject;
    int thisSofaIndex;


    public void ActiveThisSofa()
    {
        sitpassenger = TrainGameManager.instance.SofaSitPassengerCtrl;
        sitpassenger.AddedSofa(this);
        thisSofaIndex = sitpassenger.passengers.Count - 1;
    }

    public void InActiveThisSofa()
    {
        sitpassenger.DeletedSofa(thisSofaIndex);
        thisSofaIndex = -99; // 일단 어떻게 사용할지 몰라서 쓰레기값 넣어주기 
    }

    public void SitPassenger(GameObject _passenger)
    {

        NowSitPassengerObject = _passenger;

        NowSitPassengerObject.transform.parent = this.transform;
        NowSitPassengerObject.SetActive(true);
        NowSitPassengerObject.GetComponent<Passenger_Ctrl>().Passenger_LiveOn();

        Quaternion rot = Quaternion.identity;
        rot.eulerAngles = new Vector3(0, 90, 0);
        NowSitPassengerObject.transform.rotation = rot;

        // 포지션 기본값은 혀재 ㅇㅣ 오브젝트에서 받아와야지
        NowSitPassengerObject.transform.localPosition = new Vector3(0, 0.01f, 0.0085f);

      
        NowSit = true;

    }

    public void SofaHaveNotPassenger()
    {
        NowSit = false;
    }
}
