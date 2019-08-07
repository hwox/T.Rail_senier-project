﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Condition_Ctrl : MonoBehaviourPunCallbacks
{

    // 각종조건을 관리하는 스크립트
    // 이걸로 train add, monster 등장 조건 관리


    public GameObject rhino;
    public GameObject Cactus;
    public GameObject Husky;

    public GameObject NowEnemy;


    private void Awake()
    {
        photonView.RPC("Init_Make", RpcTarget.All);
    }

    [PunRPC]
    void Init_Make()
    {
        //if (!PhotonNetwork.IsMasterClient) return;

        // 처음에 만들어놓을 몬스터들이나 기관총 ㅇ총알. 총알 방식은 좀 바꿔야될걳같음 기관총이 생기는거를
        // 기차도 


      //  enemyOnStage1 = PhotonNetwork.InstantiateSceneObject(rhino.name, new Vector3(-200, 1.7f, -3.6f), Quaternion.Euler(0, 90, 0));
       // enemyOnStage2 = PhotonNetwork.InstantiateSceneObject(Cactus.name, new Vector3(-200, 1.7f, -3.6f), Quaternion.Euler(0,110, 0));
      //  enemyOnStage3 = PhotonNetwork.InstantiateSceneObject(Husky.name, new Vector3(-200, 1.7f, -3.6f), Quaternion.Euler(0, 90, 0));

        //이 주석부분 -> Enemy1_ctrl 스크립트 start()부분으로 옮김. train 생성과 같은 이유로
        //enemy1 =PhotonNetwork.InstantiateSceneObject(rhino.name, new Vector3(200, 1.7f, -3.6f), Quaternion.Euler(0,90,0));
        //enemy1_ctrl = enemy1.GetComponent<Enemy1_Ctrl>();
        //enemy1.SetActive(false);
    }

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        StartCoroutine(EnemyAppear_Condition());
    }
    public void onEnemyOnButton()
    {
        photonView.RPC("Enemy_Appear", RpcTarget.All);
    }

    //[PunRPC]
    //public void Rhino_Add()
    //{
    //    // 
    //    TrainGameManager.instance.EnemyAppear = true;
    //    NowEnemy.SetActive(true);
    //    enemy_ctrl.Enemy_On();
    //}

    [PunRPC]
    public void Enemy_Appear()
    {
        TrainGameManager.instance.EnemyAppear = true;

        switch (TrainGameManager.instance.Stage)
        {
            case 1:
                NowEnemy = rhino;
                break;
            case 2:
                NowEnemy = Cactus;
                break;
            case 3:
                NowEnemy = Husky;
                break;
            default:
                TrainGameManager.instance.Error_print();
                break;
        }
               
        NowEnemy.SetActive(true);
       
        NowEnemy.GetComponent<Enemy_Ctrl>().Enemy_On();
    }


    public void TrainAddCondition_Passenger(int _num)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        // 여기서 전달받는 _num은 역에서 총 승객을 몇명이나 먹었는지의 값을
        // 넣어주면 됨. 
        // 표지판 sign하고 다음으로 넘어가는 부분? 그 쯤에서 호출하는거 추천
        int prev_total = TrainGameManager.instance.totalPassenger / 3;

        TrainGameManager.instance.totalPassenger += _num;

        int after_total = TrainGameManager.instance.totalPassenger / 3;

        for (int i = 0; i < after_total - prev_total; i++)
        {
            // 여기서 기차 add 함수 호출
            TrainGameManager.instance.TrainCtrl.onTrainAddButtonClick();
        }
    }

    public void TrainAddCondition_Enemy()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        TrainGameManager.instance.totalkickoutEnemy += 1;

        int enemy_total = TrainGameManager.instance.totalkickoutEnemy;

        if (enemy_total % 3 == 1)
        {
            // 기차 add함수 호출 
            TrainGameManager.instance.TrainCtrl.onTrainAddButtonClick();
        }
    }

    public void EnemyDisappear()
    {
        // 적 사라지게
        // 1. 기차 -> 역에 갔다거나
        NowEnemy.GetComponent<Enemy_Ctrl>().EnemyActiveOff();
    }

    IEnumerator EnemyAppear_Condition()
    {

        if (!TrainGameManager.instance.EnemyAppear && 
            TrainGameManager.instance.TrainCtrl.Run_Meter > 30.0f)
        {
            int noiseSound = TrainGameManager.instance.Noise / TrainGameManager.instance.Noise_stat;
            int random = Random.Range(0, 250);
            if (random < noiseSound)
            {
                // enemy 호출 함수
                onEnemyOnButton();
                TrainGameManager.instance.EnemyAppear = true;
            }
        }
        yield return new WaitForSeconds(5.0f);

        StartCoroutine(EnemyAppear_Condition());

    }

}
