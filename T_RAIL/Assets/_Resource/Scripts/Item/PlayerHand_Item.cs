﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerHand_Item : MonoBehaviourPunCallbacks
{

    AllItem_Ctrl allitem;

    public Image NowHave_Image; // 현재 손ㄴ바닥에 있는 Image


    public int WhatHand; // 1-> 왼손 2-> 오른손 이거는 직접 인스펙터 창에서 설정

    int clickUI;

    bool DragEnable = true;

    // public bool hand_ItemCrack; // 손아이템용 크랙

    private void Start()
    {
        allitem = TrainGameManager.instance.allitemCtrl;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DragItem"))
        {
            // 만약 아이템이랑 충돌하면!
            // NowHave = allitem.NowDragItemInfo;


            if (WhatHand == 1)
            {
                allitem.LeftFlag = 1;
                allitem.RightFlag = 0;
            }
            else if (WhatHand == 2)
            {
                allitem.LeftFlag = 0;
                allitem.RightFlag = 1;
            }

            allitem.ItemBoxToHand = true;
        }

        //if (other.CompareTag("itemboxinven"))
        //{
        //    // 부모 세번찾아서 박스에 invoxitem 스크립트 찾고
        //    // 얘 other collider 이름으로 haveitem 배열에 넣기
        //    // other.collider.
        //    int number = int.Parse(other.name);

        //    switch (WhatHand)
        //    {
        //        case 1:
        //            // other.transform.parent.parent.parent.GetComponent<InBoxItem>().HaveItemInfo[number - 1] = TrainGameManager.instance.LeftHandItem;
        //            break;
        //        case 2:
        //            // other.transform.parent.parent.parent.GetComponent<InBoxItem>().HaveItemInfo[number - 1] = TrainGameManager.instance.RightHandItem;

        //            break;
        //    }

        //}

    }
    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("DragItem"))
        {
            allitem.ItemBoxToHand = false;
        }
    }

    public void ItemUse()
    {
        switch (WhatHand)
        {
            case 1:
                allitem.UseLeftHandItem();
                break;
            case 2:
                allitem.UseRightHandItem();
                break;
        }
    }
    public void DragMouse_Down()
    {
        // 만약 해당 슬롯에 아이템이 없다면 아무일도 안하고      
        // 아이템이 있으면 드래그.
        // 그리고 여기서 애초에 처음 위치값도 지정해줘야할듯
        allitem.ItemHTBEnable = true;
        switch (WhatHand)
        {

            case 1:
                // 왼손
                if (allitem.LeftHand_Pocket != 0)
                {
                    allitem.DragCursorSprite.transform.position = Input.mousePosition;
                    allitem.OnOff_DragMouse(true);
                    clickUI = allitem.LeftHand_Pocket;

                    DragEnable = true;
                    // 잠깐 이미지 없애기
                    NowHave_Image.sprite = allitem.NullImage;
                    // 그리고 드래그 이미지
                    allitem.Change_DragMouse(clickUI);

                    allitem.UseInLeftHand = 1;
                    allitem.UseInRightHand = 0;
                }
                else if (allitem.LeftHand_Pocket == 0)
                {

                    clickUI = 0;
                    DragEnable = false;
                }
                // 슬롯에 아이템이 존재하지 않으면 함수 종료
                //  빈 이미지의 객체를 마우스의 위치로 가져온다.
                // 슬롯의 이미지를 없애준다


                break;
            case 2:
                //오른손
                if (allitem.RightHand_Pocket != 0)
                {
                    allitem.DragCursorSprite.transform.position = Input.mousePosition;
                    allitem.OnOff_DragMouse(true);
                    clickUI = allitem.RightHand_Pocket;
                    DragEnable = true;
                    // 잠깐 이미지 없애기
                    NowHave_Image.sprite = allitem.NullImage;
                    // 그리고 드래그 이미지
                    allitem.Change_DragMouse(clickUI);

                    allitem.UseInLeftHand = 0;
                    allitem.UseInRightHand = 1;
                }
                else if (allitem.RightHand_Pocket == 0)
                {
                    // nothing
                    clickUI = 0;
                    DragEnable = false;
                }


                break;
        }

    }
    public void DragMouse()
    {
        // 이미지 이동
        // 빈 이미지의 위치를 마우스의 위치로 가져온다. 
        if (DragEnable)
        {
            allitem.Position_DragMouse();
        }
    }

    public void DragMouse_End()
    {
        if (DragEnable)
        {
            // 옮길 애들 위치 보내기 
            // 이건 테스트할거라서 원래 위치로 돌아온걸로
            allitem.OnOff_DragMouse(false);
            clickUI = 0;
        }
    }
    public void DragMouse_Up()
    {
        allitem.ItemHTBEnable = false;
        // 만약에 
        // 아이템창에 닿으면 그 아이템창에 이거 현재 아이템 정보 저장하고
        // 지금 내 아이템들은 다 초기화 시켜야 함
        // 얘가 마지막 그러면 여기서 
        switch (WhatHand)
        {
            case 1:

                if (allitem.ItemHandToBox)
                {
                    int BoxIndex = allitem.NowChoiceBox;
                    // allitem.boxItem.H
                    for (int i = 0; i < allitem.boxItem[BoxIndex].HaveItemInfo.Length; i++)
                    {
                        if (allitem.boxItem[BoxIndex].HaveItemInfo[i].Equals(0))
                        {
                            // 비교해봐서 0번이 아닌 슬롯(비어있지 않은 슬롯)에 앞에부터 채워나가기
                            //allitem.boxItem[BoxIndex].AddItem(allitem.LeftHand_Pocket);
                            allitem.boxItem[BoxIndex].photonView.RPC("AddItem", RpcTarget.All, allitem.LeftHand_Pocket);

                            break;
                        }
                    }
                    allitem.UseLeftHandItem();
                    NowHave_Image.sprite = allitem.NullImage;
                    allitem.ItemHandToBox = false;
                }
                else if (!allitem.ItemHandToBox)
                {
                    if (allitem.LeftHand_Pocket == 0)
                    {
                        NowHave_Image.sprite = allitem.NullImage;
                    }
                    else
                    {
                        NowHave_Image.sprite = allitem.ItemImage[clickUI-1];
                        allitem.ItemHandToBox = false;
                    }
                }

                break;
            case 2:
                //오른손

                if (allitem.ItemHandToBox)
                {
                    int BoxIndex = allitem.NowChoiceBox;
                    // allitem.boxItem.H
                    for (int i = 0; i < allitem.boxItem[BoxIndex].HaveItemInfo.Length; i++)
                    {
                        if (allitem.boxItem[BoxIndex].HaveItemInfo[i].Equals(0))
                        {
                            // 비교해봐서 0번이 아닌 슬롯(비어있지 않은 슬롯)에 앞에부터 채워나가기
                            //allitem.boxItem[BoxIndex].AddItem(allitem.RightHand_Pocket);
                            allitem.boxItem[BoxIndex].photonView.RPC("AddItem", RpcTarget.All, allitem.RightHand_Pocket);
                            break;
                        }
                    }
                    allitem.UseRightHandItem();
                    NowHave_Image.sprite = allitem.NullImage;
                    allitem.ItemHandToBox = false;

                }
                else if (!allitem.ItemHandToBox )
                {
                    if (allitem.RightHand_Pocket == 0)
                    {

                        NowHave_Image.sprite = allitem.NullImage;
                        Debug.Log("?");
                    }
                    else
                    {
                        NowHave_Image.sprite = allitem.ItemImage[clickUI-1];
                        allitem.ItemHandToBox = false;
                        Debug.Log("?2");
                    }
                }


                break;
        }
        allitem.ItemHandToBox = false;
        allitem.UseInLeftHand = 0;
        allitem.UseInRightHand = 0;
    }
}


