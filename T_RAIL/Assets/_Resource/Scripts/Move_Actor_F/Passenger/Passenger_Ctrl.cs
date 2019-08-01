using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HighlightingSystem;
using UnityEngine.EventSystems;
using Photon.Pun;

public class Passenger_Ctrl : MonoBehaviourPunCallbacks
{
    Passenger_Actor pass;
    public Highlighter highlighter;
    Color myColor;
    public Color hoverColor;
    public bool Live; // 태우고 소파에 앉으면 Live

    public Canvas PassengerStateRender;
    public Canvas PassengerCareButtons;
    public Button Food;
    public Button Medical;
    public Image HungryGauge;
    public Image DiseaseGauge;

    //PhotonView photonView;

    bool Clicking; // 클릭해서 관리창 띄워놓은 상태

    Animator anim;

    AllItem_Ctrl item;

    private void Awake()
    {
        pass = new Passenger_Actor();

        anim = gameObject.GetComponent<Animator>();
        // highlighter = gameObject.GetComponent<Highlighter>();
        myColor = new Color(1.0f, 1.0f, 0.57f);
        hoverColor = myColor;
    }

    private void Start()
    {
        item = TrainGameManager.instance.allitemCtrl;

        this.transform.localPosition = Vector3.zero;
        this.gameObject.SetActive(false);
        this.transform.parent = TrainGameManager.instance.gameObject.transform.GetChild(1);// (int)prefab_list.passenger);
        TrainGameManager.instance.PassengerManager.Add(this.gameObject);

        //if (Live)
        //{
        //    //마스터클라이언트만 코루틴 실행하고 다른애들한텐 그 결과만 알려주기 
        //    if (!PhotonNetwork.IsMasterClient)
        //    {
        //        return;
        //    }
        //    else
        //    {

        //        StartCoroutine(PassengerIsEffectedByEnvironment());
        //    }
        //}
    }


    private void Update()
    {
        if (Live)
        {
            DiseaseGauge.fillAmount = (float)pass.Disease / 100.0f;
            HungryGauge.fillAmount = (float)pass.Hungry / 100.0f;

            // 여기서 이제 상태 계속 체크
            //   highlighter.Hover(hoverColor);
            if (pass.Disease > 99 || pass.Hungry > 99)
            {
                // 질병수치랑 배고픔 수치가 99보다 크면 사망
                Passenger_Die();
            }

            IsButtonEnable();


        }
    }

    public void IsButtonEnable()
    {
        if (item.Usable_MediPack())
        {
            Medical.interactable = true;
        }
        else
        {
            Medical.interactable = false;
        }

        if (item.Usable_Food())
        {
            Food.interactable = true;
        }
        else
        {
            Food.interactable = false;
        }
    }

    public void Passenger_LiveOn()
    {

        pass.Disease = 0; // 질병
        pass.Hungry = 0; // 배고픔


        Live = true;
        this.transform.localPosition = Vector3.zero;
        anim.SetBool("IsSit", true);

        //마스터클라이언트만 코루틴 실행하고 다른애들한텐 그 결과만 알려주기 
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        else
        {
            StartCoroutine(PassengerIsEffectedByEnvironment());
        }

    }


    public void Passenger_Die()
    {
        // 죽었을 때 호출할 함수

        // Die 추가할 때 확인할 거 코루틴 멈추는지 안멈추는지
        StopCoroutine(PassengerIsEffectedByEnvironment());

        DiseaseGauge.fillAmount = 0;
        HungryGauge.fillAmount = 0;
    }

    // 1. 승객 hover + 현재 승객 상태
    // 2. 승객을 클릭하면 약/ 음식 선택
    // 3. 

    public void PointerEnter()
    {
        if (!Clicking)
        {
            highlighter.Hover(hoverColor);
        }
    }

    public void ClickForPassengerCare()
    {
        Clicking = true;
        // 승객의 관리를 위해 승객을 클릭
        PassengerCareButtons.gameObject.SetActive(true);
        PassengerStateRender.gameObject.SetActive(true);
        //Clicking = true;
    }

    public void EatFood()
    {
        photonView.RPC("EatFood_RPC", RpcTarget.All);
    }

    [PunRPC]
    void EatFood_RPC()
    {
        // 음식먹는 버튼
        // 조건으로 아이템 체크해서 하기 추가

        pass.Hungry -= GameValue.HungryDecrease;
        item.Use_Food();
        // 이걸 false를 시킬까? 아니면 x를 추가할까
        PassengerCareButtons.gameObject.SetActive(false);
        PassengerStateRender.gameObject.SetActive(false);

        Clicking = false;
    }


    public void EatHeal()
    {
        photonView.RPC("EatHeal_RPC", RpcTarget.All);
    }

    [PunRPC]
    void EatHeal_RPC()
    {
        // 구급상자 먹는 버튼
        pass.Disease -= GameValue.DiseaseDncrease;
        item.Use_MediPack();

        PassengerCareButtons.gameObject.SetActive(false);
        PassengerStateRender.gameObject.SetActive(false);

        Clicking = false;
    }


    IEnumerator PassengerIsEffectedByEnvironment()
    {
        int random = Random.Range(0, 35); // 별도의 랜덤 클래스 만들어보기

        if (random % 5 == 0)
        {
            // 3의 배수면
            pass.Hungry += 10;
            pass.Disease += 10;
        }
        else if (random % 7 == 0)
        {
            // 5의 배수이면
            pass.Disease += 10;
        }
        else if (random % 9 == 0)
        {
            // 7의 배수이면
            pass.Hungry += 10;
        }
        else if (random % 13 == 0)
        {
            // 만약에 13 배수면
            pass.Hungry += 20;
        }
        else if (random % 17 == 0)
        {
            pass.Disease += 20;
        }

        photonView.RPC("setHungryDisease", RpcTarget.All , pass.Hungry, pass.Disease);


        yield return new WaitForSeconds(2.5f);

        StartCoroutine(PassengerIsEffectedByEnvironment());
    }

    [PunRPC]
    public void setHungryDisease(int _hungry, int _disease)
    {
        pass.Hungry = _hungry;
        pass.Disease = _disease;
    }

    public void ExitButtonCanvas()
    {
        PassengerCareButtons.gameObject.SetActive(false);
        PassengerStateRender.gameObject.SetActive(false);
    }

    public int GetHungry()
    {
        return pass.Hungry;
    }
    public int GetDisease()
    {
        return pass.Disease;
    }
}
