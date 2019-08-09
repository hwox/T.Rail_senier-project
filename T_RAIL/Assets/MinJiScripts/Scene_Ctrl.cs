using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Scene_Ctrl : MonoBehaviourPunCallbacks
{

    public playerListController_minj playerListController;
    public Train_Ctrl Train_Ctrl;
    public GameObject egg;
    public GameObject endingtrainparent;
    bool TestMeterMode; // test용 모드위해 추가하는 거

    bool tempOn = false; //첫번째씬에서 켜는용


    Camera MCam; // maincamera
    CamCtrl MCam_Ctrl; // 카메라에 달린 camctrl
    // Use this for initialization
    void Start()
    {
        MCam = Camera.main;
        MCam_Ctrl = MCam.GetComponent<CamCtrl>();
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKey(KeyCode.O))
        {
            TestMeterMode = true;
        }
        if (Input.GetKey(KeyCode.P))
        {
            TestMeterMode = false;
        }


        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            photonView.RPC("setRunMeterZero", RpcTarget.All);
            photonView.RPC("StationSceneLoad", RpcTarget.All);
        }
        if (Input.GetKeyDown(KeyCode.F9))// 엔딩가기
        {
            photonView.RPC("EndingSceneLoad_cheat", RpcTarget.All);
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {
            photonView.RPC("endingSetting", RpcTarget.All, 1);
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            photonView.RPC("endingSetting", RpcTarget.All, 2);
        }
        if (Input.GetKeyDown(KeyCode.F12))
        {
            photonView.RPC("endingSetting", RpcTarget.All, 3);
        }




        //state==1 기차 안 / state == 3 역
        // if (TrainGameManager.instance.Scene_state == 1)
        //  {
        // 
        NextStageCheck();
        //}
        //else
        //{
        // 
        //}

        if (!PhotonNetwork.IsMasterClient) return;

        //state==1 기차 안 / state == 3 역
        if (TrainGameManager.instance.Scene_state == 1)
        {

            if (!TestMeterMode)
            {
                if (GameValue.NextStationMeter < Train_Ctrl.Run_Meter)
                {
                    photonView.RPC("setRunMeterZero", RpcTarget.All);
                    photonView.RPC("StationSceneLoad", RpcTarget.All);
                }
            }
            else
            {
                if (GameValue.TestMeter < Train_Ctrl.Run_Meter)
                {
                    photonView.RPC("setRunMeterZero", RpcTarget.All);
                    photonView.RPC("StationSceneLoad", RpcTarget.All);
                }
            }

            NextStageCheck();
        }
        else if (TrainGameManager.instance.Scene_state == 3)
        {
            photonView.RPC("SetTrainPlayer", RpcTarget.All);
            photonView.RPC("TrainSceneLoad", RpcTarget.All);
            TrainGameManager.instance.photonView.RPC("setSceneState_RPC", RpcTarget.All, 1);
            //TrainGameManager.instance.Scene_state = 1;
            NextStageCheck();



        }
        else if (TrainGameManager.instance.Scene_state == 4)
        {
            if (!TestMeterMode)
            {
                if (GameValue.NextStationMeter < Train_Ctrl.Run_Meter)
                {
                    photonView.RPC("EndingSceneLoad",RpcTarget.All);
                    //TrainGameManager.instance.Scene_state = 5;  <-- 이거 if랑 else 둘다 있어서 그냥 EndingSceneLoad 함수 안에다가 집어넣음
                }
            }
            else
            {
                if (GameValue.TestMeter < Train_Ctrl.Run_Meter)
                {
                    photonView.RPC("EndingSceneLoad", RpcTarget.All);
                    //TrainGameManager.instance.Scene_state = 5;
                }
            }

        }
    }

    [PunRPC]
    public void endingSetting(int state)
    {
        TrainGameManager.instance.Ending_Stage = state;
    }

    [PunRPC]
    public void SetTrainPlayer()// 기차로 갈때
    {
        Train_Ctrl.Appear();
        EggHide();
        highligh_off();
        TrainGameManager.instance.ConditionCtrl.TrainAddCondition_Passenger(TrainGameManager.instance.GetPassengerCount);

        TrainGameManager.instance.CoinUI.SetActive(false);
        for (int i = 0; i < playerListController.playerList.Count; ++i)
        {
            playerListController.playerList[i].player.DownSize();
            playerListController.playerList[i].player.SetTrainPlayer(i);
            playerListController.playerList[i].player_floor_minji = 1;
            playerListController.playerList[i].AxeActive();
            //Debug.Log("id : " + (i) + "  floor : " + playerListController.playerList[i].player.Where_Floor);

            // 승객을 태우기 위해서 호출하는 함수
            TrainGameManager.instance.SofaSitPassengerCtrl.PassengerRideInTrain();

            // 기차 다시 불러오면서 코루틴들 다시 호출
            TrainGameManager.instance.TrainCtrl.SceneReRoadTrain();

        }

        for (int i = 0; i < 10; ++i)
        {
            TrainGameManager.instance.Station_PassengerManager[i].SetActive(false);
        }

        TrainGameManager.instance.SoundManager.TrainDriving_Sound_Play();
    }
    [PunRPC]
    public void TrainSceneLoad()
    {

        //초원 
        if (SceneManager.GetActiveScene().buildIndex + 1 == 1)
        {
            TrainGameManager.instance.SoundManager.TrainStage1_BGMSoundPlay();
        }
        //사막
        else if (SceneManager.GetActiveScene().buildIndex + 1 == 3)
        {
            TrainGameManager.instance.SoundManager.TrainStage2_BGMSoundPlay();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    [PunRPC]
    public void setRunMeterZero()// 역으로 갈때
    {

        Train_Ctrl.Run_Meter = 0;
        highligh_off();
        Train_Ctrl.Hide();

        TrainGameManager.instance.CoinUI.SetActive(true);
        TrainGameManager.instance.Scene_state = 2;
        //Debug.LogError("id : " + (PhotonNetwork.LocalPlayer.ActorNumber - 1 )+ "  floor : " + playerListController.playerList[PhotonNetwork.LocalPlayer.ActorNumber - 1].player.Where_Floor);
        //Debug.LogError("몇명 들어와있는지: " + playerListController.playerList.Count);

        if (TrainGameManager.instance.EnemyAppear)
        {
            TrainGameManager.instance.ConditionCtrl.EnemyDisappear(); // 적 사라지게
        }
        for (int i = 0; i < playerListController.playerList.Count; ++i)
        {
            playerListController.playerList[i].player.UpSize();
            playerListController.playerList[i].player.SetStationPlayer(i);

            //Camera.main.gameObject.GetComponent<CamCtrl>().EnemyAppear_Cam(false, 0);
            playerListController.playerList[i].MCam_Ctrl.EnemyAppear_Cam(false, 0);
            playerListController.playerList[i].MCam_Ctrl.inTrain();
            playerListController.playerList[i].stair_down = false;
            playerListController.playerList[i].stair_up = false;
            playerListController.playerList[i].anim.SetBool("UpToLadder", false);

            if (playerListController.playerList[i].player_where_minji - 1 >= 0)
                TrainGameManager.instance.TrainCtrl.trainscript[playerListController.playerList[i].player_where_minji - 1].Ceiling_OnOff(false);

            playerListController.playerList[i].player_floor_minji = 4;
            playerListController.playerList[i].AxeActive();

            //photonView.RPC("setPlayerInStationState", RpcTarget.All, i);
        }

        TrainGameManager.instance.Scene_state = 2;
        TrainGameManager.instance.SoundManager.TrainDriving_Sound_Stop();
    }



    [PunRPC]
    public void StationSceneLoad()
    {

        //첫번째 역에서 브금 바뀌는지 테스트
        if (SceneManager.GetActiveScene().buildIndex + 1 == 2)
            TrainGameManager.instance.SoundManager.Train_door_open_Sound_Play();
        //설원
        else if (SceneManager.GetActiveScene().buildIndex + 1 == 4)
        {
            TrainGameManager.instance.SoundManager.TrainStage3_BGMSoundPlay();
            TrainGameManager.instance.SoundManager.Train_door_open_Sound_Play();

        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


    }

    [PunRPC]
    public void EndingSceneLoad()
    {
        Train_Ctrl.Hide();
        MCam_Ctrl.EndingCam(true, 0);
        MCam_Ctrl.Change_floor(5);
        for (int i = 0; i < playerListController.playerList.Count; ++i)
        {
            playerListController.playerList[i].player_floor_minji = 5;
            playerListController.playerList[i].gameObject.SetActive(false);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + TrainGameManager.instance.Ending_Stage);
        if (TrainGameManager.instance.EnemyAppear)
        {
            TrainGameManager.instance.ConditionCtrl.EnemyDisappear(); // 적 사라지게
        }

        TrainGameManager.instance.Scene_state = 5;
    }


    [PunRPC]
    public void EndingSceneLoad_cheat()
    {
        Train_Ctrl.Hide();
        MCam_Ctrl.EndingCam(true, 0);
        MCam_Ctrl.Change_floor(5);
        for (int i = 0; i < playerListController.playerList.Count; ++i)
        {
            playerListController.playerList[i].player_floor_minji = 5;
            playerListController.playerList[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < 13; i++)
        {
            TrainGameManager.instance.endingtrainM[i].SetActive(false);
              TrainGameManager.instance.endingtrainM[i].transform.parent=endingtrainparent.transform;
           
        }
        SceneManager.LoadScene(5+ TrainGameManager.instance.Ending_Stage);

        if (TrainGameManager.instance.EnemyAppear)
        {
            TrainGameManager.instance.ConditionCtrl.EnemyDisappear(); // 적 사라지게
        }

        TrainGameManager.instance.Scene_state = 5;
      
       


    }


    void NextStageCheck()
    {
        int NextStage = SceneManager.GetActiveScene().buildIndex;

        if (NextStage <= GameValue.Stage1Index && SceneManager.GetActiveScene().buildIndex >= 1)
        {
            TrainGameManager.instance.Stage = 1;

            if (tempOn == false)
            {

                TrainGameManager.instance.SoundManager.TrainStage1_BGMSoundPlay();
                TrainGameManager.instance.SoundManager.TrainDriving_Sound_Play();
                tempOn = true;
            }
            //Debug.Log("여기");

        }
        else if (NextStage >= GameValue.Stage1Index && NextStage < GameValue.Stage2Index)
        {
            TrainGameManager.instance.Stage = 2;
            //Debug.Log("여여기");
        }
        else if (NextStage >= GameValue.Stage2Index && NextStage < GameValue.Stage3Index)
        {
                if(NextStage==5)
                {
                    if(TrainGameManager.instance.Scene_state !=5)
                    {
                        TrainGameManager.instance.Scene_state = 4;

                      
                    }

                }
            
                TrainGameManager.instance.Stage = 3;
               
            
         
          
        }
        else if (NextStage >= GameValue.Stage3Index)
        {
            TrainGameManager.instance.Stage = 4;
          
        }
        else
        {
            Debug.Log("X");
        }

    }

    void EggHide()
    {
        for (int i = 0; i < 10; i++)
        {
            egg.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    void highligh_off()
    {

        TrainGameManager.instance.highligh_state = 0;
        TrainGameManager.instance.near_stair = false;
        TrainGameManager.instance.near_gun = false;
        TrainGameManager.instance.near_stationpassenger = false;
        TrainGameManager.instance.near_sign = false;
    }

}
