using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.UI;
public class Train_Ctrl : MonoBehaviourPunCallbacks
{
    // 여기서 기차의 속성을 총 감독
    // 기차의 Train_Object에는 기차의 '체력' 을 관리.

    //public float Durability { get; set; } // 기차의 내구도
    //public float speed { get; set; } // 현재 기차가 달리는 스피드 -> 맵에서 사용할거임
    //public float noise { get; set; } // 현재 기차가 내는 소음

    public int InTrain_Passenger; // 기차안에 승객이 몇명있는지

    int speed_count = 1; // 스피드 몇단계인지 스피드 [1~4]단계
    public float Run_Meter { get; set; } // 달린미터


    public GameObject Train_Prefab;
    public List<GameObject> train = new List<GameObject>();
    // 얘를 이렇게 하나 더 한 이유 -> 쓸 때 마다 호출하면 낭비
    [SerializeField]
    public List<Train_Object> trainscript = new List<Train_Object>();


    public Text RunMeterText;

    float perMeter = 0.03f; // 이름 뭐로 해야할지 모르겠어서 이걸로해씀 나중에 바꿀지도 기차 달릴 떄마다
    // 기차 HP 줄어드는데 그거 얼만큼 감소할지


    // 기차 처음 시작할 때 슬슬 빨라지는 애니메이션 추가하자
    // Mathf 로 계산해서


    private void Awake()
    {
        // 기본 초기화
        TrainGameManager.instance.Defence = GameValue.Durability;
        // speed = GameValue.speed;
        TrainGameManager.instance.Speed = GameValue.speed;
        TrainGameManager.instance.Noise = GameValue.noise;
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("Train_Add", RpcTarget.All);
        }

        StartCoroutine(TrainHPMinus());
    }

    private void Update()
    {
        //if (photonView.IsMine)
        RunMeterCalCulator();


        // InvokeRepeating("trainSoundInvoke", 1.0f, 10.0f);

    }

    IEnumerator TrainHPMinus()
    {
        yield return new WaitForSeconds(5.0f);

        while (true)
        {

            for (int i = 0; i < train.Count; i++)
            {
                trainscript[i].Run_TrainHPMinus(perMeter);

                yield return new WaitForSeconds(2.0f);
            }


        }
    }

    public void trainSoundInvoke()
    {
        TrainGameManager.instance.SoundManager.Train_Sound_Play();
    }

    public void RunMeterCalCulator()
    {
        if (TrainGameManager.instance.Scene_state == 1)
        {
            // 2분에 2km
            Run_Meter += (TrainGameManager.instance.Speed * TrainGameManager.instance.Speed_stat) * Time.deltaTime;

            TrainGameManager.instance.runmeter = Run_Meter;

            float temp = GameValue.NextStationMeter - Run_Meter;



            if (temp >= 0)
                RunMeterText.text = "남은 거리 :" + temp.ToString("N0") + "M";
            else
                RunMeterText.text = "남은 거리 : 0M";


            //Run_Meter
        }
        else
        {
            RunMeterText.text = " ";
        }

    }

    // 기차 추가하기
    public void onTrainAddButtonClick()
    {
        if (!TrainGameManager.instance.EnemyAppear)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("Train_Add", RpcTarget.All);
                //Debug.Log("마스터 클라가 눌럿음 ");
            }
            else
            {
                photonView.RPC("Train_Add", RpcTarget.All);
                //Debug.Log("다른 클라가 눌렀음 ");
            }
        }
    }

    [PunRPC]
    public void Train_Add()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (TrainGameManager.instance.trainindex < GameValue.MaxTrainNumber)
        {
            //GameObject newTrain = PhotonNetwork.InstantiateSceneObject(Train_Prefab.name, new Vector3(0, 2.5f, -2), Quaternion.Euler(0, -90, 0), 0, null) as GameObject;
            GameObject newTrain = PhotonNetwork.Instantiate(Train_Prefab.name, new Vector3(0, 2.5f, -2), Quaternion.Euler(0, -90, 0), 0, null) as GameObject;

        }

    }

    // 임시용
    public void AddTrainscript(int index)
    {
        trainscript.Add(train[index].GetComponent<Train_Object>());
    }

    [PunRPC]
    public void Machine_Gun_OnOff_RPC()
    {
        for (int i = 0; i < TrainGameManager.instance.trainindex; i++)
        {
            if (i < TrainGameManager.instance.trainindex - 1)
            {
                trainscript[i].Machine_Gun_OnOff(false);
            }
            else
            {
                trainscript[i].Machine_Gun_OnOff(true);
            }
        }
    }


    public void onTrainDeleteButton(int _removeindex)
    {
        //instance.trainindex-1 자리에 _removeindex를 그 인덱스가 Train_Delete로 전달됨
        photonView.RPC("Train_Delete", RpcTarget.All, TrainGameManager.instance.trainindex - 1);
    }

    [PunRPC]
    public void Train_Delete(int _removeindex)
    {
        ////일단 제일 마지막 칸이면 지워지지 않게
        //if (TrainGameManager.instance.trainindex == 1)
        //{
        //    return;
        //}

        // 세상에나..! 기차의 hp가 다 떨어져서 끝났어
        for (int i = TrainGameManager.instance.trainindex - 1; i >= _removeindex; i--)
        {

            // 만약에 removeindex면 뭔가 더 추가적으로 뭘 해야될거같음
            if (i.Equals(_removeindex))
            {
                // 추가적으로 실행할 파티클같은거
                // 얘가 일단 펑 터지고 그 다음에 기차에 불 한다음에 끝에서부터 떨어ㅣ는걸로
            }

            //이부분을 rpc로 바꿔서 삭제하면 그 함수를 부르기로. trainscript는 머신건 호출처럼 수정
            trainscript[i].Machine_Gun_OnOff(false);
            Destroy(train[i]);
            train.RemoveAt(i);
            TrainGameManager.instance.trainindex = train.Count;
            trainscript.RemoveAt(i);
        }
        trainscript[_removeindex - 1].Machine_Gun_OnOff(true);
    }


    public void Passenger_In()
    {
        // 역에서 승객을 태울 때
        InTrain_Passenger += 1;
    }
    public void Passenger_Out()
    {
        // 역에서 승객을 내리게 할때
        InTrain_Passenger -= 1;
    }

    public void onTrainStartButton()
    {
        photonView.RPC("RunStartTrain", RpcTarget.All);
    }


    [PunRPC]
    public void RunStartTrain()
    {
        TrainGameManager.instance.Speed = GameValue.speed;
        TrainGameManager.instance.Speed = 10.0f * speed_count;
    }


    public void StopTrain()
    {
        TrainGameManager.instance.Speed = 0.0f;
    }
    public void Wheel_Animation_Speed()
    {
        //  for (int i = 0; i < Wheel_Anim.Length; i++)
        {
            //     Wheel_Anim[i].speed = TrainGameManager.instance.speed / 20;
        }
    }

    // TrainAnimation_all
    void TrainAnimation()
    {
        // no contain Wheel
    }

    public void Train_HPMinus()
    {
        for (int i = 0; i < train.Count; i++)
        {
            //train[i].Run_TrainHPMinus(Run_Meter);
        }
    }
    public void Hide()
    {
        photonView.RPC("HideTrain_RPC", RpcTarget.All);
    }

    [PunRPC]
    void HideTrain_RPC()
    {
        for (int i = 0; i < TrainGameManager.instance.trainindex; i++)
        {
            train[i].SetActive(false);
        }
    }

    public void Appear()
    {
        for (int i = 0; i < TrainGameManager.instance.trainindex; i++)
        {
            train[i].SetActive(true);
        }
    }
    // hp 체크 코루틴
    // 얘를 gamemanger로 해야되나?
}
