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

    int boxNumber; // 박스 총 몇개인지

    public List<InBoxItem> boxItem = new List<InBoxItem>();

    // #hand UI
    public GameObject ItemInhand; // 손 item 
    GameObject LeftUIImage;
    GameObject RightUIImage;

    public Image DragCursorSprite; // 마우스 드래그할 때 이미지
    public int NowDragItemInfo; // 현재 드래그중인 아이템 정보

    //public GameObject LeftHand_ItemPocket; // 왼쪽 손 오브젝트
    //public GameObject RightHand_ItemPocket; // 오른쪽 손 오브젝트

    public int LeftHand_Pocket;
    public int RightHand_Pocket;

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
        if (_number.Equals(0))
        {
            DragCursorSprite.sprite = null;
        }
        else
        {
            DragCursorSprite.sprite = ItemImage[_number];
            NowDragItemInfo = _number;
        }
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
            NowDragItemInfo = 0;
        }
    }

    public void Position_DragMouse()
    {
        DragCursorSprite.transform.position = Input.mousePosition;
    }
   
}
