using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UIState_Ctrl : MonoBehaviourPunCallbacks
{

    public GameObject[] TrainUI;
    public playerListController_minj playerListController;

    // Use this for initialization
    void Start()
    {
        photonView.RPC("onTrainScrollBar", RpcTarget.All);
        //onTrainScrollBar();
    }

    public void CallRPConTrainScrollBar()
    {
        photonView.RPC("onTrainScrollBar", RpcTarget.All);
    }


    //userID가 0이고 userWhere가 1이면 --> 기차 첫번째 칸의, userID에 해당하는 색깔을 SetActive해라
    [PunRPC]
    public void onTrainScrollBar()//(int userID, int userWhere)
    {
        for (int i = 0; i < 13; ++i)
        {
            for (int j = 0; j < 5; ++j)
            {
                TrainUI[i].transform.GetChild(j + 1).gameObject.SetActive(false);
            }
        }

        //꺼져있는 기차 x 표
        for (int i = TrainGameManager.instance.trainindex; i < 13; ++i) 
        {
            TrainUI[i].transform.GetChild(5).gameObject.SetActive(true);
        }

        //플레이어의 숫자만큼 돌면서 각자가 어디있는지 확인
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
        {
            if (playerListController.eachPlayerIn[i] != 0)
                TrainUI[playerListController.eachPlayerIn[i] - 1].transform.GetChild(i + 1).gameObject.SetActive(true);
        };

    }
}
