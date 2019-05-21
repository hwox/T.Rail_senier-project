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

    int InPocket_Left; // 왼쪽 주머니 -> 왼쪽 손
    int InPocket_Right; // 오른쪽 주머니 -> 오른쪽 손

  
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

    void MyPocket_ImageSet()
    {

        for(int i=0;i<ItemImage.Length; i++)
        {
            if( i == InPocket_Left)
            {
                LeftUIImage.GetComponent<Image>().sprite = ItemImage[i];
            }
            if(i == InPocket_Right)
            {
                RightUIImage.GetComponent<Image>().sprite = ItemImage[i];
            }
        }

    }


    // 추가해야 될 것
    // 아이템 드래그
    // 드래그 1. 아이템 창에서 손으로. 손에 만약에 1~7 하여튼이 값이 아니고 다른 값이면 드래그해올 수 있고
    // 드래그 되면 그게 여기에 등록
    // 드래그 2. 아이템을 다른 공구같은거에 옮겨주기  

}
