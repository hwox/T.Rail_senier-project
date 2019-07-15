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
    public bool Live; // 태우고 소파에 앉으면 호출

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
        if(Live)
        {

            // 여기서 이제 상태 계속 체크
            //   highlighter.Hover(hoverColor);
            if (pass.HP < 1 || pass.Disease > 99)
            {
                // HP 가 1보다 작거나 질병수치가 99보다 클 때 -> 사망

                Passenger_Die();
            }

        }
    }

    public void Passenger_LiveOn()
    {

        pass.HP = 100; // HP55
        pass.Disease = 0; // 질병수치

        
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
        highlighter.Hover(hoverColor);

        // 여기가 hover라서 이거 띄우면서 현재 state 같이 띄울건데
        // 이거 듸우면서 만야겡 지금 click 한 상태라서 
        // 약줄건지 어쩔건지 선택하는 선택창 뜬상태면 얘네 꺼놓는거 추가하기
    }
}
