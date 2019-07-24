using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;


public class InTrainObjectMake : MonoBehaviourPunCallbacks
{



    public int WhereTrain_Object { get; set; } // 이 오브젝트가 기차의 몇번째 칸에 존재하는지
    public int WhatNumber_Object { get; set; } // 이 오브젝트가 기차의 행동영역 중 몇번째에 위치하는지

    // 왼쪽부터 1,2,3,4

    bool SofaMakeMode;
    bool BoxMakeMode;

    bool MakeEnable; // 조건 다 달성했으면 만들 수 있게

    public GameObject ChoiceCanvas;
    public GameObject SetButtons;
    public GameObject BoxButton;
    public GameObject SofaButton;
    public Button MakeButton;

    public GameObject BoxNeedItemExplain;
    public GameObject SofaNeedItemExplain;

    public GameObject MaterialStorage;
    //public Image DragCursorSprite; // 마우스 드래그할 때 이미지
    //public int NowDragItemInfo; // 현재 드래그중인 아이템 정보

    public GameObject ItemInhand; // 손 item 


    public void ChoiceSetOn()
    {
        if (!BoxMakeMode && !SofaMakeMode)
        {
            ChoiceCanvas.SetActive(true);
        }
    }

    public void InitSetting(int _index, int _whatnumber)
    {

        WhereTrain_Object = _index;
        WhatNumber_Object = _whatnumber;

        if (!PhotonNetwork.IsMasterClient) return;

        if (WhereTrain_Object == 1)
        {
            // 첫번째 칸이면
            int rand = Random.Range(0, 2);

            if (rand == 0)
            {
                // 소파만들기
                // photonView.RPC("ChoiceSofa", RpcTarget.All, 1, WhatNumber_Object);
            }
            else
            {
                //박스만들기
                // photonView.RPC("ChoiceBox", RpcTarget.All, 1, WhatNumber_Object);
            }
        }
    }

    public void MakeBox_Button()
    {
        photonView.RPC("MakeBox", RpcTarget.All, WhereTrain_Object, WhatNumber_Object);
    }

    public void MakeSofa_Button()
    {
        photonView.RPC("MakeSofa", RpcTarget.All, WhereTrain_Object, WhatNumber_Object);
    }

    [PunRPC]
    public void MakeBox(int WhereTrain_Object, int WhatNumber_Object)
    {
        // box on  box-> 5
        // 어떻게 된 길이냐!
        // 지금 이 inTrainObjectMake는 각 기차의 행동영역 각 한 칸을 결정해 주는 오브젝트임.
        // 그래서 얘를 누르면 sofa, box 둘중에 선택하라는 버튼이 뜨고 이 함수는 choicebox니까 박스를 선택한것
        // 그래서 보면? trainscript에 지금 어느칸에 있는지를 이용해서 InTrainSetting 함수에 접근해서 오브젝트풀링 된 오브젝트를 하나 넘겨줌.
        // 그걸 넘겨 받아서 저 함수로 가보면 
        TrainGameManager.instance.TrainCtrl.trainscript[WhereTrain_Object - 1].InTrainObject_Setting(TrainGameManager.instance.GetObject(5), WhatNumber_Object - 1, 2);
        this.gameObject.SetActive(false);
    }

    [PunRPC]
    public void MakeSofa(int WhereTrain_Object, int WhatNumber_Object)
    {
        TrainGameManager.instance.TrainCtrl.trainscript[WhereTrain_Object - 1].InTrainObject_Setting(TrainGameManager.instance.GetObject(4), WhatNumber_Object - 1, 1);
        this.gameObject.SetActive(false);
    }

    public void ExitSettings()
    {
        SofaMakeMode = false;
        BoxMakeMode = false;

        SofaNeedItemExplain.SetActive(false);
        BoxNeedItemExplain.SetActive(false);
        MaterialStorage.SetActive(false);
        ChoiceCanvas.SetActive(false);
        SetButtons.SetActive(true);
        MakeButton.interactable = false;
        ItemInhand.SetActive(false);
    }

    public void ChoiceBoxButton()
    {
        BoxMakeMode = true;
        SofaMakeMode = false;
        SetButtons.SetActive(false);
        MaterialStorage.SetActive(true);
        ItemInhand.SetActive(true);
        BoxNeedItemExplain.SetActive(false);
        SofaNeedItemExplain.SetActive(false);
    }

    public void ChoiceSofaButton()
    {
        SofaMakeMode = true;
        BoxMakeMode = false;
        SetButtons.SetActive(false);
        MaterialStorage.SetActive(true);
        ItemInhand.SetActive(true);
        BoxNeedItemExplain.SetActive(false);
        SofaNeedItemExplain.SetActive(false);
    }

    public void MakeObject()
    {
        if (MakeEnable)
        {
            if (SofaMakeMode)
            {
                MakeSofa_Button();
                ExitSettings();
            }

            else if (BoxMakeMode)
            {
                MakeBox_Button();
                ExitSettings();
            }
        }
    }

    ////////////////////////////////////   UI Hover 함수들   ////////////////////////////////////


    public void OnBoxButtonMouseOn()
    {
        BoxButton.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        BoxNeedItemExplain.SetActive(true);
    }
    public void OnBoxButtonMouseExit()
    {
        BoxButton.transform.localScale = new Vector3(1f, 1f, 1f);
        BoxNeedItemExplain.SetActive(false);
    }
    public void OnSofaButtonMouseOn()
    {
        SofaNeedItemExplain.SetActive(true);
        SofaButton.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }
    public void OnSofaButtonMouseExit()
    {
        SofaNeedItemExplain.SetActive(false);
        SofaButton.transform.localScale = new Vector3(1f, 1f, 1f);
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////

    // Drag 해서 아이템 옮겨놓는 부분 


    public void DragMouse_Up()
    {

    }

    public void DragMouse_Down()
    {
        //allitem.DragCursorSprite.transform.position = Input.mousePosition;
        // allitem.OnOff_DragMouse(true);
    }

    public void DragMouse()
    {

    }

    public void DragMouse_End()
    {

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("DragItem"))
    //    {

    //        other.GetComponent<PlayerHand_Item>().hand_ItemCrack = true;

    //        ForMakeItem.Add(other.GetComponent<PlayerHand_Item>().NowHave);
    //        other.GetComponent<PlayerHand_Item>().ItemUse();

    //        Debug.Log("아이템" +ForMakeItem[1]);
    //        // 이미지 한번 리셋. 다시 이미지 띄우기 (재료창)
    //    }
    //}

    //////////////////////////////////    UPDate    /////////////////////////////////////////

    private void Update()
    {
        // 여기서 제작버튼 활성화 해도 되는지 안되는지 검사

        if (SofaMakeMode)
        {
            // 소파 만들 수 있는 조건 되면 makeenable = true; & 제작 버튼 활성화 \
            MakeEnable = true;
            MakeButton.interactable = true;  // 테스트용
        }

        else if (BoxMakeMode)
        {
            // 박스 만들 수 있는 조건 되면 makeenable = true;  & 제작 버튼 활성화 
            MakeEnable = true;
            MakeButton.interactable = true;  // 테스트용
        }
        else
        {
            MakeEnable = false;
            MakeButton.interactable = false;
        }
    }
}

