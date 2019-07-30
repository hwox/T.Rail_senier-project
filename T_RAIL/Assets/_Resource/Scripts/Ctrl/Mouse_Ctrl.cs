using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;

public class Mouse_Ctrl : MonoBehaviourPunCallbacks
{

    // 마우스 컨트롤.
    // 말그대로 마우스로 클릭해서 하는 것들 관리
    // UI 제외

    //  float ScreenWidth;
    //  float ScreenHeight;

    public GameObject Inventory;
    public GameObject ChoiceButton;

    public GameObject StateControllerCam;


    public bool ThisCamOn;

    int IgnoreRay;

    //   layerMask = (1 << LayerMask.NameToLayer("Furniture")); 

    private void Start()
    {
        ThisCamOn = true;
        // ScreenWidth = Screen.width;
        // ScreenHeight = Screen.height;
        IgnoreRay = 1 << 31;  // ray가 wall(31번 Layer)을 무시하도록
        IgnoreRay = ~IgnoreRay;
    }

    // Update is called once per frame
    void Update()
    {
        if (ThisCamOn)
        {
            // UI이 위가 아니면

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, IgnoreRay))
            {
                if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
                {
                    // 만약에 마우스 클릭이 안된다?
                    // max distance 200.0f을 mathf.infinity로 바꿔볼것
                    //if (Physics.Raycast(ray, out hit))
                    //{
                    //   // Debug.Log(hit.collider.gameObject.layer);
                    //}
                    Debug.Log(hit.collider.gameObject.name);
                    if (hit.collider.gameObject.layer.Equals(GameValue.itembox_layer))
                    {
                        // 상자일 경우!
                        //Inventory.SetActive(true);
                        // Vector3 m_Position = Input.mousePosition;
                        //Inventory.transform.position = Input.mousePosition;
                        //new Vector3(m_Position.x, m_Position.y, m_Position.z);
                        if (TrainGameManager.instance.NowItemUIUsable)
                        {
                            hit.collider.GetComponent<InBoxItem>().OpenBoxInven();
                        }
                    }

                    else if (hit.collider.gameObject.layer.Equals(GameValue.passenger_layer))
                    {
                        // 승객일 경우 
                        hit.collider.GetComponent<Passenger_Ctrl>().ClickForPassengerCare();
                    }
                    else if (hit.collider.gameObject.layer.Equals(GameValue.choice_layer))
                    {
                        if (TrainGameManager.instance.NowItemUIUsable)
                        {
                            hit.collider.GetComponent<InTrainObjectMake>().ChoiceSetOn();
                        }
          
                        // 이거 getHitObjectRPC 이 함수안에 내용 주석처리했음
                        photonView.RPC("getHitObjectRPC", RpcTarget.AllBuffered, hit.collider.gameObject.GetPhotonView().ViewID);
                    }

                    else if (hit.collider.gameObject.layer.Equals(GameValue.sofa_layer))
                    {
                        // 소파레이어
                        //ChoiceButton.SetActive(true);
                        // ChoiceButton.transform.position = Input.mousePosition;
                        // ChoiceButton.GetComponent<UI_ChoiceButton>().GetHitObject(hit.collider.gameObject);

                    }

                    else if (hit.collider.CompareTag("state"))
                    {
                        // headtrain의 state판을 클릭하면
                        StateControllerCam.GetComponent<ControllerCamera_Ctrl>().On_StateController();
                    }

                    else if (hit.collider.gameObject.layer.Equals(GameValue.trainrepair_layer))
                    {
                        hit.collider.GetComponentInParent<Train_Object>().ClickFracturedTrain(hit.collider.name);
                    }

                }

                else if (hit.collider.gameObject.layer.Equals(GameValue.passenger_layer))
                {
                    hit.collider.GetComponent<Passenger_Ctrl>().PointerEnter();
                }
            }
        }
    }

    [PunRPC]
    public void getHitObjectRPC(int hit_object_viewID)
    {
        // ChoiceButton.transform.parent.GetComponent<UI_ChoiceButton>().GetHitObject(PhotonView.Find(hit_object_viewID).gameObject);
    }

    public void ThisCamSetOnOff(bool _onoff)
    {
        if (_onoff)
        {
            ThisCamOn = true;
        }
        else
        {
            ThisCamOn = false;
        }
    }


}
