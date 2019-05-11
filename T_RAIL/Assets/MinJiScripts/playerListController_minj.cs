using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class playerListController_minj : MonoBehaviourPunCallbacks {

    public List<Player_Ctrl> playerList;
    public int[] eachPlayerIn;


    private void Start()
    {
        Debug.Log("eachPlayerIn : " + PhotonNetwork.CurrentRoom.PlayerCount);
        eachPlayerIn = new int[PhotonNetwork.CurrentRoom.PlayerCount]; //new int[playerList.Count];
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
        {
            eachPlayerIn[i] = 1;//CountOfPlayers
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
