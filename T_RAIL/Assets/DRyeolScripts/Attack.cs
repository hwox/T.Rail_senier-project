using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Attack : MonoBehaviourPunCallbacks {

    public bool attack = false;

    // Use this for initialization
    void Start () {
		
	}

    private void OnTriggerStay(Collider other)
    {
     
        if (other.gameObject.layer.Equals(GameValue.chicken_layer))
        {


            if ( attack )
            {
                if (other.gameObject.GetComponent<Chicken_Ctrl>().live)
                {
                    photonView.RPC("attack_RPC", RpcTarget.All, other.gameObject.GetPhotonView().ViewID);
                    other.transform.LookAt(this.gameObject.transform);
                }
            }
        }

        attack = false;
    }


    [PunRPC]
    void attack_RPC(int otherViewID)
    {
        GameObject other = PhotonView.Find(otherViewID).gameObject;
        other.gameObject.GetComponent<Chicken_Ctrl>().DestPerson = this.gameObject;//닭이 따라가는 대상 선정
        other.gameObject.GetComponent<Chicken_Ctrl>().BeatenTrue();// 닭 애니메이션 및 행동 변경 함수 호출
        other.gameObject.GetComponent<Chicken_Ctrl>().gameObject.transform.Translate(0, 0, -1.0f);// 뒤로 밀어내기

        gameObject.transform.parent.GetComponent<Player_Ctrl>().attack_possible = false;
    }

}
