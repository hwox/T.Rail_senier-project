using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Condition_Ctrl : MonoBehaviourPunCallbacks
{

    // 각종조건을 관리하는 스크립트
    // 이걸로 train add, monster 등장 조건 관리


    public GameObject rhino;

    public GameObject enemy1;
    public Enemy_Ctrl enemy_ctrl;

    public GameObject enemy2; // 선인장
   // public Enemy2_Ctrl enemy2_ctrl; 

    // 흠ㅁ 그냥 다 수정해서 enemy_ctrl로 통일시킬까

    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("Init_Make", RpcTarget.All);
        }
    }

    [PunRPC]
    void Init_Make()
    {
        // 처음에 만들어놓을 몬스터들이나 기관총 ㅇ총알. 총알 방식은 좀 바꿔야될걳같음 기관총이 생기는거를
        // 기차도 

        PhotonNetwork.InstantiateSceneObject(rhino.name, new Vector3(-200, 1.7f, -3.6f), Quaternion.Euler(0, 90, 0));

        //이 주석부분 -> Enemy1_ctrl 스크립트 start()부분으로 옮김. train 생성과 같은 이유로
        //enemy1 =PhotonNetwork.InstantiateSceneObject(rhino.name, new Vector3(200, 1.7f, -3.6f), Quaternion.Euler(0,90,0));
        //enemy1_ctrl = enemy1.GetComponent<Enemy1_Ctrl>();
        //enemy1.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(EnemyAppear_Condition());
       
    }
    public void onRhinoEnemyOnButton()
    {
        photonView.RPC("Rhino_Add", RpcTarget.All);
    }

    [PunRPC]
    public void Rhino_Add()
    {
        // 
        TrainGameManager.instance.EnemyAppear = true;
        enemy1.SetActive(true);
        enemy_ctrl.Enemy_On();
    }


    public void TrainAddCondition_Passenger(int _num)
    {
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

        TrainGameManager.instance.totalkickoutEnemy += 1;

        int enemy_total = TrainGameManager.instance.totalkickoutEnemy;

        if (enemy_total % 3 == 2)
        {
            // 기차 add함수 호출 
            TrainGameManager.instance.TrainCtrl.onTrainAddButtonClick();
        }
    }



    IEnumerator EnemyAppear_Condition()
    {

        //if (!TrainGameManager.instance.EnemyAppear && 
        //    TrainGameManager.instance.TrainCtrl.Run_Meter > 30.0f)
        //{
        //    int noiseSound = TrainGameManager.instance.Noise /
        //        TrainGameManager.instance.Noise_stat;

        //    int random = Random.Range(0, 200);
        //    if (random < noiseSound)
        //    {
        //        // enemy 호출 함수
        //        onRhinoEnemyOnButton();
        //        TrainGameManager.instance.EnemyAppear = true;
        //    }
        //}
        yield return new WaitForSeconds(5.0f);

        StartCoroutine(EnemyAppear_Condition());

    }

}
