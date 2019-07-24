using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class playerListController_minj : MonoBehaviourPunCallbacks {

    public List<Player_Ctrl> playerList;
    public int[] eachPlayerIn;


    private void Start()
    {
        for (int i = 0; i < playerList.Count; ++i)
        {
            for (int j = i+1; j < playerList.Count; ++j)
            {
                if (playerList[i].gameObject.GetPhotonView().ViewID == playerList[j].gameObject.GetPhotonView().ViewID)
                {
                    GameObject temp = playerList[j].gameObject;
                    playerList.Remove(playerList[j]);
                    Destroy(temp);
                }
            }
        }


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
