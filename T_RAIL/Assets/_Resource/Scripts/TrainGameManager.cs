using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using Photon.Pun;

public class TrainGameManager : MonoBehaviourPunCallbacks
{

    public enum prefab_list
    {
        bullet = 0,
        passenger = 1,
        stationpassenger = 2,
        dustparticle = 3,
        sofa = 4,
        box = 5,
        chicken = 6,
        egg = 7
    }


    public static TrainGameManager instance = null;// TrainGameManager();

    public int AllStat;
    public int NowsumStat
    {
        get
        {
            return Speed_stat + Defence_stat + Noise_stat;
        }
    }
    public int Defence_stat { get; set; }
    public int Speed_stat { get; set; }
    public int Noise_stat { get; set; }

    public float Defence { get; set; } // 기차의 내구도
    public float Speed { get; set; } // 현재 기차가 달리는 스피드 -> 맵에서 사용할거임
    public int Noise { get; set; } // 현재 기차가 내는 소음

    public int trainindex; // 지금 기차 몇개 붙어있는지
                           // 몇개 붙어있는지 가지고 제일 마지막 위치 -> 기관총
                           // 제일 마지막 위치 -> enemy1 

    public int totalPassenger; // 승객을 총 태운 횟수
    public int totalkickoutEnemy; // 적을 총 물리친 횟수
    public int nowPassenger=0; // 현재 기차 내부에 남아있는 승객의 총 수
    public int GetPassengerCount = 0;//역에서 구출한 승객수 -> 역 다음으로 넘어가서 기차에 앉으면 다시 0으로 바꿔놔야 함 

    public int StageNumber;

    public bool EnemyAppear { get; set; } // 몬스터가 등장한 상황 -> 기차 추가 되면 안됨

    //# ConditionCtrl
    public Condition_Ctrl ConditionCtrl;

    //# ItemCtrl
    public AllItem_Ctrl allitemCtrl;

    //# PlayerCtrl
    public Player_Ctrl playerctrl;

    // # TrainCtrl
    public Train_Ctrl TrainCtrl;

    public GameObject StateCtrl;

    // #SofaPassengerCtrl
    public SofaSitPassenger_Ctrl SofaSitPassengerCtrl;

    public GameObject InGame_Notice; // 게임 내에서의 알림사항 ex) 몬스터 등장
    public Text InGame_Text;

    // # UI
    public GameObject Info_Canvas;

    // #Pool
    public GameObject[] Origin; // 프리팹들 원본

    // # Sound
    public SoundManager SoundManager;

    // 총알 )
    public List<GameObject> BulletManager; // 생성된 객체들을 저장할 리스트
    const int MAKE_BULLET_COUNT = 25;


    // 승객 )
    public List<GameObject> PassengerManager; // 생성된 객체들을 저장할 리스트
    const int MAKE_PASSENGER_COUNT = 20;

    // 역- 승객 )
    public List<GameObject> Station_PassengerManager;
    const int MAKE_STATIONPASSENGER_COUNT = 10;

    // 먼지 파티클
    public List<GameObject> DustParticle;
    const int MAKE_DUSTPARTICLE_COUNT = 5;  

    // 소파
    public List<GameObject> SofaManager;
    const int MAKE_SOFA_COUNT = 15;

    // 박스 
    public List<GameObject> BoxManager;
    const int MAKE_BOX_COUNT = 15;

    // 닭
    public List<GameObject> ChickenManager;
    const int MAKE_CHICKEN_COUNT = 10;

    //달걀
    public List<GameObject> EggManager;
    const int MAKE_EGG_COUNT = 10;


    public int Scene_state=1;


    public int LeftHandItem;
    public int RightHandItem;

  


    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        AllStat = 11;
        Speed_stat = 3;
        Noise_stat = 3;
        Defence_stat = 3;
    }
    private void Start()
    {
        SetObject_List(); // setobject 해야할 것들

    }
    public void Error_print()
    {
        Debug.Log("Error Somewhere");
    }

    public void SetObject_List()
    {
        CreateObject(Origin[(int)prefab_list.bullet], MAKE_BULLET_COUNT, (int)prefab_list.bullet); //총알생성
        CreateObject(Origin[(int)prefab_list.passenger], MAKE_PASSENGER_COUNT, (int)prefab_list.passenger); //승객생성
        CreateObject(Origin[(int)prefab_list.stationpassenger], MAKE_STATIONPASSENGER_COUNT, (int)prefab_list.stationpassenger); //승객생성
        CreateObject(Origin[(int)prefab_list.dustparticle], MAKE_DUSTPARTICLE_COUNT, (int)prefab_list.dustparticle); //승객생성
        CreateObject(Origin[(int)prefab_list.sofa], MAKE_SOFA_COUNT, (int)prefab_list.sofa); //소파생성
        CreateObject(Origin[(int)prefab_list.box], MAKE_BOX_COUNT, (int)prefab_list.box); //박스생성
        CreateObject(Origin[(int)prefab_list.chicken], MAKE_CHICKEN_COUNT, (int)prefab_list.chicken); // 치킨생성
        CreateObject(Origin[(int)prefab_list.egg], MAKE_EGG_COUNT, (int)prefab_list.egg); // 치킨생성
    }
    public void Notice_EnemyAppear()
    {
        InGame_Notice.SetActive(true);
        // 여기는 striingbuilder로 바꾸기
        InGame_Text.text = "코뿔소 등장 ! ";
    }
    ////////////////////////////////  pool   //////////////////////////////
    ///
    public void CreateObject(GameObject _obj, int _count, int prefab_index)
    {
        for (int i = 0; i < _count; i++)
        {
            switch (prefab_index)
            {
                case (int)prefab_list.bullet:
                    GameObject obj = Instantiate(_obj);
                    obj.transform.localPosition = Vector3.zero;
                    obj.SetActive(false);
                    obj.transform.parent = transform.GetChild(prefab_index);
                    BulletManager.Add(obj); 
                    break;
                case (int)prefab_list.passenger:
                    obj = PhotonNetwork.Instantiate(_obj.name, new Vector3(0,0,0), _obj.transform.rotation,0);
                    obj.transform.localPosition = Vector3.zero;
                    obj.SetActive(false);
                    obj.transform.parent = transform.GetChild(prefab_index);
                    PassengerManager.Add(obj);
                    break;
                case (int)prefab_list.stationpassenger:
                    obj = Instantiate(_obj);
                    obj.transform.localPosition = Vector3.zero;
                    obj.SetActive(false);
                    obj.transform.parent = transform.GetChild(prefab_index);
                    Station_PassengerManager.Add(obj);
                    break;
                case (int)prefab_list.dustparticle:
                    obj = Instantiate(_obj);
                    obj.transform.localPosition = Vector3.zero;
                    obj.SetActive(false);
                    obj.transform.parent = transform.GetChild(prefab_index);
                    DustParticle.Add(obj);
                    break;
                case (int)prefab_list.sofa:
                    obj = Instantiate(_obj);
                    obj.transform.localPosition = Vector3.zero;
                    obj.SetActive(false);
                    obj.transform.parent = transform.GetChild(prefab_index);
                    SofaManager.Add(obj);
                    break;
                case (int)prefab_list.box:
                    obj = Instantiate(_obj);
                    obj.transform.localPosition = Vector3.zero;
                    obj.SetActive(false);
                    obj.transform.parent = transform.GetChild(prefab_index);
                    BoxManager.Add(obj);
                    break;
                case (int)prefab_list.chicken:
                    obj = Instantiate(_obj);
                    obj.transform.localPosition = Vector3.zero;
                    obj.SetActive(false);
                    obj.transform.parent = transform.GetChild(prefab_index);
                    ChickenManager.Add(obj);
                    break;
                case (int)prefab_list.egg:
                    obj = Instantiate(_obj);
                    obj.transform.localPosition = Vector3.zero;
                    obj.SetActive(false);
                    obj.transform.parent = transform.GetChild(prefab_index);
                    EggManager.Add(obj);
                    break;
            }
        }
    }
    public GameObject GetObject(int _objIndex)
    {
        //필요한 오브젝트를 찾아서 반환
        switch (_objIndex)
        {
            case (int)prefab_list.bullet:
                if (BulletManager == null)
                {
                    return null;
                }
                int Count = BulletManager.Count;

                for (int i = 0; i < Count; i++)
                {
                    GameObject obj = BulletManager[i];

                    //활성화 돼있으면
                    if (obj.active == true)
                    {
                        // 리스트의 마지막까지 돌았는데 다 사용중이다?
                        if (i == Count - 1)
                        {
                            CreateObject(obj, 1, _objIndex);
                            return BulletManager[i + 1];
                        }
                        continue;
                    }
                    return BulletManager[i];
                }
                return null;

            case (int)prefab_list.passenger:

                if (PassengerManager == null)
                {
                    return null;
                }
                int p_Count = PassengerManager.Count;

                for (int i = 0; i < p_Count; i++)
                {
                    GameObject obj = PassengerManager[i];

                    //활성화 돼있으면
                    if (obj.active == true)
                    {
                        // 리스트의 마지막까지 돌았는데 다 사용중이다?
                        if (i == p_Count - 1)
                        {
                            CreateObject(obj, 1, _objIndex);
                            return PassengerManager[i + 1];
                        }
                        continue;
                    }
                    return PassengerManager[i];
                }
                return null;
            case (int)prefab_list.stationpassenger:

                if (Station_PassengerManager == null)
                {
                    return null;
                }
                int sp_Count = Station_PassengerManager.Count;

                for (int i = 0; i < sp_Count; i++)
                {
                    GameObject obj = Station_PassengerManager[i];

                    //활성화 돼있으면
                    if (obj.active == true)
                    {
                        // 리스트의 마지막까지 돌았는데 다 사용중이다?
                        if (i == sp_Count - 1)
                        {
                            CreateObject(obj, 1, _objIndex);
                            return Station_PassengerManager[i + 1];
                        }
                        continue;
                    }
                    return Station_PassengerManager[i];
                }
                return null;
            case (int)prefab_list.dustparticle:

                if (DustParticle == null)
                {
                    return null;
                }
                int d_Count = DustParticle.Count;

                for (int i = 0; i < d_Count; i++)
                {
                    GameObject obj = DustParticle[i];

                    //활성화 돼있으면
                    if (obj.active == true)
                    {
                        // 리스트의 마지막까지 돌았는데 다 사용중이다?
                        if (i == d_Count - 1)
                        {
                            CreateObject(obj, 1, _objIndex);
                            return DustParticle[i + 1];
                        }
                        continue;
                    }
                    return DustParticle[i];
                }
                return null;

            case (int)prefab_list.sofa:

                if (SofaManager == null)
                {
                    return null;
                }
                int s_Count = SofaManager.Count;

                for (int i = 0; i < s_Count; i++)
                {
                    GameObject obj = SofaManager[i];

                    //활성화 돼있으면
                    if (obj.active == true)
                    {
                        // 리스트의 마지막까지 돌았는데 다 사용중이다?
                        if (i == s_Count - 1)
                        {
                            CreateObject(obj, 1, _objIndex);
                            return SofaManager[i + 1];
                        }
                        continue;
                    }
                    return SofaManager[i];
                }
                return null;
            case (int)prefab_list.box:

                if (BoxManager == null)
                {
                    return null;
                }
                int b_Count = BoxManager.Count;
          
                for (int i = 0; i < b_Count; i++)
                {
                    GameObject obj = BoxManager[i];

                    //활성화 돼있으면
                    if (obj.active == true)
                    {
                        // 리스트의 마지막까지 돌았는데 다 사용중이다?
                        if (i == b_Count - 1)
                        {
                            CreateObject(obj, 1, _objIndex);
                            return BoxManager[i + 1];
                        }
                        continue;
                    }
                    return BoxManager[i];
                }
                return null;
            case (int)prefab_list.chicken:

                if (ChickenManager == null)
                {
                    return null;
                }
                int c_Count = ChickenManager.Count;

                for (int i = 0; i < c_Count; i++)
                {
                    GameObject obj = ChickenManager[i];

                    //활성화 돼있으면
                    if (obj.active == true)
                    {
                        // 리스트의 마지막까지 돌았는데 다 사용중이다?
                        if (i == c_Count - 1)
                        {
                            CreateObject(obj, 1, _objIndex);
                            return ChickenManager[i + 1];
                        }
                        continue;
                    }
                    return ChickenManager[i];
                }
                return null;

            case (int)prefab_list.egg:

                if (EggManager == null)
                {
                    return null;
                }
                int e_Count = EggManager.Count;

                for (int i = 0; i < e_Count; i++)
                {
                    GameObject obj = EggManager[i];

                    //활성화 돼있으면
                    if (obj.active == true)
                    {
                        // 리스트의 마지막까지 돌았는데 다 사용중이다?
                        if (i == e_Count - 1)
                        {
                            CreateObject(obj, 1, _objIndex);
                            return EggManager[i + 1];
                        }
                        continue;
                    }
                    return EggManager[i];
                }
                return null;
            default:
                return null;


        }
    }

    // 메모리 삭제
    public void MemoryDelete(int _objindex)
    {
        switch (_objindex)
        {
            case (int)prefab_list.bullet:

                if (BulletManager == null)
                {
                    return;
                }

                int Count = BulletManager.Count;

                for (int i = 0; i < Count; i++)
                {
                    GameObject obj = BulletManager[i];
                    GameObject.Destroy(obj);
                }
                BulletManager = null;
                break;

            case (int)prefab_list.passenger:

                if (PassengerManager == null)
                {
                    return;
                }

                int p_Count = PassengerManager.Count;

                for (int i = 0; i < p_Count; i++)
                {
                    GameObject obj = PassengerManager[i];
                    GameObject.Destroy(obj);
                }
                PassengerManager = null;
                break;

            case (int)prefab_list.stationpassenger:

                if (Station_PassengerManager == null)
                {
                    return;
                }

                int sp_Count = Station_PassengerManager.Count;

                for (int i = 0; i < sp_Count; i++)
                {
                    GameObject obj = Station_PassengerManager[i];
                    GameObject.Destroy(obj);
                }
                Station_PassengerManager = null;
                break;

            case (int)prefab_list.sofa:

                if (SofaManager == null)
                {
                    return;
                }

                int s_Count = SofaManager.Count;

                for (int i = 0; i < s_Count; i++)
                {
                    GameObject obj = SofaManager[i];
                    GameObject.Destroy(obj);
                }
                SofaManager = null;
                break;

            case (int)prefab_list.box:

                if (BoxManager == null)
                {
                    return;
                }

                int b_Count = BoxManager.Count;

                for (int i = 0; i < b_Count; i++)
                {
                    GameObject obj = BoxManager[i];
                    GameObject.Destroy(obj);
                }
                BoxManager = null;
                break;
            case (int)prefab_list.chicken:

                if (ChickenManager == null)
                {
                    return;
                }

                int c_Count = ChickenManager.Count;

                for (int i = 0; i < c_Count; i++)
                {
                    GameObject obj = ChickenManager[i];
                    GameObject.Destroy(obj);
                }
                ChickenManager = null;
                break;

            case (int)prefab_list.egg:

                if (EggManager == null)
                {
                    return;
                }

                int e_Count = EggManager.Count;

                for (int i = 0; i < e_Count; i++)
                {
                    GameObject obj = EggManager[i];
                    GameObject.Destroy(obj);
                }
                EggManager = null;
                break;
        }
    }
    ///////////////////////////////////////////////////////////////////////////////////////
    ///



}
