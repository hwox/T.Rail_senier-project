using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllItem_Ctrl : MonoBehaviour
{

    public enum itemCategory
    {
        nail = 1, // 못
        ironpan = 2, // 판(철판)
        food_tomato = 3, // 음식
        food_bean = 4,
        food_chicken = 5,
        hammer = 6, // 도끼
        //spanner = 7, // 스패너
    }

    public Sprite[] ItemImage;
    public Sprite NullImage;
    public Sprite StandardImage; //기본 아이템창 백이미지

    int boxNumber; // 박스 총 몇개인지

    public List<InBoxItem> boxItem = new List<InBoxItem>();

    // #hand UI
    public GameObject ItemInhand; // 손 item 
    GameObject LeftUIImage;
    GameObject RightUIImage;

    public Image DragCursorSprite; // 마우스 드래그할 때 이미지
    public int NowDragItemInfo; // 현재 드래그중인 아이템 정보

    public GameObject LeftHand_PocketObject; // 왼쪽 손 오브젝트
    public GameObject RightHand_PocketObject; // 오른쪽 손 오브젝트

    public int LeftHand_Pocket;
    public int RightHand_Pocket;

    public bool ItemCrack; // 아이템 옮겨졌는가?ㅜ
    public int LeftFlag = 0; // 왼손에 닿았는가ㅎ
    public int RightFlag = 0; // 오른손에 닿았는가 ㅎ


    void Start()
    {
        LeftUIImage = ItemInhand.transform.GetChild(0).GetChild(0).gameObject;
        RightUIImage = ItemInhand.transform.GetChild(1).GetChild(0).gameObject;
    }

    public void AddedItemBox(InBoxItem inbox)
    {
        boxItem.Add(inbox);
        boxNumber += 1;
    }
    public void DeletedItemBox(int index)
    {
        boxItem.RemoveAt(index);
        boxNumber -= 1;
    }

    public void HandItemUiOnOff(bool _onoff)
    {
        if (_onoff)
        {
            ItemInhand.SetActive(true);
        }
        else
        {
            ItemInhand.SetActive(false);
        }
    }
    public void Change_DragMouse(int _number)
    {
        //if (_number.Equals(0))
        //{
        //    Debug.Log("응?");
        //    DragCursorSprite.sprite = null;
        //}
        //else
        //{
        DragCursorSprite.sprite = ItemImage[_number-1];
        NowDragItemInfo = _number;
        // }
    }
    public void OnOff_DragMouse(bool _onoff)
    {
        if (_onoff)
        {
            DragCursorSprite.gameObject.SetActive(true);
        }
        else if (!_onoff)
        {
            DragCursorSprite.gameObject.SetActive(false);
            ItemCrack = false;
            NowDragItemInfo = 0;
        }
    }

    public void Position_DragMouse()
    {
        DragCursorSprite.transform.position = Input.mousePosition;
    }

    public void SetLeftHandItem()
    {
        TrainGameManager.instance.LeftHandItem = NowDragItemInfo;
        LeftHand_Pocket = NowDragItemInfo;
        LeftHand_PocketObject.GetComponent<Image>().sprite = ItemImage[LeftHand_Pocket-1];
    }

    public void SetRightHandItem()
    {
        TrainGameManager.instance.RightHandItem = NowDragItemInfo;
        RightHand_Pocket = NowDragItemInfo;
        RightHand_PocketObject.GetComponent<Image>().sprite = ItemImage[RightHand_Pocket-1];

    }
}
