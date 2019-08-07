using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FootSoundScript : MonoBehaviourPunCallbacks {

    void OnTriggerEnter(Collider _col)
    {
        if (_col.gameObject.layer == LayerMask.NameToLayer("Station"))
        {
            if(transform.root.GetComponent<PhotonView>().IsMine)
                TrainGameManager.instance.SoundManager.Player_foot_Sound_Play();

        }
    }
}
