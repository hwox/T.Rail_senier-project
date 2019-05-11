using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTrain : MonoBehaviour {


    Camera MCam;
    CamCtrl MCam_Ctrl;


	// Use this for initialization
	void Start () {
        MCam = Camera.main;
        MCam_Ctrl = MCam.GetComponent<CamCtrl>();
	}
	

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(GameValue.enemy_layer))
        {
            MCam_Ctrl.Hit_EnemyCam(true);
            
            // 아잇 root가 안움직여서 어쩔수 없이 만든 스크립트
            // 걍 이왕 만든ㄴ김에 더 써야겠다.
           // TrainGameManager.instance.TrainCtrl.trainscript[TrainGameManager.instance.trainindex - 1].HP -= other.GetComponent<Enemy1_Ctrl>().E_damage;

            Debug.Log("기차랑 충돌");
        }
    }
}
