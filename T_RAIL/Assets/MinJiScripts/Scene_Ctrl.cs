using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Scene_Ctrl : MonoBehaviourPunCallbacks {

    public playerListController_minj playerListController;
    public Train_Ctrl Train_Ctrl;


	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {

        if (TrainGameManager.instance.Scene_state==1)
        {
           
            if (GameValue.NextStationMeter < Train_Ctrl.Run_Meter)
            {
                photonView.RPC("setRunMeterZero", RpcTarget.All);
                photonView.RPC("StationSceneLoad", RpcTarget.All);
            }
        }
        else if(TrainGameManager.instance.Scene_state == 3)
        {
            photonView.RPC("SetTrainPlayer", RpcTarget.All);
            photonView.RPC("TrainSceneLoad", RpcTarget.All);
            TrainGameManager.instance.Scene_state = 1;
        }

        
	}

    [PunRPC]
    public void SetTrainPlayer()// 기차로 갈때
    {
        Train_Ctrl.Appear();
        for (int i = 0; i < playerListController.playerList.Count; ++i)
        {
            playerListController.playerList[i].player.DownSize();
            playerListController.playerList[i].player.SetTrainPlayer(i);
            playerListController.playerList[i].player.Where_Floor = 1;
            //Debug.Log("id : " + (i) + "  floor : " + playerListController.playerList[i].player.Where_Floor);


            // 승객을 태우기 위해서 호출하는 함수
            TrainGameManager.instance.SofaSitPassengerCtrl.PassengerRideInTrain();

        }

        for (int i = 0; i < 10; ++i)
        {
            TrainGameManager.instance.Station_PassengerManager[i].SetActive(false);
        }
    }
    [PunRPC]
    public void TrainSceneLoad()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Train_Stage1-2");
    }

    [PunRPC]
    public void setRunMeterZero()// 역으로 갈때
    {
        
            Train_Ctrl.Run_Meter = 0;
            Train_Ctrl.Hide();
            TrainGameManager.instance.Scene_state = 2;
            TrainGameManager.instance.GetPassengerCount = 0;
            //Debug.LogError("id : " + (PhotonNetwork.LocalPlayer.ActorNumber - 1 )+ "  floor : " + playerListController.playerList[PhotonNetwork.LocalPlayer.ActorNumber - 1].player.Where_Floor);
            Debug.LogError("몇명 들어와있는지: " + playerListController.playerList.Count);

            for (int i = 0; i < playerListController.playerList.Count; ++i)
            {
                playerListController.playerList[i].player.UpSize();
                playerListController.playerList[i].player.SetStationPlayer(i);
                playerListController.playerList[i].player.Where_Floor = 4;
                Debug.Log("id : " + (i) + "  floor : " + playerListController.playerList[i].player.Where_Floor);

                //photonView.RPC("setPlayerInStationState", RpcTarget.All, i);
            }
        
    }
    [PunRPC]
    public void StationSceneLoad()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Station_Stage1");
        //UnityEngine.SceneManagement.SceneManager.LoadScene("Station_Stage2");
    }

}
