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
        debris1 = 2,
        debris2 = 3,
        debris3 = 4,
        debris4 = 5,
        debris5 = 6,
    }


    public static TrainGameManager instance = null;// TrainGameManager();


    public float Durability { get; set; } // 기차의 내구도
    public float speed { get; set; } // 현재 기차가 달리는 스피드 -> 맵에서 사용할거임
    public float noise { get; set; } // 현재 기차가 내는 소음

    public int trainindex; // 지금 기차 몇개 붙어있는지
                           // 몇개 붙어있는지 가지고 제일 마지막 위치 -> 기관총
                           // 제일 마지막 위치 -> enemy1 

    //# ConditionCtrl
    public Condition_Ctrl ConditionCtrl;

    // # TrainCtrl
    public Train_Ctrl TrainCtrl;

    public GameObject InGame_Notice; // 게임 내에서의 알림사항 ex) 몬스터 등장
    public Text InGame_Text;

    // # UI
    public GameObject Info_Canvas;


    // #Pool
    public GameObject[] Origin; // 프리팹들 원본

    // 총알 )
    public List<GameObject> BulletManager; // 생성된 객체들을 저장할 리스트
    const int MAKE_BULLET_COUNT = 25;
    // 총알 잔해 )

    public List<GameObject> BulletDebrisManager; //
    const int MAKE_BULLETDEBRIS_COUNT = 10; // 한 방에 한 10개 정도?
    public StringBuilder debris_name = new StringBuilder();

    // 승객 )
    public List<GameObject> PassengerManager; // 생성된 객체들을 저장할 리스트
    const int MAKE_PASSENGER_COUNT = 20;



    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        SetObject_List(); // setobject 해야할 것들
        Save_DebrisName(); // debris 의 이름들을 (나중에 호출할 때 사용) stringbuilder에 저장
    }
    public void Error_print()
    {
        Debug.Log("Error Somewhere");
    }
    public void Save_DebrisName()
    {
        debris_name.Append("debris1");
        debris_name.Append("debris2");
        debris_name.Append("debris3");
        debris_name.Append("debris4");
        debris_name.Append("debris5");
    }
    public void SetObject_List()
    {
        SetObject(Origin[(int)prefab_list.bullet], MAKE_BULLET_COUNT, (int)prefab_list.bullet); //총알생성
        SetObject(Origin[(int)prefab_list.passenger], MAKE_PASSENGER_COUNT, (int)prefab_list.passenger); //승객생성
        SetObject(Origin[(int)prefab_list.debris1], MAKE_BULLETDEBRIS_COUNT, (int)prefab_list.debris1); // 총알 잔해 생성
        SetObject(Origin[(int)prefab_list.debris2], MAKE_BULLETDEBRIS_COUNT, (int)prefab_list.debris2); // 총알 잔해 생성
        SetObject(Origin[(int)prefab_list.debris3], MAKE_BULLETDEBRIS_COUNT, (int)prefab_list.debris3); // 총알 잔해 생성
        SetObject(Origin[(int)prefab_list.debris4], MAKE_BULLETDEBRIS_COUNT, (int)prefab_list.debris4); // 총알 잔해 생성
        SetObject(Origin[(int)prefab_list.debris5], MAKE_BULLETDEBRIS_COUNT, (int)prefab_list.debris5); // 총알 잔해 생성
    }
    public void Notice_EnemyAppear()
    {
        InGame_Notice.SetActive(true);

        // 여기는 striingbuilder로 바꾸기
        InGame_Text.text = "코뿔소 등장 ! ";
    }

    ////////////////////////////////  pool   //////////////////////////////
    ///
    public void SetObject(GameObject _obj, int _count, int prefab_index)
    {
        for (int i = 0; i < _count; i++)
        {
            GameObject obj = Instantiate(_obj);
            obj.transform.localPosition = Vector3.zero; // 위치정하기
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
                case (int)prefab_list.debris1:
                case (int)prefab_list.debris2:
                case (int)prefab_list.debris3:
                case (int)prefab_list.debris4:
                case (int)prefab_list.debris5:
                    BulletDebrisManager.Add(obj);
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
                            SetObject(obj, 1, _objIndex);
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
                            SetObject(obj, 1, _objIndex);
                            return PassengerManager[i + 1];
                        }
                        continue;
                    }
                    return PassengerManager[i];
                }
                return null;
            case (int)prefab_list.debris1:
            case (int)prefab_list.debris2:
            case (int)prefab_list.debris3:
            case (int)prefab_list.debris4:
            case (int)prefab_list.debris5:
                if (PassengerManager == null)
                {
                    return null;
                }
                int d_Count = BulletDebrisManager.Count;

                for (int i = 0; i < d_Count; i += 5)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        GameObject obj = BulletDebrisManager[i + j];
                        
                        if (obj.active == true)
                        {

                            return null;
                        }
                        return BulletDebrisManager[i + j];
                    }

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
            case (int)prefab_list.debris1:
            case (int)prefab_list.debris2:
            case (int)prefab_list.debris3:
            case (int)prefab_list.debris4:
            case (int)prefab_list.debris5:
                if (BulletDebrisManager == null)
                {
                    return;
                }

                int d_Count = BulletDebrisManager.Count;

                for (int i = 0; i < d_Count; i++)
                {
                    GameObject obj = BulletDebrisManager[i];
                    GameObject.Destroy(obj);
                }
                BulletDebrisManager = null;
                break;
        }
    }
    ///////////////////////////////////////////////////////////////////////////////////////
    ///


}
