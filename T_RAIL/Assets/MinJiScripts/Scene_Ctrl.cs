using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Scene_Ctrl : MonoBehaviourPunCallbacks
{

    public playerListController_minj playerListController;
    public Train_Ctrl Train_Ctrl;


    bool TestMeterMode; // test용 모드위해 추가하는 거

    bool tempOn = false; //첫번째씬에서 켜는용

    // Use this for initialization
    void Start()
    {
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

        //state==1 기차 안 / state == 3 역
        if (TrainGameManager.instance.Scene_state == 1)
        {
            NextStageCheck();
        }

            if (!PhotonNetwork.IsMasterClient) return;

        //state==1 기차 안 / state == 3 역
        if (TrainGameManager.instance.Scene_state == 1)
        {
            //NextStageCheck();

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
        }
        else if (TrainGameManager.instance.Scene_state == 3)
        {
            photonView.RPC("SetTrainPlayer", RpcTarget.All);
            photonView.RPC("TrainSceneLoad", RpcTarget.All);
            TrainGameManager.instance.photonView.RPC("setSceneState_RPC", RpcTarget.All, 1);
            //TrainGameManager.instance.Scene_state = 1;
           
        }
    }


    [PunRPC]
    public void SetTrainPlayer()// 기차로 갈때
    {
        Train_Ctrl.Appear();

        TrainGameManager.instance.CoinUI.SetActive(false);
        for (int i = 0; i < playerListController.playerList.Count; ++i)
        {
            playerListController.playerList[i].player.DownSize();
            playerListController.playerList[i].player.SetTrainPlayer(i);
            playerListController.playerList[i].player.Where_Floor = 1;
            playerListController.playerList[i].player.AxeActive();
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
            playerListController.playerList[i].player.Where_Floor = 4;
            playerListController.playerList[i].player.AxeActive();
            //Debug.Log("id : " + (i) + "  floor : " + playerListController.playerList[i].player.Where_Floor);

            //photonView.RPC("setPlayerInStationState", RpcTarget.All, i);
        }

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

    void NextStageCheck()
    {
        int NextStage = SceneManager.GetActiveScene().buildIndex;

        if (NextStage <= GameValue.Stage1Index && SceneManager.GetActiveScene().buildIndex >= 1)
        {
            TrainGameManager.instance.Stage = 1;
            
            if(tempOn == false)
            {
                TrainGameManager.instance.SoundManager.TrainStage1_BGMSoundPlay();
                tempOn = true;
            }
            //Debug.Log("stage1");
        }
        else if (NextStage >= GameValue.Stage1Index && NextStage < GameValue.Stage2Index)
        {
            TrainGameManager.instance.Stage = 2;
            //Debug.Log("stage2");
        }
        else if (NextStage >= GameValue.Stage2Index && NextStage < GameValue.Stage3Index)
        {
            TrainGameManager.instance.Stage = 3;
            //Debug.Log("stage3");
        }
        else if (NextStage >= GameValue.Stage3Index)
        {
            // 엔딩
        }
    }
}
