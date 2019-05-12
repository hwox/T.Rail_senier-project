using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class TrainGameManager : MonoBehaviour
{

    public enum prefab_list
    {
        bullet = 0,
        passenger = 1,
        stationpassenger = 2,
        dustparticle = 3,
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
    public int nowPassenger; // 현재 기차 내부에 남아있는 승객의 총 수


    public int StageNumber;

    public bool EnemyAppear { get; set; } // 몬스터가 등장한 상황 -> 기차 추가 되면 안됨

    //# ConditionCtrl
    public Condition_Ctrl ConditionCtrl;

   

    // # TrainCtrl
    public Train_Ctrl TrainCtrl;

    public GameObject StateCtrl;

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
    const int MAKE_DUSTPARTICLE_COUNT = 5;  public int GetPassengerCount=0; 


    public bool InStation;

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
            GameObject obj = Instantiate(_obj);
            obj.transform.localPosition = Vector3.zero; 
            obj.SetActive(false);
            obj.transform.parent = transform.GetChild(prefab_index);
            switch (prefab_index)
            {
                case (int)prefab_list.bullet:
                    BulletManager.Add(obj); // 
                    break;
                case (int)prefab_list.passenger:
                    PassengerManager.Add(obj);
                    break;
                case (int)prefab_list.stationpassenger:
                    Station_PassengerManager.Add(obj);
                    break;
                case (int)prefab_list.dustparticle:
                    DustParticle.Add(obj);
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
        }
    }
    ///////////////////////////////////////////////////////////////////////////////////////
    ///



}
