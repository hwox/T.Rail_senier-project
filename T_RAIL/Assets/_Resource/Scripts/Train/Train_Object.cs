using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Train_Object : MonoBehaviourPunCallbacks
{


    public float HP { get; set; } //  (기차의 체력) 
    float PrevHP = 80; // 얘는 %이다. 80으로 시작하는 이유는 100, 80, 60, 40, 20, 0 이렇게 체크할건데 
    // (일단 테스트하려고 올려놨음.)
    // 80으로 체크해야ㅑ 깎이는지 확인가능
    
    [SerializeField]
    int index; // 몇번째 기차인지 

    Transform tr;

    bool Position_Set_Go; // 포지션을 세팅하면서 달려와서 붙는거. slerp 연산 중에는 true, 연산 끝나면 false
    Vector3 Position_Set_Destination;


    int passenger; // 이 기차에 승객이 몇명있는지
    int box;  // 이 기차에 박스 몇개있는지

   // bool on2Floor; // 2층에 누가 있는지없는지

    float Coroutine_calltime; // 코루틴 안끄곸ㅋㅋㅋㅋ 그 안에 호출할 상황이면 0.01
                              // 호출안할 상황이면 0.5


    public int[] ThisTrainNowObjects; // 4개의 영역에 각각 어떤 오브젝트가 들어가있는지 


   // 미리가지고 있어야 할 기차 내부의 메타
    public GameObject Machine_gun;
    public GameObject Ladder;
    public GameObject Ceiling; // 천장
    public GameObject BackWall;// 제일 끝에 wall. 플레이어가 못 빠져나가게
    public GameObject TrainChain; // chain. 제일 마지막 칸만 off될 예정

    public GameObject[] FracturedWall; // HP 떨어질 때마다 기차의 내구도 변화를 표현하는 기차의 벽면 0부터 3까지 왼쪽 순서대로
    public bool[] BrokenWall; // 벽 부셔졌으면 true


    public bool[] InTrainObjectUsed;
    public GameObject[] choiceInTrainObject;
    [SerializeField]
    public Train_Ctrl ctrl;

    PhotonView photonView;

    private void Awake()
    {
        ctrl = TrainGameManager.instance.TrainCtrl.GetComponent<Train_Ctrl>();
        photonView = GetComponent<PhotonView>();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        tr = gameObject.transform;
        Coroutine_calltime = 0.5f;
        StartCoroutine(Train_Position_Setting_Change());
        StartCoroutine(TrainHPCheck()); // 어차피 Train_Object()가 먼저 실행되니까 여기서 코루틴 실행해도 HP 100부터.
        ThisTrainNowObjects = new int[4];
        InTrainObjectUsed = new bool[4];
        BrokenWall = new bool[4];
        Init_AddTrain();
    }

    public Train_Object()
    {
        HP = GameValue.Train_Standard_HP; // 기차의 기본 체력
        Position_Set_Go = true;
        CoroutineCallTimeSet(Position_Set_Go);
        // 처음 기차의 초깃값 설정 
        // 여기로 처음에는 가야돼
    }

    public void Run_TrainHPMinus(float meter)
    {
        // 기차가 달릴수록 체력이 감소
        HP -= meter;
    }

    public void ChangeTrainSetting(int _index)
    {

        SetIndex(_index);

        // 근데 만약에 1번 기차가 떨어지면??

        if (index.Equals(1))
        {
            Position_Set_Go = false;
            // 1번 기차는 그럴 필요가 없어서
            // 일단 꺼놨음

            StopCoroutine(Train_Position_Setting_Change());
            // 코루틴도 껐음 1번기차는
        }
        else
        {
            Position_Set_Go = true;
            Position_Set_Destination = new Vector3(GameValue.Train_distance * (_index - 1),
            GameValue.Train_y, GameValue.Train_z);

        }

    }

    public void Machine_Gun_OnOff(bool onoff)
    {
        // train_ctrl에서 기차가 새로 add되거나 delete되면서 기차의 index가 변하고
        // 제일 마지막에 있는 머신건만 켜져야하니까 마지막이면 true 아니면 false
        if (onoff)
        {
            Machine_gun.SetActive(true);
            BackWall.SetActive(true);
            TrainChain.SetActive(false);  // 여기서 false 되는거 맞음! 제일 마지막 칸이라서 ㅊㅔ인꺼주는거
        }
        else
        {
            Machine_gun.SetActive(false);
            BackWall.SetActive(false);
            TrainChain.SetActive(true);
        }
    }
    public void Ceiling_OnOff(bool onoff)
    {
        // 천장 onoff함수
        if (onoff)
        {
            Ceiling.SetActive(true);
        }
        else
        {
            Ceiling.SetActive(false);
        }
    }
    public void SetIndex(int _index)
    {
        index = _index;
    }
    void CoroutineCallTimeSet(bool _Position_Set_Go)
    {
        // position_set_go를 넣을거임
        if (_Position_Set_Go)
        {
            Coroutine_calltime = 0.001f;
        }
        else
        {
            Coroutine_calltime = 0.5f;
        }
    }
    void Position_Set()
    {
        // 기차가 slerp 연산을 통해 add가 될 때 자연스럽게 달려오면서 붙는것처럼 하기위한 position_set
        // 코루틴에서 호출중 -> 굳이 update에서 호출할 필요없어서
        if (Position_Set_Go)
        {
            tr.position = Vector3.Slerp(tr.position, Position_Set_Destination, Time.deltaTime * 30.0f);

            if (tr.position.x == Position_Set_Destination.x)
            {
                Position_Set_Go = false;
                CoroutineCallTimeSet(Position_Set_Go);
            }
        }
    }
    // startcoroutine쓰면서 stopcoroutine쓸거면
    // iEnumerator 변수명
    // 이렇게해서 startcoroutine(변수명)
    // 이렇게 지정해줘야 한다는데 애초에 nullrefer가 뜨는데
    // 그럴거면 그냥 코루틴 게속 켜놓는 게 나을거같음
    IEnumerator Train_Position_Setting_Change()
    {
        while (true)
        {
            Position_Set();

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator TrainHPCheck()
    {
        while (true)
        {
            // 조건 필요
            if (HP < PrevHP && HP >= 10)
            {
                FracturedTrain();
                PrevHP -= 20;
            }

            if(HP <= 0)
            {
                if (index != 1)
                {
                    Debug.Log("끝");
                    ctrl.Train_Delete(index - 1);
                }
                else if(index == 1)
                {
                    // 1번기차면 게임이 끝나야돼!
                    // 우선은 break;
                }
            }
            yield return new WaitForSeconds(0.3f);
        }

    }
   void Init_AddTrain()
    {

        //train Add부분 한번 호출했으니 여기로 옮김
        ctrl.train.Add(this.gameObject);
        ctrl.trainscript.Add(this);
        TrainGameManager.instance.trainindex = ctrl.train.Count;
        ChangeTrainSetting(ctrl.train.Count);

        if (TrainGameManager.instance.trainindex != 1)
        {
            ctrl.train[TrainGameManager.instance.trainindex - 1].transform.position =
                new Vector3(GameValue.Train_distance * (ctrl.train.Count), GameValue.Train_y, GameValue.Train_z);

        }
        else if (TrainGameManager.instance.trainindex == 1)
        {
            ctrl.train[TrainGameManager.instance.trainindex - 1].transform.position =
                new Vector3(GameValue.Train_distance * (ctrl.train.Count - 1), GameValue.Train_y, GameValue.Train_z);
        }

        this.gameObject.SetActive(true);

        for (int i = 0; i < TrainGameManager.instance.trainindex; i++)
        {
            if (i < TrainGameManager.instance.trainindex - 1)
            {
                ctrl.trainscript[i].Machine_Gun_OnOff(false);
            }
            else
            {
                ctrl.trainscript[i].Machine_Gun_OnOff(true);
            }
        }


        //if (ctrl.train.Count == 1) 
            Invoke("callFirstTrainInit", 3.0f);
    }

    public void callFirstTrainInit()
    {
        for (int i = 0; i < 4; i++)
        {
            this.gameObject.transform.GetChild(1).GetChild(i).gameObject.GetComponent<InTrainObjectMake>().InitSetting(ctrl.train.Count, i);
        }
    }


    public void InTrainObject_Setting(GameObject _obj, int _whatnumber, int _kind)
    {
        // 여기서 이제 전달받은 오브젝트를 
        // setactive해주고 위치를 알맞게 조정해주는 것

        // 이 if문 왜 주석처리해야하냐면 어차피 수리도구로 바꿔야 돼서 주석처리해야함

        if (!InTrainObjectUsed[_whatnumber]) {
            ThisTrainNowObjects[_whatnumber] = _kind;
            _obj.transform.parent = this.transform.GetChild(1);
            switch (_kind)
            {
                case 1:
                    //소파
                    _obj.transform.localPosition = new Vector3(GameValue.T_ObjectX, GameValue.T_Sofa_ObjectY, GameValue.T_ObjectZ[_whatnumber]);
                    _obj.transform.Rotate(0, 0, -90);
                    _obj.GetComponent<InSofaPassenger>().ActiveThisSofa();
                    break;
                case 2:
                    // 박스 
                    _obj.transform.localPosition = new Vector3(GameValue.T_ObjectX, GameValue.T_Box_ObjectY, GameValue.T_ObjectZ[_whatnumber]);
                    _obj.GetComponent<InBoxItem>().ActiveThisBox();
                    _obj.SetActive(true);
                    break;

            }

            _obj.SetActive(true);
            
            // 부모로 이 밑으로 달아주고 로컬 포지션을 바꿨음
            InTrainObjectUsed[_whatnumber] = true; // 사용중이니까 false시킴
        }
        // 없앨때는 parent true시키고 다시 저리로 가져다 줘야 함. 아니면 그냥 delete 시켜버리거나
    }


    public void FracturedTrain()
    {
        int randomWall = Random.Range(0, 4);

        if (FracturedWall[randomWall].active && !BrokenWall[randomWall])
        {
            // 켜져있으면 끄고 끝
            FracturedWall[randomWall].SetActive(false);
            BrokenWall[randomWall] = true;
        
        }
        else
        {
            FracturedTrain();
        }
    }
}
