using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class InBoxItem : MonoBehaviourPunCallbacks
{


    // 이거 누르면 얘가 가지고 있는거 inventory로 켜기 
    public enum itemCategory
    {
        nail = 1, // 못
        ironpan = 2, // 판(철판)
        food_tomato = 3, // 음식
        food_bean = 4,
        food_chicken = 5,
        hammer = 6, // 도끼
                    //  spanner = 7, // 스패너
    }

    public Canvas MyInvenCanvas;

    [SerializeField]
    AllItem_Ctrl allitem;

    public int[] HaveItemInfo;
    int thisBoxIndex;

    bool BoxFull; // 박스가 꽉찼는지 아닌지 

    int clickUI;
    int clickUI_image;
    public Image[] ItemImages; // 아이템창의 아이템 슬롯이미지 창

    bool DragEnable = false;

    void Start()
    {

        allitem = TrainGameManager.instance.allitemCtrl;
        ItemImages = new Image[6];
        HaveItemInfo = new int[6];
        ActiveThisBox();
        GetImageComponent();

        // 이 밑 for문은 테스트 하려고 랜덤으로 아이템 집어넣은 거임 
        // additem 수정되면 지워질 부분
        //   for (int i = 0; i < HaveItemInfo.Length; i++)
        //  {
        //    int temp = Random.Range(1, 7);

        // -> temp 다음에 들어가는 i는 초기화를 위한 i임 수정 끝나면
        // 받는 파라미터도 같이 지워야 됨
        //     AddItem(temp, i);
        // }

        ShowInInventory();

    }


    void GetImageComponent()
    {
        for (int i = 0; i < HaveItemInfo.Length; i++)
        {
            ItemImages[i] = MyInvenCanvas.transform.GetChild(0).GetChild(i).GetComponent<Image>();
        }
    }
    public void ActiveThisBox()
    {
        // active라는 말이 이 상자가 사용될 때
        // 아직 이걸 오브젝트 풀링으로 할지 어떻게 할지 정하질 않아서
        // 함수 이름 그냥 activethisbox라고 해놨음 (active될 때 쓸거니까.)
        allitem.AddedItemBox(this);
        thisBoxIndex = allitem.boxItem.Count - 1;
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

        // 
        //// ShowInInventory();
    }
    public void OffBoxInven()
    {
        //  allitem.ItemInhand.SetActive(false);위에 동일한 거 true는 되는데 false는 안됨. 왜?

        MyInvenCanvas.gameObject.SetActive(false);
        allitem.ItemInhand.SetActive(false);
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
            int BoxCount = 0;

            for (int j = 0; j < HaveItemInfo.Length; j++)
            {
                if (HaveItemInfo[j] != 0)
                {
                    BoxCount += 1;

                    if (BoxCount == 6)
                    {
                        BoxFull = true;
                    }
                }
            }
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

            BoxFull = false;
        }
    }
    void ShowInInventory()
    {
        // 인벤토리에 보이는거 이미지 등록
        for (int i = 0; i < HaveItemInfo.Length; i++)
        {
            switch (HaveItemInfo[i])
            {
                case (int)itemCategory.nail:
                    ItemImages[i].sprite = allitem.ItemImage[0];
                    break;
                case (int)itemCategory.ironpan:
                    ItemImages[i].sprite = allitem.ItemImage[1];
                    break;
                case (int)itemCategory.food_tomato:
                    ItemImages[i].sprite = allitem.ItemImage[2];
                    break;
                case (int)itemCategory.food_bean:
                    ItemImages[i].sprite = allitem.ItemImage[3];
                    break;
                case (int)itemCategory.food_chicken:
                    ItemImages[i].sprite = allitem.ItemImage[4];
                    break;
                case (int)itemCategory.hammer:
                    ItemImages[i].sprite = allitem.ItemImage[5];
                    break;
                    //  case (int)itemCategory.spanner:
                    //    MyInvenCanvas.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite = allitem.ItemImage[7];
                    //     break;
            }
        }
    }
    public void DragMouse_Up()
    {
        // 마우스 버튼을 뗐음
        // 현재 슬롯의 정보 업데이트
        // 빈 이미지 객체를 비활성화
      //  ItemImages[clickUI].sprite = allitem.ItemImage[clickUI_image];

        // 이게 끝인거같음
        // 얘가 마지막 그러면 여기서 
        if (!allitem.ItemCrack)
        {
            ItemImages[clickUI].sprite = allitem.ItemImage[clickUI_image];

            allitem.ItemCrack = false;
        } 
        else if(allitem.ItemCrack)
        {
            ItemImages[clickUI].sprite = allitem.NullImage;

            if(allitem.LeftFlag == 1)
            {
                allitem.SetLeftHandItem();
            }
            else if(allitem.RightFlag == 1)
            {
                allitem.SetRightHandItem();
            }

            allitem.ItemCrack = false;
        }
    }
    public void DragMouse_Down(int _number)
    {
        // 만약 해당 슬롯에 아이템이 없다면 아무일도 안하고      
        // 아이템이 있으면 드래그.
        // 그리고 여기서 애초에 처음 위치값도 지정해줘야할듯
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
        // 슬롯에 아이템이 존재하지 않으면 함수 종료
        //  빈 이미지의 객체를 마우스의 위치로 가져온다.
        // 슬롯의 이미지를 없애준다
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
            clickUI_image = 0; 
        }
    }

    public bool IsBoxFull()
    {
        return BoxFull;
    }
}
