using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class playerManager_minj : MonoBehaviourPunCallbacks
{

    [Tooltip("The Player's UI GameObject Prefab")]
    [SerializeField]
    private GameObject playerUiPrefab;

    public Color myHatColor;
    public GameObject myHat;

    public Player_Ctrl Player_Ctrl;

    void Start()
    {
        Player_Ctrl = GetComponent<Player_Ctrl>();

        if (this.playerUiPrefab != null)
        {
            GameObject _uiGo = Instantiate(this.playerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }

        //foreach (Renderer r in GetComponentsInChildren<Renderer>())
        //{
        //    r.material.color = AsteroidsGame.GetColor(photonView.Owner.GetPlayerNumber());
        //}

        //for (int i = 0; i <= PhotonNetwork.CountOfPlayersInRooms; ++i) 

        //foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            //Debug.LogError(myHat.GetComponent<MeshRenderer>().materials[1].name);
            myHat.GetComponent<MeshRenderer>().materials[1].color = GetColor(photonView.Owner.ActorNumber-1);
        }
    }


    private void Update()
    {
        //if (PhotonNetwork.IsMasterClient != true) return;


        ////플레이어들이 어디에 있는지 확인
        //for (int i = 0; i <= PhotonNetwork.CountOfPlayersInRooms; ++i)
        //{
        //    //각 플레이어에게 지금 어디냐고 rpc로 물어보고 rpc로 답을 받음
        //    photonView.RPC("Question_Where_I_am", PhotonNetwork.PlayerList[i], eachPlayerIn[i]);
        //    //Player_Ctrl.Confirm_Where_I_am(PhotonNetwork.PlayerList[i]);
        //}
    }
    
    void CalledOnLevelWasLoaded(int level)
    {
        // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 15f, 0f);
        }
    
        GameObject _uiGo = Instantiate(this.playerUiPrefab);
        _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
    }
    
    
    public static Color GetColor(int colorChoice)
    {
        switch (colorChoice)
        {
            case 0: return Color.red;
            case 1: return Color.yellow;
            case 2: return Color.blue;
            case 3: return Color.green;
            case 4: return Color.cyan;
            case 5: return Color.grey;
            case 6: return Color.magenta;
            case 7: return Color.white;
        }
    
        return Color.black;
    }
    
    
}
