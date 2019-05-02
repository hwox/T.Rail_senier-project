using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Condition_Ctrl : MonoBehaviourPunCallbacks {

    // 각종조건을 관리하는 스크립트
    // 이걸로 train add, monster 등장 조건 관리


    public GameObject rhino;

    public GameObject enemy1;
    public Enemy1_Ctrl enemy1_ctrl;
    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("Init_Make", RpcTarget.All); 
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    [PunRPC]
    void Init_Make()
    {
        // 처음에 만들어놓을 몬스터들이나 기관총 ㅇ총알. 총알 방식은 좀 바꿔야될걳같음 기관총이 생기는거를
        // 기차도 

        PhotonNetwork.InstantiateSceneObject(rhino.name, new Vector3(200, 1.7f, -3.6f), Quaternion.Euler(0, 90, 0));

        //이 주석부분 -> Enemy1_ctrl 스크립트 start()부분으로 옮김. train 생성과 같은 이유로
        //enemy1 =PhotonNetwork.InstantiateSceneObject(rhino.name, new Vector3(200, 1.7f, -3.6f), Quaternion.Euler(0,90,0));
        //enemy1_ctrl = enemy1.GetComponent<Enemy1_Ctrl>();
        //enemy1.SetActive(false);
    }


    public void onRhinoEnemyOnButton()
    {
        photonView.RPC("Rhino_Add", RpcTarget.All);
    }

    [PunRPC]
    public void Rhino_Add()
    {
        // 
        enemy1.SetActive(true);
        enemy1_ctrl.Enemy1_On();
    }

}
