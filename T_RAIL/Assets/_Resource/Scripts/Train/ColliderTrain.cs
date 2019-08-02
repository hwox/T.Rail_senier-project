using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ColliderTrain : MonoBehaviourPunCallbacks
{


    Camera MCam;
    CamCtrl MCam_Ctrl;

    // Use this for initialization
    void Start()
    {
        MCam = Camera.main;
        MCam_Ctrl = MCam.GetComponent<CamCtrl>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(GameValue.enemy_layer))
        {
            MCam_Ctrl.Hit_EnemyCam(true);

            TrainGameManager.instance.TrainCtrl.trainscript[TrainGameManager.instance.trainindex - 1].photonView.RPC("Run_TrainHPMinus_RPC", RpcTarget.All, 2.0f); //HP -=2/*other.GetComponent<Enemy1_Ctrl>().E_damage*/;
            Debug.Log("HP"+TrainGameManager.instance.TrainCtrl.trainscript[TrainGameManager.instance.trainindex - 1].HP);

            TrainGameManager.instance.SoundManager.enemy_attack_Sound_Play();
        }
    }

}
