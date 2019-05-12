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
                UnityEngine.SceneManagement.SceneManager.LoadScene("Station_Stage1");
                TrainGameManager.instance.InStation = true;
            }
        }
        
	}


    [PunRPC]
    public void setRunMeterZero()
    {
        Train_Ctrl.Run_Meter = 0;
        Train_Ctrl.Hide();
        playerListController.playerList[PhotonNetwork.LocalPlayer.ActorNumber - 1].player.UpSize();
        playerListController.playerList[PhotonNetwork.LocalPlayer.ActorNumber - 1].player.DownPos();
        playerListController.playerList[PhotonNetwork.LocalPlayer.ActorNumber - 1].player.Where_Floor = 4;
      
    }

   
}
