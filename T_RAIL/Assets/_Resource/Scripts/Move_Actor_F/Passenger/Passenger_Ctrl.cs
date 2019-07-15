using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;
using UnityEngine.EventSystems;

public class Passenger_Ctrl : MonoBehaviour
{


    Passenger_Actor pass;
    public Highlighter highlighter;
    Color myColor;
    public Color hoverColor;
    public bool Live; // 태우고 소파에 앉으면 Live

    public Canvas PassengerStateRender;
    public Canvas PassengerCareButtons;


    bool Clicking; // 클릭해서 관리창 띄워놓은 상태

    Animator anim;


    private void Awake()
    {
        pass = new Passenger_Actor();
        anim = gameObject.GetComponent<Animator>();
        // highlighter = gameObject.GetComponent<Highlighter>();
        myColor = new Color(1.0f, 1.0f, 0.57f);
        hoverColor = myColor;
    }


    private void Update()
    {
        if (Live)
        {

            // 여기서 이제 상태 계속 체크
            //   highlighter.Hover(hoverColor);
            if (pass.Disease > 99 || pass.Hungry > 99)
            {
                // 질병수치랑 배고픔 수치가 99보다 크면 사망

                Passenger_Die();
            }

     

        }
    }

    public void Passenger_LiveOn()
    {

        pass.Disease = 0; // 질병
        pass.Hungry = 0; // 배고픔


        Live = true;
        this.transform.localPosition = Vector3.zero;
        anim.SetBool("IsSit", true);

    }


    public void Passenger_Die()
    {
        // 죽었을 때 호출할 함수
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
        // 음식먹는 버튼
        // 조건으로 아이템 체크해서 하기 추가

        pass.Hungry -= GameValue.HungryDecrease;

        // 이걸 false를 시킬까? 아니면 x를 추가할까
        PassengerCareButtons.gameObject.SetActive(false);
        PassengerStateRender.gameObject.SetActive(false);

        Clicking = false;
    }
    public void EatHeal()
    {
        // 구급상자 먹는 버튼
        pass.Disease -= GameValue.DiseaseDncrease;

        PassengerCareButtons.gameObject.SetActive(false);
        PassengerStateRender.gameObject.SetActive(false);

        Clicking = false;
    }
}
