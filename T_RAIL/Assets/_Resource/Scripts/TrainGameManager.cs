using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainGameManager : MonoBehaviour
{

    enum prefab_list
    {
        bullet = 0,
    }


    public static TrainGameManager instance = null;

    // 여기서 각 플레이어들 체력 관리해야되나?
    // 여기서 뭐해야될까

    // 아 머리깨지겠네 진짜 스트ㅡ레스당


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


    // 총알 pool
    public GameObject[] Origin; // 프리팹들 원본
    public List<GameObject> BulletManager; // 생성된 객체들을 저장할 리스트
    const int nMAKE_BULLET_COUNT = 20;




    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        SetObject(Origin[(int)prefab_list.bullet], 20, (int)prefab_list.bullet); //총알생성
    }
    public void Error_print()
    {
        Debug.Log("Error Somewhere");
    }


    public void Notice_EnemyAppear()
    {
        InGame_Notice.SetActive(true);
        InGame_Text.text = "코뿔소 등장 ! ";
    }

    /////////////////pool
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
            }
          

        }
    }
    public GameObject GetObject(int _objIndex)
    {
        //필요한 오브젝트를 찾아서 반환
        switch (_objIndex)
        {
            case (int)prefab_list.bullet:
                if(BulletManager == null)
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

                if(BulletManager == null)
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


        }
    }
}
