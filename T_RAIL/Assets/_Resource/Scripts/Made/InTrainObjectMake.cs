using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InTrainObjectMake : MonoBehaviourPunCallbacks {

    bool ChoiceOn;
    
    public int WhereTrain_Object { get; set; } // 이 오브젝트가 기차의 몇번째 칸에 존재하는지
    public int WhatNumber_Object { get; set; } // 이 오브젝트가 기차의 행동영역 중 몇번째에 위치하는지

    // 왼쪽부터 1,2,3,4

    bool Sofa;
    bool Box;

    public GameObject SetButtons;
    public GameObject BoxButton;
    public GameObject SofaButton;

    public void ChoiceSetOn()
    {
        if (!Box && !Sofa)
        {
            SetButtons.SetActive(true);
            ChoiceOn = true;
        }
    }

    public void InitSetting(int _index, int _whatnumber)
    {

        WhereTrain_Object = _index;
        WhatNumber_Object = _whatnumber;

        if (!PhotonNetwork.IsMasterClient) return;

        if(WhereTrain_Object == 1)
        {
            // 첫번째 칸이면
            int rand = Random.Range(0, 2);

            if (rand == 0)
            {
                // 소파만들기
                //photonView.RPC("ChoiceSofa", RpcTarget.All, 1, WhatNumber_Object);
            }
            else
            {
                //박스만들기
                //photonView.RPC("ChoiceBox", RpcTarget.All, 1, WhatNumber_Object);
            }
        }
    }

    public void ChoiceBox_Button()
    {
        photonView.RPC("ChoiceBox", RpcTarget.All, WhereTrain_Object, WhatNumber_Object);
    }

    public void ChoiceSofa_Button()
    {
        photonView.RPC("ChoiceSofa", RpcTarget.All, WhereTrain_Object, WhatNumber_Object);
    }

    [PunRPC]
    public void ChoiceBox(int WhereTrain_Object, int WhatNumber_Object)
    {
        Box = true;
        Sofa = false;
        ChoiceOn = false;


        // box on  box-> 5
        // 어떻게 된 길이냐!
        // 지금 이 inTrainObjectMake는 각 기차의 행동영역 각 한 칸을 결정해 주는 오브젝트임.
        // 그래서 얘를 누르면 sofa, box 둘중에 선택하라는 버튼이 뜨고 이 함수는 choicebox니까 박스를 선택한것
        // 그래서 보면? trainscript에 지금 어느칸에 있는지를 이용해서 InTrainSetting 함수에 접근해서 오브젝트풀링 된 오브젝트를 하나 넘겨줌.
        // 그걸 넘겨 받아서 저 함수로 가보면 
        Debug.Log("★ WhereTrain_Object : " + WhereTrain_Object);
        Debug.Log("★ WhatNumber_Object: " + WhatNumber_Object);

        TrainGameManager.instance.TrainCtrl.trainscript[WhereTrain_Object - 1].InTrainObject_Setting(TrainGameManager.instance.GetObject(5), WhatNumber_Object-1, 2);


        SetButtons.SetActive(false);
        this.gameObject.SetActive(false);

    }

    [PunRPC]
    public void ChoiceSofa(int WhereTrain_Object, int WhatNumber_Object)
    {

        Sofa = true;
        Box = false;
        ChoiceOn = false;

        // sofa on sofa-> 4
        Debug.Log("♥ WhereTrain_Object : " + WhereTrain_Object);
        Debug.Log("♥ WhatNumber_Object : " + WhatNumber_Object);

        TrainGameManager.instance.TrainCtrl.trainscript[WhereTrain_Object - 1].InTrainObject_Setting(TrainGameManager.instance.GetObject(4), WhatNumber_Object-1, 1);

        SetButtons.SetActive(false);
        this.gameObject.SetActive(false);

    }

    // 누르고 3초 후에 만들어주기 약간 뚝딱뚝딱 이런거 ㅎㅎ

    public void OnBoxButtonMouseOn()
    {
        BoxButton.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }
    public void OnBoxButtonMouseExit()
    {
        BoxButton.transform.localScale = new Vector3(1f, 1f, 1f);
    }
    public void OnSofaButtonMouseOn()
    {
        SofaButton.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }
    public void OnSofaButtonMouseExit()
    {
        SofaButton.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    // 근데 만약에 이거 없애고 다른거 만들고 싶으면 delete누르고
    // 여기서 다른 걸로 바꾸는 함수, 기능 호출



}
