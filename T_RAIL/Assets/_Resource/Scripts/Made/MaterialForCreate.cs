using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MaterialForCreate : MonoBehaviour
{


    // 소파나 박스 눌렀을 때 뜨는 재료창
    List<int> ForMakeItem = new List<int>();  // 옮겨놓은 아이템
    int[] ItemCount; // 똑같은 아이템 몇개인지
    // 나중에 손으로 가져갈 거 대비한 crack
    public bool ItemCrack;
    AllItem_Ctrl allitem;

    GameObject[] Storage;
    int[] WhatInStorage; // 뭐가 자꾸 늘어나네 얘는 그 인덱스에 있는게 몇번째 아이템인ㄴ지
    Text[] StorageCount;

    // Dictionary<int, int> Storage_test = new Dictionary<int, int>();
    // 앞에는 index 뒤에는 count

    int StorageIndex = 0;

    // Use this for initialization
    void Start()
    {
        allitem = TrainGameManager.instance.allitemCtrl;
        Storage = new GameObject[6];
        StorageCount = new Text[6];
        WhatInStorage = new int[6];
        ItemCount = new int[(int)GameValue.itemCategory.woodboard];

        for (int i = 0; i < 6; i++)
        {
            Storage[i] = gameObject.transform.GetChild(i).GetChild(0).gameObject;
            StorageCount[i] = gameObject.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>();
        }
    }



    ////////////////////////////////////////////////////////////////////

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

    ////////////////////////////////////////////////////////////////////

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DragItem"))
        {

            if (StorageIndex <= 6)
            {
                //ForMakeItem.Add(other.GetComponent<PlayerHand_Item>().NowHave);
                if (allitem.UseInLeftHand == 1)
                {
                    ForMakeItem.Add(allitem.LeftHand_Pocket);
                    allitem.UseLeftHandItem();
                }
                if (allitem.UseInRightHand == 1)
                {
                    ForMakeItem.Add(allitem.RightHand_Pocket);
                    allitem.UseRightHandItem();
                }

                DrawMaterialStorageImage();
            }

            else
            {
                Debug.Log("보관함 6개 넘어서 못넣어 이제");
            }
        }
    }

    ////////////////////////////////////////////////////////////////////

    void DrawMaterialStorageImage()
    {
        //int StorageIndex = 0;
        bool Enable = false;
        int index = 0;

        // 
        // ItemCount에는 몇개 있는지.


        for (int i = 0; i < 6; i++)
        {

            if (WhatInStorage[i].Equals(ForMakeItem[ForMakeItem.Count - 1]))
            {

                // count-1값이 제일 마지막에 들어온 값일거니끼ㅏ
                Enable = true;
                index = i;
                break;
            }

        }
        ItemCount[ForMakeItem[ForMakeItem.Count - 1] - 1] += 1;

        if (Enable)
        {
            // 이거는 겹치는게 있다는거 
            // 그러니까 count 값 하나 증가시키고 
            // text
            StorageCount[index].text = ItemCount[ForMakeItem[ForMakeItem.Count - 1] - 1].ToString();
        }

        else if (!Enable)
        {
            // 겹치는거없어
            // 이미지 띄우기
            WhatInStorage[StorageIndex] = ForMakeItem[ForMakeItem.Count - 1];
            Storage[StorageIndex].GetComponent<Image>().sprite = allitem.ItemImage[ForMakeItem[ForMakeItem.Count - 1] - 1];
            StorageCount[index].text = ItemCount[ForMakeItem[ForMakeItem.Count - 1] - 1].ToString();
            StorageIndex += 1;
        }


        for (int j = StorageIndex; j < 6; j++)
        {
            Storage[StorageIndex].GetComponent<Image>().sprite = allitem.NullImage;

        }
        //for (int i = 0; i < ForMakeItem.Count; i++)
        //{
        //    // 정렬을 사용하려고 했으나 그러면 맨 먼저 들어왔던 이미지가 걔보다 enum값이 클 경우
        //    // 부자연스럽게 뒤로 밀릴 수 있어서 그렇게 안하기로함

        //    if (i != 0 && ItemCount[ForMakeItem[i] - 1].Equals(0))
        //    {

        //        //0 이면 이전에 등록이 안됐다는 소리
        //        Storage[StorageIndex].GetComponent<Image>().sprite = allitem.ItemImage[ForMakeItem[i] - 1];
        //        StorageIndex += 1;
        //        ItemCount[ForMakeItem[i] - 1] += 1;
        //        Debug.Log("1"+ItemCount[ForMakeItem[i] - 1]);
        //        StorageCount[StorageIndex].text = ItemCount[ForMakeItem[i] - 1].ToString();
        //    }
        //    else if (i == 0 && ForMakeItem[0] != 0)
        //    {
        //        Storage[StorageIndex].GetComponent<Image>().sprite = allitem.ItemImage[ForMakeItem[0] - 1];
        //        ItemCount[ForMakeItem[0] - 1] += 1;
        //        StorageCount[0].text = ItemCount[ForMakeItem[0] - 1].ToString();
        //        Debug.Log("2" + ItemCount[ForMakeItem[i] - 1]);
        //        StorageIndex += 1;

        //    }
        //    else
        //    {
        //        // 증가가 됐었음
        //        // 인덱스 증가할 필요없고
        //        // Count 증가해야함
        //        //text 다시
        //        ItemCount[ForMakeItem[i] - 1] += 1;
        //        StorageCount[StorageIndex].text = ItemCount[ForMakeItem[i] - 1].ToString();
        //        Debug.Log("3" + ItemCount[ForMakeItem[i] - 1]);
        //    }

        //}


        //for (int j = StorageIndex; j < 6; j++)
        //{
        //    Storage[StorageIndex].GetComponent<Image>().sprite = allitem.NullImage;

        //}
        //for(int a = 0; a < 8; a++)
        //{
        //    // 미친듯한 for문ㅅ ㅏ용...
        //    ItemCount[a] = 0;
        //}

    }

}
