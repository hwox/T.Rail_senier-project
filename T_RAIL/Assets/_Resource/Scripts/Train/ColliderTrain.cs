using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTrain : MonoBehaviour
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

            TrainGameManager.instance.TrainCtrl.trainscript[TrainGameManager.instance.trainindex - 1].HP -= 5/*other.GetComponent<Enemy1_Ctrl>().E_damage*/;
            TrainGameManager.instance.SoundManager.enemy_attack_Sound_Play();
        }
    }
}
