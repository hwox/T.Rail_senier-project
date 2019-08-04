using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class InBoxItem : MonoBehaviourPunCallbacks
{


    // 이거 누르면 얘가 가지고 있는거 inventory로 켜기 
    //public enum itemCategory
    //{
    //    nail = 1, // 못
    //    ironpan = 2, // 판(철판)
    //    food_tomato = 3, // 음식
    //    food_bean = 4,
    //    food_chicken = 5,
    //    hammer = 6, // 도끼
    //    medipack = 7,
    //    woodboard = 8,
    //    //  spanner = 7, // 스패너
    //}

    public Canvas MyInvenCanvas;

    [SerializeField]
    AllItem_Ctrl allitem;

    public int[] HaveItemInfo;
    public int thisBoxIndex;

    bool BoxFull; // 박스가 꽉찼는지 아닌지 

    int clickUI;
    int clickUI_image;
    public Image[] ItemImages; // 아이템창의 아이템 슬롯이미지 창

    bool DragEnable = false;

    public GameObject particle;

    void Start()
    {
        ItemImages = new Image[8];
        HaveItemInfo = new int[GameValue.ITEMLIMIT];
        // ActiveThisBox();
        GetImageComponent();

        GameObject temp = Instantiate(particle);
        temp.transform.parent = this.transform;

        this.transform.localPosition = Vector3.zero;
        this.gameObject.SetActive(false);
        this.transform.parent = TrainGameManager.instance.gameObject.transform.GetChild(5);// (int)prefab_list.passenger);
        TrainGameManager.instance.BoxManager.Add(this.gameObject);
    }

    void GetImageComponent()
    {

        for (int i = 0; i < HaveItemInfo.Length; i++)
        {
            ItemImages[i] = MyInvenCanvas.transform.GetChild(0).GetChild(i).GetChild(0).GetComponent<Image>();
        }
    }
    public void ActiveThisBox()
    {
        allitem = TrainGameManager.instance.allitemCtrl;
        allitem.AddedItemBox(this);
        thisBoxIndex = allitem.boxItem.Count - 1;


        ShowInInventory();
    }
    public void InActiveThisBox()
    {
        allitem.DeletedItemBox(thisBoxIndex);
        thisBoxIndex = -99; // 일단 어떻게 사용할지 몰라서 쓰레기값 넣어주기 
    }


    public void OpenBoxInven()
    {

        // 이거 on되면 손도 같이  on되게
        MyInvenCanvas.gameObject.SetActive(true);
        //  allitem.HandItemUiOnOff(true);
        allitem.ItemInhand.SetActive(true);
        TrainGameManager.instance.NowItemUIUsable = false;
        // 
        //// ShowInInventory();
    }
    public void OffBoxInven()
    {
        //  allitem.ItemInhand.SetActive(false);위에 동일한 거 true는 되는데 false는 안됨. 왜?

        MyInvenCanvas.gameObject.SetActive(false);

        if (!TrainGameManager.instance.UISettingCtrl.HandButtonOn)
        {
            allitem.ItemInhand.SetActive(false);
        }

        TrainGameManager.instance.NowItemUIUsable = true;
    }


    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("DragItem"))
        {
            // 원래 손->박스를 위한 충돌내용이 여기 있어야 하지만
            // 여기는 충돌이 안되는문제로 ItemCanvas->Inven에 ForUICollision이라는 
            // 충돌용 함수를 만들어놨음
            //Debug.Log("부딪힘");
            //// 걍 아이템 넣고 훅
            //if (!BoxFullCheck())
            //{
            //    allitem.ItemHandToBox = true;
            //    allitem.ForItemToBoxIndex(thisBoxIndex);
            //}
            //else
            //{
            //    allitem.ItemHandToBox = false;
            //}
            // 이 상자에 자리 없으면 안들어가게 -> 다시 원상복귀
            // 상자에 자리 있으면 앞칸부터 검사해서 채워넣기

        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("DragItem"))
        {
            Debug.Log("나감");
            allitem.ItemHandToBox = false;
        }
    }

    public void DestroyTrain()
    {
        //allitem.HaveItemInfo.RemoveAt();
        allitem.DeletedItemBox(thisBoxIndex);
        thisBoxIndex = -99; // 일단 어떻게 사용할지 몰라서 쓰레기값 넣어주기 
    }

    public void AddItem(int _item)
    {

        if (!BoxFull)
        {
            for (int i = 0; i < HaveItemInfo.Length; i++)
            {
                if (HaveItemInfo[i].Equals(0))
                {
                    // 비교해봐서 0번이 아닌 슬롯(비어있지 않은 슬롯)에 앞에부터 채워나가기
                    HaveItemInfo[i] = _item;

                    break;
                }
            }

            BoxFullCheck();

            ShowInInventory();
        }
        else
        {
            Debug.Log("6초과 ");
        }
    }

    public void DeleteItem(int _item)
    {
        if (HaveItemInfo.Length > 0)
        {
            // 삭제
            HaveItemInfo[_item] = 0;
            ShowInInventory();
            BoxFull = false;
        }
    }

    public bool BoxFullCheck()
    {
        int BoxCount = 0;
        for (int j = 0; j < HaveItemInfo.Length; j++)
        {
            if (HaveItemInfo[j] != 0)
            {
                BoxCount += 1;

                if (BoxCount == GameValue.ITEMLIMIT)
                {
                    BoxFull = true;
                }
                else
                {
                    BoxFull = false;
                }
            }
        }
        return BoxFull;
    }
    void ShowInInventory()
    {
        // 인벤토리에 보이는거 이미지 등록
        for (int i = 0; i < HaveItemInfo.Length; i++)
        {
            switch (HaveItemInfo[i])
            {
                case (int)GameValue.itemCategory.food_tomato:
                    ItemImages[i].sprite = allitem.ItemImage[0];
                    break;
                case (int)GameValue.itemCategory.food_bean:
                    ItemImages[i].sprite = allitem.ItemImage[1];
                    break;
                case (int)GameValue.itemCategory.food_chicken:
                    ItemImages[i].sprite = allitem.ItemImage[2];
                    break;
                case (int)GameValue.itemCategory.hammer:
                    ItemImages[i].sprite = allitem.ItemImage[3];
                    break;
                case (int)GameValue.itemCategory.nail:
                    ItemImages[i].sprite = allitem.ItemImage[4];
                    break;
                case (int)GameValue.itemCategory.medipack:
                    ItemImages[i].sprite = allitem.ItemImage[5];
                    break;
                case (int)GameValue.itemCategory.woodboard:
                    ItemImages[i].sprite = allitem.ItemImage[6];
                    break;
                case (int)GameValue.itemCategory.ironpan:
                    ItemImages[i].sprite = allitem.ItemImage[7];
                    break;
                default:
                    ItemImages[i].sprite = allitem.NullImage;
                    break;

            }
        }
    }
    public void DragMouse_Up()
    {
        // 마우스 버튼을 뗐음
        // 현재 슬롯의 정보 업데이트
        // 빈 이미지 객체를 비활성화

        // 이게 끝인거같음
        // 얘가 마지막 그러면 여기서 
        if (!allitem.ItemHandToBox)
        {
            if (!allitem.ItemBoxToHand)
            {
                // 아이템 안갖다넣음 그래서 이미지 null 아니고 그대로 있는거
                ItemImages[clickUI].sprite = allitem.ItemImage[clickUI_image];

                allitem.ItemBoxToHand = false;
            }
            else if (allitem.ItemBoxToHand)
            {
                ItemImages[clickUI].sprite = allitem.NullImage;

                // 아이템 갖다넣기 그 후 아이템 지우기
                if (allitem.LeftFlag == 1)
                {
                    allitem.SetLeftHandItem();
                }
                else if (allitem.RightFlag == 1)
                {
                    allitem.SetRightHandItem();
                }
                photonView.RPC("deleteItem_RPC", RpcTarget.All, clickUI);
                allitem.ItemBoxToHand = false;
                allitem.LeftFlag = 0;
                allitem.RightFlag = 0;
            }
        }
    }

    [PunRPC]
    void deleteItem_RPC(int _clickUI)
    {
        DeleteItem(_clickUI);
    }

    public void DragMouse_Down(int _number)
    {
        // 만약 해당 슬롯에 아이템이 없다면 아무일도 안하고      
        // 아이템이 있으면 드래그.
        // 그리고 여기서 애초에 처음 위치값도 지정해줘야할듯
        if (!allitem.ItemHandToBox)
        {
            if (HaveItemInfo[_number] != 0)
            {
                allitem.DragCursorSprite.transform.position = Input.mousePosition;
                allitem.OnOff_DragMouse(true);
                clickUI = _number;
                clickUI_image = HaveItemInfo[_number];
                DragEnable = true;
                // 잠깐 이미지 없애기
                ItemImages[_number].sprite = allitem.StandardImage;
                // 그리고 드래그 이미지
                allitem.Change_DragMouse(clickUI_image);
            }
            else if (HaveItemInfo[_number].Equals(0))
            {
                // nothing
                clickUI = 0;
            }
        }
        // 슬롯에 아이템이 존재하지 않으면 함수 종료
        //  빈 이미지의 객체를 마우스의 위치로 가져온다.
        // 슬롯의 이미지를 없애준다
    }
    public void DragMouse()
    {
        // 이미지 이동
        // 빈 이미지의 위치를 마우스의 위치로 가져온다. 
        if (DragEnable && !allitem.ItemHandToBox)
        {
            allitem.Position_DragMouse();
        }
    }
    public void DragMouse_End()
    {
        if (DragEnable && !allitem.ItemHandToBox)
        {
            // 옮길 애들 위치 보내기 
            // 이건 테스트할거라서 원래 위치로 돌아온걸로
            allitem.OnOff_DragMouse(false);
            clickUI = 0;
            clickUI_image = 0;
        }
    }

    public bool IsBoxFull()
    {
        return BoxFull;
    }
}
