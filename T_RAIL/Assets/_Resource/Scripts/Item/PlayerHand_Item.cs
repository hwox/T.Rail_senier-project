using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHand_Item : MonoBehaviour {

    AllItem_Ctrl allitem;

    int NowHave; // 현재 가지고 있는 아이템
    public Image NowHave_Image; // 현재 손ㄴ바닥에 있는 Image


    public int WhatHand; // 1-> 왼손 2-> 오른손 이거는 직접 인스펙터 창에서 설정

    private void Start()
    {
        allitem = TrainGameManager.instance.allitemCtrl;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DragItem"))
        {
            // 만약 아이템이랑 충돌하면!
            NowHave = allitem.NowDragItemInfo;

            NowHave_Image.sprite = allitem.ItemImage[NowHave];

            // 그리고 drag up 함수도 실행해줘야 드래그 다한거 끝나는거아냐?
            // 여기서 drag up 함수 호출해주기? 아니면 allitem에서 drag 끝났다고 알려주면서?

        // 일단 바뀌는것 까지 됨
        // 추가해야될 것
        // 1. 아이템을 가졌음. 그 값 저장
        // 2. End함수 호출

        }
    }

}
