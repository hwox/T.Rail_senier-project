using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Scene_Ctrl : MonoBehaviourPunCallbacks {

    public Train_Ctrl Train_Ctrl;



	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		if(GameValue.NextStationMeter < Train_Ctrl.Run_Meter)
        {
            photonView.RPC("setRunMeterZero", RpcTarget.All);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Station_Stage1");
        }

	}


    [PunRPC]
    public void setRunMeterZero()
    {
        Train_Ctrl.Run_Meter = 0;
        Train_Ctrl.Hide();
    }

    

}
