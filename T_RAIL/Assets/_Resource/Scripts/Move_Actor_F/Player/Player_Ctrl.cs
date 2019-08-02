using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;




public class Player_Ctrl : MonoBehaviourPunCallbacks, IPunObservable
{
    enum player_space_state
    {

        Ladder = 1,
        Ladder_Down = 2,
        Machine_gun = 3,
        bullet = 4,

        prevjump = 5,
        nextjump = 6,

        stationpassenger = 7,
        sign = 8
    }

    // 기본 플레이어에 달린 컴포넌트들
    public Player_Actor player;
    Transform tr;

    public Transform LeftShoulder;
    public Transform RightShoulder;
    Animator anim;
    Rigidbody ri;
    public Color hoverColor = Color.white;
    Highlighter highlighter;


    Transform Near_Object; // 사다리, 머신건 등 space_state로 할 모든 object담기
    Transform gun_child; // 머신건.... 각도 회전하려면 밑에 자식 오브젝트 담아와야 돼서 총전용
    MachineGun_Ctrl gun_ctrl; // 그 머신건에 달린 ctrl 스크립트. 머신건을 받아올 떄 마다 얘도 같이


    float runTime; // 걷는 거 2초이상 달리기

    bool stair_up; // 사다리 올라가고 있는 중
    bool stair_down; // 사다리 내려가고 있는 중
    bool jump_now;

    int space_state = 0; // 기본은 0인데 space가 눌려지는 상황 (highlight되는 모든애들) 에서 state change
    bool near_stair; // 사다리근처
    bool near_gun; // 머신건 근처
    bool near_stationpassenger;// 역승객 근처
    bool near_sign; // 표지판

    Transform floor1;
    Transform floor2;


    // render 
    Camera MCam; // maincamera
    CamCtrl MCam_Ctrl; // 카메라에 달린 camctrl
    StationCam_Ctrl SCam_Ctrl;

    // ui
    // public GameObject Push_Space_UI_pref; // space 누르라고 뜨는 ui. 얘는 프리팹 연결
    GameObject Push_Space_UI; // space 누르라고 뜨는 ui


    // particle

    public GameObject parti_player_move;

    // 총알발사
    float Attack_Gap; //발사간격
    bool ContinuousFire; // 계속발사할것인지플래그
    float m_MinLaunchForce = 10.0f;
    float m_MaxLaunchForce = 60.0f;
    float m_MaxChargeTime = 1.5f;

    float m_CurrentLaunchForce;
    float m_ChargeSpeed;
    bool m_Fired;

    // 현재 손에 갖고 있는 아이템




    //역 공격
    public GameObject attackleach;
    public bool attack_possible = true;
    public GameObject axe;
    bool invincibility = false;
    iTweenPath itp;


    /// ////////////////////////////////////////////////////////////////////////

    public playerListController_minj playerListController;
    public UIState_Ctrl UIState_Ctrl;
    public int whereIam;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        //if (photonView.ViewID % 2 == 0)
        //{
        //    //Destroy(this.gameObject);
        //}
        //else
        {
            player = new Player_Actor();

            Make_PushSpaceUI();
            Init_Set_Value();

            //생성되면 플레이어 리스트에 스스로를 넣어줌.
            playerListController = TrainGameManager.instance.PlayerListCtrl.GetComponent<playerListController_minj>();
            playerListController.playerList.Add(this.gameObject.GetComponent<Player_Ctrl>());
            UIState_Ctrl = TrainGameManager.instance.UIStateCtrl.GetComponent<UIState_Ctrl>();
            whereIam = player.Where_Train;
        }
    }

    private void Start()
    {
        player.axe = axe;
        ////if (!photonView.IsMine) return;
        //if (photonView.ViewID % 1000 == 2) Destroy(this.gameObject);

        ////생성되면 플레이어 리스트에 스스로를 넣어줌.
        //playerListController = GameObject.Find("PlayerList_Ctrl").GetComponent<playerListController_minj>();
        //playerListController.playerList.Add(this.gameObject.GetComponent<Player_Ctrl>());
        //UIState_Ctrl = GameObject.Find("UIState_Ctrl").GetComponent<UIState_Ctrl>();
        //whereIam = player.Where_Train;

    }

    void Init_Set_Value()
    {
        // 파티클찾기
        //  parti_player_move = this.transform.GetChild(0).gameObject;
        MCam = Camera.main; // 메인카메라 찾기
        MCam_Ctrl = MCam.GetComponent<CamCtrl>();

        anim = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        jump_now = true;
        ri = GetComponent<Rigidbody>();
        itp = GetComponent<iTweenPath>();


        Attack_Gap = 1.0f;
        ContinuousFire = true;
        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }




    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;


        if (!stair_up && !stair_down)
        {

            if (!near_stair && other.gameObject.layer.Equals(GameValue.ladder_layer))
            {
                space_state = (int)player_space_state.Ladder;
                Near_Object = other.transform;
                highlighter = Near_Object.GetComponent<Highlighter>();
                near_stair = true;
                //   Push_Space_UI.SetActive(true);

                //   Push_Space_UI.transform.position = MCam.WorldToScreenPoint(Near_Object.position) + new Vector3(10, 150, 0);
            }
            if (other.gameObject.layer.Equals(GameValue.floor2_layer))
            {
                // 내려갈때 쓸거얌
                space_state = (int)player_space_state.Ladder_Down;

            }
        }

        if (other.gameObject.layer.Equals(GameValue.machinegun_layer))
        {
            // 머신건 근처
            if (!near_gun)
            {
                space_state = (int)player_space_state.Machine_gun;
                Near_Object = other.transform;
                gun_child = other.transform.GetChild(0);
                gun_ctrl = gun_child.GetComponent<MachineGun_Ctrl>();
                highlighter = Near_Object.GetComponent<Highlighter>();
                near_gun = true;
                //  Push_Space_UI.SetActive(true);
                //  Push_Space_UI.transform.position = MCam.WorldToScreenPoint(Near_Object.position) + new Vector3(-20, 130, 0);
            }
        }
        if (other.gameObject.layer.Equals(GameValue.statiopassenger_layer))
        {
            // 역 승객
            if (!near_stationpassenger)
            {
                space_state = (int)player_space_state.stationpassenger;
                Near_Object = other.transform;
                highlighter = Near_Object.GetComponent<Highlighter>();
                near_stationpassenger = true;
                //    Push_Space_UI.SetActive(true);
                //    Push_Space_UI.transform.position = MCam.WorldToScreenPoint(Near_Object.position) + new Vector3(-20, 130, 0);
            }
        }
        if (other.gameObject.layer.Equals(GameValue.sign_layer))
        {
            // 표지판
            if (!near_sign)
            {
              //  space_state = (int)player_space_state.sign;
              //  Near_Object = other.transform;
              //  highlighter = Near_Object.GetComponent<Highlighter>();
              //  near_sign = true;
                //  Push_Space_UI.SetActive(true);
                //   Push_Space_UI.transform.position = MCam.WorldToScreenPoint(Near_Object.position) + new Vector3(0, 130, 0);
            }
        }

        if (other.gameObject.layer.Equals(GameValue.chicken_layer))//닭에게 공격 받기
        {
            if (!invincibility)
            {
                StartCoroutine("Beaten");
            }
            
        }




        }
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.layer.Equals(GameValue.wall_layer)
               || other.gameObject.layer.Equals(GameValue.sofa_layer)
            || other.gameObject.layer.Equals(GameValue.itembox_layer))
        {
            player.WallConflictDirection();
        }


        //역에서 승객 먹기
        if (other.gameObject.layer.Equals(GameValue.statiopassenger_layer))
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                //  near_stair = false;
                //  Push_Space_UI.SetActive(false);
                //  Destroy(other.gameObject);
                //GameObject.Find("SofaSitPassenger_Ctrl").GetComponent<SofaSitPassenger_Ctrl>().sofaNubmer
             
             
            
               
                if (TrainGameManager.instance.SopaNum > TrainGameManager.instance.totalPassenger)
                {
                    for (int i = 0; i < TrainGameManager.instance.Station_PassengerManager.Count; ++i)
                    {
                        if (other.gameObject == TrainGameManager.instance.Station_PassengerManager[i])
                        {
                            photonView.RPC("passengerTouch", RpcTarget.All, i); //, eachPlayerIn[i]);
                            StartCoroutine(CoinParticle(other.transform));
                            if (near_stationpassenger)
                            {
                                space_state = 0;
                                near_stationpassenger = false;
                                
                            }
                        }
                    }
                }
            }
        }
        //표지판에서 기차출발시키기
        if (other.gameObject.layer.Equals(GameValue.sign_layer))
        {
            if (Input.GetKeyDown(KeyCode.V))
            {


                for (int i = 0; i < TrainGameManager.MAKE_CHICKEN_COUNT; i++)
                {
                    if (gameObject.activeSelf == true)
                    {
                        TrainGameManager.instance.ChickenManager[i].GetComponent<Chicken_Ctrl>().die();
                    }
                }

                //여기서 이제다시 기차로
                TrainGameManager.instance.photonView.RPC("setSceneState_RPC", RpcTarget.All, 3);
                //TrainGameManager.instance.Scene_state = 3;
            }
        }


        //계란 줍기
        if (other.gameObject.layer.Equals(GameValue.egg_layer))
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
               TrainGameManager.instance.allitemCtrl.ItemGet_Random(other.gameObject.GetPhotonView().ViewID);
            }
        }

        // 자판기 사용
        if (other.gameObject.layer.Equals(GameValue.vandingmachine_layer))
        {
            if (Input.GetKeyDown(KeyCode.V))
            {

                if (other.GetComponent<VendingMachine>().VendingMachine_on)
                {
                    MCam_Ctrl.Vending_Machine_Cam(false, 0);
                    other.GetComponent<VendingMachine>().VendingMachine_on = false;
                    player.Where_Floor = 4;
                }
                else
                {
                    anim.SetBool("IsWalk", false);
                    MCam_Ctrl.Vending_Machine_Cam(true, 0);
                    other.GetComponent<VendingMachine>().VendingMachine_on = true;
                    player.Where_Floor = 5;
                }
              
            }
        }
    }


    //traingameManager로 이동
    //[PunRPC]
    //public void setSceneState_RPC(int _state)
    //{
    //    TrainGameManager.instance.Scene_state = _state;
    //}



    [PunRPC]
    public void passengerTouch(int i)
    {
        TrainGameManager.instance.Station_PassengerManager[i].gameObject.SetActive(false);
        TrainGameManager.instance.GetPassengerCount++;
        TrainGameManager.instance.totalPassenger++;
        Debug.Log("GetPassengerCount " + TrainGameManager.instance.GetPassengerCount);
    }

    [PunRPC]
    public void eggEat_RPC(int otherViewID)
    {
        GameObject _other = PhotonView.Find(otherViewID).gameObject;
        _other.gameObject.SetActive(false);

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    GameObject.Find("Item_Ctrl").GetComponent<AllItem_Ctrl>().ItemGet_Random();
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (!photonView.IsMine) return;

        // 사다리! 올라가는 중이 아니며 
        if (!stair_up && other.gameObject.layer.Equals(GameValue.ladder_layer))
        {
            if (near_stair)
            {
                space_state = 0;
                near_stair = false;
                //   Push_Space_UI.SetActive(false);
            }
        }

        // 머신건 근처
        if (other.gameObject.layer.Equals(GameValue.machinegun_layer))
        {
            if (near_gun)
            {
                space_state = 0;
                near_gun = false;
                //   Push_Space_UI.SetActive(false);
            }
        }
        //역 승객
        if (other.gameObject.layer.Equals(GameValue.statiopassenger_layer))
        {
            if (near_stationpassenger)
            {
                space_state = 0;
                near_stationpassenger = false;
                //  Push_Space_UI.SetActive(false);
            }
        }

        // 표지판 
        if (other.gameObject.layer.Equals(GameValue.sign_layer))
        {
            if (near_sign)
            {
              //  space_state = 0;
              //  near_sign = false;
                //  Push_Space_UI.SetActive(false);
            }
        }




        //if (other.gameObject.layer.Equals(GameValue.NextTrain_layer))
        //{
        //    space_state = 0;
        //    jump_ok = true;
        //}

        //// 이전칸 trigger
        //if (other.gameObject.layer.Equals(GameValue.PrevTrain_layer))
        //{
        //    space_state = 0;
        //    jump_ok = true;
        //}



        if (other.gameObject.layer.Equals(GameValue.wall_layer)
            || other.gameObject.layer.Equals(GameValue.sofa_layer)
            || other.gameObject.layer.Equals(GameValue.itembox_layer))
        {
            player.WallConflictDirections_Reset();
        }   

    }


    private Vector3 CurPos = new Vector3(-1, 3.3f, -2.5f);
    private Quaternion CurRot = Quaternion.identity;//네트워크에서는 선언과 동시에 초기화해야한다 
    private Vector3 CurSize = new Vector3(1, 1, 1);//네트워크에서는 선언과 동시에 초기화해야한다 

    // Update is called once per frame
    void Update()
    {
        tr.localScale = new Vector3(player.size.x, player.size.y, player.size.z);
        if (!photonView.IsMine) return;

        whereIam = player.Where_Train;

        // 이 highlight는 나중에 따로 함수로 뺄고야 일단 정리ㅣ되면 빼겟음
        if (near_stair || near_sign || near_stationpassenger)
        {
            // 근데 이것도 사다리 올라가는 중에는 X 
            highlighter.Hover(hoverColor);
        }

        anim.SetFloat("RunTime", runTime);
        anim.SetBool("IsAttack", false);
        SetPlayerMoveSpeed();


        // 키입력
        GetKeyInput();


        if (stair_up)
        {
            // 사다리 오르고 있을 때
            player.To_UpStair(floor1.position.x);

            tr.position = new Vector3(player.position.x, player.position.y, player.position.z);

            Quaternion rot = Quaternion.identity;
            rot.eulerAngles = new Vector3(player.rotate.x, player.rotate.y, player.rotate.z);
            tr.rotation = rot;

            if (tr.position.y >= floor2.position.y)
            {
                stair_up = false;
                // 파티클도 추가하고 2층으로 올라간 위치에 생기게 해야함
                player.Where_Floor = 2;


                anim.SetBool("UpToLadder", false);

                player.On_Floor2_yPosition();
                MCam_Ctrl.uptoCeiling();

                // trainmanager의 trainctrl에 연결해서 그 컨트롤의 list 중 trainscript에서 천장 onoff 변경
                TrainGameManager.instance.TrainCtrl.trainscript[player.Where_Train - 1].Ceiling_OnOff(true);

                //false는 여기가 아니고 space눌렀을 때ㄹ.
            }

        }
        if (stair_down)
        {
            // 분명 up과 다른건 +,- 뿐인데
            // 얘는 제대로 안됨 왜일까?
            player.To_DownStair(floor2.position.x);
            tr.position = new Vector3(player.position.x, player.position.y, player.position.z);

            Quaternion rot = Quaternion.identity;
            rot.eulerAngles = new Vector3(player.rotate.x, player.rotate.y, player.rotate.z);
            tr.rotation = rot;
            // anim.speed = -1;


            if (tr.position.y <= floor1.position.y)
            {
                stair_down = false;
                // 1층으로 내려와
                player.Where_Floor = 1;

                anim.speed = 1;
                anim.SetBool("UpToLadder", false);
                player.On_Floor1_yPosition();
                MCam_Ctrl.inTrain();
            }
        }

        // 점프하고 있을 떄 
        if (!jump_now)
        {
            // 일단 가는방향 받아와야 하고

            //  player.Jump_NextTrain();
            player.Jump();
            // 여기서 계속 증가하고 
            if (anim.GetBool("IsJump"))
            {
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                {

                    anim.SetBool("IsJump", false);
                    jump_now = true;
                }

            }
        }


        // 카메라에 플레이어가 몇층에 있는지 전달 
        MCam_Ctrl.Change_floor(player.Where_Floor);


        switch (player.Where_Floor)
        {

            case 1:
                // 1층에서 칸 이동이나 그런거할 떄

                // 플레이어의 x좌표를 전달해줌(카메라 이동관련)
                MCam_Ctrl.GetPlayerX(player.position.x);

                break;
            case 2:
                MCam_Ctrl.GetPlayerX(player.position.x);
                break;
            case 3:
            case 4:
                MCam_Ctrl.GetPlayerX(player.position.x);
                break;
        }

        //////플레이어들이 어디에 있는지 확인
        //for (int i = 0; i < PhotonNetwork.CountOfPlayers; ++i)
        //{
        //    //각 플레이어에게 지금 어디냐고 rpc로 물어보고 rpc로 답을 받음
        //    photonView.RPC("Question_Where_I_am", RpcTarget.All, i); //, eachPlayerIn[i]);
        //}
    }

    //[PunRPC]
    //public void Question_Where_I_am(int who)//, int whichTrain)
    //{
    //    playerListController.eachPlayerIn[who] = playerListController.playerList[who].player.Where_Train;
    //}


    //내 id를 알려주고 내 위치를 변경하라고 알려줌
    [PunRPC]
    public void changeMy_Where_Train(int playerID, int i)
    {
        //Debug.LogError("시발!!! : " + playerID + " i 는 :" + i);
        playerListController.playerList[playerID].player.Where_Train = i;
        playerListController.eachPlayerIn[playerID] = playerListController.playerList[playerID].player.Where_Train;

        UIState_Ctrl.CallRPConTrainScrollBar();
        //UIState_Ctrl.onTrainScrollBar();
    }
    /// ////////////////////////////////////////////////////////////////////////
    // key 관련

    void GetKeyInput()
    {
        switch (player.Where_Floor)
        {
            // 플레이어가 몇 층에 있는지에 따라서 입력을 다르게 받을 거라서.
            // where_floor -> player가 이게 3이면 기관총에 앉아잇는거
            case 1:
                // 1층
                Player_key_floor1();
                break;
            case 2:
                // 2층
                Player_key_floor2();
                break;
            case 3:
                // player가 머신건 근처에 있으면 space_state가 2가 되고
                // 2층에 있을 때 머신건 근처에서 스페이스를 누르면 where_floor가 3됨
                Player_key_MachinGun();
                break;
            case 4:
                Player_key_Station();
                break;
            default:
                break;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            // 기차 추가
            TrainGameManager.instance.TrainCtrl.onTrainAddButtonClick();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            // 몬스터 추가

            TrainGameManager.instance.ConditionCtrl.onEnemyOnButton();
            TrainGameManager.instance.EnemyAppear = true;

        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            TrainGameManager.instance.allitemCtrl.ItemGet_FoodTomato_Button();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            TrainGameManager.instance.allitemCtrl.ItemGet_Hammer_Button();
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            TrainGameManager.instance.allitemCtrl.ItemGet_Nail_Button();
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            TrainGameManager.instance.allitemCtrl.ItemGet_MediPack_Button();
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            TrainGameManager.instance.allitemCtrl.ItemGet_WoodBoard_Button();
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            TrainGameManager.instance.allitemCtrl.ItemGet_Ironpan_Button();
        }
 

        Quaternion rot = Quaternion.identity;
        rot.eulerAngles = new Vector3(player.rotate.x, player.rotate.y, player.rotate.z);
        // tr.position = new Vector3(player.position.x, player.position.y, player.position.z);
        tr.position = Vector3.Lerp(tr.position, new Vector3(player.position.x, player.position.y, player.position.z), Time.deltaTime * 10.0f);
        tr.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5.0f);
        tr.localScale = Vector3.Lerp(tr.localScale, new Vector3(player.size.x, player.size.y, player.size.z), Time.deltaTime * 10.0f);

    }

    // 1층에 올라갔을 때의 키입력 함수
    void Player_key_floor1()
    {
        WhereTrain_CalculPosition(player.position.x);
        // 사다리 올라가는 중 아닐때만 가능
        if (!stair_up && jump_now)
        {
            if (Input.GetKey(KeyCode.A))
            {
                Move('a');
                anim.SetBool("IsWalk", true);
                runTime += Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.D))
            {
                Move('d');
                anim.SetBool("IsWalk", true);
                runTime += Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                Move('s');
                anim.SetBool("IsWalk", true);
                runTime += Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.W))
            {
                Move('w');
                anim.SetBool("IsWalk", true);
                runTime += Time.deltaTime;
            }

            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) ||
                Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.W))
            {
                anim.SetBool("IsWalk", false);
                runTime = 0;
            }


            if (Input.GetKeyDown(KeyCode.V))
            {
                // 사다리 가까이서 space누르면 올라가기 == 1
                // 기관총 앞에서 space누르면 기관총에 장착되기 == 2

                if (space_state.Equals((int)player_space_state.Ladder))
                {
                    stair_up = true;
                    anim.SetBool("UpToLadder", true);
                    near_stair = false;
                    //Push_Space_UI.SetActive(false);

                    floor1 = Near_Object.transform.GetChild(0);
                    floor2 = Near_Object.transform.GetChild(1);

                    //parti_player_move.SetActive(true);
                    // Instantiate(parti_player_move, tr.position+ Vector3.up*2.0f, Quaternion.identity);
                    // 그러고 나서사다리 끝나면
                    // 올라가고 stair_up = false; 하고
                    // 천장에 올라가면 뚜껑도 setactive.true해줘야되네
                }


            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (jump_now)
                {
                    //jump_now = true;
                    anim.SetBool("IsWalk", false);
                    anim.SetBool("IsJump", true);
                    jump_now = false;
                }
            }
        }

    }

    // 2층에 올라갔을 때의 키입력 함수
    void Player_key_floor2()
    {
        WhereTrain_CalculPosition(player.position.x);
        if (!stair_up && !stair_down)
        {
            // 그냥 2층으로 올라온 상태
            if (Input.GetKey(KeyCode.A))
            {
                // x = -1
                // 임시 이동
                Move('a');
                anim.SetBool("IsWalk", true);

            }
            if (Input.GetKey(KeyCode.D))
            {
                // x = +1  
                Move('d');
                anim.SetBool("IsWalk", true);
            }

            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) ||
                Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.W))
            {
                anim.SetBool("IsWalk", false);
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                // 머신건에 앉기
                if (space_state.Equals((int)player_space_state.Machine_gun))
                {
                    // 주변에 머신건이 있으면?
                    player.Where_Floor = 3;
                    MCam_Ctrl.EnemyAppear_Cam(true, player.Where_Train);

                    space_state = 0;
                    near_gun = false;
                    //Push_Space_UI.SetActive(false);
                }

                // 밑층으로 내려가기
                if (space_state.Equals((int)player_space_state.Ladder_Down))
                {
                    player.position.y = floor2.position.y;
                    TrainGameManager.instance.TrainCtrl.trainscript[player.Where_Train - 1].Ceiling_OnOff(false);
                    anim.SetBool("UpToLadder", true);
                    stair_down = true;
                }
            }
        }
    }

    void Player_key_MachinGun()
    {
        WhereTrain_CalculPosition(player.position.x);
        // player.where_floor = 3일 때 호출되는 함수.
        //머신건에 앉아있음

        // 카메라 위치 조정

        if (Input.GetKey(KeyCode.Q))
        {
            // 기관총에서 벗어나자!
            MCam_Ctrl.EnemyAppear_Cam(false, 0);
            player.Where_Floor = 2;
        }
        // 

        // 머신건의 각도 조절 
        if (Input.GetKey(KeyCode.S))
        {
            gun_ctrl.gun_down();
        }
        if (Input.GetKey(KeyCode.W))
        {
            gun_ctrl.gun_up();
        }
        if (Input.GetKey(KeyCode.A))
        {
            gun_ctrl.gun_left();

        }
        if (Input.GetKey(KeyCode.D))
        {
            gun_ctrl.gun_right();
        }

        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        {
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire();
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            m_Fired = false;
            //  m_CurrentLaunchForce = m_MinLaunchForce;
            // shoot sound 


        }
        else if (Input.GetKey(KeyCode.F) && !m_Fired)
        {
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.F) && !m_Fired)
        {
            Fire();
            TrainGameManager.instance.SoundManager.Machine_Gun_Sound_Play();
        }
        // 카메라 조절은 마우스로

    }
    void Player_key_Station()
    {
        if (!invincibility)
        {
            if (Input.GetKey(KeyCode.A))
            {
                Move('a');
                anim.SetBool("IsWalk", true);
                runTime += Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.D))
            {
                Move('d');
                anim.SetBool("IsWalk", true);
                runTime += Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                Move('s');
                anim.SetBool("IsWalk", true);
                runTime += Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.W))
            {
                Move('w');
                anim.SetBool("IsWalk", true);
                runTime += Time.deltaTime;
            }
          

            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) ||
                Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.W))
            {
                anim.SetBool("IsWalk", false);
                runTime = 0;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetBool("IsAttack", true);
                if (attack_possible)
                {
                    attackleach.GetComponent<Attack>().attack = true;
                }
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                anim.SetBool("IsAttack", false);
                attack_possible = true;

            }
        }

    }

    void Fire()
    {
        m_Fired = false;
        // bullet에 .velocity = m_CurrentLauchForce 전달해주고 
        BulletInfoSetting(TrainGameManager.instance.GetObject(0), m_CurrentLaunchForce);
        iTween.ShakePosition(gun_child.gameObject, iTween.Hash("time", 0.5f, "z", 0.2f));
        iTween.ShakePosition(gameObject, iTween.Hash("time", 0.5f, "z", 0.2f));
      

      m_CurrentLaunchForce = m_MinLaunchForce;
    }

    public void Move(char key)
    {
        player.Move(key);
        player.Animate_State(1); // walk로 state바꾸기
        // 아 c#에서는 enum을 어떻게 써야되는지 모르겠네
        // 좀 다른듯. 그래서 지금 있는 순서 idle,walk,jump,run,attack 유지하면서
        // 더 필요한 state들은 뒤로 추가하는걸로
    }

    public void SetPlayerMoveSpeed()
    {

        if (runTime > 2.0f)
        {
            player.speed = 35.0f;
        }
        else
        {
            player.speed = 20.0f;
        }
    }

    /// ////////////////////////////////////////////////////////////////////////
    ///// UI

    void Make_PushSpaceUI()
    {
        //   Push_Space_UI = Instantiate(Push_Space_UI_pref);
        // Push_Space_UI.name = "player1_PushSpace_UI";
        //   Push_Space_UI.transform.parent = TrainGameManager.instance.Info_Canvas.transform;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {//원격 네트워크 플레이어가 만든 탱크의 위치와 회전 정보를 실시간으로 동기화하도록 하는 콜백 함수
     //(쉽게 말해서 다른 유저의 실시간 위치와 회전 값이 보이게 하는것)
        if (stream.IsWriting)
        {//신호를 보낸다. 송신 로컬플레이의 위치 정보 송신(패킷을 날린다고 표현)
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
            stream.SendNext(tr.localScale);
        }
        else // 원격 플레이어의 위치 정보 수신
        {
            CurPos = (Vector3)stream.ReceiveNext();
            CurRot = (Quaternion)stream.ReceiveNext();
            CurSize = (Vector3)stream.ReceiveNext();
        }
    }

    /// ////////////////////////////////////////////////////////////////////////
    void WhereTrain_CalculPosition(float position)
    {

        float traindistance = GameValue.Train_distance; // -13
        float dist2 = traindistance / 2.0f; // -6.5

        if (position > 7.0f)
        {
            player.Where_Train = 0;
            photonView.RPC("changeMy_Where_Train", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber - 1, 0);
        }
        else
        {
            for (int i = 0; i < GameValue.MaxTrainNumber; i++)
            {
                if (((i * traindistance) + dist2) < position && ((i * traindistance) - dist2) > position)
                {
                    photonView.RPC("changeMy_Where_Train", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber - 1, i + 1);
                }

            }
        }


    }

    /// ////////////////////////////////////////////////////////////////////////
    /// 총알발사

    // 총알정보셋팅 여기서 물리계산
    void BulletInfoSetting(GameObject _Bullet, float _value)
    {
        if (_Bullet == null) return;
        _Bullet.transform.position = gun_child.GetChild(0).position; //총알 위치 설정
        _Bullet.transform.rotation = gun_child.localRotation;

        _Bullet.SetActive(true);

        _Bullet.GetComponent<Bullet_Ctrl>().CallMoveCoroutin(_value);
    }

    ///////////////////////////
    IEnumerator Beaten()
    {
        player.HP--;
        invincibility = true;
        //player.position.z--;
        anim.SetBool("IsWalk", false);
        runTime = 0;
        yield return new WaitForSeconds(1.0f);
        invincibility = false;
    }

    void BeatenPath()
    {
        itp.SetNode(0, new Vector3(10.0f, 0.0f, 0.0f));
        itp.SetNode(0, new Vector3(10.0f, 0.0f, 0.0f));
        itp.SetNode(0, new Vector3(10.0f, 0.0f, 0.0f));
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("New Path 1"), "time", 7));
    }

    IEnumerator CoinParticle( Transform other)
    {
        Debug.Log("들어옴");
        GameObject Cp = TrainGameManager.instance.GetObject(8);
        Cp.SetActive(true);
        TrainGameManager.instance.CoinNum += 10;
        Cp.transform.position = other.position;
      //  Cp.transform.Translate(Vector3.up);
        yield return new WaitForSeconds(3.0f);
        Cp.SetActive(false);
    }

}
