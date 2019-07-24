﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialForCreate : MonoBehaviour
{


    // 소파나 박스 눌렀을 때 뜨는 재료창
    List<int> ForMakeItem = new List<int>();  // 옮겨놓은 아이템

    // 나중에 손으로 가져갈 거 대비한 crack
    public bool ItemCrack;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void DragMouse_Up()
    {

    }

    public void DragMouse_Down(int _number)
    {

    }

    public void DragMouse()
    {

    }

    public void DragMouse_End()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DragItem"))
        {

            other.GetComponent<PlayerHand_Item>().hand_ItemCrack = true;

            ForMakeItem.Add(other.GetComponent<PlayerHand_Item>().NowHave);
            other.GetComponent<PlayerHand_Item>().ItemUse();

            Debug.Log("아이템" + ForMakeItem[1]);
            // 이미지 한번 리셋. 다시 이미지 띄우기 (재료창)
        }
    }
}
