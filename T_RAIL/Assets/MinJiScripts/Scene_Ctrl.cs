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
            
        Debug.Log(GameValue.NextStationMeter);
        Debug.Log(Train_Ctrl.Run_Meter);
        if (!TrainGameManager.instance.InStation)
        {
           
            if (GameValue.NextStationMeter < Train_Ctrl.Run_Meter)
            {
                photonView.RPC("setRunMeterZero", RpcTarget.All);
                photonView.RPC("StationSceneLoad", RpcTarget.All);
            }
        }
        
	}


    [PunRPC]
    public void setRunMeterZero()
    {
        Train_Ctrl.Run_Meter = 0;
        Train_Ctrl.Hide();
        TrainGameManager.instance.InStation = true;
        //Debug.LogError("id : " + (PhotonNetwork.LocalPlayer.ActorNumber - 1 )+ "  floor : " + playerListController.playerList[PhotonNetwork.LocalPlayer.ActorNumber - 1].player.Where_Floor);
        Debug.LogError("몇명 들어와있는지: " + playerListController.playerList.Count);

        for (int i = 0; i < playerListController.playerList.Count; ++i)
        {
            playerListController.playerList[i].player.UpSize();
            playerListController.playerList[i].player.DownPos();
            playerListController.playerList[i].player.Where_Floor = 4;
            Debug.Log("id : " + (i) + "  floor : " + playerListController.playerList[i].player.Where_Floor);

            //photonView.RPC("setPlayerInStationState", RpcTarget.All, i);
        }
    }

    [PunRPC]
    public void setPlayerInStationState(int id)
    {
        playerListController.playerList[id].player.UpSize();
        playerListController.playerList[id].player.DownPos();
        playerListController.playerList[id].player.Where_Floor = 4;
        Debug.Log("id : " + (id) + "  floor : " + playerListController.playerList[id].player.Where_Floor);
    }

    [PunRPC]
    public void StationSceneLoad()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Station_Stage1");
    }
   
}
