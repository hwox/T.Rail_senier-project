using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class playerListController_minj : MonoBehaviourPunCallbacks {

    public List<Player_Ctrl> playerList;
    public int[] eachPlayerIn;


    private void Awake()
    {
        eachPlayerIn = new int[PhotonNetwork.CountOfPlayers];
        for (int i = 0; i < PhotonNetwork.CountOfPlayers; ++i)
        {
            eachPlayerIn[i] = 1;
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
