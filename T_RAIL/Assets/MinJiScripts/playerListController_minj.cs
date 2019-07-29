using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class playerListController_minj : MonoBehaviourPunCallbacks {

    public List<Player_Ctrl> playerList;
    public int[] eachPlayerIn;


    private void Start()
    {
        Invoke("start2SecondLater", 2.0f);

        Debug.Log("eachPlayerIn : " + PhotonNetwork.CurrentRoom.PlayerCount);
        eachPlayerIn = new int[PhotonNetwork.CurrentRoom.PlayerCount]; //new int[playerList.Count];
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
        {
            eachPlayerIn[i] = 1;//CountOfPlayers
        }

    }


    void start2SecondLater()
    {
        for (int i = 0; i < playerList.Count; ++i)
        {
            for (int j = i + 1; j < playerList.Count; ++j)
            {
                Debug.Log("playerList[i].gameObject.GetPhotonView().Owner.NickName " + playerList[i].gameObject.GetPhotonView().Owner.NickName);
                Debug.Log("playerList[j].gameObject.GetPhotonView().Owner.NickName " + playerList[j].gameObject.GetPhotonView().Owner.NickName);
                if (playerList[i].gameObject.GetPhotonView().Owner.NickName == playerList[j].gameObject.GetPhotonView().Owner.NickName)
                {
                    GameObject temp = playerList[j].gameObject;
                    playerList.Remove(playerList[j]);
                    Destroy(temp);
                }
            }
        }


    }


    // Update is called once per frame
    void Update () {
		
	}
}
